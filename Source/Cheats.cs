using System;
using HarmonyLib;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public static class Cheats
{
    public delegate void ReadyForCheatRegistrationEventHandler(CheatsManager cheatsManager);
    public static event ReadyForCheatRegistrationEventHandler ReadyForCheatRegistration;

    private static CheatsManager _manager = null;
    public static CheatsManager Manager
    {
        get
        {
            if (_manager == null)
            {
                if (CheatsManager.Instance != null)
                {
                    Log.ExpectedInfo($"Had to get CheatsManager via CheatsManager.Instance (then cached the value)");
                    _manager = CheatsManager.Instance;
                }
            }

            return _manager;
        }
    }

    [HarmonyPatch(typeof(CheatsManager), "Start", new Type[] { })]
    static class CheatsManagerStartPatch
    {

        public static void Prefix(CheatsManager __instance)
        {
            _manager = __instance;
            MaybeWaitForCheatRegistration();
        }

        public static void Postfix(CheatsManager __instance)
        {
            TryRegisterCheats();
        }
    }

    public const string RadiantAllEnemies = "nyxpiri.radiant-all-enemies";
    public const string Immortality = "nyxpiri.immortality";
    public const string SandAllEnemies = "nyxpiri.sand-all-enemies";
    public const string OverrideCybergrindStartingWave = "nyxpiri.override-cybergrind-starting-wave";
    public const string DisableStops = "nyxpiri.disable-stops";
    public const string DisableSlowdown = "nyxpiri.disable-slowdown";
    public const string UltraStop = "nyxpiri.ultra-stop";
    public const string ShortHitStop = "nyxpiri.short-hit-stop";
    public const string PlayCleanMusicWithBattle = "nyxpiri.clean-music-with-battle";
    public const string AlwaysBattleMusic = "nyxpiri.always-battle-music";
    public const string LogEIDInfo = "nyxpiri.dev.log-eid-info";

    public static bool IsCheatEnabled(string cheatID)
    {
        if (!Enabled)
        {
            return false;
        }

        return Cheats.Manager.GetCheatState(cheatID);
    }

    public static bool IsCheatDisabled(string cheatID)
    {
        return !IsCheatEnabled(cheatID);
    }

    public static void Initialize()
    {
        SceneEvents.OnSceneLoad += OnSceneWasLoaded;
        UpdateEvents.OnUpdate += LateUpdate;
        PlayerEvents.PredictedDeath += PlayerPredictedDeath;
    }

    private static void PlayerPredictedDeath(EventMethodCanceler canceler, PlayerComponents player, int damage)
    {
        if (Cheats.IsCheatEnabled(Cheats.Immortality))
        {
            canceler.CancelMethod();
        }
    }

    private static void LateUpdate()
    {
        TryRegisterCheats();
    }

    private static void TryRegisterCheats()
    {
        if (CheatsController.Instance == null)
        {
            return;
        }

        if (WaitingForCheatRegistration)
        {
            if (Cheats.Manager == null)
            {
                return;
            }

            if (Cheats.Manager.GetCheatState(Cheats.RadiantAllEnemies))
            {
                OptionsManager.forceRadiance = true;
            }

            if (Cheats.Manager.GetCheatState(Cheats.SandAllEnemies))
            {
                OptionsManager.forceSand = true;
            }

            WaitingForCheatRegistration = false;

            RegisterCheats();
        }
    }

    static bool WaitingForCheatRegistration = false;
    private static void OnSceneWasLoaded(Scene scene, string levelName, string unitySceneName)
    {
        MaybeWaitForCheatRegistration();
    }

    private static void MaybeWaitForCheatRegistration()
    {
        if (Cheats.Manager == null)
        {
            return;
        }

        WaitingForCheatRegistration = Cheats.Manager.GetCheatInstance<ToggleCheat>() == null;
    }

    public static bool Enabled { get => (CheatsController.Instance?.cheatsEnabled).GetValueOrDefault(false); }

    private static void RegisterCheats()
    {
        if (Options.RegisterHideCheatsStatusCheat.Value)
        {
            Cheats.Manager.RegisterCheat(new HideCheatsStatus(), "meta");
        }

        if (Options.RegisterForceNextWaveCheat.Value)
        {
            Cheats.Manager.RegisterCheat(new ToggleCheat(
                "Force Next Wave",
                "nyxpiri.force-next-cybergrind-wave",
                onDisable: (cheat) =>
                {
                },
                onEnable: (cheat, manager) =>
                {
                    Cheats.Manager.DisableCheat("nyxpiri.force-next-cybergrind-wave");
                    if (Cybergrind.IsActive && Cybergrind.IsInCybergrindLevel)
                    {
                        Cybergrind.EndlessGrid.GetComponent<ActivateNextWave>().deadEnemies = 99999;
                    }
                }
            ), "CYBERGRIND");
        }

        if (Options.RegisterOverrideCybergrindStartingWaveCheat.Value)
        {
            Cheats.Manager.RegisterCheat(new ToggleCheat(
                "Override Starting Wave",
                OverrideCybergrindStartingWave,
                onDisable: (cheat) =>
                {
                },
                onEnable: (cheat, manager) =>
                {
                }
            ), "CYBERGRIND");
        }

        Cheats.Manager.RegisterCheat(new ToggleCheat(
            "Radiant All Enemies",
            Cheats.RadiantAllEnemies,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "SELF SABOTAGE");

        if (Options.RegisterSandAllEnemiesCheat.Value)
        {
            Cheats.Manager.RegisterCheat(new ToggleCheat(
                "Sand All Enemies",
                Cheats.SandAllEnemies,
                onDisable: (cheat) =>
                {
                    OptionsManager.forceSand = false;
                },
                onEnable: (cheat, manager) =>
                {
                    OptionsManager.forceSand = true;
                }
            ), "SELF SABOTAGE");
        }

        if (Options.RegisterImmortalityCheat.Value)
        {
            Cheats.Manager.RegisterCheat(new ToggleCheat(
                "Immortality",
                Cheats.Immortality,
                onDisable: (cheat) =>
                {
                },
                onEnable: (cheat, manager) =>
                {
                }
            ), "I FORGOT");
        }

        /*Cheats.Manager.RegisterCheat(new ToggleCheat(
        "Log Eid Info On Start", 
        Cheats.LogEIDInfo,
        onDisable: (cheat) =>
        {
        },
        onEnable: (cheat, manager) =>
        {
        }
        ), "dev stuff");*/

        /*Cheats.Manager.RegisterCheat(new ToggleCheat(
            "Print the ALL!!!!", 
            "ukaiw.dev.print-all-children",
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
                Cheats.Manager.DisableCheat("ukaiw.dev.print-all-children");
                var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                var roots = scene.GetRootGameObjects();
                foreach (var root in roots)
                {
                    root.DebugPrintChildren();
                }
            }
        ), "dev stuff");*/

        ReadyForCheatRegistration?.Invoke(Manager);
        Cheats.Manager.RebuildMenu();
    }

    [HarmonyPatch(typeof(TeleportCheat), "Teleport")]
    static class TeleportCheatTeleportPatch
    {
        public static void Prefix(TeleportCheat __instance, Transform target)
        {
            if (!Cheats.Enabled)
            {
                return;
            }

            var activator = GameObject.FindAnyObjectByType<PlayerActivator>();
            if (activator != null)
            {
                activator.transform.position = target.position;
            }
        }

        public static void Postfix(TeleportCheat __instance, Transform target)
        {
        }
    }
}