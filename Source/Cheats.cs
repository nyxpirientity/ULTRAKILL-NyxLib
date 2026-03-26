using System;
using HarmonyLib;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
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
            }

            public static void Postfix(CheatsManager __instance)
            {
            }
        }

        public const string RadiantAllEnemies = "nyxpiri.radiant-all-enemies";
        public const string SandAllEnemiesID = "nyxpiri.sand-all-enemies";
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
            ScenesEvents.OnSceneWasLoaded += OnSceneWasLoaded;
            UpdateEvents.OnUpdate += LateUpdate;
        }

        private static void LateUpdate()
        {
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

                if (Cheats.Manager.GetCheatState(Cheats.SandAllEnemiesID))
                {
                    OptionsManager.forceSand = true;
                }

                RegisterCheats();
                WaitingForCheatRegistration = false;
            }
        }

        static bool WaitingForCheatRegistration = false;
        private static void OnSceneWasLoaded(Scene scene, string levelName, string unitySceneName)
        {
            if (Cheats.Manager == null)
            {
                return;
            }

            WaitingForCheatRegistration = true;
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
                    Cheats.SandAllEnemiesID,
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
            // TODO: many of these should probably be their own thing somewhere
            /*Cheats.Manager.RegisterCheat(new ToggleCheat(
                "Disable Stops", 
                Cheats.DisableStops,
                onDisable: (cheat) =>
                {
                },
                onEnable: (cheat, manager) =>
                {
                }
            ), "misc");

            Cheats.Manager.RegisterCheat(new ToggleCheat(
                "Disable Slowdown", 
                Cheats.DisableSlowdown,
                onDisable: (cheat) =>
                {
                },
                onEnable: (cheat, manager) =>
                {
                }
            ), "misc");

            Cheats.Manager.RegisterCheat(new ToggleCheat(
                "ULTRASTOP (possible flashing lights!)", 
                Cheats.UltraStop,
                onDisable: (cheat) =>
                {
                },
                onEnable: (cheat, manager) =>
                {
                }
            ), "???");

            Cheats.Manager.RegisterCheat(new ToggleCheat(
                "Shortened Hitstop", 
                Cheats.ShortHitStop,
                onDisable: (cheat) =>
                {
                },
                onEnable: (cheat, manager) =>
                {
                }
            ), "misc");

            Cheats.Manager.RegisterCheat(new ToggleCheat(
                "Always Play Battle Music", 
                Cheats.AlwaysBattleMusic,
                onDisable: (cheat) =>
                {
                },
                onEnable: (cheat, manager) =>
                {
                }
            ), "music");

            Cheats.Manager.RegisterCheat(new ToggleCheat(
                "Play Clean Music with Battle Music", 
                Cheats.PlayCleanMusicWithBattle,
                onDisable: (cheat) =>
                {
                },
                onEnable: (cheat, manager) =>
                {
                }
            ), "music");*/

            /*Cheats.Manager.RegisterCheat(new ToggleCheat(
                "No Corpses", 
                Cheats.NoCorpses,
                onDisable: (cheat) =>
                {
                },
                onEnable: (cheat, manager) =>
                {
                }
            ), "???");*/

            /*Cheats.Manager.RegisterCheat(new ToggleCheat(
                "Hitstop On Heavy Hydra Load", 
                Cheats.HitstopOnHeavyHydraLoad,
                onDisable: (cheat) =>
                {
                },
                onEnable: (cheat, manager) =>
                {
                }
            ), "???");*/ // why is this a cheat and not a setting?

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

            /*Cheats.Manager.RegisterCheat(new ToggleCheat(
                "dev-button-0", 
                "ukaiw.dev.button-0",
                onDisable: (cheat) =>
                {
                },
                onEnable: (cheat, manager) =>
                {
                    Cheats.Manager.DisableCheat("ukaiw.dev.button-0");
                    var objects = UnityEngine.Object.FindObjectsOfType<TextMeshProUGUI>();
                    HashSet<GameObject> printedObjects = new HashSet<GameObject>(256);
                    foreach (var obj in objects)
                    {
                        if (!obj.text.Contains("INTRUDER"))
                        {
                            continue;
                        }
                        
                        MelonLogger.Msg($"Found {obj.gameObject}!");
                        if (printedObjects.Contains(obj.gameObject.transform.root.gameObject))
                        {
                            continue;
                        }
                        obj.gameObject.transform.root.gameObject.DebugPrintChildren();
                        printedObjects.Add(obj.gameObject.transform.root.gameObject);
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

        [HarmonyPatch(typeof(HeatResistance), "Awake")]
        static class APatch
        {
            public static void Prefix(HeatResistance __instance)
            {

            }
            
            public static void Postfix(HeatResistance __instance)
            {
            }
        }
    }
}