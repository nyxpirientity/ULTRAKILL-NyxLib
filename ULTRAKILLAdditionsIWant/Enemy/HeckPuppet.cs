using System;
using MelonLoader;
using SettingsMenu.Components.Pages;
using TMPro;
using UnityEngine;

namespace UKAIW
{
    public class HeckPuppet : MonoBehaviour
    {
        public EnemyAdditions Leader = null;
        public EnemyIdentifier Eid { get; private set; } = null;
        public EnemyGameplayRank GameplayRank;
        public StyleRanks StyleRank;

        protected void Start()
        {
            Eid = GetComponent<EnemyIdentifier>();
            Eid.dontCountAsKills = true;
            Eid.puppet = true;
        }

        private bool PrevDead = false;
        protected void FixedUpdate()
        {
            if (Leader == null)
            {
                Eid.InstaKill();
            }

            if (Leader.Eid.Dead)
            {
                Eid.InstaKill();
            }

            if (!PrevDead && Eid.Dead)
            {
                
            }

            PrevDead = Eid.Dead;
        }
    }
}