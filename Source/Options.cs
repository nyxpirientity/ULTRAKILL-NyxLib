using System;
using System.Collections.Generic;
using System.IO;
using BepInEx.Configuration;
using Nyxpiri.ULTRAKILL.NyxLib;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class ConfigFileManager : MonoBehaviour
    {
        public Action OnReload;

        public void Initialize(ConfigFile config)
        {
            Assert.IsNotNull(config);
            Config = config;
        }

        protected void Start()
        {
            Assert.IsNotNull(Config, "Config should not be null when ConfigFIleManager Start is called, please call Initialize with a valid ConfigFile");

            if (!File.Exists(Config.ConfigFilePath))
            {
                Config.Save();
            }
        }

        protected void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Config.Reload();
                OnReload?.Invoke();
            }
        }

        private ConfigFile Config = null;
    }

    public static class Options
    {
        internal static ConfigFile Config = null;

        static ConfigEntry<bool> IncludePerformanceLogsEntry = null;
        static ConfigEntry<bool> IncludeTraceExpectedLogsEntry = null;
        static ConfigEntry<bool> IncludeExpectedLogsEntry = null;
        static ConfigEntry<bool> IncludeLikelyLogsEntry = null;
        static ConfigEntry<bool> IncludeUnlikelyLogsEntry = null;
        static ConfigEntry<bool> IncludeUnexpectedLogsEntry = null;
        public static ConfigEntry<bool> ShowErrorNotification = null;
        public static ConfigEntry<bool> LogEnemyTypeOnStart = null;
        public static ConfigEntry<bool> DisableQuickLoad = null;
        
        static ConfigEntry<float> RadianceAllTierEntry = null;
        static ConfigEntry<float> RadianceAllSpeedTierEntry = null;
        static ConfigEntry<float> RadianceAllDamageTierEntry = null;
        static ConfigEntry<float> RadianceAllHealthTierEntry = null;

        public static ConfigEntry<int> BloodOptimizerCapNumUpdatesPerTick = null;
        public static ConfigEntry<int> BloodHeavyModdedEnemiesCapNumBloodPerTick = null;

        public static ConfigEntry<bool> EnableStreetCleanerDodgeFix = null;
        public static ConfigEntry<bool> EnableStreetCleanerDodgeFixOnlyWhenNeeded = null;
        public static ConfigEntry<float> StreetCleanerDodgeFixInterpRate = null;

        static public bool IncludePerformanceLogs { get => IncludePerformanceLogsEntry.Value; }
        static public bool IncludeTraceExpectedLogs { get => IncludeTraceExpectedLogsEntry.Value; }
        static public bool IncludeExpectedLogs { get => IncludeExpectedLogsEntry.Value; }
        static public bool IncludeLikelyLogs { get => IncludeLikelyLogsEntry.Value; }
        static public bool IncludeUnlikelyLogs { get => IncludeUnlikelyLogsEntry.Value; }
        static public bool IncludeUnexpectedLogs { get => IncludeUnexpectedLogsEntry.Value; }

        static public float RadianceAllTier { get => RadianceAllTierEntry.Value; }
        static public float RadianceAllSpeedTier { get => RadianceAllSpeedTierEntry.Value; }
        static public float RadianceAllDamageTier { get => RadianceAllDamageTierEntry.Value; }
        static public float RadianceAllHealthTier { get => RadianceAllHealthTierEntry.Value; }

        const string ModCat = "NyxLib";
        const string DebugCat = "Debug";
        const string DevCat = "Development";
        const string BugFixCat = "BugFixes";
        const string CheatsCat = "Cheats";
        const string RadianceAllCat = "RadianceAllCat";
        const string BasicBloodOptimizerCat = "BasicBloodOptimizer";

        public static void Initialize()
        {
            ShowErrorNotification = Config.Bind($"{ModCat}.{DebugCat}", "ShowErrorNotification", false, "Shows text saying An Error has Occured! At the top of your screen as a 'QuickMsg', with a timestamp (using your locally set timezone!)");
            IncludePerformanceLogsEntry = Config.Bind($"{ModCat}.{DebugCat}", "IncludePerformanceLogs", false);
            IncludeTraceExpectedLogsEntry = Config.Bind($"{ModCat}.{DebugCat}", "IncludeTraceExpectedLogs", false);
            IncludeExpectedLogsEntry = Config.Bind($"{ModCat}.{DebugCat}", "IncludeExpectedLogs", false);
            IncludeLikelyLogsEntry = Config.Bind($"{ModCat}.{DebugCat}", "IncludeLikelyLogs", false);
            IncludeUnlikelyLogsEntry = Config.Bind($"{ModCat}.{DebugCat}", "IncludeUnlikelyLogs", false);
            IncludeUnexpectedLogsEntry = Config.Bind($"{ModCat}.{DebugCat}", "IncludeUnexpectedLogs", false);
            LogEnemyTypeOnStart = Config.Bind($"{ModCat}.{DebugCat}.{DevCat}", "LogEnemyTypeOnEnemyStart", false);
            DisableQuickLoad = Config.Bind($"{ModCat}.{DebugCat}.{DevCat}", "DisableGameInitLevelQuickLoad", false);
            
            EnableStreetCleanerDodgeFix = Config.Bind($"{ModCat}.{BugFixCat}", "EnableStreetCleanerDodgeFix", true);
            EnableStreetCleanerDodgeFixOnlyWhenNeeded = Config.Bind($"{ModCat}.{BugFixCat}", "EnableStreetCleanerDodgeFixOnlyWhenNeeded", true);
            StreetCleanerDodgeFixInterpRate = Config.Bind($"{ModCat}.{BugFixCat}", "StreetCleanerDodgeFixInterpRate", 8.0f);
            
            RadianceAllTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{RadianceAllCat}", "RadianceAllTier", 1.0f);
            RadianceAllSpeedTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{RadianceAllCat}", "RadianceAllSpeedTier", 1.5f);
            RadianceAllDamageTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{RadianceAllCat}", "RadianceAllDamageTier", 1.5f);
            RadianceAllHealthTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{RadianceAllCat}", "RadianceAllHealthTier", 1.5f);

            BloodOptimizerCapNumUpdatesPerTick =  Config.Bind($"{ModCat}.{CheatsCat}.{BasicBloodOptimizerCat}", "BloodOptimizerCapNumUpdatesPerTick", 90);
            BloodHeavyModdedEnemiesCapNumBloodPerTick =  Config.Bind($"{ModCat}.{CheatsCat}.{BasicBloodOptimizerCat}", "BloodHeavyModdedEnemiesCapNumBloodPerTick", 2);
        }
    }
}