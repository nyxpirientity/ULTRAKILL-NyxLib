using System;
using System.Collections.Generic;
using Nyxpiri;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public class EnemyCloning : EnemyModifier
{
    public EnemyCloneStore Store { get => _instances; }
    RegistrationTracker InstancesRegistrator = null;
    /* direct access to the prefab game object, not actually recommended to be used for instantiating prefab instances, prefer Instances.GetNewInstance() instead */
    public GameObject PrefabDirectGameObject => _prefab;
    public GameObject InstanceParent { get => _spawnedInstanceParent ?? null; }
    [SerializeField] private GameObject _prefabHolder = null;
    private ActivateNextWave _activateNextWave = null;

    public ActivateNextWave ActivateNextWave
    {
        get
        {
            if (_activateNextWave != null)
            {
                return _activateNextWave;
            }

            _activateNextWave = GetComponentInParent<ActivateNextWave>();

            return _activateNextWave;
        }
    }

    public EnemyCloning()
    {
        InstancesRegistrator = new RegistrationTracker(registerAction: () =>
        {
            if (_instances == null)
            {
                return false;
            }

            Log.TraceExpectedInfo($"{gameObject} (EnemyPrefabStore): Registering self to InstanceStore");

            _instances.RegisterEnemy(this);

            return true;
        },
        unregisterAction: () =>
        {
            if (_instances == null)
            {
                return false;
            }

            Log.TraceExpectedInfo($"{gameObject} (EnemyPrefabStore): Unregistering self to InstanceStore");

            _instances.UnregisterEnemy(this);

            return true;
        });
    }

    public void StorePrefab(bool force = false)
    {
        try
        {
            StorePrefabUnsafe(force);
        }
        catch (System.Exception)
        {
            IsStoringPrefab = false;
            throw;
        }
    }

    protected void Awake()
    {
        GetComps();

        if (_prefab != null && _spawnedInstanceParent == null)
        {
            GameObject parent = ((MonoBehaviour)ActivateNextWave ?? _enemy.RootGameObject.GetComponentInParent<GoreZone>())?.gameObject;
            _spawnedInstanceParent = parent;
        }
    }

    protected void Start()
    {
        InstancesRegistrator.Register();

        _enemy.PostDeath += PostDeath;
        if (!((Store.SpawnedInstanceParent?.gameObject?.activeInHierarchy).GetValueOrDefault(false)) && ActivateNextWave != null)
        {
            Store.SpawnedInstanceParent = ActivateNextWave.transform;
        }
    }

    private void PostDeath(EventMethodCancelInfo cancelInfo, bool instakill)
    {
        if (cancelInfo.Cancelled)
        {
            return;
        }

        if (!Options.UnregisterEnemyFromCloneStoreOnDeath.Value)
        {
            return;
        }

        InstancesRegistrator.Unregister();
    }

    protected void OnEnable()
    {
        InstancesRegistrator.Register();
    }

    protected void OnDisable()
    {
        InstancesRegistrator.Unregister();
    }

    [SerializeField] private EnemyCloneStore _instances = null;
    [SerializeField] private GameObject _spawnedInstanceParent = null;
    [SerializeField] private GameObject _prefab = null;
    [SerializeField] private EnemyIdentifier _eid = null;
    [SerializeField] private EnemyComponents _enemy = null;

    private bool IsPrefab { get; set; } = false;

    private static bool IsStoringPrefab = false;

    private void OnDestroy()
    {
        if (IsPrefab)
        {
            //Log.TraceExpectedInfo($"PREFAB object {gameObject} being destroyed...");
            //StackDebug.PrintStack();
        }
    }

    private void Update()
    {
        if (!Cheats.Enabled)
        {
            return;
        }

        if (_prefab == null)
        {
            StorePrefab();
        }
    }

    private void StorePrefabUnsafe(bool force = false)
    {
        if (IsStoringPrefab)
        {
            Log.UnexpectedInfo($"EnemyPrefabStore tried to store a prefab whilst we were storing a prefab");
            return;
        }

        if (_prefab != null && !force)
        {
            Log.TraceExpectedInfo($"EnemyPrefabMod found that {name} already had a prefab, and force is false, no need to make a new one");
            return;
        }
        else if (_prefab != null && force)
        {
            Log.TraceExpectedInfo($"EnemyPrefabMod found that {name} already had a prefab, but force is true, need to make a new one");
        }
        else if (_prefab == null)
        {
            Log.TraceExpectedInfo($"EnemyPrefabMod found that {name} did not have a prefab, need to make a new one");
        }

        if (!Cheats.Enabled)
        {
            return;
        }

        if (_prefabHolder == null)
        {
            _prefabHolder = new GameObject($"{name}'s nyxlib clone prefab holder");
            _prefabHolder.SetActive(false);
        }

        GetComps();

        GameObject templateGo;

        templateGo = _enemy.RootGameObject;

        IsStoringPrefab = true;

        _prefab = UnityEngine.Object.Instantiate(templateGo, _prefabHolder.transform);

        _prefab.SetActive(false);

        Assert.IsNotNull(templateGo);
        Assert.IsNotNull(templateGo.transform);

        var prefabEadd = _prefab.GetComponent<EnemyComponents>() ?? _prefab.GetComponentInChildren<EnemyComponents>(true);
        var prefabEid = prefabEadd.Eid;

        if (_instances == null)
        {
            _instances = ScriptableObject.CreateInstance<EnemyCloneStore>();
            _instances.Initialize(_prefab, ActivateNextWave?.transform, prefabEadd, $"EnemyCloneStore For '{gameObject}'");

            if (isActiveAndEnabled)
            {
                InstancesRegistrator.Register();
            }
        }

        _instances.Prefab = _prefab;
        _instances.PrefabHolder = _prefabHolder.transform;
        _instances.SpawnedInstanceParent = ActivateNextWave?.transform;
        prefabEadd.Cloning.IsPrefab = true;

        prefabEid.activateOnDeath = new GameObject[0];
        prefabEid.drillers = new System.Collections.Generic.List<Harpoon>();
        prefabEid.stuckMagnets = new System.Collections.Generic.List<Magnet>();
        prefabEid.blessed = false;
        prefabEid.destroyOnDeath = new System.Collections.Generic.List<GameObject>();

        prefabEid.onDeath = new UnityEngine.Events.UnityEvent();

        var onDestroy = prefabEid.GetComponent<EventOnDestroy>();

        if (onDestroy != null)
        {
            onDestroy.stuff = new UnityEngine.Events.UnityEvent();
        }

        if (prefabEid.machine != null)
        {
            prefabEid.machine.musicRequested = false;
        }

        if (prefabEid.zombie != null)
        {
            prefabEid.zombie.musicRequested = false;
        }

        if (prefabEid.statue != null)
        {
            prefabEid.statue.musicRequested = false;
        }

        prefabEadd.Cloning._instances = _instances;
        prefabEadd.Cloning._prefab = _prefab;

        if (prefabEid.enemyType == global::EnemyType.Swordsmachine)
        {
            var swordsMachine = prefabEid.GetComponent<SwordsMachine>();
            swordsMachine.secondPhasePosTarget = null;
            swordsMachine.firstPhase = false;
            swordsMachine.GetComponent<EnemyIdentifier>().spawnIn = true;
            swordsMachine.inAction = false;
            swordsMachine.inSemiAction = false;
            swordsMachine.moveAtTarget = false;
        }

        IsStoringPrefab = false;
    }

    private void GetComps()
    {
        if (_enemy == null)
        {
            _enemy = GetComponent<EnemyComponents>();
        }

        if (_eid == null)
        {
            _eid = GetComponent<EnemyIdentifier>();
        }
    }
}