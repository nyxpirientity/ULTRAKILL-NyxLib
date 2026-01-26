using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            private static int SharedIDIncrementer = 0;
            private void Awake()
            {
                Hydra.SharedDatas.Add(this);
                name = name + SharedIDIncrementer.ToString();
                SharedIDIncrementer += 1;
                PrefabPool.Capacity = Options.HydraPrefabPoolCapacity;
                Log.TraceExpectedInfo($"EnemyHydraMod.SharedData '{name}' with creator '{CreatorName}' awakened!");
            }

            private void OnDestroy()
            {
                Log.TraceExpectedInfo($"EnemyHydraMod.SharedData '{name}' with creator '{CreatorName}' with prefab '{Prefab}' destroyed!");
                Hydra.SharedDatas.Remove(this);
                foreach (var prefab in PrefabPool)
                {
                    Destroy(prefab);
                }
                
                OnDestroyed?.Invoke();
            }

            public void InstantiatePrefabToPool()
            {
                Assert.IsNotNull(Prefab);

                var newGo = Instantiate(Prefab);

                PrefabPool.Add(newGo);

                newGo.SetActive(false);
            }

            public GameObject GetNewInstance()
            {
                Assert.IsNotNull(Prefab);

                if (PrefabPool.Count > 0)
                {
                    var go = PrefabPool[PrefabPool.Count - 1];
                    PrefabPool.RemoveAt(PrefabPool.Count - 1);
                    return go;
                }

                return Instantiate(Prefab);
            }

            public int InstanceCount = 0;
            public bool CountAsKill = false;
            public List<GameObject> PrefabPool = new List<GameObject>();
            public GameObject Prefab = null;
            public Bounds Bounds = new Bounds();
            public Action OnDestroyed = null;
            internal string CreatorName = "";
            public ScriptableObject EnemySpecificShared = null;
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
        public bool ContributesToInstanceCount = false;
        public Action PreDeath = null;

        private bool ExcludedFromHydraCheat = false;

        private float NoDupeTime = 0.0f;

        protected void OnDestroy()
        {
            if (Depth > 0)
            {
                //Player.PreDeath -= DestroySelf;
            }
            
            TryDecrementInstanceCount();
        }

        private void TryDecrementInstanceCount()
        {
            if (ExcludedFromHydraCheat)
            {
                return;
            }
            
            if (!Eid.dead)
            {
                PreDeath?.Invoke();
            }

            if (ContributesToInstanceCount)
            {
                Shared.InstanceCount -= 1;
                ContributesToInstanceCount = false;

                if (Shared.InstanceCount == 0)
                {
                    Destroy(Shared);
                }
                
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
            if (Eid.enemyType == EnemyType.Idol || Eid.enemyType == EnemyType.Centaur)
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
            Assert.IsNotNull(Shared, $"For object by name {gameObject.name}");
         
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
                gameObject.AddComponent<DestroyOnCheckpointRestart>();
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
            
            TryDecrementInstanceCount();
            if (!CanDuplicate && Shared.InstanceCount == 0)
            {
                Eid.puppet = false;

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
                    default:
                        break;
                }
                //dupeInfo.Position += (dupeInfo.Rotation * Vector3.Normalize(Vector3.Lerp(Vector3.right, Vector3.forward, UnityEngine.Random.Range(0.0f, 1.0f))))
                //                     * (isB ? -1.0f : 1.0f);
                dupeInfo.Position += Vector3.Project(dupeInfo.SharedData.Bounds.size, dupeInfo.Rotation * Vector3.right) * (isB ? -1.0f : 1.0f) * 0.3f * additionalOffsetScalar;
            }

            Hydra.EnqueueDupe(dupeInfo);
            Shared.InstanceCount += 1;
        }

        public void InitializeAsNew()
        {
            Shared = ScriptableObject.CreateInstance<SharedData>();
            Shared.InstanceCount += 1;
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
            

            ContributesToInstanceCount = true;
            Depth = 0;
            Shared.CreatorName = gameObject.name;
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
            TryDecrementInstanceCount();
        }
    }
}