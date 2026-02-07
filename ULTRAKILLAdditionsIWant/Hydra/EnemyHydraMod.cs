using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using MelonLoader;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.Assertions;

namespace UKAIW
{
    [Serializable]
    public class EnemyHydraMod : MonoBehaviour
    {
        public class SharedData : ScriptableObject
        {
            protected SharedData()
            {
            }

            public void InstantiatePrefabToPool()
            {
                Assert.IsNotNull(Prefab);

                if (PrefabPoolFull)
                {
                    return;
                }

                var newGo = Instantiate(Prefab);

                PrefabPool.Push(newGo);

                newGo.SetActive(false);
            }

            public GameObject GetNewInstance()
            {
                Assert.IsNotNull(Prefab);

                if (PrefabPool.Count > 0)
                {
                    return PrefabPool.Pop();
                }

                return Instantiate(Prefab);
            }

            private ReserveList<GameObject> Instances = new ReserveList<GameObject>(16);
            public bool CountAsKill = false;
            private Stack<GameObject> PrefabPool = new Stack<GameObject>(32);
            public GameObject Prefab = null;
            public Bounds Bounds = new Bounds();
            public Action OnDeactivated = null;
            internal string CreatorName = "";
            public bool PrefabPoolFull { get => PrefabPool.Count >= Options.HydraPrefabPoolCapacity; }
            public ScriptableObject EnemySpecificShared = null;
            public int InstanceCount { get => Instances.Count; }
            public int GlobalIdx { get; private set; } = -1;
            public bool Active { get; private set; } = false;

            internal void UnregisterInstance(int sharedIdx)
            {
                Instances.RemoveAt(sharedIdx);

                if (InstanceCount == 0)
                {
                    Deactivate();
                }
            }

            internal int RegisterInstance(GameObject gameObject)
            {
                int idx = Instances.Add(gameObject);

                if (!Active)
                {
                    Activate();
                }

                return idx;
            }

            private void Deactivate()
            {
                Assert.IsTrue(Active);
                
                Log.TraceExpectedInfo($"EnemyHydraMod.SharedData '{name}' with creator '{CreatorName}' with prefab '{Prefab}' deactivated!");

                Hydra.SharedDatas.RemoveAt(GlobalIdx);
                
                OnDeactivated?.Invoke();

                Active = false;
            }
            
            private void Activate()
            {
                Assert.IsFalse(Active);

                Log.TraceExpectedInfo($"EnemyHydraMod.SharedData '{name}' with creator '{CreatorName}' with prefab '{Prefab}' activated!");

                GlobalIdx = Hydra.SharedDatas.Add(this);

                Active = true;
            }

            private static int SharedIDIncrementer = 0;
            private void Awake()
            {
                name = name + SharedIDIncrementer.ToString();
                SharedIDIncrementer += 1;
                Log.TraceExpectedInfo($"EnemyHydraMod.SharedData '{name}' with creator '{CreatorName}' awakened!");
            }

            private void OnDestroy()
            {
                Log.TraceExpectedInfo($"EnemyHydraMod.SharedData '{name}' with creator '{CreatorName}' with prefab '{Prefab}' destroyed!");

                foreach (var prefab in PrefabPool)
                {
                    Destroy(prefab);
                }

                if (Active)
                {
                    Deactivate();
                }
            }
        }

        public bool CanDuplicate 
        { 
            get
            {
                return Shared.InstanceCount < Options.HydraMaxFromOne && (NoDupeTime < 0.0f || Depth == 0);
            }
        }

        public SharedData Shared = null;
        public int Depth = -1;

        [NonSerialized] public EnemyIdentifier Eid = null;
        [NonSerialized] public EnemyAdditions Eadd = null;

        public EnemyGameplayRank GameplayRank = EnemyGameplayRank.Ultraboss;
        [NonSerialized] public bool ContributesToInstanceCount = false;
        [NonSerialized] public Action PreDeath = null;

        private bool ExcludedFromHydraCheat = false;

        private float NoDupeTime = 0.0f;
        public bool HydraKilled { get; private set; } = false;
        public bool HydraDuped { get; private set; } = false;
        public int SharedIdx {get; private set; } = -1;
        public string SharedName { get; private set; } = null;

