using System;
using System.Reflection;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using ULTRAKILL.Cheats;
using UKAIW.Diagnostics.Debug;

namespace UKAIW
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

        public static void Initialize()
        {
            Log.TraceExpectedInfo($"PlayerEvents.Initialize called!");
            ScenesEvents.OnSceneWasLoaded += OnSceneWasLoaded;
        }

        private static void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            var player = MonoSingleton<NewMovement>.Instance;
            
            if (player == null)
            {
                return;
            }

            player.gameObject.AddComponent<PlayerAdditions>();
        }
    }
}