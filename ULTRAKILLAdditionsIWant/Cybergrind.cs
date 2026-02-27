using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using ULTRAKILL.Cheats;
using UnityEngine;

public static class CybergrindAdditions
{
    // calling EndlessGrid.Instance after endless grid has been not null once but is now null seems to cause lots of lag for some reason.
    public static EndlessGrid LastStartedEndlessGrid = null;

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

            CybergrindActive = true;
            CybergrindShuffleTimestamp.UpdateToNow();
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
            LastStartedEndlessGrid = __instance;
        }
    }

    [HarmonyPatch(typeof(EndlessGrid), "NextWave", null)]
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
            ("GET UP ON THEIR BACK", new string[]{Cheats.HydraMode, Cheats.DemandingHell}),
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
            ("FURIOUS MITOSIS", new string[]{ Cheats.SaltyEnemies, Cheats.DemandingHell, Cheats.HydraMode }),
            ("HEAT OF GREED", new string[]{ Cheats.SandAllEnemiesID, Cheats.BloodFueledEnemies, Cheats.DemandingHell }),
            ("TECH ISSUES", new string[]{ Cheats.BadGyro, Cheats.DemandingHell, Cheats.SelfConscience }),
        };

        private static int hardChallengeIdx = 0;
        private static int challengeIdx = 0;

        public static void Postfix(EndlessGrid __instance)
        {
            for (int i = 0; i < CheatsEnabledByUs.Length; i++)
            {
                CheatsManager.Instance.DisableCheat(CheatsEnabledByUs[i]);
            }

            if (Cheats.IsHydraModeOn)
            {
                CheatsManager.Instance.ToggleCheat(CheatsManager.Instance.GetCheatInstance<KillAllEnemies>());
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
            
            FieldPublisher<CheatsManager, Dictionary<string, ICheat>> idToCheat = new FieldPublisher<CheatsManager, Dictionary<string, ICheat>>(CheatsManager.Instance, "idToCheat");
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
                cheat.Enable(CheatsManager.Instance);
            }
            
            CheatsManager.Instance.RefreshCheatStates();
        }
    }

    public static void Initialize()
    {
        PlayerEvents.PreDeath += PrePlayerDeath;
        PlayerEvents.PostHurt += PostPlayerHurt;
        UpdateEvents.OnFixedUpdate += OnFixedUpdate;
        ScenesEvents.OnSceneWasLoaded += OnSceneWasLoaded;
    }

    private static void OnSceneWasLoaded(int arg1, string arg2)
    {
        CybergrindActive = false;
    }

    private static FixedTimeStamp CybergrindShuffleTimestamp;
    private static void OnFixedUpdate()
    {
        var endlessGrid = LastStartedEndlessGrid;
        if (CybergrindShuffleTimestamp.TimeSince > 6.0f && CybergrindActive && Cheats.IsCheatEnabled(Cheats.CybergrindShuffle))
        {
            PropertyPublisher<EndlessGrid, ArenaPattern[]> currentPatternPool = new PropertyPublisher<EndlessGrid, ArenaPattern[]>(endlessGrid, "CurrentPatternPool");
            FieldPublisher<EndlessGrid, int> currentPatternNum = new FieldPublisher<EndlessGrid, int>(endlessGrid, "currentPatternNum");
            MethodInfo loadPatternMI = typeof(EndlessGrid).GetMethod("LoadPattern", BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo shuffleDecksMI = typeof(EndlessGrid).GetMethod("ShuffleDecks", BindingFlags.Instance | BindingFlags.NonPublic);

            if (currentPatternPool.Value.Length > currentPatternNum.Value)
            {
                loadPatternMI.Invoke(endlessGrid, new object[] {currentPatternPool.Value[currentPatternNum.Value]});
            }
            else
            {
                shuffleDecksMI.Invoke(endlessGrid, null);
                loadPatternMI.Invoke(endlessGrid, new object[] {currentPatternPool.Value[currentPatternNum.Value]});
            }

            currentPatternNum.Value += 1;
            CybergrindShuffleTimestamp.UpdateToNow();
        }
    }

    public static bool CybergrindActive { get; private set; } = false;
    public static bool IsInCybergrind { get => LastStartedEndlessGrid != null; }

    private static bool InvincibilityEnabledByUs = false;

    private static void PrePlayerDeath(NewMovement newMovement, int damage)
    {
        if (Cheats.IsCheatEnabled(Cheats.CybergrindQuickRestart) && IsInCybergrind)
        {
            CheatsManager.Instance.ToggleCheat(CheatsManager.Instance.GetCheatInstance<KillAllEnemies>());
            newMovement.ForceAntiHP(0.0f);
            newMovement.FullHeal(false);
            newMovement.FullStamina();
            MonoSingleton<TimeController>.Instance.SlowDown(0.0f);
            MonoSingleton<TimeController>.Instance.ParryFlash();
            newMovement.antiHpFlash.Flash(1.0f);
            
            if (!CheatsManager.Instance.GetCheatInstance<Invincibility>().IsActive)
            {
                CheatsManager.Instance.ToggleCheat(CheatsManager.Instance.GetCheatInstance<Invincibility>());
                InvincibilityEnabledByUs = true;
            }

            if (damage >= 99)
            {
                newMovement.gameObject.transform.position = new Vector3(0.0f, 100.0f, 64.0f);            
            }

            var endlessGrid = MonoSingleton<EndlessGrid>.Instance;

            if (endlessGrid.enemyAmount > 0)
            {
                endlessGrid.enemyAmount = 0;
                FieldInfo maxPointsFieldInfo = endlessGrid.GetType().GetField("maxPoints", BindingFlags.NonPublic | BindingFlags.Instance);

                var endlessGridMaxPoints = (int)maxPointsFieldInfo.GetValue(endlessGrid);
                endlessGrid.currentWave = Math.Max(endlessGrid.startWave - 1, 0);
                endlessGridMaxPoints = 10;
                for (int i = 1; i <= endlessGrid.currentWave; i++)
                {
                    endlessGridMaxPoints += 3 + i / 3;
                }

                maxPointsFieldInfo.SetValue(endlessGrid, endlessGridMaxPoints);
                endlessGrid.currentWave = CybergrindAdditions.LastStartedEndlessGrid.startWave - 1;
            }
        }
    }

    private static void PostPlayerHurt(NewMovement newMovement, int processedDamage, bool invincible, float scoreLossMultiplier, bool explosion, bool instablack, float hardDamageMultiplier, bool ignoreInvincibility)
    {
        if (InvincibilityEnabledByUs)
        {
            CheatsManager.Instance.ToggleCheat(CheatsManager.Instance.GetCheatInstance<Invincibility>());
        }

        InvincibilityEnabledByUs = false;
    }
}