        protected void OnDestroy()
        {
            if (Depth > 0)
            {
                //Player.PreDeath -= DestroySelf;
            }
            
            TryUnregisterWithShared();
        }
                
        protected void OnEnable()
        {
            if (SharedName == null)
            {
                return;
            }

            TryRegisterWithShared();
        }

        protected void OnDisable()
        {
            if (SharedName == null)
            {
                return;
            }
            
            TryUnregisterWithShared();
        }

        private void TryUnregisterWithShared()
        {            
            if (ExcludedFromHydraCheat)
            {
                return;
            }

            Assert.IsNotNull(Shared, $"Shared was null! Shared Name: '{SharedName}'");

            if (ContributesToInstanceCount)
            {            
                if (!Eid.dead)
                {
                    PreDeath?.Invoke();
                }

                Shared.UnregisterInstance(SharedIdx);
                SharedIdx = -1;
                ContributesToInstanceCount = false;

                if (Depth != 0)
                {
                    Destroy(gameObject);
                }

                /*if (Shared.InstanceCount == 0)
                {
                    Destroy(Shared);
                }*/
                
                MusicManager.Instance?.PlayCleanMusic();
            }
        }

        protected void Update()
        {
            if (!Cheats.IsHydraModeOn)
            {
                return;
            }

            if (ExcludedFromHydraCheat)
            {
                return;
            }
            
            if (Eid.Dead)
            {
                return;
            }
            
            if (NoDupeTime >= 0.0f)
            {
                NoDupeTime -= Time.deltaTime / Mathf.Max(1.0f, (Shared.InstanceCount * 0.3f) + 0.667f);

                if (NoDupeTime <= 0.0f)
                {
                    NoDupeTime = -1.0f;
                }                
            }
        }

        protected void Awake()
        {
            Eid = GetComponent<EnemyIdentifier>();
            Eadd = GetComponent<EnemyAdditions>();
        }

        protected void Start()
        {
            if (Eid.enemyType == EnemyType.Idol || (Eid.enemyType == EnemyType.Centaur && Eid.gameObject.name.Contains("rain", StringComparison.OrdinalIgnoreCase)) || Eid.enemyType == EnemyType.V2Second)
            {
                ExcludedFromHydraCheat = true;
                return;
            }

            Assert.IsNotNull(Eid, $"For object by name {gameObject.name}");

            if (Eid.dead)
            {
                return;
            }

            Assert.IsTrue(Depth >= 0, $"For object by name {gameObject.name}");
            Assert.IsNotNull(Shared, $"For object by name {gameObject.name} shared was null! Shared Name: '{SharedName}'");

            MusicManager.Instance?.PlayBattleMusic();

            if (Depth > 0)
            {
                Eid.dontCountAsKills = true;
            }
            else
            {
                Shared.CountAsKill = !Eid.dontCountAsKills;
            }

            GameplayRank = EnemyUtils.GetEnemyGameplayRank(Eid);

            switch (GameplayRank)
            {
                case EnemyGameplayRank.Normal:
                    NoDupeTime = Options.HydraDefaultWaitTime;
                    break;
                case EnemyGameplayRank.Miniboss:
                    NoDupeTime = Options.HydraMiniBossWaitTime;
                    break;
                case EnemyGameplayRank.Boss:
                    NoDupeTime = Options.HydraBossWaitTime;
                    break;
                case EnemyGameplayRank.Ultraboss:
                    NoDupeTime = Options.HydraUltraBossWaitTime;
                    break;
            }

            var newHealth = Eid.health;
            for (int i = 0; i < Depth; i++)
            {
                newHealth *= Options.HydraHealthDecayScale;
            }

            if (Eid.zombie)
            {
                Eid.zombie.health = newHealth;
            }
            else if (Eid.spider)
            {
                Eid.spider.health = newHealth;
            }
            else if (Eid.machine)
            {
                Eid.machine.health = newHealth;
            }
            else if (Eid.drone)
            {
                Eid.drone.health = newHealth;
            }
            else if (Eid.statue)
            {
                Eid.statue.health = newHealth;
            }

            if (Depth > 0)
            {
                //Player.PreDeath += DestroySelf;
                if (Eid.enemyType == EnemyType.MaliciousFace)
                {
                    gameObject.transform.parent.gameObject.AddComponent<DestroyOnCheckpointRestart>();
                }
                else
                {
                    gameObject.AddComponent<DestroyOnCheckpointRestart>();                    
                }
            }

            DroneFlesh droneFlesh = Eid.GetComponent<DroneFlesh>();
            if (Depth > 0 && (droneFlesh != null))
            {
                var mainLight = GetComponent<Light>();
                if (mainLight != null)
                {
                    mainLight.enabled = false;
                }
                var lights = GetComponentsInChildren<Light>();
                foreach (var light in lights)
                {
                    light.enabled = false;
                }

            }

            if (droneFlesh != null)
            {
                gameObject.AddComponent<FleshDroneHydra>();
            }

            switch (Eid.enemyType)
            {
                case EnemyType.FleshPanopticon:
                    gameObject.AddComponent<FleshPanopticonHydra>();
                    break;
                case EnemyType.Drone:

                    break;

                default:
                    break;
            }

            TryRegisterWithShared();
        }

