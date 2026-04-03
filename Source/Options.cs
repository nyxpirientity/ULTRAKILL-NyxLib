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

        public static ConfigEntry<bool> RegisterHideCheatsStatusCheat { get; private set; } = null;
        public static ConfigEntry<bool> RegisterSandAllEnemiesCheat { get; private set; } = null;
        public static ConfigEntry<bool> RegisterForceNextWaveCheat { get; private set; } = null;
        public static ConfigEntry<bool> ForcePlayCleanMusicWithBattleMusic { get; private set; } = null;

        const string DebugCat = "Debug";
        const string DevCat = "Development";
        const string ExtrasCat = "Extras";
        const string CheatsCat = "Cheats";
        const string RadianceAllCat = "RadianceAll";

        public static void Initialize()
        {
            ShowErrorNotification = Config.Bind($"{DebugCat}", "ShowErrorNotification", false, "Shows text saying An Error has Occured! At the top of your screen as a 'QuickMsg', with a timestamp (using your locally set timezone!)");
            IncludePerformanceLogsEntry = Config.Bind($"{DebugCat}", "IncludePerformanceLogs", false);
            IncludeTraceExpectedLogsEntry = Config.Bind($"{DebugCat}", "IncludeTraceExpectedLogs", false);
            IncludeExpectedLogsEntry = Config.Bind($"{DebugCat}", "IncludeExpectedLogs", false);
            IncludeLikelyLogsEntry = Config.Bind($"{DebugCat}", "IncludeLikelyLogs", false);
            IncludeUnlikelyLogsEntry = Config.Bind($"{DebugCat}", "IncludeUnlikelyLogs", true);
            IncludeUnexpectedLogsEntry = Config.Bind($"{DebugCat}", "IncludeUnexpectedLogs", true);
            LogEnemyTypeOnStart = Config.Bind($"{DebugCat}.{DevCat}", "LogEnemyTypeOnEnemyStart", false);
            DisableQuickLoad = Config.Bind($"{DebugCat}.{DevCat}", "DisableGameInitLevelQuickLoad", false);
            
            RadianceAllTierEntry = Config.Bind($"{CheatsCat}.{RadianceAllCat}", "RadianceAllTier", 1.0f);
            RadianceAllSpeedTierEntry = Config.Bind($"{CheatsCat}.{RadianceAllCat}", "RadianceAllSpeedTier", 1.25f);
            RadianceAllDamageTierEntry = Config.Bind($"{CheatsCat}.{RadianceAllCat}", "RadianceAllDamageTier", 1.1f);
            RadianceAllHealthTierEntry = Config.Bind($"{CheatsCat}.{RadianceAllCat}", "RadianceAllHealthTier", 1.25f);

            RegisterHideCheatsStatusCheat = Config.Bind($"{CheatsCat}", "RegisterHideCheatsStatusCheat", false);
            RegisterSandAllEnemiesCheat = Config.Bind($"{CheatsCat}", "RegisterSandAllEnemiesCheat", true);
            RegisterForceNextWaveCheat = Config.Bind($"{CheatsCat}", "RegisterForceNextWaveCheat", true);
            ForcePlayCleanMusicWithBattleMusic = Config.Bind($"{ExtrasCat}", "ForcePlayCleanMusicWithBattleMusic", false);
        }
    }
}