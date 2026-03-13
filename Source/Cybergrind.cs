using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Nyxpiri.ULTRAKILL.NyxLib;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Cybergrind
{
    // calling EndlessGrid.Instance after endless grid has been not null once but is now null seems to cause lots of lag for some reason.
    public static EndlessGrid EndlessGrid { get; private set; } = null;

    [HarmonyPatch(typeof(EndlessGrid), "OnTriggerEnter", new Type[] { typeof(Collider) })]
    static class CybergrindStartPatch
    {
        public static void Prefix(Collider other)
        {
            if (!((Component)(object)other).CompareTag("Player"))
            {
                return;
            }
        }

        public static void Postfix(Collider other)
        {
            if (!((Component)(object)other).CompareTag("Player"))
            {
                return;
            }

            IsActive = true;
        }
    }

    [HarmonyPatch(typeof(EndlessGrid), "Start")]
    static class EndlessGridStartPatch
    {
        public static void Prefix(EndlessGrid __instance)
        {
        }

        public static void Postfix(EndlessGrid __instance)
        {
            EndlessGrid = __instance;
        }
    }

    /*[HarmonyPatch(typeof(EndlessGrid), "NextWave", null)] TODO: figure out where to put this.
    static class EndlessGridNextWavePatch
    {
        public static void Prefix(EndlessGrid __instance)
        {
            
        }

        private static string[] CheatsEnabledByUs = new string[0];
        
        private static List<(string, string[])> Challenges = new List<(string, string[])>
        {
            //("SEEING DOUBLE", new string[]{Cheats.HeckPuppets, Cheats.GiveEnemiesFriends}),
            ("BRUTAL HECK", new string[]{Cheats.AggressiveAgony, Cheats.DemandingHell}),
            //("GET UP ON THEIR BACK", new string[]{Cheats.HydraMode, Cheats.DemandingHell}),
            ("MISCONFIGURED", new string[]{Cheats.SelfConscience, Cheats.BadGyro}),
            ("ULTRACARE", new string[]{Cheats.GiveEnemiesFriends, Cheats.BloodFueledEnemies}),
            ("NOW SWAP", new string[]{Cheats.SandAllEnemiesID, Cheats.BloodFueledEnemies}),
            ("BAD GAME DESIGN", new string[]{ Cheats.BadGyro, Cheats.MundaneMurder }),
            ("PAINFULLY SALTY", new string[]{ Cheats.SaltyEnemies, Cheats.AggressiveAgony }),
            ("FORCERAD++", new string[]{ Cheats.RadiantAllEnemies, Cheats.AggressiveAgony }),
        };

        private static List<(string, string[])> HardChallenges = new List<(string, string[])>
        {
            ("SSADISTIC HECK", new string[]{ Cheats.SaltyEnemies, Cheats.SelfConscience, Cheats.DemandingHell, Cheats.AggressiveAgony }),
            ("HECK SPECIAL", new string[]{ Cheats.SaltyEnemies, Cheats.HeckPuppets, Cheats.DemandingHell, Cheats.AggressiveAgony }),
            ("STYLE ISSUE", new string[]{ Cheats.SaltyEnemies, Cheats.SelfConscience, Cheats.DemandingHell, Cheats.HeckPuppets }),
            //("FURIOUS MITOSIS", new string[]{ Cheats.SaltyEnemies, Cheats.DemandingHell, Cheats.HydraMode }),
            ("HEAT OF GREED", new string[]{ Cheats.SandAllEnemiesID, Cheats.BloodFueledEnemies, Cheats.DemandingHell }),
            ("TECH ISSUES", new string[]{ Cheats.BadGyro, Cheats.DemandingHell, Cheats.SelfConscience }),
        };

        private static int hardChallengeIdx = 0;
        private static int challengeIdx = 0;

        public static void Postfix(EndlessGrid __instance)
        {
            for (int i = 0; i < CheatsEnabledByUs.Length; i++)
            {
                Cheats.Manager.DisableCheat(CheatsEnabledByUs[i]);
            }

            if (Cheats.IsHydraModeOn)
            {
                Cheats.Manager.ToggleCheat(Cheats.Manager.GetCheatInstance<KillAllEnemies>());
            }
            
            CheatsEnabledByUs = new string[0];

            if (Cheats.IsCheatDisabled(Cheats.CybergrindCheatRandomization))
            {
                return;
            }

            bool useHardChallenges = ((__instance.currentWave % 5) == 0) && __instance.currentWave != __instance.startWave;
            List<(string, string[])> challengePool = useHardChallenges ? HardChallenges : Challenges;
            
            ref int currentChallengeIdx = ref (useHardChallenges ? ref challengeIdx : ref hardChallengeIdx);

            if (currentChallengeIdx == 0 || currentChallengeIdx == 2)
            {
                if (useHardChallenges)
                {
                    for (int i = 0; i < HardChallenges.Count; i++)
                    {
                        int targetIdx = UnityEngine.Random.Range(0, HardChallenges.Count - 1);
                        var movingValue = HardChallenges[i];
                        HardChallenges.RemoveAt(i);
                        HardChallenges.Insert(targetIdx, movingValue);
                    }
                }
                else
                {
                    for (int i = 0; i < Challenges.Count; i++)
                    {
                        int targetIdx = UnityEngine.Random.Range(0, Challenges.Count - 1);
                        var movingValue = Challenges[i];
                        Challenges.RemoveAt(i);
                        Challenges.Insert(targetIdx, movingValue);
                    }
                }
            }
            
            var challenge = challengePool[currentChallengeIdx];
            currentChallengeIdx = (currentChallengeIdx + 1) % (useHardChallenges ? HardChallenges.Count : Challenges.Count);
            
            FieldPublisher<CheatsManager, Dictionary<string, ICheat>> idToCheat = new FieldPublisher<CheatsManager, Dictionary<string, ICheat>>(Cheats.Manager, "idToCheat");
            CheatsEnabledByUs = challenge.Item2;

            float downwardOffsetBase = 0.0f;

            if (useHardChallenges)
            {
                QuickMsgPool.DisplayQuickMsg($"INTENSITY SPIKE", new Color(1.0f, 0.1f, 0.1f), 5.0f, velocity: Vector3.down * 200.0f, 42.0f);
                QuickMsgPool.DisplayQuickMsg($"{challenge.Item1}", new Color(1.0f, 0.3f, 0.3f), 5.0f, velocity: Vector3.down * 320.0f, 36.0f);
                downwardOffsetBase = 500.0f;
            }
            else
            {
                QuickMsgPool.DisplayQuickMsg($"{challenge.Item1}", new Color(1.0f, 0.4f, 0.4f), 5.0f, velocity: Vector3.down * 220.0f, 36.0f);
                downwardOffsetBase = 350.0f;
            }

            for (int i = 0; i < challenge.Item2.Length; i++)
            {
                string cheatName = challenge.Item2[i];
                var cheat = idToCheat.Value[cheatName];
                
                QuickMsgPool.DisplayQuickMsg($"+ {cheat.LongName.ToUpper()}", new Color(0.875f, 0.75f, 1.0f), 5.0f, velocity: Vector3.down * ((float)((i) * 90.0f) + downwardOffsetBase), 24.0f);
                cheat.Enable(Cheats.Manager);
            }
            
            Cheats.Manager.RefreshCheatStates();
        }
    }*/

    public static void Initialize()
    {
        UpdateEvents.OnFixedUpdate += OnFixedUpdate;
        ScenesEvents.OnSceneWasLoaded += OnSceneWasLoaded;
    }

    private static void OnSceneWasLoaded(Scene scene, string sceneName)
    {
        IsActive = false;
    }

    private static void OnFixedUpdate()
    {
    }

    public static bool IsActive { get; private set; } = false;
    public static bool IsInCybergrindLevel { get => EndlessGrid != null; }
}