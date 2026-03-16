using System.Collections.Generic;
using Nyxpiri;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

public class EnemyPrefabStore : EnemyModifier
{
    public class InstanceStore : ScriptableObject
    {
        public void Initialize(GameObject prefab, GameObject prefabParent, string debugName)
        {
            Prefab = prefab;
            PrefabParent = prefabParent;
            _debugName = debugName;

            Log.TraceExpectedInfo($"New instance store by the name of {debugName} being created with prefab {Prefab}");

            Assert.IsNotNull(Prefab);

            RegistrationTracker = new RegistrationTracker(
                registerAction: () =>
                {
                    Log.TraceExpectedInfo($"{_debugName}: Registering to prefab manager");
                    Assert.IsNotNull(Prefab);
                    RegistrationIdx = EnemyPrefabManager.RegisterInstanceStore(this);
                    return true;
                },
                unregisterAction: () =>
                {
                    Log.TraceExpectedInfo($"{_debugName}: Unregistering from prefab manager");
                    Assert.IsNotNull(Prefab);
                    EnemyPrefabManager.UnregisterInstanceStore(RegistrationIdx);
                    RegistrationIdx = -1;
                    return true;
                }
            );
        }

        public void InstantiateAndStore()
        {
            if (Prefab == null)
            {
                RegistrationTracker.Unregister();
                Log.Error($"{_debugName}: InstanceStore had instantiate and store called despite prefab being null, and thus destroyed.");
                return;
            }

            if (PrefabParent == null)
            {
                RegistrationTracker.Unregister();
                Log.Error($"{_debugName}: InstanceStore had instantiate and store called despite prefab parent being null, and thus destroyed.");
                return;
            }

            if (IsFull)
            {
                return;
            }

            var newGo = Instantiate(Prefab, PrefabParent?.transform);

            Log.TraceExpectedInfo($"{_debugName}: Instantiating and storing for prefab {Prefab}");

            Instances.Push(newGo);

            newGo.SetActive(false);
        }

        public GameObject Prefab = null;

        public GameObject PrefabParent = null;

        private string _debugName = "UNNAMED";
        Stack<GameObject> Instances = new Stack<GameObject>();

        public void RegisterStore(EnemyPrefabStore store)
        {
            RegisteredStores.Add(store);
            
            if (RegisteredStores.Count == 1)
            {
                RegistrationTracker.Register();
            }

            Assert.IsNotNull(Prefab);
        }

        public void UnregisterStore(EnemyPrefabStore store)
        {
            RegisteredStores.Remove(store);

            if (RegisteredStores.Count == 0)
            {
                RegistrationTracker.Unregister();
            }
            
            Assert.IsNotNull(Prefab);
        }

        public GameObject GetNewInstance()
        {
            Assert.IsNotNull(Prefab);

            if (Instances.Count > 0)
            {
                return Instances.Pop();
            }

            return Instantiate(Prefab);
        }

        HashSet<EnemyPrefabStore> RegisteredStores = new HashSet<EnemyPrefabStore>(32);
        RegistrationTracker RegistrationTracker = null;
        private int RegistrationIdx = -1;

        public bool IsFull { get => Instances.Count >= 5; }
    }

    [SerializeField] private InstanceStore _Instances = null;
    public InstanceStore Instances { get => _Instances; }
    RegistrationTracker InstancesRegistrator = null;
    public GameObject Prefab = null;
    [SerializeField] private GameObject _PrefabParent = null;
    public GameObject PrefabParent { get => _PrefabParent ?? null; }
    [SerializeField] private EnemyIdentifier Eid = null;
    [SerializeField] private EnemyComponents Enemy = null;

    private bool IsPrefab { get; set; } = false;

    private static bool IsStoringPrefab = false;

