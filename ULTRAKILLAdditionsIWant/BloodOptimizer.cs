using System;
using MelonLoader;
using UnityEngine;
using HarmonyLib;
using MelonLoader.Utils;
using System.Reflection;
using System.Collections.Generic;
using UKAIW.Diagnostics.Debug;
using ULTRAKILL.Cheats;
using UnityEngine.UI;

namespace UKAIW
{
    public static class BloodOptimizer
    {
        public static void Initialize()
        {
            UpdateEvents.OnFixedUpdate += FixedUpdate;
        }

        private static int RemainingBloodFxThisTick = 0;
        private static bool BloodDisabledByUs = false;
        public static void DecrementRemainingBloodFxThisTick()
        {
            RemainingBloodFxThisTick -= 1;
            
            bool wasBloodEnabled = MonoSingleton<PrefsManager>.Instance.GetBoolLocal("bloodEnabled");

            if (RemainingBloodFxThisTick <= 0 && wasBloodEnabled)
            {
                MonoSingleton<PrefsManager>.Instance.SetBoolLocal("bloodEnabled", false);
                BloodDisabledByUs = true;
            }
        }

        public static int InstantiatedThisTick = 0;
        public static ulong NumFixedUpdates = 0;
        private static void FixedUpdate()
        {
            if (!Cheats.Enabled)
            {
                return;
            }

            if ((NumFixedUpdates % (ulong)Options.BloodOptimizerCapNumUpdatesPerTick.Value) == 0)
            {
                RemainingBloodFxThisTick = Options.BloodHeavyModdedEnemiesCapNumBloodPerTick.Value;
                
                if (BloodDisabledByUs)
                {
                    MonoSingleton<PrefsManager>.Instance.SetBoolLocal("bloodEnabled", true);
                    BloodDisabledByUs = false;
                }
            }
        }
    }
}