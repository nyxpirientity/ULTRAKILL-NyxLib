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
            UpdateEvents.OnUpdate += Update;
            LastRefreshTimeStamp.UpdateToNow();
        }

        private static int RemainingBloodFxThisTick = 0;
        private static bool BloodDisabledByUs = false;
        public static void DecrementRemainingBloodFxThisTick()
        {
            RemainingBloodFxThisTick -= 1;
            
            bool wasBloodEnabled = GamePrefs.Manager.GetBoolLocal("bloodEnabled");

            if (RemainingBloodFxThisTick <= 0 && wasBloodEnabled)
            {
                GamePrefs.Manager.SetBoolLocal("bloodEnabled", false);
                BloodDisabledByUs = true;
            }
        }

        public static int InstantiatedThisTick = 0;
        public static GlobalTimeStamp LastRefreshTimeStamp; 
        private static void Update()
        {
            if (!Cheats.Enabled)
            {
                return;
            }

            if ((LastRefreshTimeStamp.TimeSince > (Time.fixedDeltaTime * Options.BloodOptimizerCapNumUpdatesPerTick.Value)))
            {
                RemainingBloodFxThisTick = Options.BloodHeavyModdedEnemiesCapNumBloodPerTick.Value;
                LastRefreshTimeStamp.UpdateToNow();
                if (BloodDisabledByUs)
                {
                    GamePrefs.Manager.SetBoolLocal("bloodEnabled", true);
                    BloodDisabledByUs = false;
                }
            }
        }
    }
}