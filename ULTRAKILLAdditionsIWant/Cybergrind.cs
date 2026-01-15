using System;
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
        }
    }

    public static void Initialize()
    {
        Player.PreDeath += PrePlayerDeath;
        Player.PostHurt += PostPlayerHurt;
    }

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

            var endlessGrid = MonoSingleton<EndlessGrid>.Instance;
            endlessGrid.enemyAmount = 0;
            FieldInfo maxPointsFieldInfo = endlessGrid.GetType().GetField("maxPoints", BindingFlags.NonPublic | BindingFlags.Instance);

            if (damage >= 99)
            {
                newMovement.gameObject.transform.position = new Vector3(0.0f, 100.0f, 64.0f);            
            }

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

    private static void PostPlayerHurt(NewMovement newMovement, int processedDamage, bool invincible, float scoreLossMultiplier, bool explosion, bool instablack, float hardDamageMultiplier, bool ignoreInvincibility)
    {
        if (InvincibilityEnabledByUs)
        {
            CheatsManager.Instance.ToggleCheat(CheatsManager.Instance.GetCheatInstance<Invincibility>());
        }

        InvincibilityEnabledByUs = false;
    }
}