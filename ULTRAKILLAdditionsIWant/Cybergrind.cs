using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using ULTRAKILL.Cheats;
using UnityEngine;

public static class CybergrindAdditions
{
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

    [HarmonyPatch(typeof(EndlessGrid), "NextWave", null)]
    static class EndlessGridNextWavePatch
    {
        public static void Prefix(EndlessGrid __instance)
        {
            if (Cheats.IsCheatDisabled(Cheats.CybergrindCheatRandomization))
            {
                return;
            }

            ValueTuple<int, string>[] cheats =
            {
                (1, Cheats.BloodFueledEnemies),
                (1, Cheats.DemandingHell),
                (1, Cheats.HeckPuppets),
                (1, Cheats.SaltyEnemies),
                (1, Cheats.HydraMode),
                (1, Cheats.GiveEnemiesFriends),
                (1, Cheats.RadiantAllEnemies),
                (1, Cheats.SelfConscience),
                (1, Cheats.MundaneMurder),
                (1, Cheats.SandAllEnemiesID),
                (1, Cheats.BadGyro),
            };

            int numRandomCheats = Options.NumRandomCheats.Value;
            numRandomCheats = Math.Min(cheats.Length, numRandomCheats);

            for (int i = 0; i < cheats.Length; i++)
            {
                CheatsManager.Instance.DisableCheat(cheats[i].Item2);
            }
            
            FieldPublisher<CheatsManager, Dictionary<string, ICheat>> idToCheat = new FieldPublisher<CheatsManager, Dictionary<string, ICheat>>(CheatsManager.Instance, "idToCheat");

            for (int i = 0,j = 0; i < numRandomCheats; i++,j++)
            {
                int cheatIdx = UnityEngine.Random.Range(0, cheats.Length);
                var cheat = idToCheat.Value[cheats[cheatIdx].Item2];
                
                if (cheat.IsActive && j < 20)
                {
                    i--;
                    continue;
                }
                else if (cheat.IsActive)
                {
                    continue;
                }

                QuickMsgPool.DisplayQuickMsg($"+ {cheat.LongName.ToUpper()}", new Color(0.875f, 0.75f, 1.0f), 5.0f, velocity: Vector3.down * ((float)((i) * 120.0f) + 200.0f));
                cheat.Enable(CheatsManager.Instance);
            }

            CheatsManager.Instance.RefreshCheatStates();
        }

        public static void Postfix(EndlessGrid __instance)
        {
        }
    }

    public static void Initialize()
    {
        Player.PreDeath += PrePlayerDeath;
        Player.PostHurt += PostPlayerHurt;
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
        var endlessGrid = EndlessGrid.Instance;
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
    public static bool IsInCybergrind { get => EndlessGrid.Instance != null; }

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
                endlessGrid.currentWave = EndlessGrid.Instance.startWave - 1;
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