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
    public class EnemyHydraMod : ModoBehaviour
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
                Log.TraceExpectedInfo($"EnemyHydraMod.SharedData '{name}' with prefab '{Prefab}' destroyed!");
                Hydra.SharedDatas.Remove(this);
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
            internal string CreatorName = "";
        }

        public bool CanDuplicate 
        { 
            get
            {
                return Shared.InstanceCount < Options.HydraMaxFromOne && NoDupeTime < 0.0f;
            }
        }

        public SharedData Shared = null;
        public int Depth = -1;

        public EnemyIdentifier Eid = null;

        public EnemyGameplayRank GameplayRank = EnemyGameplayRank.Ultraboss;
        public bool ContributesToInstanceCount = false;

        private bool ExcludedFromHydraCheat = false;

        private float NoDupeTime = 0.0f;
        public override void ModoFixedUpdate()
        {

        }

        public override void ModoLateUpdate()
        {
        }

        public override void ModoOnDestroy()
        {
            if (Depth > 0)
            {
                Player.PreDeath -= DestroySelf;
            }
            
            TryDecrementInstanceCount();
        }

        private void TryDecrementInstanceCount()
        {
            if (ExcludedFromHydraCheat)
            {
                return;
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

        public override void ModoOnDisable()
        {
        }

        public override void ModoOnEnable()
        {
        }

        public override void OnModRemoved()
        {
        }

        public override void ModoUpdate()
        {
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

        protected override void ModoAwake()
        {
        }

        protected override void ModoStart()
        {   
            Eid = Mono.GetComponent<EnemyIdentifier>();

            if (Eid.enemyType == EnemyType.Idol)
            {
                ExcludedFromHydraCheat = true;
                return;
            }

            Assert.IsNotNull(Eid, $"For object by name {Mono.gameObject.name}");

            if (Eid.dead)
            {
                return;
            }

            Assert.IsTrue(Depth >= 0, $"For object by name {Mono.gameObject.name}");
            Assert.IsNotNull(Shared, $"For object by name {Mono.gameObject.name}");
         
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
                Player.PreDeath += DestroySelf;
            }
        }

        private void DestroySelf(NewMovement movement, int damage)
        {
            Destroy(GameObject);
        }

        public void OnDeath()
        {
            if (ExcludedFromHydraCheat)
            {
                return;
            }

            if (Eid == null)
            {
                Eid = Mono.GetComponent<EnemyIdentifier>();
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

            if (Cheats.IsHydraModeOn)
            {
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
                }
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

            Assert.IsNotNull(Mono, $"For object by name {Mono.gameObject.name}");
            Assert.IsTrue(Mono is EnemyAdditions, $"For object by name {Mono.gameObject.name}");
            Assert.IsNotNull(((EnemyAdditions)Mono).PrefabMod, $"For object by name {Mono.gameObject.name}");
            Assert.IsNotNull(((EnemyAdditions)Mono).PrefabMod.Prefab, $"For object by name {Mono.gameObject.name}");

            Hydra.QueuedDupeInfo dupeInfo = new Hydra.QueuedDupeInfo();
            
            if (Eid.enemyType == EnemyType.Drone)
            {
                dupeInfo.Position = Eid.drone.GetComponent<Rigidbody>().transform.position;
            }
            else
            {
                dupeInfo.Position = Transform.position;
            }
            
            dupeInfo.Rotation = Transform.rotation;
            dupeInfo.LocalScale = Transform.localScale;
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
                    default:
                        break;
                }
                //dupeInfo.Position += (dupeInfo.Rotation * Vector3.Normalize(Vector3.Lerp(Vector3.right, Vector3.forward, UnityEngine.Random.Range(0.0f, 1.0f))))
                //                     * (isB ? -1.0f : 1.0f);
                dupeInfo.Position += Vector3.Project(dupeInfo.SharedData.Bounds.size, dupeInfo.Rotation * Vector3.right) * (isB ? -1.0f : 1.0f) * 0.3f * additionalOffsetScalar;
            }


            //Hydra.EnqueueDupe(dupeInfo);
            Hydra.EnqueueDupe(dupeInfo);
            Shared.InstanceCount += 1;
        }

        public void InitializeAsNew()
        {
            Shared = ScriptableObject.CreateInstance<SharedData>();
            Shared.InstanceCount += 1;
            Shared.Bounds = EnemyUtils.SolveEnemyBounds(GameObject);
            ContributesToInstanceCount = true;
            Depth = 0;
            Shared.CreatorName = Mono.gameObject.name;
        }

        public void PassPrefabToShared()
        {
            if (Shared.Prefab == null)
            {
                Shared.Prefab = ((EnemyAdditions)Mono).PrefabMod.Prefab;
            }
        }

        public override void OnClonedFrom(ModoBehaviour ClonedFrom)
        {
        }

        internal void DuringDeath()
        {
            TryDecrementInstanceCount();
        }
    }
}