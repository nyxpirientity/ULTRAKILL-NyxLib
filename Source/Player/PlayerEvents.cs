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

        public delegate void PredictedDeathEventHandler(EventMethodCanceler canceler, PlayerComponents player, int damage);
        public static event PredictedDeathEventHandler PredictedDeath;
        public delegate void PreHurtEventHandler(EventMethodCanceler canceler, PlayerComponents player, int unprocessedDamage, int processedDamage, bool invincible, float scoreLossMultiplier, bool explosion, bool instablack, float hardDamageMultiplier, bool ignoreInvincibility);
        public static event PreHurtEventHandler PreHurt;
        public delegate void PostHurtEventHandler(EventMethodCancelInfo cancelInfo, PlayerComponents player, int unprocessedDamage, int processedDamage, bool invincible, float scoreLossMultiplier, bool explosion, bool instablack, float hardDamageMultiplier, bool ignoreInvincibility);
        public static event PostHurtEventHandler PostHurt;


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
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(NewMovement __instance, int damage, bool invincible, float scoreLossMultiplier = 1f, bool explosion = false, bool instablack = false, float hardDamageMultiplier = 0.35f, bool ignoreInvincibility = false)
            {
                _cancellationTracker.Reset();
                NewMovement newMovement = __instance;
                if (newMovement.dead || newMovement.levelOver || !(!invincible || newMovement.gameObject.layer != 15 || ignoreInvincibility) || damage <= 0)
                {
                    return true;
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
                PlayerComponents player = newMovement.GetComponent<PlayerComponents>();
                PlayerEvents.PreHurt?.Invoke(_cancellationTracker.GetCanceler(), player, damage, ProcessedDamage, invincible, scoreLossMultiplier, explosion, instablack, hardDamageMultiplier, ignoreInvincibility);

                WasPreHurtCalled = true;

                bool mortal = !Invincibility.Enabled && !Cheats.IsCheatEnabled(Cheats.Immortality); 

                if (newMovement.hp - ProcessedDamage <= 0 && mortal)
                {
                    PlayerEvents.PredictedDeath?.Invoke(_cancellationTracker.GetCanceler(), player, ProcessedDamage);
                }
                
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }

            public static void Postfix(NewMovement __instance, int damage, bool invincible, float scoreLossMultiplier = 1f, bool explosion = false, bool instablack = false, float hardDamageMultiplier = 0.35f, bool ignoreInvincibility = false)
            {
                NewMovement newMovement = __instance;
                
                if (!WasPreHurtCalled)
                {
                    return;
                }

                PlayerEvents.PostHurt?.Invoke(_cancellationTracker.GetCancelInfo(), newMovement.GetComponent<PlayerComponents>(), damage, ProcessedDamage, invincible, scoreLossMultiplier, explosion, instablack, hardDamageMultiplier, ignoreInvincibility);
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