    public EnemyPrefabStore()
    {
        InstancesRegistrator = new RegistrationTracker(registerAction: () =>
        {
            if (_Instances == null)
            {
                return false;
            }
            
            Log.TraceExpectedInfo($"{gameObject} (EnemyPrefabStore): Registering self to InstanceStore");

            _Instances.RegisterStore(this);
            
            return true;
        },
        unregisterAction: () =>
        {
            if (_Instances == null)
            {
                return false;
            }
            
            Log.TraceExpectedInfo($"{gameObject} (EnemyPrefabStore): Unregistering self to InstanceStore");

            _Instances.UnregisterStore(this);

            return true;
        });
    }

    protected void Awake()
    {
        GetComps();

        if (Prefab != null && _PrefabParent == null)
        {
            _PrefabParent = Enemy.RootGameObject.transform.parent?.gameObject;
        }
    }

    protected void Start()
    {
        InstancesRegistrator.Register();
    }

    protected void OnEnable()
    {
        InstancesRegistrator.Register();
    }

    protected void OnDisable()
    {
        InstancesRegistrator.Unregister();
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

    private void OnDestroy()
    {
        if (IsPrefab)
        {
            Log.TraceExpectedInfo($"PREFAB object {gameObject} being destroyed...");
            StackDebug.PrintStack();
        }
    }

    private void StorePrefabUnsafe(bool force = false)
    {
        if (IsStoringPrefab)
        {
            Log.UnexpectedInfo($"EnemyPrefabStore tried to store a prefab whilst we were storing a prefab");
            return;
        }

        if (Prefab != null && !force)
        {
            Log.TraceExpectedInfo($"EnemyPrefabMod found that {name} already had a prefab, and force is false, no need to make a new one");
            return;
        }
        else if (Prefab != null && force)
        {
            Log.TraceExpectedInfo($"EnemyPrefabMod found that {name} already had a prefab, but force is true, need to make a new one");
        }
        else if (Prefab == null)
        {
            Log.TraceExpectedInfo($"EnemyPrefabMod found that {name} did not have a prefab, need to make a new one");
        }

        GetComps();

        GameObject templateGo;

        templateGo = Enemy.RootGameObject;

        IsStoringPrefab = true;

        Prefab = UnityEngine.Object.Instantiate(templateGo);

        Assert.IsNotNull(templateGo);
        Assert.IsNotNull(templateGo.transform);
        _PrefabParent = templateGo.transform.parent?.gameObject;
        Prefab.SetActive(false);

        if (_Instances == null)
        {
            _Instances = ScriptableObject.CreateInstance<InstanceStore>();
            _Instances.Initialize(Prefab, _PrefabParent, $"InstanceStore For '{gameObject}'");

            if (isActiveAndEnabled)
            {
                InstancesRegistrator.Register();                
            }
        }

        _Instances.Prefab = Prefab;
        _Instances.PrefabParent = _PrefabParent;

        var prefabEadd = Prefab.GetComponent<EnemyComponents>() ?? Prefab.GetComponentInChildren<EnemyComponents>(true);
        var prefabEid = prefabEadd.Eid;

        prefabEid.destroyOnDeath = new System.Collections.Generic.List<GameObject>();
        prefabEid.activateOnDeath = new GameObject[0];
        prefabEid.drillers = new System.Collections.Generic.List<Harpoon>();
        prefabEid.stuckMagnets = new System.Collections.Generic.List<Magnet>();
        prefabEid.blessed = false;

        prefabEid.onDeath.RemoveAllListeners();

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

        prefabEadd.PrefabStore._Instances = _Instances;
        prefabEadd.PrefabStore.Prefab = Prefab;
        prefabEadd.PrefabStore.IsPrefab = true;

        if (prefabEid.enemyType == EnemyType.Swordsmachine)
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
        if (Enemy == null)
        {
            Enemy = GetComponent<EnemyComponents>();
        }

        if (Eid == null)
        {
            Eid = GetComponent<EnemyIdentifier>();
        }
    }
}