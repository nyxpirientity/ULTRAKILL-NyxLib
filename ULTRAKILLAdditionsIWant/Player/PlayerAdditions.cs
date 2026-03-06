using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using ULTRAKILL.Cheats;

namespace UKAIW
{
    public class PlayerAdditions : MonoBehaviour
    {
        NewMovement player = null;
        int Difficulty = 0;

        private void Start()
        {
            player = gameObject.GetComponent<NewMovement>();
            gameObject.AddComponent<DemandingHell>();
            gameObject.AddComponent<SelfConscience>();
            gameObject.AddComponent<HeckPuppetObserver>();
            gameObject.AddComponent<PlayerPain>();
            gameObject.AddComponent<BadGyro>();

            Difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty");
        }

        private void FixedUpdate()
        {
        }

        private int PrevStylePoints = MonoSingleton<StatsManager>.Instance.stylePoints;
        private float PrevAntiHpCooldown = 0.0f;
        private float PrevAntiHp = 0.0f;

        private void Update()
        {
            try
            {
                UnsafeUpdate();
            }
            catch (System.Exception e)
            {
                MelonLogger.Error(e.ToString());
            }
        }

        private void UnsafeUpdate()
        {
            var stats = MonoSingleton<StatsManager>.Instance;
            
            if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.WasPerformedThisFrame && player.activated && !player.slowMode && !GameStateManager.Instance.PlayerInputLocked)
            {
                if (Cheats.Manager.GetCheatState(Cheats.GiveSelfRadiance))
                {
                    player.boostCharge += 50.0f;
                }
            }

            if (Cheats.Manager.GetCheatState(Cheats.GiveSelfRadiance))
            {
                if (MonoSingleton<FistControl>.Instance.fistCooldown > -1f)
                {
                    MonoSingleton<WeaponCharges>.Instance.punchStamina = Mathf.MoveTowards(MonoSingleton<WeaponCharges>.Instance.punchStamina, 2f, Time.deltaTime * 0.625f);
                }
            }
            else
            {
            }

            FieldInfo antiHpCooldownFI = typeof(NewMovement).GetField("antiHpCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
            var antiHpCooldown = (float)antiHpCooldownFI.GetValue(player);

            if (Cheats.Manager.GetCheatState(Cheats.HardDamageRebalance)) // OH this is actually in the code still?? yeah this stuff is jank as heck and not balanced btw lol
            {
                float spDeltaF = stats.stylePoints - PrevStylePoints;
                
                if (spDeltaF > 0.0f)
                {
                    int rankIndex = MonoSingleton<StyleHUD>.Instance.rankIndex;
                    float antiHpHeal = Mathf.Pow(spDeltaF * 0.04f, 1.1f);
                    float rankMulti = Mathf.Pow((rankIndex + 1.0f) * 0.4f, 1.1f) + 1.0f;
                    antiHpHeal *= rankMulti;
                    player.antiHp -= Mathf.Max(player.antiHp, antiHpHeal) * 0.1f;
                }

                float antiHpDelta = player.antiHp - PrevAntiHp;
                float antiHpCooldownDelta = antiHpCooldown - PrevAntiHpCooldown;
                
                if (antiHpDelta <= 0.0f)
                {
                    player.antiHp += Mathf.Max(0.0f, antiHpDelta * -0.7f);
                }
                else
                {
                    player.antiHp += Mathf.Max(0.0f, antiHpDelta * ( 0.9f * ((Difficulty + 1.0f) / 6.5f)));
                }
                
                antiHpCooldown += Mathf.Max(0.0f, antiHpCooldownDelta * -0.65f);
            }
            
            antiHpCooldownFI.SetValue(player, antiHpCooldown);
            PrevAntiHpCooldown = antiHpCooldown;
            PrevAntiHp = player.antiHp;
            PrevStylePoints = stats.stylePoints;
        }
    }

    [HarmonyPatch(typeof(NewMovement), "GetHurt", new Type[] { typeof(int), typeof(bool), typeof(float), typeof(bool), typeof(bool), typeof(float), typeof(bool) })]
    static class PlayerHurtPatch
    {
        private static bool WasPreHurtCalled = false;
        private static int ProcessedDamage = 0;
        
        public static void Prefix(NewMovement __instance, int damage, bool invincible, float scoreLossMultiplier = 1f, bool explosion = false, bool instablack = false, float hardDamageMultiplier = 0.35f, bool ignoreInvincibility = false)
        {
            NewMovement newMovement = __instance;
            if (newMovement.dead || newMovement.levelOver || !(!invincible || newMovement.gameObject.layer != 15 || ignoreInvincibility) || damage <= 0)
            {
                return;
            }

            ProcessedDamage = damage;
            
            var assistController = AssistController.Instance;
            if (assistController.majorEnabled)
            {
                ProcessedDamage = Mathf.RoundToInt((float)ProcessedDamage * assistController.damageTaken);
            }
            
            if (Invincibility.Enabled)
            {
                ProcessedDamage = 0;
            }

            PlayerEvents.PreHurt?.Invoke(newMovement, ProcessedDamage, invincible, scoreLossMultiplier, explosion, instablack, hardDamageMultiplier, ignoreInvincibility);

            WasPreHurtCalled = true;

            bool mortal = !Invincibility.Enabled && !Cheats.IsCheatEnabled(Cheats.Immortality); 

            if (newMovement.hp - ProcessedDamage <= 0 && mortal)
            {
                PlayerEvents.PreDeath?.Invoke(newMovement, ProcessedDamage);
            }
        }

        public static void Postfix(NewMovement __instance, int damage, bool invincible, float scoreLossMultiplier = 1f, bool explosion = false, bool instablack = false, float hardDamageMultiplier = 0.35f, bool ignoreInvincibility = false)
        {
            NewMovement newMovement = __instance;
            
            if (!WasPreHurtCalled)
            {
                return;
            }

            PlayerEvents.PostHurt?.Invoke(newMovement, ProcessedDamage, invincible, scoreLossMultiplier, explosion, instablack, hardDamageMultiplier, ignoreInvincibility);
        }
    }

    [HarmonyPatch(typeof(NewMovement), "FullStamina")]
    static class PlayerFullStaminaPatch
    {
        public static void Prefix(NewMovement __instance)
        {
            PlayerEvents.PreFullStamina?.Invoke(__instance);
        }

        public static void Postfix(NewMovement __instance)
        {
            PlayerEvents.PostFullStamina?.Invoke(__instance);
        }
    }

    [HarmonyPatch(typeof(NewMovement), "Update")]
    static class PlayerUpdatePatch
    {
        public static void Prefix(NewMovement __instance)
        {
            PlayerEvents.PreUpdate?.Invoke(__instance);
        }

        public static void Postfix(NewMovement __instance)
        {
            PlayerEvents.PostUpdate?.Invoke(__instance);
        }
    }
}