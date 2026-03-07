using System.Collections.Generic;
using BepInEx.Configuration;
using Nyxpiri.ULTRAKILL.NyxLib;

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

    static ConfigEntry<float> HydraHealthDecayScaleEntry = null;
    static ConfigEntry<float> HydraDefaultWaitTimeEntry = null;
    static ConfigEntry<float> HydraMiniBossWaitTimeEntry = null;
    static ConfigEntry<float> HydraBossWaitTimeEntry = null;
    static ConfigEntry<float> HydraUltraBossWaitTimeEntry = null;
    static ConfigEntry<int> HydraMaxDepthEntry = null;
    static ConfigEntry<int> HydraMaxFromOneBossEntry = null;
    static ConfigEntry<int> HydraMaxFromOneEntry = null;
    static ConfigEntry<int> HydraMaxPerUpdateEntry = null;
    static ConfigEntry<int> HydraPrefabPoolCapacityEntry = null;
    static ConfigEntry<int> HydraPrefabPoolGrowPerUpdateEntry = null;
    static ConfigEntry<int> HydraKillBonusEntry = null;
    static ConfigEntry<int> HydraMiniBossKillBonusEntry = null;
    static ConfigEntry<int> HydraBossKillBonusEntry = null;
    static ConfigEntry<int> HydraUltraBossKillBonusEntry = null;

    static ConfigEntry<int> NumFriendsToSpawnEntry = null;

    static ConfigEntry<float> BloodFuelEnemiesHealScalarEntry = null;
    static ConfigEntry<float> BloodFuelEnemiesDistDivisorEntry = null;

    static ConfigEntry<float> DestructiveRadianceTierEntry = null;
    static ConfigEntry<float> ChaoticRadianceTierEntry = null;
    static ConfigEntry<float> BrutalRadianceTierEntry = null;
    static ConfigEntry<float> AnarchicRadianceTierEntry = null;
    static ConfigEntry<float> SupremeRadianceTierEntry = null;
    static ConfigEntry<float> SSadisticRadianceTierEntry = null;
    static ConfigEntry<float> SSSensoredStormRadianceTierEntry = null;
    static ConfigEntry<float> ULTRAKILLNoEnrageRadianceTierEntry = null;
    static ConfigEntry<float> ULTRAKILLRadianceTierEntry = null;
    static ConfigEntry<bool> SaltEffectHealthEntry = null;
    static ConfigEntry<bool> SaltEffectSpeedEntry = null;
    static ConfigEntry<bool> SaltEffectDamageEntry = null;
    
    static ConfigEntry<float> RadianceAllTierEntry = null;
    static ConfigEntry<float> RadianceAllSpeedTierEntry = null;
    static ConfigEntry<float> RadianceAllDamageTierEntry = null;
    static ConfigEntry<float> RadianceAllHealthTierEntry = null;

    public static ConfigEntry<bool> HitstopOnEnemyParry = null;


    public static ConfigEntry<float> DemandingHellDestructiveHeatResDrainEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellDestructiveHeatResRecoveryEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellDestructiveHeatResExplosiveSizeBase { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellDestructiveHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellDestructiveHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellDestructiveHeatResExplosiveDmgScalar { get; private set; } = null;
    public static ConfigEntry<bool> DemandingHellDestructiveHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static ConfigEntry<float> DemandingHellChaoticHeatResDrainEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellChaoticHeatResRecoveryEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellChaoticHeatResExplosiveSizeBase { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellChaoticHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellChaoticHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellChaoticHeatResExplosiveDmgScalar { get; private set; } = null;
    public static ConfigEntry<bool> DemandingHellChaoticHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static ConfigEntry<float> DemandingHellBrutalHeatResRecoveryEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellBrutalHeatResDrainEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellBrutalHeatResExplosiveSizeBase { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellBrutalHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellBrutalHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellBrutalHeatResExplosiveDmgScalar { get; private set; } = null;
    public static ConfigEntry<bool> DemandingHellBrutalHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static ConfigEntry<float> DemandingHellAnarchicHeatResDrainEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellAnarchicHeatResRecoveryEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellAnarchicHeatResExplosiveSizeBase { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellAnarchicHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellAnarchicHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellAnarchicHeatResExplosiveDmgScalar { get; private set; } = null;
    public static ConfigEntry<bool> DemandingHellAnarchicHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static ConfigEntry<float> DemandingHellSupremeHeatResDrainEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSupremeHeatResRecoveryEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSupremeHeatResExplosiveSizeBase { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSupremeHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSupremeHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSupremeHeatResExplosiveDmgScalar { get; private set; } = null;
    public static ConfigEntry<bool> DemandingHellSupremeHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static ConfigEntry<float> DemandingHellSSadisticHeatResDrainEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSSadisticHeatResRecoveryEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSSadisticHeatResExplosiveSizeBase { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSSadisticHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSSadisticHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSSadisticHeatResExplosiveDmgScalar { get; private set; } = null;
    public static ConfigEntry<bool> DemandingHellSSadisticHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static ConfigEntry<float> DemandingHellSSSensoredStormHeatResDrainEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSSSensoredStormHeatResRecoveryEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSSSensoredStormHeatResExplosiveSizeBase { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSSSensoredStormHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSSSensoredStormHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellSSSensoredStormHeatResExplosiveDmgScalar { get; private set; } = null;
    public static ConfigEntry<bool> DemandingHellSSSensoredStormHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static ConfigEntry<float> DemandingHellULTRAKILLHeatResDrainEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellULTRAKILLHeatResRecoveryEntry { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellULTRAKILLHeatResExplosiveSizeBase { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellULTRAKILLHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellULTRAKILLHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static ConfigEntry<float> DemandingHellULTRAKILLHeatResExplosiveDmgScalar { get; private set; } = null;
    public static ConfigEntry<bool> DemandingHellULTRAKILLHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static ConfigEntry<float> SelfConscienceDestructiveDashCostScale { get; private set; } = null;
    public static ConfigEntry<float> SelfConscienceChaoticDashCostScale { get; private set; } = null;
    public static ConfigEntry<float> SelfConscienceBrutalDashCostScale { get; private set; } = null;
    public static ConfigEntry<float> SelfConscienceAnarchicDashCostScale { get; private set; } = null;
    public static ConfigEntry<float> SelfConscienceSupremeDashCostScale { get; private set; } = null;
    public static ConfigEntry<float> SelfConscienceSSadisticDashCostScale { get; private set; } = null;
    public static ConfigEntry<float> SelfConscienceSSSensoredStormDashCostScale { get; private set; } = null;
    public static ConfigEntry<float> SelfConscienceULTRAKILLDashCostScale { get; private set; } = null;
    public static ConfigEntry<float> SelfConscienseDashCostIncreaseInterpRate { get; private set; } = null;
    public static ConfigEntry<float> SelfConscienseDashCostDecreaseInterpRate { get; private set; } = null;
    
    public class HeckPuppetStyleEntry
    {
        public class HeckPuppetOptions
        {
            public ConfigEntry<int> NumHeckPuppets;
            public ConfigEntry<float> HeckPuppetDamageBuffScalar;
            public ConfigEntry<float> HeckPuppetSpeedBuffScalar;
            public ConfigEntry<float> HeckPuppetHealthBuffScalar;
            public ConfigEntry<float> HeckPuppetHealthScalar;
            public ConfigEntry<int> MaxHeckPuppetHealth;
        }

        public Dictionary<EnemyGameplayRank, HeckPuppetOptions> HeckPuppetsOptions = new Dictionary<EnemyGameplayRank, HeckPuppetOptions>();
    }

    public static ConfigEntry<int> BloodOptimizerCapNumUpdatesPerTick = null;
    public static ConfigEntry<int> BloodHeavyModdedEnemiesCapNumBloodPerTick = null;

    public static ConfigEntry<bool> EnableStreetCleanerDodgeFix = null;
    public static ConfigEntry<bool> EnableStreetCleanerDodgeFixOnlyWhenNeeded = null;
    public static ConfigEntry<float> StreetCleanerDodgeFixInterpRate = null;

    public static Dictionary<StyleRanks, HeckPuppetStyleEntry> HeckPuppetsStyleEntries = new Dictionary<StyleRanks, HeckPuppetStyleEntry>();


    static public float HydraHealthDecayScale { get => HydraHealthDecayScaleEntry.Value; } 

    static public float HydraDefaultWaitTime { get => HydraDefaultWaitTimeEntry.Value; }
    static public float HydraMiniBossWaitTime { get => HydraMiniBossWaitTimeEntry.Value; }
    static public float HydraBossWaitTime { get => HydraBossWaitTimeEntry.Value; }
    static public float HydraUltraBossWaitTime { get => HydraUltraBossWaitTimeEntry.Value; }

    static public int HydraMaxDepth { get => HydraMaxDepthEntry.Value; }
    static public int HydraMaxFromOne { get => HydraMaxFromOneEntry.Value; }
    static public int HydraMaxFromOneBoss { get => HydraMaxFromOneBossEntry.Value; }
    static public int HydraMaxPerUpdate { get => HydraMaxPerUpdateEntry.Value; }
    static public int HydraPrefabPoolCapacity { get => HydraPrefabPoolCapacityEntry.Value; }
    static public int HydraPrefabPoolGrowPerUpdate { get => HydraPrefabPoolGrowPerUpdateEntry.Value; }
    static public int HydraKillBonus { get => HydraKillBonusEntry.Value; }
    static public int HydraMiniBossKillBonus { get => HydraMiniBossKillBonusEntry.Value; }
    static public int HydraBossKillBonus { get => HydraBossKillBonusEntry.Value; }
    static public int HydraUltraBossKillBonus { get => HydraUltraBossKillBonusEntry.Value; }

    static public bool IncludePerformanceLogs { get => IncludePerformanceLogsEntry.Value; }
    static public bool IncludeTraceExpectedLogs { get => IncludeTraceExpectedLogsEntry.Value; }
    static public bool IncludeExpectedLogs { get => IncludeExpectedLogsEntry.Value; }
    static public bool IncludeLikelyLogs { get => IncludeLikelyLogsEntry.Value; }
    static public bool IncludeUnlikelyLogs { get => IncludeUnlikelyLogsEntry.Value; }
    static public bool IncludeUnexpectedLogs { get => IncludeUnexpectedLogsEntry.Value; }

    static public int NumFriendsToSpawn { get => NumFriendsToSpawnEntry.Value; }

    static public float BloodFuelEnemiesHealScalar { get => BloodFuelEnemiesHealScalarEntry.Value; }
    static public float BloodFuelEnemiesDistDivisor { get => BloodFuelEnemiesDistDivisorEntry.Value; }

    static public float DestructiveRadianceTier { get => DestructiveRadianceTierEntry.Value; }
    static public float ChaoticRadianceTier { get => ChaoticRadianceTierEntry.Value; }
    static public float BrutalRadianceTier { get => BrutalRadianceTierEntry.Value; }
    static public float AnarchicRadianceTier { get => AnarchicRadianceTierEntry.Value; }
    static public float SupremeRadianceTier { get => SupremeRadianceTierEntry.Value; }
    static public float SSadisticRadianceTier { get => SSadisticRadianceTierEntry.Value; }
    static public float SSSensoredStormRadianceTier { get => SSSensoredStormRadianceTierEntry.Value; }
    static public float ULTRAKILLNoEnrageRadianceTier { get => ULTRAKILLNoEnrageRadianceTierEntry.Value; }
    static public float ULTRAKILLRadianceTier { get => ULTRAKILLRadianceTierEntry.Value; }

    static public bool SaltEffectSpeed { get => SaltEffectSpeedEntry.Value; }
    static public bool SaltEffectHealth { get => SaltEffectHealthEntry.Value; }
    static public bool SaltEffectDamage { get => SaltEffectDamageEntry.Value; }

    static public float RadianceAllTier { get => RadianceAllTierEntry.Value; }
    static public float RadianceAllSpeedTier { get => RadianceAllSpeedTierEntry.Value; }
    static public float RadianceAllDamageTier { get => RadianceAllDamageTierEntry.Value; }
    static public float RadianceAllHealthTier { get => RadianceAllHealthTierEntry.Value; }

    const string ModCat = "NyxLib";
    const string DebugCat = "Debug";
    const string DevCat = "Development";
    const string BugFixCat = "BugFixes";
    const string CheatsCat = "Cheats";
    const string HydraCat = "Hydra";
    const string FriendsCat = "EnemyFriends";
    const string BloodFueledEnemiesCat = "BloodFueledEnemies";
    const string SaltyEnemiesCat = "SaltyEnemies";
    const string RadianceAllCat = "RadianceAllCat";
    const string EnemyFeedbackersCat = "EnemyFeedbackers";
    const string HeatOfHeckCat = "HeatOfHeck";
    const string SelfConscienceCat = "SelfConscience";
    const string BasicBloodOptimizerCat = "BasicBloodOptimizer";
    const string HeckPuppetsCat = "HeckPuppets";

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

        HydraHealthDecayScaleEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraHealthDecayScale", 0.5f);

        HydraDefaultWaitTimeEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraDefaultWaitTime", 0.45f);
        HydraMiniBossWaitTimeEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraMiniBossWaitTime", 0.75f);
        HydraBossWaitTimeEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraBossWaitTime", 1.5f);
        HydraUltraBossWaitTimeEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraUltraBossWaitTime", 2.75f);

        HydraMaxFromOneBossEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraMaxFromOneBoss", 6);
        HydraMaxDepthEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraMaxDepth", 16);
        HydraMaxFromOneEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraMaxFromOne", 12);
        HydraMaxPerUpdateEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraMaxPerFrame", 4);

        HydraPrefabPoolCapacityEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraPrefabPoolCapacity", 20);
        HydraPrefabPoolGrowPerUpdateEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraPrefabPoolGrowPerUpdate", 3);

        HydraKillBonusEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraKillBonus", 10);
        HydraMiniBossKillBonusEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraMiniBossKillBonus", 50);
        HydraBossKillBonusEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraBossKillBonus", 100);
        HydraUltraBossKillBonusEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HydraCat}", "HydraUltraBossKillBonus", 1000);

        NumFriendsToSpawnEntry = Config.Bind($"{ModCat}.{CheatsCat}.{FriendsCat}", "NumFriendsToSpawn", 1);

        BloodFuelEnemiesHealScalarEntry = Config.Bind($"{ModCat}.{CheatsCat}.{BloodFueledEnemiesCat}", "BloodFuelEnemiesHealScalar", 0.4f);
        BloodFuelEnemiesDistDivisorEntry = Config.Bind($"{ModCat}.{CheatsCat}.{BloodFueledEnemiesCat}", "BloodFuelEnemiesDistDivisor", 8.0f);

        DestructiveRadianceTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{SaltyEnemiesCat}", "DestructiveRadianceTier", 0.0f);
        ChaoticRadianceTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{SaltyEnemiesCat}", "ChaoticRadianceTier", 0.0f);
        BrutalRadianceTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{SaltyEnemiesCat}", "BrutalRadianceTier", 1.0f);
        AnarchicRadianceTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{SaltyEnemiesCat}", "AnarchicRadianceTier", 1.1f);
        SupremeRadianceTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{SaltyEnemiesCat}", "SupremeRadianceTier", 1.25f);
        SSadisticRadianceTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{SaltyEnemiesCat}", "SSadisticRadianceTier", 1.4f);
        SSSensoredStormRadianceTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{SaltyEnemiesCat}", "SSSensoredStormRadianceTier", 1.6f);
        ULTRAKILLNoEnrageRadianceTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{SaltyEnemiesCat}", "ULTRAKILLNoEnrageRadianceTier", 2.0f);
        ULTRAKILLRadianceTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{SaltyEnemiesCat}", "ULTRAKILLRadianceTier", 1.8f);

        SaltEffectSpeedEntry = Config.Bind($"{ModCat}.{CheatsCat}.{SaltyEnemiesCat}", "SaltEffectSpeed", true);
        SaltEffectHealthEntry = Config.Bind($"{ModCat}.{CheatsCat}.{SaltyEnemiesCat}", "SaltEffectHealth", false);
        SaltEffectDamageEntry = Config.Bind($"{ModCat}.{CheatsCat}.{SaltyEnemiesCat}", "SaltEffectDamage", false);
        
        RadianceAllTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{RadianceAllCat}", "RadianceAllTier", 1.0f);
        RadianceAllSpeedTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{RadianceAllCat}", "RadianceAllSpeedTier", 1.5f);
        RadianceAllDamageTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{RadianceAllCat}", "RadianceAllDamageTier", 1.5f);
        RadianceAllHealthTierEntry = Config.Bind($"{ModCat}.{CheatsCat}.{RadianceAllCat}", "RadianceAllHealthTier", 1.5f);

        HitstopOnEnemyParry = Config.Bind($"{ModCat}.{CheatsCat}.{EnemyFeedbackersCat}", "HitstopOnEnemyParry", false);

        DemandingHellDestructiveHeatResDrainEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Destructive", "HeatResDrain", -1.0f);
        DemandingHellDestructiveHeatResRecoveryEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Destructive", "HeatResRecovery", 100.0f);
        DemandingHellDestructiveHeatResExplosiveSizeBase = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Destructive", "HeatResExplosiveSizeBase", -1.0f);
        DemandingHellDestructiveHeatResExplosiveSizeNormMin = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Destructive", "HeatResExplosiveSizeNormMin", -1.0f);
        DemandingHellDestructiveHeatResExplosiveSizeNormMax = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Destructive", "HeatResExplosiveSizeNormMax", -1.0f);
        DemandingHellDestructiveHeatResExplosiveDmgScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Destructive", "HeatResExplosiveDmgScalar", -1.0f);
        DemandingHellDestructiveHeatResExplosiveDmgPlayer = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Destructive", "HeatResExplosiveDmgPlayer", false);

        DemandingHellChaoticHeatResDrainEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Chaotic", "HeatResDrain", -1.0f);
        DemandingHellChaoticHeatResRecoveryEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Chaotic", "HeatResRecovery", 100.0f);
        DemandingHellChaoticHeatResExplosiveSizeBase = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Chaotic", "HeatResExplosiveSizeBase", -1.0f);
        DemandingHellChaoticHeatResExplosiveSizeNormMin = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Chaotic", "HeatResExplosiveSizeNormMin", -1.0f);
        DemandingHellChaoticHeatResExplosiveSizeNormMax = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Chaotic", "HeatResExplosiveSizeNormMax", -1.0f);
        DemandingHellChaoticHeatResExplosiveDmgScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Chaotic", "HeatResExplosiveDmgScalar", -1.0f);
        DemandingHellChaoticHeatResExplosiveDmgPlayer = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Chaotic", "HeatResExplosiveDmgPlayer", false);

        DemandingHellBrutalHeatResDrainEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Brutal", "HeatResDrain", -1.0f);
        DemandingHellBrutalHeatResRecoveryEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Brutal", "HeatResRecovery", 100.0f);
        DemandingHellBrutalHeatResExplosiveSizeBase = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Brutal", "HeatResExplosiveSizeBase", -1.0f);
        DemandingHellBrutalHeatResExplosiveSizeNormMin = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Brutal", "HeatResExplosiveSizeNormMin", -1.0f);
        DemandingHellBrutalHeatResExplosiveSizeNormMax = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Brutal", "HeatResExplosiveSizeNormMax", -1.0f);
        DemandingHellBrutalHeatResExplosiveDmgScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Brutal", "HeatResExplosiveDmgScalar", -1.0f);
        DemandingHellBrutalHeatResExplosiveDmgPlayer = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Brutal", "HeatResExplosiveDmgPlayer", false);

        DemandingHellAnarchicHeatResDrainEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Anarchic", "HeatResDrain", 30.0f);
        DemandingHellAnarchicHeatResRecoveryEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Anarchic", "HeatResRecovery", 1.7f);
        DemandingHellAnarchicHeatResExplosiveSizeBase = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Anarchic", "HeatResExplosiveSizeBase", -1.0f);
        DemandingHellAnarchicHeatResExplosiveSizeNormMin = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Anarchic", "HeatResExplosiveSizeNormMin", -1.0f);
        DemandingHellAnarchicHeatResExplosiveSizeNormMax = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Anarchic", "HeatResExplosiveSizeNormMax", -1.0f);
        DemandingHellAnarchicHeatResExplosiveDmgScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Anarchic", "HeatResExplosiveDmgScalar", -1.0f);
        DemandingHellAnarchicHeatResExplosiveDmgPlayer = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Anarchic", "HeatResExplosiveDmgPlayer", false);

        DemandingHellSupremeHeatResDrainEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Supreme", "HeatResDrain", 60.0f);
        DemandingHellSupremeHeatResRecoveryEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Supreme", "HeatResRecovery", 2.0f);
        DemandingHellSupremeHeatResExplosiveSizeBase = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Supreme", "HeatResExplosiveSizeBase", -1.0f);
        DemandingHellSupremeHeatResExplosiveSizeNormMin = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Supreme", "HeatResExplosiveSizeNormMin", -1.0f);
        DemandingHellSupremeHeatResExplosiveSizeNormMax = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Supreme", "HeatResExplosiveSizeNormMax", -1.0f);
        DemandingHellSupremeHeatResExplosiveDmgScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Supreme", "HeatResExplosiveDmgScalar", -1.0f);
        DemandingHellSupremeHeatResExplosiveDmgPlayer = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.Supreme", "HeatResExplosiveDmgPlayer", false);

        DemandingHellSSadisticHeatResDrainEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSadistic", "HeatResDrain", 70.0f);
        DemandingHellSSadisticHeatResRecoveryEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSadistic", "HeatResRecovery", 2.0f);
        DemandingHellSSadisticHeatResExplosiveSizeBase = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSadistic", "HeatResExplosiveSizeBase", 12.0f);
        DemandingHellSSadisticHeatResExplosiveSizeNormMin = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSadistic", "HeatResExplosiveSizeNormMin", 2.5f);
        DemandingHellSSadisticHeatResExplosiveSizeNormMax = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSadistic", "HeatResExplosiveSizeNormMax", 7.0f);
        DemandingHellSSadisticHeatResExplosiveDmgScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSadistic", "HeatResExplosiveDmgScalar", 0.35f);
        DemandingHellSSadisticHeatResExplosiveDmgPlayer = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSadistic", "HeatResExplosiveDmgPlayer", true);

        DemandingHellSSSensoredStormHeatResDrainEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSSensoredStorm", "HeatStormResDrain", 85.0f);
        DemandingHellSSSensoredStormHeatResRecoveryEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSSensoredStorm", "HeatStormResRecovery", 1.9f);
        DemandingHellSSSensoredStormHeatResExplosiveSizeBase = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSSensoredStorm", "HeatResExplosiveSizeBase", 14.0f);
        DemandingHellSSSensoredStormHeatResExplosiveSizeNormMin = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSSensoredStorm", "HeatResExplosiveSizeNormMin", 0.15f);
        DemandingHellSSSensoredStormHeatResExplosiveSizeNormMax = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSSensoredStorm", "HeatResExplosiveSizeNormMax", 8.5f);
        DemandingHellSSSensoredStormHeatResExplosiveDmgScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSSensoredStorm", "HeatResExplosiveDmgScalar", 0.5f);
        DemandingHellSSSensoredStormHeatResExplosiveDmgPlayer = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.SSSensoredStorm", "HeatResExplosiveDmgPlayer", true);

        DemandingHellULTRAKILLHeatResRecoveryEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.ULTRAKILL", "HeatResRecovery", 2.7f);
        DemandingHellULTRAKILLHeatResDrainEntry = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.ULTRAKILL", "HeatResDrain", 100.0f);
        DemandingHellULTRAKILLHeatResExplosiveSizeBase = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.ULTRAKILL", "HeatResExplosiveSizeBase", 20.0f);
        DemandingHellULTRAKILLHeatResExplosiveSizeNormMin = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.ULTRAKILL", "HeatResExplosiveSizeNormMin", 0.15f);
        DemandingHellULTRAKILLHeatResExplosiveSizeNormMax = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.ULTRAKILL", "HeatResExplosiveSizeNormMax", 6.5f);
        DemandingHellULTRAKILLHeatResExplosiveDmgScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.ULTRAKILL", "HeatResExplosiveDmgScalar", 0.8f);
        DemandingHellULTRAKILLHeatResExplosiveDmgPlayer = Config.Bind($"{ModCat}.{CheatsCat}.{HeatOfHeckCat}.ULTRAKILL", "HeatResExplosiveDmgPlayer", true);

        SelfConscienceDestructiveDashCostScale = Config.Bind($"{ModCat}.{CheatsCat}.{SelfConscienceCat}.Destructive", "DashCostScale", 1.35f);
        SelfConscienceChaoticDashCostScale = Config.Bind($"{ModCat}.{CheatsCat}.{SelfConscienceCat}.Chaotic", "DashCostScale", 1.1f);
        SelfConscienceBrutalDashCostScale = Config.Bind($"{ModCat}.{CheatsCat}.{SelfConscienceCat}.Brutal", "DashCostScale", 1.05f);
        SelfConscienceAnarchicDashCostScale = Config.Bind($"{ModCat}.{CheatsCat}.{SelfConscienceCat}.Anarchic", "DashCostScale", 1.0f);
        SelfConscienceSupremeDashCostScale = Config.Bind($"{ModCat}.{CheatsCat}.{SelfConscienceCat}.Supreme", "DashCostScale", 0.9f);
        SelfConscienceSSadisticDashCostScale = Config.Bind($"{ModCat}.{CheatsCat}.{SelfConscienceCat}.SSadistic", "DashCostScale", 0.85f);
        SelfConscienceSSSensoredStormDashCostScale = Config.Bind($"{ModCat}.{CheatsCat}.{SelfConscienceCat}.SSSensoredStorm", "DashCostScale", 0.825f);
        SelfConscienceULTRAKILLDashCostScale = Config.Bind($"{ModCat}.{CheatsCat}.{SelfConscienceCat}.ULTRAKILL", "DashCostScale", 0.825f);

        SelfConscienseDashCostIncreaseInterpRate = Config.Bind($"{ModCat}.{CheatsCat}.{SelfConscienceCat}", "DashCostIncreaseInterpRate", 0.3f);
        SelfConscienseDashCostDecreaseInterpRate = Config.Bind($"{ModCat}.{CheatsCat}.{SelfConscienceCat}", "DashCostIDecreaseInterpRate", 1.0f);

        BloodOptimizerCapNumUpdatesPerTick =  Config.Bind($"{ModCat}.{CheatsCat}.{BasicBloodOptimizerCat}", "BloodOptimizerCapNumUpdatesPerTick", 90);
        BloodHeavyModdedEnemiesCapNumBloodPerTick =  Config.Bind($"{ModCat}.{CheatsCat}.{BasicBloodOptimizerCat}", "BloodHeavyModdedEnemiesCapNumBloodPerTick", 2);

        HeckPuppetsStyleEntries = new Dictionary<StyleRanks, HeckPuppetStyleEntry>()
        {
            {
                StyleRanks.Destructive, new HeckPuppetStyleEntry()
                {
                    HeckPuppetsOptions = new Dictionary<EnemyGameplayRank, HeckPuppetStyleEntry.HeckPuppetOptions>
                    {
                        {
                            EnemyGameplayRank.Normal, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "DestructiveUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                    }
                }
            },
            {
                StyleRanks.Chaotic, new HeckPuppetStyleEntry()
                {
                    HeckPuppetsOptions = new Dictionary<EnemyGameplayRank, HeckPuppetStyleEntry.HeckPuppetOptions>
                    {
                        {
                            EnemyGameplayRank.Normal, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ChaoticUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                    }
                }
            },
            {
                StyleRanks.Brutal, new HeckPuppetStyleEntry()
                {
                    HeckPuppetsOptions = new Dictionary<EnemyGameplayRank, HeckPuppetStyleEntry.HeckPuppetOptions>
                    {
                        {
                            EnemyGameplayRank.Normal, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "BrutalUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                    }
                }
            },
            {
                StyleRanks.Anarchic, new HeckPuppetStyleEntry()
                {
                    HeckPuppetsOptions = new Dictionary<EnemyGameplayRank, HeckPuppetStyleEntry.HeckPuppetOptions>
                    {
                        {
                            EnemyGameplayRank.Normal, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "AnarchicUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                    }
                }
            },
            {
                StyleRanks.Supreme, new HeckPuppetStyleEntry()
                {
                    HeckPuppetsOptions = new Dictionary<EnemyGameplayRank, HeckPuppetStyleEntry.HeckPuppetOptions>
                    {
                        {
                            EnemyGameplayRank.Normal, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SupremeUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                    }
                }
            },
            {
                StyleRanks.SSadistic, new HeckPuppetStyleEntry()
                {
                    HeckPuppetsOptions = new Dictionary<EnemyGameplayRank, HeckPuppetStyleEntry.HeckPuppetOptions>
                    {
                        {
                            EnemyGameplayRank.Normal, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSadisticUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                    }
                }
            },
            {
                StyleRanks.SSSensoredStorm, new HeckPuppetStyleEntry()
                {
                    HeckPuppetsOptions = new Dictionary<EnemyGameplayRank, HeckPuppetStyleEntry.HeckPuppetOptions>
                    {
                        {
                            EnemyGameplayRank.Normal, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "SSSensoredStormUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                    }
                }
            },
            {
                StyleRanks.ULTRAKILL, new HeckPuppetStyleEntry()
                {
                    HeckPuppetsOptions = new Dictionary<EnemyGameplayRank, HeckPuppetStyleEntry.HeckPuppetOptions>
                    {
                        {
                            EnemyGameplayRank.Normal, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = Config.Bind($"{ModCat}.{CheatsCat}.{HeckPuppetsCat}", "ULTRAKILLUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                    }
                }
            },
        };
    }
}