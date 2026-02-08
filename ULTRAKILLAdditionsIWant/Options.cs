using System;
using System.Collections.Generic;
using MelonLoader;
using UKAIW;
using UnityEngine;

public static class Options
{
    static MelonPreferences_Category LoggingCategory = null;
    static MelonPreferences_Entry<bool> IncludePerformanceLogsEntry = null;
    static MelonPreferences_Entry<bool> IncludeTraceExpectedLogsEntry = null;
    static MelonPreferences_Entry<bool> IncludeExpectedLogsEntry = null;
    static MelonPreferences_Entry<bool> IncludeLikelyLogsEntry = null;
    static MelonPreferences_Entry<bool> IncludeUnlikelyLogsEntry = null;
    static MelonPreferences_Entry<bool> IncludeUnexpectedLogsEntry = null;

    static MelonPreferences_Category HydraCategory = null;
    static MelonPreferences_Entry<float> HydraHealthDecayScaleEntry = null;
    static MelonPreferences_Entry<float> HydraDefaultWaitTimeEntry = null;
    static MelonPreferences_Entry<float> HydraMiniBossWaitTimeEntry = null;
    static MelonPreferences_Entry<float> HydraBossWaitTimeEntry = null;
    static MelonPreferences_Entry<float> HydraUltraBossWaitTimeEntry = null;
    static MelonPreferences_Entry<int> HydraMaxDepthEntry = null;
    static MelonPreferences_Entry<int> HydraMaxFromOneBossEntry = null;
    static MelonPreferences_Entry<int> HydraMaxFromOneEntry = null;
    static MelonPreferences_Entry<int> HydraMaxPerUpdateEntry = null;
    static MelonPreferences_Entry<int> HydraBloodCapNumUpdatesPerTickEntry = null;
    static MelonPreferences_Entry<int> HydraBloodCapNumBloodPerTickEntry = null;
    static MelonPreferences_Entry<int> HydraPrefabPoolCapacityEntry = null;
    static MelonPreferences_Entry<int> HydraPrefabPoolGrowPerUpdateEntry = null;
    static MelonPreferences_Entry<int> HydraKillBonusEntry = null;
    static MelonPreferences_Entry<int> HydraMiniBossKillBonusEntry = null;
    static MelonPreferences_Entry<int> HydraBossKillBonusEntry = null;
    static MelonPreferences_Entry<int> HydraUltraBossKillBonusEntry = null;

    static MelonPreferences_Category FriendsCategory = null;
    static MelonPreferences_Entry<int> NumFriendsToSpawnEntry = null;

    static MelonPreferences_Category BloodFuelEnemiesCategory = null;
    static MelonPreferences_Entry<float> BloodFuelEnemiesHealScalarEntry = null;
    static MelonPreferences_Entry<float> BloodFuelEnemiesDistDivisorEntry = null;

    static MelonPreferences_Category SaltCategory = null;
    static MelonPreferences_Entry<float> DestructiveRadianceTierEntry = null;
    static MelonPreferences_Entry<float> ChaoticRadianceTierEntry = null;
    static MelonPreferences_Entry<float> BrutalRadianceTierEntry = null;
    static MelonPreferences_Entry<float> AnarchicRadianceTierEntry = null;
    static MelonPreferences_Entry<float> SupremeRadianceTierEntry = null;
    static MelonPreferences_Entry<float> SSadisticRadianceTierEntry = null;
    static MelonPreferences_Entry<float> SSSensoredStormRadianceTierEntry = null;
    static MelonPreferences_Entry<float> ULTRAKILLNoEnrageRadianceTierEntry = null;
    static MelonPreferences_Entry<float> ULTRAKILLRadianceTierEntry = null;
    static MelonPreferences_Entry<bool> SaltEffectHealthEntry = null;
    static MelonPreferences_Entry<bool> SaltEffectSpeedEntry = null;
    static MelonPreferences_Entry<bool> SaltEffectDamageEntry = null;
    
    static MelonPreferences_Category RadianceAllCategory = null;
    static MelonPreferences_Entry<float> RadianceAllTierEntry = null;
    static MelonPreferences_Entry<float> RadianceAllSpeedTierEntry = null;
    static MelonPreferences_Entry<float> RadianceAllDamageTierEntry = null;
    static MelonPreferences_Entry<float> RadianceAllHealthTierEntry = null;

    static MelonPreferences_Category DemandingHellCategory = null;

