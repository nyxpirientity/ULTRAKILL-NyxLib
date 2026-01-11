using System;
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

    static MelonPreferences_Category FriendsCategory = null;
    static MelonPreferences_Entry<int> NumFriendsToSpawnEntry = null;

    static MelonPreferences_Category BloodFuelEnemiesCategory = null;
    static MelonPreferences_Entry<float> BloodFuelEnemiesHealScalarEntry = null;
    static MelonPreferences_Entry<float> BloodFuelEnemiesDistDivisorEntry = null;

    static public float HydraHealthDecayScale = 0.5f;

    static public float HydraDefaultWaitTime = 0.5f;
    static public float HydraMiniBossWaitTime = 0.5f;
    static public float HydraBossWaitTime = 0.5f;
    static public float HydraUltraBossWaitTime = 0.5f;

    static public int HydraMaxDepth = 16;
    static public int HydraMaxFromOne = 12;
    static public int HydraMaxFromOneBoss = 6;
    static public int HydraMaxPerUpdate = 4;
    static public int HydraBloodCapNumUpdatesPerTick = 50;
    static public int HydraBloodCapNumBloodPerTick = 10;
    static public int HydraPrefabPoolCapacity = 20;
    static public int HydraPrefabPoolGrowPerUpdate = 3;

    static public bool IncludePerformanceLogs = false;
    static public bool IncludeTraceExpectedLogs = false;
    static public bool IncludeExpectedLogs = false;
    static public bool IncludeLikelyLogs = false;
    static public bool IncludeUnlikelyLogs = false;
    static public bool IncludeUnexpectedLogs = false;

    static public int NumFriendsToSpawn = 1;

    static public float BloodFuelEnemiesHealScalar = 0.2f;
    static public float BloodFuelEnemiesDistDivisor = 8.0f;

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

        FriendsCategory = MelonPreferences.CreateCategory("UKAIW-Friends");
        NumFriendsToSpawnEntry = FriendsCategory.CreateEntry<int>("NumFriendsToSpawn", 1);

        BloodFuelEnemiesCategory = MelonPreferences.CreateCategory("UKAIW-BloodFueledEnemies");
        BloodFuelEnemiesHealScalarEntry = BloodFuelEnemiesCategory.CreateEntry<float>("BloodFuelEnemiesHealScalar", 0.4f);
        BloodFuelEnemiesDistDivisorEntry = BloodFuelEnemiesCategory.CreateEntry<float>("BloodFuelEnemiesDistDivisor", 8.0f);

        Reload();
    }

    public static void Reload()
    {
        HydraHealthDecayScale = HydraHealthDecayScaleEntry.Value;

        HydraDefaultWaitTime = HydraDefaultWaitTimeEntry.Value;
        HydraMiniBossWaitTime = HydraMiniBossWaitTimeEntry.Value;
        HydraBossWaitTime = HydraBossWaitTimeEntry.Value;
        HydraUltraBossWaitTime = HydraUltraBossWaitTimeEntry.Value;

        HydraMaxDepth = HydraMaxDepthEntry.Value;

        HydraMaxFromOne = HydraMaxFromOneEntry.Value;
        HydraMaxPerUpdate = HydraMaxPerUpdateEntry.Value;
        HydraMaxFromOneBoss = HydraMaxFromOneBossEntry.Value;

        HydraPrefabPoolCapacity = HydraPrefabPoolCapacityEntry.Value;
        HydraPrefabPoolGrowPerUpdate = HydraPrefabPoolGrowPerUpdateEntry.Value;
        
        IncludePerformanceLogs = IncludePerformanceLogsEntry.Value;
        IncludeTraceExpectedLogs = IncludeTraceExpectedLogsEntry.Value;
        IncludeExpectedLogs = IncludeExpectedLogsEntry.Value;
        IncludeLikelyLogs = IncludeLikelyLogsEntry.Value;
        IncludeUnlikelyLogs = IncludeUnlikelyLogsEntry.Value;
        IncludeUnexpectedLogs = IncludeUnexpectedLogsEntry.Value;

        HydraBloodCapNumUpdatesPerTick = HydraBloodCapNumUpdatesPerTickEntry.Value;
        HydraBloodCapNumBloodPerTick = HydraBloodCapNumBloodPerTickEntry.Value;

        NumFriendsToSpawn = NumFriendsToSpawnEntry.Value;

        BloodFuelEnemiesHealScalar = BloodFuelEnemiesHealScalarEntry.Value;
        BloodFuelEnemiesDistDivisor = BloodFuelEnemiesDistDivisorEntry.Value;
    }
}