        private void TryRegisterWithShared()
        {
            Assert.IsNotNull(Shared, $"Shared was null! Shared Name: '{SharedName}'");

            if (!ContributesToInstanceCount)
            {
                SharedIdx = Shared.RegisterInstance(gameObject);
                ContributesToInstanceCount = true;
            }
        }
        
        private void DestroySelf(NewMovement movement, int damage)
        {
            Destroy(gameObject);
        }

        public void NotifyOfDeath()
        {
            if (ExcludedFromHydraCheat)
            {
                return;
            }

            if (Eid == null)
            {
                Eid = GetComponent<EnemyIdentifier>();
            }

            if (Eid.Dead)
            {
                return;
            }

            if (!ContributesToInstanceCount)
            {
                return;
            }

            if (!Cheats.IsCheatEnabled(Cheats.HydraMode))
            {
                return;
            }

            Eid.dontCountAsKills = true;
            PreDeath?.Invoke();

            if (Eid.enemyType != EnemyType.SisyphusPrime && Eid.enemyType != EnemyType.MinosPrime)
            {
                Eid.puppet = true;
            }
            
            if (Depth == 0 && Shared.CountAsKill)
            {
                ContributeToActivateNextWave();
            }

            TryEnqueueDupe(false);
            TryEnqueueDupe(true);
            
            TryUnregisterWithShared();

            if (!HydraDuped && Shared.InstanceCount == 0)
            {
                Eid.puppet = false;
                HydraKilled = true;

                if (Shared.CountAsKill)
                {
                    StatsManager.Instance.kills += 1;
                }

                TimeDilation.ModDisableHitstop = true;
                Hydra.Hitstop(-1.0);
                Hydra.Hitstop(-1.0);
                TimeDilation.ModDisableHitstop = false;
                switch (GameplayRank)
                {
                    case EnemyGameplayRank.Normal:
                        StyleHUD.Instance.AddPoints(Options.HydraKillBonus, "<color=#a2beff>HYDRA KILL</color>", null, Eid);
                        break;
                    case EnemyGameplayRank.Miniboss:
                        StyleHUD.Instance.AddPoints(Options.HydraMiniBossKillBonus, "<color=#8d96fe>KINDA BIG HYDRA KILL</color>", null, Eid);
                        break;
                    case EnemyGameplayRank.Boss:
                        StyleHUD.Instance.AddPoints(Options.HydraBossKillBonus, "<color=#8a2af7>BIG HYDRA KILL</color>", null, Eid);
                        break;
                    case EnemyGameplayRank.Ultraboss:
                        StyleHUD.Instance.AddPoints(Options.HydraUltraBossKillBonus, "<color=#ffdb00>?? ULTRA HYDRA KILL ??</color>", null, Eid);
                        StyleHUD.Instance.AddPoints(0, "<color=#ffdb00>?? HOW ??</color>", null, Eid);
                        break;
                }
            }
            else
            {
                Eadd.QueuedForDestruction = true;
            }
        }