    public static MelonPreferences_Entry<float> DemandingHellDestructiveHeatResDrainEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellDestructiveHeatResRecoveryEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellDestructiveHeatResExplosiveSizeBase { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellDestructiveHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellDestructiveHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellDestructiveHeatResExplosiveDmgScalar { get; private set; } = null;
    public static MelonPreferences_Entry<bool> DemandingHellDestructiveHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static MelonPreferences_Entry<float> DemandingHellChaoticHeatResDrainEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellChaoticHeatResRecoveryEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellChaoticHeatResExplosiveSizeBase { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellChaoticHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellChaoticHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellChaoticHeatResExplosiveDmgScalar { get; private set; } = null;
    public static MelonPreferences_Entry<bool> DemandingHellChaoticHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static MelonPreferences_Entry<float> DemandingHellBrutalHeatResRecoveryEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellBrutalHeatResDrainEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellBrutalHeatResExplosiveSizeBase { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellBrutalHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellBrutalHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellBrutalHeatResExplosiveDmgScalar { get; private set; } = null;
    public static MelonPreferences_Entry<bool> DemandingHellBrutalHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static MelonPreferences_Entry<float> DemandingHellAnarchicHeatResDrainEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellAnarchicHeatResRecoveryEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellAnarchicHeatResExplosiveSizeBase { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellAnarchicHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellAnarchicHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellAnarchicHeatResExplosiveDmgScalar { get; private set; } = null;
    public static MelonPreferences_Entry<bool> DemandingHellAnarchicHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static MelonPreferences_Entry<float> DemandingHellSupremeHeatResDrainEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSupremeHeatResRecoveryEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSupremeHeatResExplosiveSizeBase { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSupremeHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSupremeHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSupremeHeatResExplosiveDmgScalar { get; private set; } = null;
    public static MelonPreferences_Entry<bool> DemandingHellSupremeHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static MelonPreferences_Entry<float> DemandingHellSSadisticHeatResDrainEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSSadisticHeatResRecoveryEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSSadisticHeatResExplosiveSizeBase { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSSadisticHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSSadisticHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSSadisticHeatResExplosiveDmgScalar { get; private set; } = null;
    public static MelonPreferences_Entry<bool> DemandingHellSSadisticHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static MelonPreferences_Entry<float> DemandingHellSSSensoredStormHeatResDrainEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSSSensoredStormHeatResRecoveryEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSSSensoredStormHeatResExplosiveSizeBase { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSSSensoredStormHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSSSensoredStormHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellSSSensoredStormHeatResExplosiveDmgScalar { get; private set; } = null;
    public static MelonPreferences_Entry<bool> DemandingHellSSSensoredStormHeatResExplosiveDmgPlayer { get; private set; } = null;

    public static MelonPreferences_Entry<float> DemandingHellULTRAKILLHeatResDrainEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellULTRAKILLHeatResRecoveryEntry { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellULTRAKILLHeatResExplosiveSizeBase { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellULTRAKILLHeatResExplosiveSizeNormMin { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellULTRAKILLHeatResExplosiveSizeNormMax { get; private set; } = null;
    public static MelonPreferences_Entry<float> DemandingHellULTRAKILLHeatResExplosiveDmgScalar { get; private set; } = null;
    public static MelonPreferences_Entry<bool> DemandingHellULTRAKILLHeatResExplosiveDmgPlayer { get; private set; } = null;

    static public float HydraHealthDecayScale { get => HydraHealthDecayScaleEntry.Value; } 

    static public float HydraDefaultWaitTime { get => HydraDefaultWaitTimeEntry.Value; }
    static public float HydraMiniBossWaitTime { get => HydraMiniBossWaitTimeEntry.Value; }
    static public float HydraBossWaitTime { get => HydraBossWaitTimeEntry.Value; }
    static public float HydraUltraBossWaitTime { get => HydraUltraBossWaitTimeEntry.Value; }

    static public int HydraMaxDepth { get => HydraMaxDepthEntry.Value; }
    static public int HydraMaxFromOne { get => HydraMaxFromOneEntry.Value; }
    static public int HydraMaxFromOneBoss { get => HydraMaxFromOneBossEntry.Value; }
    static public int HydraMaxPerUpdate { get => HydraMaxPerUpdateEntry.Value; }
    static public int HydraBloodCapNumUpdatesPerTick { get => HydraBloodCapNumUpdatesPerTickEntry.Value; }
    static public int HydraBloodCapNumBloodPerTick { get => HydraBloodCapNumBloodPerTickEntry.Value; }
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
        LoggingCategory = MelonPreferences.CreateCategory("UKAIW-Logging");
        
        IncludePerformanceLogsEntry = LoggingCategory.CreateEntry<bool>("IncludePerformanceLogs", false);
        IncludeTraceExpectedLogsEntry = LoggingCategory.CreateEntry<bool>("IncludeTraceExpectedLogs", false);
        IncludeExpectedLogsEntry = LoggingCategory.CreateEntry<bool>("IncludeExpectedLogs", false);
        IncludeLikelyLogsEntry = LoggingCategory.CreateEntry<bool>("IncludeLikelyLogs", false);
        IncludeUnlikelyLogsEntry = LoggingCategory.CreateEntry<bool>("IncludeUnlikelyLogs", true);
        IncludeUnexpectedLogsEntry = LoggingCategory.CreateEntry<bool>("IncludeUnexpectedLogs", true);

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

        HydraBloodCapNumUpdatesPerTickEntry = HydraCategory.CreateEntry<int>("HydraBloodCapNumUpdatesPerTick", 50);
        HydraBloodCapNumBloodPerTickEntry = HydraCategory.CreateEntry<int>("HydraBloodCapNumBloodPerTick", 10);

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
        DemandingHellSSadisticHeatResExplosiveDmgScalar = DemandingHellCategory.CreateEntry<float>("SSadisticHeatResExplosiveDmgScalar", 0.25f);
        DemandingHellSSadisticHeatResExplosiveDmgPlayer = DemandingHellCategory.CreateEntry<bool>("SSadisticHeatResExplosiveDmgPlayer", true);

        DemandingHellSSSensoredStormHeatResDrainEntry = DemandingHellCategory.CreateEntry<float>("SSSensoredStormHeatStormResDrain", 85.0f);
        DemandingHellSSSensoredStormHeatResRecoveryEntry = DemandingHellCategory.CreateEntry<float>("SSSensoredStormHeatStormResRecovery", 1.9f);
        DemandingHellSSSensoredStormHeatResExplosiveSizeBase = DemandingHellCategory.CreateEntry<float>("SSSensoredStormHeatResExplosiveSizeBase", 14.0f);
        DemandingHellSSSensoredStormHeatResExplosiveSizeNormMin = DemandingHellCategory.CreateEntry<float>("SSSensoredStormHeatResExplosiveSizeNormMin", 0.15f);
        DemandingHellSSSensoredStormHeatResExplosiveSizeNormMax = DemandingHellCategory.CreateEntry<float>("SSSensoredStormHeatResExplosiveSizeNormMax", 8.5f);
        DemandingHellSSSensoredStormHeatResExplosiveDmgScalar = DemandingHellCategory.CreateEntry<float>("SSSensoredStormHeatResExplosiveDmgScalar", 0.4f);
        DemandingHellSSSensoredStormHeatResExplosiveDmgPlayer = DemandingHellCategory.CreateEntry<bool>("SSSensoredStormHeatResExplosiveDmgPlayer", true);

        DemandingHellULTRAKILLHeatResRecoveryEntry = DemandingHellCategory.CreateEntry<float>("ULTRAKILLHeatResRecovery", 2.7f);
        DemandingHellULTRAKILLHeatResDrainEntry = DemandingHellCategory.CreateEntry<float>("ULTRAKILLHeatResDrain", 100.0f);
        DemandingHellULTRAKILLHeatResExplosiveSizeBase = DemandingHellCategory.CreateEntry<float>("ULTRAKILLHeatResExplosiveSizeBase", 20.0f);
        DemandingHellULTRAKILLHeatResExplosiveSizeNormMin = DemandingHellCategory.CreateEntry<float>("ULTRAKILLHeatResExplosiveSizeNormMin", 0.15f);
        DemandingHellULTRAKILLHeatResExplosiveSizeNormMax = DemandingHellCategory.CreateEntry<float>("ULTRAKILLHeatResExplosiveSizeNormMax", 6.5f);
        DemandingHellULTRAKILLHeatResExplosiveDmgScalar = DemandingHellCategory.CreateEntry<float>("ULTRAKILLHeatResExplosiveDmgScalar", 0.65f);
        DemandingHellULTRAKILLHeatResExplosiveDmgPlayer = DemandingHellCategory.CreateEntry<bool>("ULTRAKILLHeatResExplosiveDmgPlayer", true);
    }
}