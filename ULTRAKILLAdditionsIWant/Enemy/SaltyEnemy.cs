using System;
using MelonLoader;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

namespace UKAIW
{
    public class SaltyEnemy : ModoBehaviour
    {
        EnemyIdentifier Eid = null;
        bool RequestedBuffs = false;

        public override void ModoFixedUpdate()
        {
            int rankIndex = StyleHUD.Instance.rankIndex;
            StyleRanks rank = (StyleRanks)(rankIndex);
            var radienceTier = rank switch
            {
                StyleRanks.Destructive => Options.DestructiveRadianceTier,
                StyleRanks.Chaotic => Options.ChaoticRadianceTier,
                StyleRanks.Brutal => Options.BrutalRadianceTier,
                StyleRanks.Anarchic => Options.AnarchicRadianceTier,
                StyleRanks.Supreme => Options.SupremeRadianceTier,
                StyleRanks.SSadistic => Options.SSadisticRadianceTier,
                StyleRanks.SSSensoredStorm => Options.SSSensoredStormRadianceTier,
                StyleRanks.ULTRAKILL => Options.ULTRAKILLRadianceTier,
                _ => throw new Exception("A great sadness has struck the city."),
            };
            
            if (radienceTier <= 0.01f)
            {
                UnrequestBuffs();
                return;
            }
            
            RequestBuffs();

            Eid.radianceTier = radienceTier;
        }

        private void RequestBuffs()
        {
            if (RequestedBuffs)
            {
                return;
            }

            Eid.BuffAll();
            RequestedBuffs = true;
        }

        private void UnrequestBuffs()
        {
            if (!RequestedBuffs)
            {
                return;
            }

            Eid.UnbuffAll();
            RequestedBuffs = false;
        }

        public override void ModoLateUpdate()
        {
        }

        public override void ModoOnDestroy()
        {
        }

        public override void ModoOnDisable()
        {
        }

        public override void ModoOnEnable()
        {
        }

        public override void ModoUpdate()
        {
        }

        public override void OnClonedFrom(ModoBehaviour ClonedFrom)
        {
        }

        public override void OnModRemoved()
        {
        }

        protected override void ModoAwake()
        {
        }

        protected override void ModoStart()
        {
            Eid = ((EnemyAdditions)Mono).Eid;
        }
    }
}