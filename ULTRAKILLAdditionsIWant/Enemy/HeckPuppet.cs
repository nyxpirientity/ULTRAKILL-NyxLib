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

        protected void Start()
        {
            if (!GetComponent<DestroyOnCheckpointRestart>())
            {
                gameObject.AddComponent<DestroyOnCheckpointRestart>();
            }

            Eid = GetComponent<EnemyIdentifier>();
            Eid.dontCountAsKills = true;
            GivePoints = true;
        }

        internal bool PrevDead = false;
        protected void FixedUpdate()
        {
            Eid.puppet = true;
            
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

            PrevDead = Eid.Dead;
        }

        private void TryGivePoints()
        {
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