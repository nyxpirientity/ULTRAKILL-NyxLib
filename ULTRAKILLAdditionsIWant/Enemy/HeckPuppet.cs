using System;
using MelonLoader;
using SettingsMenu.Components.Pages;
using TMPro;
using UnityEngine;

namespace UKAIW
{
    public class HeckPuppet : MonoBehaviour
    {
        public HeckPuppetLeader Leader = null;
        public EnemyIdentifier Eid { get; private set; } = null;
        public bool GivePoints { get; private set; } = true;
        public ulong HeckPuppetID = 0;

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
            Eid.puppet = true;
            GivePoints = true;
            EidPuppetSpawnTimer = new FieldPublisher<EnemyIdentifier, float>(Eid, "puppetSpawnTimer");
        }

        protected void Update()
        {
            FieldPublisher<EnemyIdentifier, float> puppetSpawnTimer = new FieldPublisher<EnemyIdentifier, float>(Eid, "puppetSpawnTimer");
            puppetSpawnTimer.Value = Mathf.Max(puppetSpawnTimer.Value, 0.995f);
        }

        internal bool PrevDead = false;
        protected void FixedUpdate()
        {
            if (Leader == null)
            {
                Eid.InstaKill();
                GivePoints = false;
            }

            if (Leader.Eid.Dead)
            {
                Eid.InstaKill();
                GivePoints = false;
            }

            if (Cheats.IsCheatDisabled(Cheats.HeckPuppets))
            {
                Eid.InstaKill();
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

        private void TryGivePoints()
        {
            EidPuppetSpawnTimer.Value = 1.0f;

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
    }
}