using System;
using MelonLoader;
using SettingsMenu.Components.Pages;
using TMPro;
using UnityEngine;

namespace UKAIW
{
    public class SelfConscience : MonoBehaviour
    {
        const float BaseDashCost = 100.0f;
        float DashCostScale = 1.0f;
        StyleHUD Shud = null;

        protected void Start()
        {
            Shud = StyleHUD.Instance;
            Player.PreUpdate += PrePlayerUpdate;
        }

        protected void OnDestroy()
        {
            Player.PreUpdate -= PrePlayerUpdate;
        }

        private void PrePlayerUpdate(NewMovement player)
        {
            if (!Cheats.IsCheatEnabled(Cheats.SelfConscience))
            {
                DashCostScale = BaseDashCost;
                return;
            }

            if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.WasPerformedThisFrame && player.activated && !player.slowMode && !GameStateManager.Instance.PlayerInputLocked)
            {
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

            switch ((StyleRanks)Shud.rankIndex)
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
                case StyleRanks.Null:
                case StyleRanks.ULTRAKILL:
                dashCostTarget *= Options.SelfConscienceULTRAKILLDashCostScale.Value;
                    break;
            }

            DashCostScale = Mathf.MoveTowards(DashCostScale, dashCostTarget, Time.fixedDeltaTime * (dashCostTarget > BaseDashCost ? Options.SelfConscienseDashCostIncreaseInterpRate.Value : Options.SelfConscienseDashCostDecreaseInterpRate.Value));
        }

        protected void LateUpdate()
        {

        }
    }
}