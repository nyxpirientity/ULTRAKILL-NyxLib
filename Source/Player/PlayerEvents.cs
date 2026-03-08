using System;
using HarmonyLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class PlayerEvents
    {
        public static Action<NewMovement> PreUpdate = null;
        public static Action<NewMovement> PostUpdate = null;
        public static Action<NewMovement> PreFullStamina = null;
        public static Action<NewMovement> PostFullStamina = null;
        public static Action<NewMovement, int> PreDeath = null;
        // (NewMovement nm, int processedDamage, bool invincible, float scoreLossMultiplier, bool explosion, bool instablack, float hardDamageMultiplier, bool ignoreInvincibility)
        public static Action<NewMovement, int, bool, float, bool, bool, float, bool> PreHurt = null;
        // (NewMovement nm, int processedDamage, bool invincible, float scoreLossMultiplier, bool explosion, bool instablack, float hardDamageMultiplier, bool ignoreInvincibility)
        public static Action<NewMovement, int, bool, float, bool, bool, float, bool> PostHurt = null;


        [HarmonyPatch(typeof(NewMovement), "Awake", new Type[] { })]
        static class NewMovementAwakePatch
        {
            public static void Prefix(NewMovement __instance)
            {
            }

            public static void Postfix(NewMovement __instance)
            {
                __instance.gameObject.AddComponent<PlayerComponents>();
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
}