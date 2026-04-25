using System;
using System.Collections.Generic;
using Nyxpiri;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class EnemyPrefabStore : EnemyModifier
    {
        public static void RequestInstanceStoreCapacity(int requestedCapacity)
        {
            InstanceStoreCapacityModsAdditional = Math.Max(InstanceStoreCapacity, requestedCapacity);
        }

        public static int InstanceStoreCapacity => Mathf.Min(InstanceStoreCapacityModsAdditional, Options.EnemyPrefabInstanceStoreCapacityMax.Value);
        public static int InstanceStoreCapacityModsAdditional { get; private set; } = 0;

        public class InstanceStore : ScriptableObject
        {
            public void Initialize(GameObject prefab, GameObject prefabParent, EnemyComponents prefabEadd, string debugName)
            {
                Prefab = prefab;
                PrefabParent = prefabParent;
                PrefabEadd = prefabEadd;
                _debugName = debugName;

                Log.TraceExpectedInfo($"New instance store by the name of {debugName} being created with prefab {Prefab}");
                  
                if (Cheats.Enabled)
                {
                    Assert.IsNotNull(Prefab);
                }

                RegistrationTracker = new RegistrationTracker(
                    registerAction: () =>
                    {
                        Log.TraceExpectedInfo($"{_debugName}: Registering to prefab manager");
                        
                        if (Cheats.Enabled)
                        {
                            Assert.IsNotNull(Prefab);
                        }

                        RegistrationIdx = EnemyPrefabManager.RegisterInstanceStore(this);
                        return true;
                    },
                    unregisterAction: () =>
                    {
                        Log.TraceExpectedInfo($"{_debugName}: Unregistering from prefab manager");
                        
                        if (Cheats.Enabled)
                        {
                            Assert.IsNotNull(Prefab);
                        }

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

                var newGo = Instantiate(Prefab);

                Log.TraceExpectedInfo($"{_debugName}: Instantiating and storing for prefab {Prefab}");

                Instances.Push(newGo);

                newGo.SetActive(false);
            }

            public GameObject Prefab = null;

            public GameObject PrefabParent = null;

            public EnemyComponents PrefabEadd { get; private set; }

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

                GameObject instGo = null;

                if (Instances.Count > 0)
                {
                    instGo = Instances.Pop();
                }
            
                instGo ??= Instantiate(Prefab);

                instGo.transform.SetParent(PrefabParent?.transform);

                if (PrefabEadd.Eid.enemyType == EnemyType.Stalker) // TODO: this is necessary to make them not... ragdoll instead of explode. not sure what the best approach is to fixing right now
                {
                    var instEnemy = instGo.GetComponent<EnemyComponents>();
                    instEnemy.PreDeath += (canceler, instakill) => { instGo.GetComponent<Stalker>().SandExplode(); };
                    instEnemy.PostDeath += (cancelInfo, instakill) => { instGo.GetComponent<EnemyComponents>().InstaDestroy(); };
                }

                return instGo;
            }

            HashSet<EnemyPrefabStore> RegisteredStores = new HashSet<EnemyPrefabStore>(32);
            RegistrationTracker RegistrationTracker = null;
            private int RegistrationIdx = -1;

            public bool IsFull { get => Instances.Count >= InstanceStoreCapacity; }
        }

        public InstanceStore Instances { get => _instances; }
        RegistrationTracker InstancesRegistrator = null;
        /* direct access to the prefab game object, not actually recommended to be used for instantiating prefab instances, prefer Instances.GetNewInstance() instead */
        public GameObject PrefabDirectGameObject => _prefab;
        public GameObject PrefabParent { get => _prefabParent ?? null; }

        public EnemyPrefabStore()
        {
            InstancesRegistrator = new RegistrationTracker(registerAction: () =>
            {
                if (_instances == null)
                {
                    return false;
                }
                
                Log.TraceExpectedInfo($"{gameObject} (EnemyPrefabStore): Registering self to InstanceStore");

                _instances.RegisterStore(this);
                
                return true;
            },
            unregisterAction: () =>
            {
                if (_instances == null)
                {
                    return false;
                }
                
                Log.TraceExpectedInfo($"{gameObject} (EnemyPrefabStore): Unregistering self to InstanceStore");

                _instances.UnregisterStore(this);

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

            if (_prefab != null && _prefabParent == null)
            {
                _prefabParent = _enemy.RootGameObject.transform.parent?.gameObject;
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

        [SerializeField] private InstanceStore _instances = null;
        [SerializeField] private GameObject _prefabParent = null;
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
            
            GetComps();

            GameObject templateGo;

            templateGo = _enemy.RootGameObject;

            IsStoringPrefab = true;

            var templateActive = templateGo.activeSelf;
            var prefabHolder = new GameObject();

            prefabHolder.SetActive(false); 
            
            if (templateActive)
            {
                //templateGo.SetActive(false);
            }

            _prefab = UnityEngine.Object.Instantiate(templateGo, prefabHolder.transform);
            
            if (templateActive)
            {
                //templateGo.SetActive(true);
            }

            _prefab.SetActive(false);

            Assert.IsNotNull(templateGo);
            Assert.IsNotNull(templateGo.transform);
            _prefabParent = templateGo.transform.parent?.gameObject;

            var prefabEadd = _prefab.GetComponent<EnemyComponents>() ?? _prefab.GetComponentInChildren<EnemyComponents>(true);
            var prefabEid = prefabEadd.Eid;

            if (_instances == null)
            {
                _instances = ScriptableObject.CreateInstance<InstanceStore>();
                _instances.Initialize(_prefab, _prefabParent, prefabEadd, $"InstanceStore For '{gameObject}'");

                if (isActiveAndEnabled)
                {
                    InstancesRegistrator.Register();                
                }
            }

            _instances.Prefab = _prefab;
            _instances.PrefabParent = _prefabParent;
            prefabEadd.PrefabStore.IsPrefab = true;

            prefabEid.activateOnDeath = new GameObject[0];
            prefabEid.drillers = new System.Collections.Generic.List<Harpoon>();
            prefabEid.stuckMagnets = new System.Collections.Generic.List<Magnet>();
            prefabEid.blessed = false;
            prefabEid.destroyOnDeath = new System.Collections.Generic.List<GameObject>();
            
            prefabEid.onDeath = new UnityEngine.Events.UnityEvent();

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

            prefabEadd.PrefabStore._instances = _instances;
            prefabEadd.PrefabStore._prefab = _prefab;

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
}