        private void ContributeToActivateNextWave()
        {
            ActivateNextWave componentInParent = Eid.GetComponentInParent<ActivateNextWave>();
            componentInParent?.AddDeadEnemy();
        }

        private void TryEnqueueDupe(bool isB)
        {
            if (ExcludedFromHydraCheat)
            {
                return;
            }
            
            if (!CanDuplicate)
            {
                return;
            }

            /*Assert.IsNotNull(GetComponent<EnemyAdditions>(), $"For object by name {gameObject.name}");
            Assert.IsNotNull(GetComponent<EnemyAdditions>().PrefabMod, $"For object by name {gameObject.name}");
            Assert.IsNotNull(GetComponent<EnemyAdditions>().PrefabMod.Prefab, $"For object by name {gameObject.name}");
            */
            HydraDuped = true;
            Hydra.QueuedDupeInfo dupeInfo = new Hydra.QueuedDupeInfo();
            
            if (Eid.enemyType == EnemyType.Drone)
            {
                dupeInfo.Position = Eid.drone.GetComponent<Rigidbody>().transform.position;
            }
            else
            {
                dupeInfo.Position = transform.position;
            }
            
            dupeInfo.Rotation = transform.rotation;
            dupeInfo.LocalScale = transform.localScale;
            dupeInfo.SharedData = Shared;
            dupeInfo.Depth = Depth + 1;
            dupeInfo.EnemyType = Eid.enemyType;
            dupeInfo.BossBar = GetComponent<BossHealthBar>() != null;

            if (Eid.enemyType == EnemyType.Sisyphus)
            {
                dupeInfo.Position += (dupeInfo.Rotation * Vector3.right) * (isB ? -4.25f : 4.25f);
            }
            else
            {
                float additionalOffsetScalar = 1.0f;

                switch (Eid.enemyType)
                {
                    case EnemyType.HideousMass:
                        additionalOffsetScalar = 0.0f;
                        break;
                    case EnemyType.Minos:
                        additionalOffsetScalar = 0.0f;
                        break;
                    case EnemyType.FleshPanopticon:
                    case EnemyType.FleshPrison:
                        additionalOffsetScalar = 0.0f;
                        break;
                    case EnemyType.CancerousRodent:
                        additionalOffsetScalar = 0.00f;
                        break;
                    case EnemyType.VeryCancerousRodent:
                        additionalOffsetScalar = 0.00f;
                        break;
                    default:
                        break;
                }
                
                if (GetComponent<CancerousRodent>() != null)
                {
                    additionalOffsetScalar = 0.01f;
                }

                //dupeInfo.Position += (dupeInfo.Rotation * Vector3.Normalize(Vector3.Lerp(Vector3.right, Vector3.forward, UnityEngine.Random.Range(0.0f, 1.0f))))
                //                     * (isB ? -1.0f : 1.0f);
                dupeInfo.Position += Vector3.Project(dupeInfo.SharedData.Bounds.size, dupeInfo.Rotation * Vector3.right) * (isB ? -1.0f : 1.0f) * 0.3f * additionalOffsetScalar;
            }

            Hydra.EnqueueDupe(dupeInfo);
        }

        public void InitializeAsNew()
        {
            Shared = ScriptableObject.CreateInstance<SharedData>();
            TryRegisterWithShared();
            Shared.Bounds = EnemyUtils.SolveEnemyBounds(gameObject);
                    
            if (GetComponent<DroneFlesh>() != null)
            {
                Shared.EnemySpecificShared = new FleshDroneHydra.SharedData();
            }

            switch (Eid.enemyType)
            {
                case EnemyType.FleshPanopticon:
                    Shared.EnemySpecificShared = new FleshPanopticonHydra.SharedData();
                break;
                default:
                break;
            }
            
            Depth = 0;
            Shared.CreatorName = gameObject.name;
            SharedName = Shared.name;
        }

        public void PassPrefabToShared()
        {
            if (Shared.Prefab == null)
            {
                Shared.Prefab = GetComponent<EnemyAdditions>().PrefabMod.Prefab;
            }
        }

        internal void DuringDeath()
        {
            TryUnregisterWithShared();
        }
    }
}