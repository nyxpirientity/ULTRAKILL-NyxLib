using System;
using MelonLoader;
using SettingsMenu.Components.Pages;
using TMPro;
using UKAIW.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.AI;

namespace UKAIW
{
    public class HeckPuppet : EnemyModifier
    {
        public GameObject LeaderGo = null;
        public HeckPuppetLeader Leader = null;
        public EnemyIdentifier Eid { get; private set; } = null;
        public bool GivePoints { get; set; } = true;
        public ulong HeckPuppetID = 0;
        public EnemyRadiance.Modifier RadianceMod = null;

        public EnemyGameplayRank GameplayRank;
        public StyleRanks StyleRank;
        FieldPublisher<EnemyIdentifier, float> EidPuppetSpawnTimer = null;

        protected void Awake()
        {
            Eid = GetComponent<EnemyIdentifier>();
            Eid.dontCountAsKills = true;
        }

        protected void Start()
        {
            if (!GetComponent<DestroyOnCheckpointRestart>())
            {
                gameObject.AddComponent<DestroyOnCheckpointRestart>();
            }

            Eid.checkingSpawnStatus = false;
            GivePoints = true;
            
            if (Eid.machine != null)
            {
                Eid.machine.onDeath = new UnityEngine.Events.UnityEvent();
                Eid.machine.destroyOnDeath = new GameObject[0];
            }
            if (Eid.drone != null)
            {
                FieldPublisher<Drone, bool> exploded = new FieldPublisher<Drone, bool>(Eid.drone, "exploded");
                exploded.Value = true;
            }

            var stray = Eid.GetComponent<ZombieProjectiles>();
            if (stray != null)
            {
                stray.wanderer = false;
                stray.afraid = false;
                stray.chaser = false;
            }
            
            var sisypheanInsurrectionist = Eid.GetComponent<Sisyphus>();
            if (sisypheanInsurrectionist != null)
            {
                RadianceMod.SpeedEnabled = false; // disable speed for Sisyphean Insurrectionists because their ring persists for ages, for some reason. Looks like a bug when it happens even though it isn't, and isn't fun at all.
            }
            
            if (Leader == null)
            {
                TryDestroy();
                return;
            }
            LeaderGo = Leader.gameObject;

            Eid.onDeath.AddListener(() =>
            {
                MaybeDeathDestroy();
            });
        }

        private int NumUpdates = 0;
        protected void Update()
        {
            if (NumUpdates == 1 && !Eid.Dead)
            {
                Eid.PuppetSpawn();
                EidPuppetSpawnTimer = new FieldPublisher<EnemyIdentifier, float>(Eid, "puppetSpawnTimer");
                EidPuppetSpawnTimer.Value = 1f - 0.001f; // should instantly finish the puppet spawn animation once EID update is called
            }
            
            NumUpdates += 1;

            if (Eid.machine != null && Eid.enemyType != EnemyType.Centaur)
            {
                if (Eid.GetComponent<NavMeshAgent>() == null || Eid.GetComponent<Animator>() == null)
                {
                    UnityEngine.Object.Destroy(GetComponent<EnemyAdditions>().RootGameObject);
                }
            }
        }

        bool HasDecrementedBlood = false;
        public void TryDecrementRemainingBlood()
        {
            if (HasDecrementedBlood)
            {
                return;
            }

            HasDecrementedBlood = true;
            BloodOptimizer.DecrementRemainingBloodFxThisTick();
        }

        internal bool PrevDead = false;
        protected void FixedUpdate()
        {
            if (Leader == null)
            {
                GivePoints = false;
                InstaKill();
                return;
            }

            if (LeaderGo == null)
            {
                GivePoints = false;
                InstaKill();
                return;
            }

            if (!Leader.isActiveAndEnabled)
            {
                GivePoints = false;
                InstaKill();
                return;
            }

            if (Leader.Eid.Dead)
            {
                GivePoints = false;
                InstaKill();
            }

            if (Cheats.IsCheatDisabled(Cheats.HeckPuppets))
            {
                GivePoints = false;
                InstaKill();
            }
            
            if (!PrevDead && Eid.Dead)
            {
                Leader.NotifyPuppetDeath(this, HeckPuppetID);

                TryGivePoints();
            }
            
            Eid.BossBar(false);

            PrevDead = Eid.Dead;
        }

        public void InstaKill()
        {
            if ((Eid.NullInvalid()?.Dead).GetValueOrDefault(true))
            {
                return;
            }

            if (Eid.enemyType == EnemyType.Virtue)
            {
                FieldPublisher<Drone, bool> exploded = new FieldPublisher<Drone, bool>(Eid.drone, "exploded");
                exploded.Value = true;
            }
            
            Eid.InstaKill();
            TryDecrementRemainingBlood();
            
            MaybeDeathDestroy();
        }

        private void MaybeDeathDestroy()
        {
            if (Eid.enemyType == EnemyType.Swordsmachine || Eid.enemyType == EnemyType.Streetcleaner)
            {
                TryDestroy();
            }
        }

        private bool TriedDestroy = false;
        private void TryDestroy()
        {
            if (TriedDestroy)
            {
                return;
            }

            Destroy(GetComponent<EnemyAdditions>().RootGameObject);
            TriedDestroy = true;
        }

        private void TryGivePoints()
        {
            if (EidPuppetSpawnTimer != null && Eid != null)
            {
                EidPuppetSpawnTimer.Value = 1.0f;
            }

            if (GivePoints)
            {
                switch (GameplayRank)
                {
                    case EnemyGameplayRank.Normal:
                        HeckPuppetObserver.Instance.IncrementNormalPuppetsCombo();
                        break;
                    case EnemyGameplayRank.Miniboss:
                        HeckPuppetObserver.Instance.IncrementMiniBossPuppetsCombo();
                        break;
                    case EnemyGameplayRank.Boss:
                        HeckPuppetObserver.Instance.IncrementBossPuppetsCombo();
                        break;
                    case EnemyGameplayRank.Ultraboss:
                        HeckPuppetObserver.Instance.IncrementUltraBossPuppetsCombo();
                        break;
                }
            }
        }

        protected void OnDestroy()
        {
            if (!PrevDead)
            {
                Leader.NullInvalid()?.NotifyPuppetDeath(this, HeckPuppetID);
                TryGivePoints();
                PrevDead = true;
            }
        }
    }
}