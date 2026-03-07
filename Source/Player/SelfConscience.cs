using System;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class SelfConscience : MonoBehaviour
    {
        const float BaseDashCost = 100.0f;
        float DashCostScale = 1.0f;
        StyleHUD Shud = null;

        protected void Start()
        {
            Shud = StyleHUD.Instance;
            PlayerEvents.PreUpdate += PrePlayerUpdate;
        }

        protected void OnDestroy()
        {
            PlayerEvents.PreUpdate -= PrePlayerUpdate;
        }

        private void PrePlayerUpdate(NewMovement player)
        {
            if (!Cheats.IsCheatEnabled(Cheats.SelfConscience))
            {
                DashCostScale = 1.0f;
                return;
            }

            if (!player.slowMode && MonoSingleton<InputManager>.Instance.InputSource.Dodge.WasPerformedThisFrame)
            {
                if (((bool)player.groundProperties && !player.groundProperties.canDash) || player.modNoDashSlide)
                {
                    return;
                }

                if (player.boostCharge >= BaseDashCost * DashCostScale && !((player.groundProperties && !player.groundProperties.canDash) || player.modNoDashSlide))
                {
                    player.boostCharge += BaseDashCost - BaseDashCost * DashCostScale;
                }
            }
        }

        protected void FixedUpdate()
        {
            if (!Cheats.IsCheatEnabled(Cheats.SelfConscience))
            {
                DashCostScale = 1.0f;
                return;
            }

            float dashCostTarget = 1.0f;

            switch (Shud.GetStyleRank())
            {
                case StyleRanks.Destructive:
                dashCostTarget *= Options.SelfConscienceDestructiveDashCostScale.Value;
                    break;
                case StyleRanks.Chaotic:
                dashCostTarget *= Options.SelfConscienceChaoticDashCostScale.Value;
                    break;
                case StyleRanks.Brutal:
                dashCostTarget *= Options.SelfConscienceBrutalDashCostScale.Value;
                    break;
                case StyleRanks.Anarchic:
                dashCostTarget *= Options.SelfConscienceAnarchicDashCostScale.Value;
                    break;
                case StyleRanks.Supreme:
                dashCostTarget *= Options.SelfConscienceSupremeDashCostScale.Value;
                    break;
                case StyleRanks.SSadistic:
                dashCostTarget *= Options.SelfConscienceSSadisticDashCostScale.Value;
                    break;
                case StyleRanks.SSSensoredStorm:
                dashCostTarget *= Options.SelfConscienceSSSensoredStormDashCostScale.Value;
                    break;
                case StyleRanks.ULTRAKILL:
                dashCostTarget *= Options.SelfConscienceULTRAKILLDashCostScale.Value;
                    break;
                case StyleRanks.Null:
                    throw new NotImplementedException();
            }

            DashCostScale = Mathf.MoveTowards(DashCostScale, dashCostTarget, Time.fixedDeltaTime * (dashCostTarget > BaseDashCost ? Options.SelfConscienseDashCostIncreaseInterpRate.Value : Options.SelfConscienseDashCostDecreaseInterpRate.Value));
        }

        protected void LateUpdate()
        {

        }
    }
}