using System;
using MelonLoader;
using SettingsMenu.Components.Pages;
using TMPro;
using UnityEngine;

namespace UKAIW
{
    public class HeckPuppet : MonoBehaviour
    {
        public GameObject LeaderGo = null;
        public HeckPuppetLeader Leader = null;
        public EnemyIdentifier Eid { get; private set; } = null;
        public bool GivePoints { get; private set; } = true;
        public ulong HeckPuppetID = 0;
        public Radiance.Modifier RadianceMod = null;

        public EnemyGameplayRank GameplayRank;
        public StyleRanks StyleRank;
        FieldPublisher<EnemyIdentifier, float> EidPuppetSpawnTimer = null;

        protected void Start()
        {
            if (!GetComponent<DestroyOnCheckpointRestart>())
            {
                gameObject.AddComponent<DestroyOnCheckpointRestart>();
            }

            Eid = GetComponent<EnemyIdentifier>();
            Eid.dontCountAsKills = true;
            Eid.PuppetSpawn();
            GivePoints = true;
            EidPuppetSpawnTimer = new FieldPublisher<EnemyIdentifier, float>(Eid, "puppetSpawnTimer");
            Eid.onDeath = null;
            
            if (Eid.machine != null)
            {
                Eid.machine.onDeath = null;
                Eid.machine.destroyOnDeath = new GameObject[0];
            }
            if (Eid.drone != null)
            {
                FieldPublisher<Drone, bool> exploded = new FieldPublisher<Drone, bool>(Eid.drone, "exploded");
                exploded.Value = true;
            }
            
            var sisypheanInsurrectionist = Eid.GetComponent<Sisyphus>();
            if (sisypheanInsurrectionist != null)
            {
                RadianceMod.SpeedEnabled = false; // disable speed for Sisyphean Insurrectionists because their ring persists for ages, for some reason. Looks like a bug when it happens even though it isn't, and isn't fun at all.
            }
            
            LeaderGo = Leader.gameObject;
        }

        protected void Update()
        {
            FieldPublisher<EnemyIdentifier, float> puppetSpawnTimer = new FieldPublisher<EnemyIdentifier, float>(Eid, "puppetSpawnTimer");
            puppetSpawnTimer.Value = Mathf.Max(puppetSpawnTimer.Value, 0.995f);
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
                InstaDestroy();
                return;
            }

            if (LeaderGo == null)
            {
                InstaDestroy();
                return;
            }

            if (!Leader.isActiveAndEnabled)
            {
                InstaDestroy();
                return;
            }

            if (Leader.Eid.Dead)
            {
                InstaKill();
                GivePoints = false;
            }

            if (Cheats.IsCheatDisabled(Cheats.HeckPuppets))
            {
                InstaKill();
                GivePoints = false;
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
                Leader.NotifyPuppetDeath(this, HeckPuppetID);
                TryGivePoints();
                PrevDead = true;
            }
        }

        bool InstaDestroyed = false;
        internal void InstaDestroy()
        {
            if (InstaDestroyed)
            {
                return;
            }
            
            InstaDestroyed = true;
            GivePoints = false;
            
            Assert.IsNotNull(gameObject);
            Assert.IsNotNull(GetComponent<EnemyAdditions>());
            Assert.IsNotNull(GetComponent<EnemyAdditions>().RootGameObject);

            Destroy(GetComponent<EnemyAdditions>().RootGameObject);
        }
    }
}