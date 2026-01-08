using System;
using System.Reflection;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using ULTRAKILL.Cheats;
using UKAIW.Diagnostics.Debug;

namespace UKAIW
{
    public static class Player
    {
        public static Action<NewMovement> PreDeath = null;
        public static Action<NewMovement, int, bool, float, bool, bool, float, bool> PreHurt = null;
        public static Action<NewMovement, int, bool, float, bool, bool, float, bool> PostHurt = null;

        public static void Initialize()
        {
            Log.TraceExpectedInfo($"Player.Initialize called!");
            ScenesEvents.OnSceneWasLoaded += OnSceneWasLoaded;
        }

        private static void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            PreDeath = null;
            
            var player = MonoSingleton<NewMovement>.Instance;
            
            player?.gameObject.AddComponent<PlayerAdditions>();
        }
    }

    [HarmonyPatch(typeof(NewMovement), "GetHurt", new Type[] { typeof(int), typeof(bool), typeof(float), typeof(bool), typeof(bool), typeof(float), typeof(bool) })]
    static class PlayerHurtPatch
    {
        private static bool wasPreHurtCalled = false;
        private static int processedDamage = 0;
        public static void Prefix(NewMovement __instance, int damage, bool invincible, float scoreLossMultiplier = 1f, bool explosion = false, bool instablack = false, float hardDamageMultiplier = 0.35f, bool ignoreInvincibility = false)
        {
            NewMovement newMovement = __instance;
            if (newMovement.dead || newMovement.levelOver || !(!invincible || newMovement.gameObject.layer != 15 || ignoreInvincibility) || damage <= 0)
            {
                return;
            }

            if (newMovement.asscon.majorEnabled)
            {
                damage = Mathf.RoundToInt((float)damage * newMovement.asscon.damageTaken);
            }

            if (Invincibility.Enabled)
            {
                damage = 0;
            }

            processedDamage = damage;
            Player.PreHurt?.Invoke(newMovement, damage, invincible, scoreLossMultiplier, explosion, instablack, hardDamageMultiplier, ignoreInvincibility);

            wasPreHurtCalled = true;

            if (CheatsManager.Instance.GetCheatState(Cheats.GiveSelfRadiance))
            {
                newMovement.hp += (int)((damage / 1.5f));
                newMovement.antiHp -= (float)damage * (hardDamageMultiplier / 1.5f);
            }

            bool mortal = !Invincibility.Enabled && !Cheats.IsCheatEnabled(Cheats.Immortality); 

            if (newMovement.hp - damage <= 0 && mortal)
            {
                Player.PreDeath?.Invoke(newMovement);
            }
        }

        public static void Postfix(NewMovement __instance, int damage, bool invincible, float scoreLossMultiplier = 1f, bool explosion = false, bool instablack = false, float hardDamageMultiplier = 0.35f, bool ignoreInvincibility = false)
        {
            NewMovement newMovement = __instance;
            
            if (!wasPreHurtCalled)
            {
                return;
            }

            Player.PostHurt?.Invoke(newMovement, processedDamage, invincible, scoreLossMultiplier, explosion, instablack, hardDamageMultiplier, ignoreInvincibility);
        }
    }

    public class PlayerAdditions : MonoBehaviour
    {
        NewMovement Player = null;
        int Difficulty = 0;

        private void Start()
        {
            Player = gameObject.GetComponent<NewMovement>();

            Difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty");
        }

        private int PrevStylePoints = MonoSingleton<StatsManager>.Instance.stylePoints;
        private float PrevAntiHpCooldown = 0.0f;
        private float PrevAntiHp = 0.0f;

        private void Update()
        {
            try
            {
                UnsafeUpdate();
            }
            catch (System.Exception e)
            {
                MelonLogger.Error(e.ToString());
            }
        }

        private void UnsafeUpdate()
        {
            var stats = MonoSingleton<StatsManager>.Instance;
            
            if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.WasPerformedThisFrame && Player.activated && !Player.slowMode && !GameStateManager.Instance.PlayerInputLocked)
            {
                if (CheatsManager.Instance.GetCheatState(Cheats.GiveSelfRadiance))
                {
                    Player.boostCharge += 50.0f;
                }
            }

            if (CheatsManager.Instance.GetCheatState(Cheats.GiveSelfRadiance))
            {
                if (MonoSingleton<FistControl>.Instance.fistCooldown > -1f)
                {
                    MonoSingleton<WeaponCharges>.Instance.punchStamina = Mathf.MoveTowards(MonoSingleton<WeaponCharges>.Instance.punchStamina, 2f, Time.deltaTime * 0.625f);
                }
            }
            else
            {
            }

            FieldInfo antiHpCooldownFI = typeof(NewMovement).GetField("antiHpCooldown", BindingFlags.NonPublic | BindingFlags.Instance);
            var antiHpCooldown = (float)antiHpCooldownFI.GetValue(Player);

            if (CheatsManager.Instance.GetCheatState(Cheats.HardDamageRebalance))
            {
                float spDeltaF = stats.stylePoints - PrevStylePoints;
                
                if (spDeltaF > 0.0f)
                {
                    int rankIndex = MonoSingleton<StyleHUD>.Instance.rankIndex;
                    float antiHpHeal = Mathf.Pow(spDeltaF * 0.04f, 1.1f);
                    float rankMulti = Mathf.Pow((rankIndex + 1.0f) * 0.4f, 1.1f) + 1.0f;
                    antiHpHeal *= rankMulti;
                    Player.antiHp -= Mathf.Max(Player.antiHp, antiHpHeal) * 0.1f;
                }

                float antiHpDelta = Player.antiHp - PrevAntiHp;
                float antiHpCooldownDelta = antiHpCooldown - PrevAntiHpCooldown;
                
                if (antiHpDelta <= 0.0f)
                {
                    Player.antiHp += Mathf.Max(0.0f, antiHpDelta * -0.7f);
                }
                else
                {
                    Player.antiHp += Mathf.Max(0.0f, antiHpDelta * ( 0.9f * ((Difficulty + 1.0f) / 6.5f)));
                }
                
                antiHpCooldown += Mathf.Max(0.0f, antiHpCooldownDelta * -0.65f);
            }
            
            if (Cheats.IsCheatEnabled(Cheats.MundaneMurder))
            {
                
            }
            
            antiHpCooldownFI.SetValue(Player, antiHpCooldown);
            PrevAntiHpCooldown = antiHpCooldown;
            PrevAntiHp = Player.antiHp;
            PrevStylePoints = stats.stylePoints;
        }
    }
}