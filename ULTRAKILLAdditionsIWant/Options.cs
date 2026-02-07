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

    static MelonPreferences_Category BrutalHellCategory = null;

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

        RadianceAllCategory = MelonPreferences.CreateCategory("UKAIW-RadianceAll");
    }
}