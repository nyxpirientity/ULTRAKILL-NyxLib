using System.Collections.Generic;
using Nyxpiri;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

public class EnemyPrefabStore : EnemyModifier
{
    public class InstanceStore : ScriptableObject
    {
        public void Initialize(GameObject prefab)
        {
            Prefab = prefab;

            RegistrationTracker = new RegistrationTracker(
                registerAction: () =>
                {
                    RegistrationIdx = EnemyPrefabManager.RegisterInstanceStore(this);
                    return true;
                },
                unregisterAction: () =>
                {
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
                Assert.IsNotNull(Prefab); // then just print this to the logs *once*
            }

            if (IsFull)
            {
                return;
            }

            var newGo = Instantiate(Prefab, Prefab.transform.parent);

            Instances.Push(newGo);

            newGo.SetActive(false);
        }

        private GameObject Prefab = null;
        Stack<GameObject> Instances = new Stack<GameObject>();

        public void RegisterStore(EnemyPrefabStore store)
        {
            RegisteredStores.Add(store);
            
            if (RegisteredStores.Count == 1)
            {
                RegistrationTracker.Register();
            }
        }

        public void UnregisterStore(EnemyPrefabStore store)
        {
            RegisteredStores.Remove(store);

            if (RegisteredStores.Count == 0)
            {
                RegistrationTracker.Unregister();
            }
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
    private GameObject _PrefabParent = null;
    public GameObject PrefabParent { get => _PrefabParent ?? null; }
    private EnemyIdentifier Eid = null;
    public EnemyAdditions Eadd { get; private set; } = null;

    private static bool IsStoringPrefab = false;

    public EnemyPrefabStore()
    {
        InstancesRegistrator = new RegistrationTracker(registerAction: () =>
        {
            if (_Instances == null)
            {
                return false;
            }
            
            _Instances.RegisterStore(this);
            return true;
        },
        unregisterAction: () =>
        {
            if (_Instances == null)
            {
                return false;
            }
            
            _Instances.UnregisterStore(this);
            
            return true;
        });
    }

    protected void Awake()
    {
        Eid = GetComponent<EnemyIdentifier>();
        Eadd = GetComponent<EnemyAdditions>();
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

        GameObject templateGo;
        
        templateGo = Eadd.RootGameObject;
        
        IsStoringPrefab = true;

        Prefab = UnityEngine.Object.Instantiate(templateGo, templateGo.transform.parent);

        _PrefabParent = templateGo.transform.parent.gameObject;
        Prefab.SetActive(false);
                
        if (_Instances == null)
        {
            _Instances = ScriptableObject.CreateInstance<InstanceStore>();
            _Instances.Initialize(Prefab);
            InstancesRegistrator.Register();
        }

        var prefabEid = Prefab.GetComponent<EnemyIdentifier>() ?? Prefab.GetComponentInChildren<EnemyIdentifier>();
        var prefabEadd = Prefab.GetComponent<EnemyAdditions>() ?? Prefab.GetComponentInChildren<EnemyAdditions>();

        prefabEid.destroyOnDeath = new System.Collections.Generic.List<GameObject>();
        prefabEid.activateOnDeath = new GameObject[0];
        prefabEid.drillers = new System.Collections.Generic.List<Harpoon>();
        prefabEid.stuckMagnets = new System.Collections.Generic.List<Magnet>();
        
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
}