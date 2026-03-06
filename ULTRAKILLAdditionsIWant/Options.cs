using System.Collections.Generic;
using BepInEx.Configuration;
using UKAIW;

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

    public static void Initialize()
    {
        
        ShowErrorNotification = Config.Bind("UKAIW.Debug", "ShowErrorNotification", false, "Shows text saying An Error has Occured! At the top of your screen as a 'QuickMsg', with a timestamp (using your locally set timezone!)");
        IncludePerformanceLogsEntry = Config.Bind("UKAIW.Debug", "IncludePerformanceLogs", false);
        IncludeTraceExpectedLogsEntry = Config.Bind("UKAIW.Debug", "IncludeTraceExpectedLogs", false);
        IncludeExpectedLogsEntry = Config.Bind("UKAIW.Debug", "IncludeExpectedLogs", false);
        IncludeLikelyLogsEntry = Config.Bind("UKAIW.Debug", "IncludeLikelyLogs", false);
        IncludeUnlikelyLogsEntry = Config.Bind("UKAIW.Debug", "IncludeUnlikelyLogs", false);
        IncludeUnexpectedLogsEntry = Config.Bind("UKAIW.Debug", "IncludeUnexpectedLogs", false);
        LogEnemyTypeOnStart = Config.Bind("UKAIW.Debug.Development", "LogEnemyTypeOnEnemyStart", false);
        DisableQuickLoad = Config.Bind("UKAIW.Debug.Development", "DisableGameInitLevelQuickLoad", false);
        
        BugFixesCategory = MelonPreferences.CreateCategory("UKAIW-BugFixes");
        EnableStreetCleanerDodgeFix = BugFixesCategory.CreateEntry<bool>("EnableStreetCleanerDodgeFix", true);
        EnableStreetCleanerDodgeFixOnlyWhenNeeded = BugFixesCategory.CreateEntry<bool>("EnableStreetCleanerDodgeFixOnlyWhenNeeded", true);
        StreetCleanerDodgeFixInterpRate = BugFixesCategory.CreateEntry<float>("StreetCleanerDodgeFixInterpRate", 8.0f);

        HydraCategory = MelonPreferences.CreateCategory("UKAIW-Hydra");

        HydraHealthDecayScaleEntry = HydraCategory.CreateEntry<float>("HydraHealthDecayScale", 0.5f);

        HydraDefaultWaitTimeEntry = HydraCategory.CreateEntry<float>("HydraDefaultWaitTime", 0.45f);
        HydraMiniBossWaitTimeEntry = HydraCategory.CreateEntry<float>("HydraMiniBossWaitTime", 0.75f);
        HydraBossWaitTimeEntry = HydraCategory.CreateEntry<float>("HydraBossWaitTime", 1.5f);
        HydraUltraBossWaitTimeEntry = HydraCategory.CreateEntry<float>("HydraUltraBossWaitTime", 2.75f);

        HydraMaxFromOneBossEntry = HydraCategory.CreateEntry<int>("HydraMaxFromOneBoss", 6);
        HydraMaxDepthEntry = HydraCategory.CreateEntry<int>("HydraMaxDepth", 16);
        HydraMaxFromOneEntry = HydraCategory.CreateEntry<int>("HydraMaxFromOne", 12);
        HydraMaxPerUpdateEntry = HydraCategory.CreateEntry<int>("HydraMaxPerFrame", 4);

        HydraPrefabPoolCapacityEntry = HydraCategory.CreateEntry<int>("HydraPrefabPoolCapacity", 20);
        HydraPrefabPoolGrowPerUpdateEntry = HydraCategory.CreateEntry<int>("HydraPrefabPoolGrowPerUpdate", 3);

        HydraKillBonusEntry = HydraCategory.CreateEntry<int>("HydraKillBonus", 10);
        HydraMiniBossKillBonusEntry = HydraCategory.CreateEntry<int>("HydraMiniBossKillBonus", 50);
        HydraBossKillBonusEntry = HydraCategory.CreateEntry<int>("HydraBossKillBonus", 100);
        HydraUltraBossKillBonusEntry = HydraCategory.CreateEntry<int>("HydraUltraBossKillBonus", 1000);

        FriendsCategory = MelonPreferences.CreateCategory("UKAIW-Friends");
        NumFriendsToSpawnEntry = FriendsCategory.CreateEntry<int>("NumFriendsToSpawn", 1);

        BloodFuelEnemiesCategory = MelonPreferences.CreateCategory("UKAIW-BloodFueledEnemies");
        BloodFuelEnemiesHealScalarEntry = BloodFuelEnemiesCategory.CreateEntry<float>("BloodFuelEnemiesHealScalar", 0.4f);
        BloodFuelEnemiesDistDivisorEntry = BloodFuelEnemiesCategory.CreateEntry<float>("BloodFuelEnemiesDistDivisor", 8.0f);

        SaltCategory = MelonPreferences.CreateCategory("UKAIW-Salt");
        DestructiveRadianceTierEntry = SaltCategory.CreateEntry<float>("DestructiveRadianceTier", 0.0f);
        ChaoticRadianceTierEntry = SaltCategory.CreateEntry<float>("ChaoticRadianceTier", 0.0f);
        BrutalRadianceTierEntry = SaltCategory.CreateEntry<float>("BrutalRadianceTier", 1.0f);
        AnarchicRadianceTierEntry = SaltCategory.CreateEntry<float>("AnarchicRadianceTier", 1.1f);
        SupremeRadianceTierEntry = SaltCategory.CreateEntry<float>("SupremeRadianceTier", 1.25f);
        SSadisticRadianceTierEntry = SaltCategory.CreateEntry<float>("SSadisticRadianceTier", 1.4f);
        SSSensoredStormRadianceTierEntry = SaltCategory.CreateEntry<float>("SSSensoredStormRadianceTier", 1.6f);
        ULTRAKILLNoEnrageRadianceTierEntry = SaltCategory.CreateEntry<float>("ULTRAKILLNoEnrageRadianceTier", 2.0f);
        ULTRAKILLRadianceTierEntry = SaltCategory.CreateEntry<float>("ULTRAKILLRadianceTier", 1.8f);

        SaltEffectSpeedEntry = SaltCategory.CreateEntry<bool>("SaltEffectSpeed", true);
        SaltEffectHealthEntry = SaltCategory.CreateEntry<bool>("SaltEffectHealth", false);
        SaltEffectDamageEntry = SaltCategory.CreateEntry<bool>("SaltEffectDamage", false);
        
        RadianceAllCategory = MelonPreferences.CreateCategory("UKAIW-RadianceAll");
        RadianceAllTierEntry = RadianceAllCategory.CreateEntry<float>("RadianceAllTier", 1.0f);
        RadianceAllSpeedTierEntry = RadianceAllCategory.CreateEntry<float>("RadianceAllSpeedTier", 1.5f);
        RadianceAllDamageTierEntry = RadianceAllCategory.CreateEntry<float>("RadianceAllDamageTier", 1.5f);
        RadianceAllHealthTierEntry = RadianceAllCategory.CreateEntry<float>("RadianceAllHealthTier", 1.5f);

        EnemyFeedbackersCategory = MelonPreferences.CreateCategory("UKAIW-EnemyFeedbackers");
        HitstopOnEnemyParry = EnemyFeedbackersCategory.CreateEntry<bool>("HitstopOnEnemyParry", false);

        DemandingHellCategory = MelonPreferences.CreateCategory("UKAIW-DemandingHell");

        DemandingHellDestructiveHeatResDrainEntry = DemandingHellCategory.CreateEntry<float>("DestructiveHeatResDrain", -1.0f);
        DemandingHellDestructiveHeatResRecoveryEntry = DemandingHellCategory.CreateEntry<float>("DestructHeatResRecovery", 100.0f);
        DemandingHellDestructiveHeatResExplosiveSizeBase = DemandingHellCategory.CreateEntry<float>("DestructiveHeatResExplosiveSizeBase", -1.0f);
        DemandingHellDestructiveHeatResExplosiveSizeNormMin = DemandingHellCategory.CreateEntry<float>("DestructiveHeatResExplosiveSizeNormMin", -1.0f);
        DemandingHellDestructiveHeatResExplosiveSizeNormMax = DemandingHellCategory.CreateEntry<float>("DestructiveHeatResExplosiveSizeNormMax", -1.0f);
        DemandingHellDestructiveHeatResExplosiveDmgScalar = DemandingHellCategory.CreateEntry<float>("DestructiveHeatResExplosiveDmgScalar", -1.0f);
        DemandingHellDestructiveHeatResExplosiveDmgPlayer = DemandingHellCategory.CreateEntry<bool>("DestructiveHeatResExplosiveDmgPlayer", false);

        DemandingHellChaoticHeatResDrainEntry = DemandingHellCategory.CreateEntry<float>("ChaoticHeatResDrain", -1.0f);
        DemandingHellChaoticHeatResRecoveryEntry = DemandingHellCategory.CreateEntry<float>("ChaoticHeatResRecovery", 100.0f);
        DemandingHellChaoticHeatResExplosiveSizeBase = DemandingHellCategory.CreateEntry<float>("ChaoticHeatResExplosiveSizeBase", -1.0f);
        DemandingHellChaoticHeatResExplosiveSizeNormMin = DemandingHellCategory.CreateEntry<float>("ChaoticHeatResExplosiveSizeNormMin", -1.0f);
        DemandingHellChaoticHeatResExplosiveSizeNormMax = DemandingHellCategory.CreateEntry<float>("ChaoticHeatResExplosiveSizeNormMax", -1.0f);
        DemandingHellChaoticHeatResExplosiveDmgScalar = DemandingHellCategory.CreateEntry<float>("ChaoticHeatResExplosiveDmgScalar", -1.0f);
        DemandingHellChaoticHeatResExplosiveDmgPlayer = DemandingHellCategory.CreateEntry<bool>("ChaoticHeatResExplosiveDmgPlayer", false);

        DemandingHellBrutalHeatResDrainEntry = DemandingHellCategory.CreateEntry<float>("BrutalHeatResDrain", -1.0f);
        DemandingHellBrutalHeatResRecoveryEntry = DemandingHellCategory.CreateEntry<float>("BrutalHeatResRecovery", 100.0f);
        DemandingHellBrutalHeatResExplosiveSizeBase = DemandingHellCategory.CreateEntry<float>("BrutalHeatResExplosiveSizeBase", -1.0f);
        DemandingHellBrutalHeatResExplosiveSizeNormMin = DemandingHellCategory.CreateEntry<float>("BrutalHeatResExplosiveSizeNormMin", -1.0f);
        DemandingHellBrutalHeatResExplosiveSizeNormMax = DemandingHellCategory.CreateEntry<float>("BrutalHeatResExplosiveSizeNormMax", -1.0f);
        DemandingHellBrutalHeatResExplosiveDmgScalar = DemandingHellCategory.CreateEntry<float>("BrutalHeatResExplosiveDmgScalar", -1.0f);
        DemandingHellBrutalHeatResExplosiveDmgPlayer = DemandingHellCategory.CreateEntry<bool>("BrutalHeatResExplosiveDmgPlayer", false);

        DemandingHellAnarchicHeatResDrainEntry = DemandingHellCategory.CreateEntry<float>("AnarchicHeatResDrain", 30.0f);
        DemandingHellAnarchicHeatResRecoveryEntry = DemandingHellCategory.CreateEntry<float>("AnarchicHeatResRecovery", 1.7f);
        DemandingHellAnarchicHeatResExplosiveSizeBase = DemandingHellCategory.CreateEntry<float>("AnarchicHeatResExplosiveSizeBase", -1.0f);
        DemandingHellAnarchicHeatResExplosiveSizeNormMin = DemandingHellCategory.CreateEntry<float>("AnarchicHeatResExplosiveSizeNormMin", -1.0f);
        DemandingHellAnarchicHeatResExplosiveSizeNormMax = DemandingHellCategory.CreateEntry<float>("AnarchicHeatResExplosiveSizeNormMax", -1.0f);
        DemandingHellAnarchicHeatResExplosiveDmgScalar = DemandingHellCategory.CreateEntry<float>("AnarchicHeatResExplosiveDmgScalar", -1.0f);
        DemandingHellAnarchicHeatResExplosiveDmgPlayer = DemandingHellCategory.CreateEntry<bool>("AnarchicHeatResExplosiveDmgPlayer", false);

        DemandingHellSupremeHeatResDrainEntry = DemandingHellCategory.CreateEntry<float>("SupremeHeatResDrain", 60.0f);
        DemandingHellSupremeHeatResRecoveryEntry = DemandingHellCategory.CreateEntry<float>("SupremeHeatResRecovery", 2.0f);
        DemandingHellSupremeHeatResExplosiveSizeBase = DemandingHellCategory.CreateEntry<float>("SupremeHeatResExplosiveSizeBase", -1.0f);
        DemandingHellSupremeHeatResExplosiveSizeNormMin = DemandingHellCategory.CreateEntry<float>("SupremeHeatResExplosiveSizeNormMin", -1.0f);
        DemandingHellSupremeHeatResExplosiveSizeNormMax = DemandingHellCategory.CreateEntry<float>("SupremeHeatResExplosiveSizeNormMax", -1.0f);
        DemandingHellSupremeHeatResExplosiveDmgScalar = DemandingHellCategory.CreateEntry<float>("SupremeHeatResExplosiveDmgScalar", -1.0f);
        DemandingHellSupremeHeatResExplosiveDmgPlayer = DemandingHellCategory.CreateEntry<bool>("SupremeHeatResExplosiveDmgPlayer", false);

        DemandingHellSSadisticHeatResDrainEntry = DemandingHellCategory.CreateEntry<float>("SSadisticHeatResDrain", 70.0f);
        DemandingHellSSadisticHeatResRecoveryEntry = DemandingHellCategory.CreateEntry<float>("SSadistricHeatResRecovery", 2.0f);
        DemandingHellSSadisticHeatResExplosiveSizeBase = DemandingHellCategory.CreateEntry<float>("SSadisticHeatResExplosiveSizeBase", 12.0f);
        DemandingHellSSadisticHeatResExplosiveSizeNormMin = DemandingHellCategory.CreateEntry<float>("SSadisticHeatResExplosiveSizeNormMin", 2.5f);
        DemandingHellSSadisticHeatResExplosiveSizeNormMax = DemandingHellCategory.CreateEntry<float>("SSadisticHeatResExplosiveSizeNormMax", 7.0f);
        DemandingHellSSadisticHeatResExplosiveDmgScalar = DemandingHellCategory.CreateEntry<float>("SSadisticHeatResExplosiveDmgScalar", 0.35f);
        DemandingHellSSadisticHeatResExplosiveDmgPlayer = DemandingHellCategory.CreateEntry<bool>("SSadisticHeatResExplosiveDmgPlayer", true);

        DemandingHellSSSensoredStormHeatResDrainEntry = DemandingHellCategory.CreateEntry<float>("SSSensoredStormHeatStormResDrain", 85.0f);
        DemandingHellSSSensoredStormHeatResRecoveryEntry = DemandingHellCategory.CreateEntry<float>("SSSensoredStormHeatStormResRecovery", 1.9f);
        DemandingHellSSSensoredStormHeatResExplosiveSizeBase = DemandingHellCategory.CreateEntry<float>("SSSensoredStormHeatResExplosiveSizeBase", 14.0f);
        DemandingHellSSSensoredStormHeatResExplosiveSizeNormMin = DemandingHellCategory.CreateEntry<float>("SSSensoredStormHeatResExplosiveSizeNormMin", 0.15f);
        DemandingHellSSSensoredStormHeatResExplosiveSizeNormMax = DemandingHellCategory.CreateEntry<float>("SSSensoredStormHeatResExplosiveSizeNormMax", 8.5f);
        DemandingHellSSSensoredStormHeatResExplosiveDmgScalar = DemandingHellCategory.CreateEntry<float>("SSSensoredStormHeatResExplosiveDmgScalar", 0.5f);
        DemandingHellSSSensoredStormHeatResExplosiveDmgPlayer = DemandingHellCategory.CreateEntry<bool>("SSSensoredStormHeatResExplosiveDmgPlayer", true);

        DemandingHellULTRAKILLHeatResRecoveryEntry = DemandingHellCategory.CreateEntry<float>("ULTRAKILLHeatResRecovery", 2.7f);
        DemandingHellULTRAKILLHeatResDrainEntry = DemandingHellCategory.CreateEntry<float>("ULTRAKILLHeatResDrain", 100.0f);
        DemandingHellULTRAKILLHeatResExplosiveSizeBase = DemandingHellCategory.CreateEntry<float>("ULTRAKILLHeatResExplosiveSizeBase", 20.0f);
        DemandingHellULTRAKILLHeatResExplosiveSizeNormMin = DemandingHellCategory.CreateEntry<float>("ULTRAKILLHeatResExplosiveSizeNormMin", 0.15f);
        DemandingHellULTRAKILLHeatResExplosiveSizeNormMax = DemandingHellCategory.CreateEntry<float>("ULTRAKILLHeatResExplosiveSizeNormMax", 6.5f);
        DemandingHellULTRAKILLHeatResExplosiveDmgScalar = DemandingHellCategory.CreateEntry<float>("ULTRAKILLHeatResExplosiveDmgScalar", 0.8f);
        DemandingHellULTRAKILLHeatResExplosiveDmgPlayer = DemandingHellCategory.CreateEntry<bool>("ULTRAKILLHeatResExplosiveDmgPlayer", true);

        SelfConscienceCategory = MelonPreferences.CreateCategory("UKAIW-SelfConscience");
        SelfConscienceDestructiveDashCostScale = SelfConscienceCategory.CreateEntry<float>("DestructiveDashCostScale", 1.35f);
        SelfConscienceChaoticDashCostScale = SelfConscienceCategory.CreateEntry<float>("ChaoticDashCostScale", 1.1f);
        SelfConscienceBrutalDashCostScale = SelfConscienceCategory.CreateEntry<float>("BrutalDashCostScale", 1.05f);
        SelfConscienceAnarchicDashCostScale = SelfConscienceCategory.CreateEntry<float>("AnarchicDashCostScale", 1.0f);
        SelfConscienceSupremeDashCostScale = SelfConscienceCategory.CreateEntry<float>("SupremeDashCostScale", 0.9f);
        SelfConscienceSSadisticDashCostScale = SelfConscienceCategory.CreateEntry<float>("SSadisticDashCostScale", 0.85f);
        SelfConscienceSSSensoredStormDashCostScale = SelfConscienceCategory.CreateEntry<float>("SSSensoredStormDashCostScale", 0.825f);
        SelfConscienceULTRAKILLDashCostScale = SelfConscienceCategory.CreateEntry<float>("ULTRAKILLDashCostScale", 0.825f);

        SelfConscienseDashCostIncreaseInterpRate = SelfConscienceCategory.CreateEntry<float>("DashCostIncreaseInterpRate", 0.3f);
        SelfConscienseDashCostDecreaseInterpRate = SelfConscienceCategory.CreateEntry<float>("DashCostIDecreaseInterpRate", 1.0f);

        BloodOptimizer = MelonPreferences.CreateCategory("UKAIW-BloodOptimizer");
        BloodOptimizerCapNumUpdatesPerTick = BloodOptimizer.CreateEntry<int>("BloodOptimizerCapNumUpdatesPerTick", 90);
        BloodHeavyModdedEnemiesCapNumBloodPerTick = BloodOptimizer.CreateEntry<int>("BloodHeavyModdedEnemiesCapNumBloodPerTick", 2);

        CybergrindCheatRandomization = MelonPreferences.CreateCategory("UKAIW-CybergrindCheatRandomization");
        NumRandomCheats = CybergrindCheatRandomization.CreateEntry<int>("NumRandomCheats", 3);

        HeckPuppetsCategory = MelonPreferences.CreateCategory("UKAIW-HeckPuppets");
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
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("DestructiveNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("DestructiveNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("DestructiveMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("DestructiveMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("DestructiveBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("DestructiveBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("DestructiveUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("DestructiveUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("DestructiveUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
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
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("ChaoticNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("ChaoticNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("ChaoticMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("ChaoticMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("ChaoticBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("ChaoticBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("ChaoticUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("ChaoticUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ChaoticUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
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
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("BrutalNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("BrutalNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("BrutalMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("BrutalMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("BrutalBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("BrutalBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("BrutalUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("BrutalUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("BrutalUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
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
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("AnarchicNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("AnarchicNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("AnarchicMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("AnarchicMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("AnarchicBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("AnarchicBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("AnarchicUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("AnarchicUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("AnarchicUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
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
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("SupremeNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("SupremeNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("SupremeMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("SupremeMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("SupremeBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("SupremeBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("SupremeUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("SupremeUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SupremeUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
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
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("SSadisticNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("SSadisticNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("SSadisticMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("SSadisticMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("SSadisticBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("SSadisticBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("SSadisticUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("SSadisticUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSadisticUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
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
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("SSSensoredStormNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("SSSensoredStormNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("SSSensoredStormMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("SSSensoredStormMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("SSSensoredStormBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("SSSensoredStormBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("SSSensoredStormUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("SSSensoredStormUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("SSSensoredStormUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
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
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("ULTRAKILLNormalNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("ULTRAKILLNormalMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLNormalHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLNormalHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLNormalHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLNormalHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Miniboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("ULTRAKILLMinibossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("ULTRAKILLMinibossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLMinibossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLMinibossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLMinibossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLMinibossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Boss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("ULTRAKILLBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("ULTRAKILLBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                        {
                            EnemyGameplayRank.Ultraboss, new HeckPuppetStyleEntry.HeckPuppetOptions()
                            {
                                NumHeckPuppets = HeckPuppetsCategory.CreateEntry<int>("ULTRAKILLUltraBossNumHeckPuppets", 0),
                                MaxHeckPuppetHealth = HeckPuppetsCategory.CreateEntry<int>("ULTRAKILLUltraBossMaxHeckPuppetHealth", 10),
                                HeckPuppetHealthScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLUltraBossHeckPuppetHealthScalar", 0.5f),
                                HeckPuppetHealthBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLUltraBossHeckPuppetHealthBuffScalar", 0.5f),
                                HeckPuppetDamageBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLUltraBossHeckPuppetDamageBuffScalar", 0.5f),
                                HeckPuppetSpeedBuffScalar = HeckPuppetsCategory.CreateEntry<float>("ULTRAKILLUltraBossHeckPuppetSpeedBuffScalar", 0.5f),
                            }
                        },
                    }
                }
            },
        };
    }
}