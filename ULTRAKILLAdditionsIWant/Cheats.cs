using System;
using System.Collections.Generic;
using System.Diagnostics;
using HarmonyLib;
using MelonLoader;
using TMPro;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using ULTRAKILL.Cheats;
using UnityEngine;

public static class Cheats
{
    public const string OverrideCybergrindStartingWaveID = "ukaiw.cybergrind-start-wave-override";
    public const string CybergrindQuickRestart = "ukaiw.cybergrind-quick-restart";
    public const string RadiantAllEnemies = "ukaiw.radiant-all-enemies";
    public const string SandAllEnemiesID = "ukaiw.sand-all-enemies";
    public const string BossBarAllEnemiesID = "ukaiw.boss-bar-all";
    public const string GiveEnemiesFriends = "ukaiw.give-enemies-friends";
    public const string GiveSelfRadiance = "ukaiw.give-self-radiance";
    public const string HideCheatsHud = "ukaiw.hide-cheats-hud";
    public const string HardDamageRebalance = "ukaiw.hard-damage-rebalance";
    public const string Immortality = "ukaiw.immortality";
    public const string MundaneMurder = "ukaiw.mundane-murder";
    public const string NoCorpses = "ukaiw.no-corpses";
    public const string DisableStops = "ukaiw.disable-stops";
    public const string DisableSlowdown = "ukaiw.disable-slowdown";
    public const string UltraStop = "ukaiw.ultra-stop";
    public const string ShortHitStop = "ukaiw.short-hit-stop";
    public const string HydraMode = "ukaiw.hydra-mode";
    public const string HitstopOnHeavyHydraLoad = "ukaiw.hitstop-on-heavy-hydra-load";
    public const string PlayCleanMusicWithBattle = "ukaiw.clean-music-with-battle";
    public const string AlwaysBattleMusic = "ukaiw.always-battle-music";
    public const string BloodFueledEnemies = "ukaiw.blood-fueled-enemies";
    public const string SaltyEnemies = "ukaiw.salty-enemies";
    public const string DemandingHell = "ukaiw.demanding-hell";
    public const string SelfConscience = "ukaiw.self-conscious-v1";
    public const string CybergrindCheatRandomization = "ukaiw.cybergrind-cheat-randomization";
    public const string HeckPuppets = "ukaiw.heck-puppets";
    public const string BadGyro = "ukaiw.bad-gyro";
    public const string FeedbackerForAll = "ukaiw.feedbacker-for-all";
    public const string CybergrindShuffle = "ukaiw.cybergrind-shuffle";
    public const string StrongerInNumbers = "ukaiw.stronger-in-numbers";
    public const string Tymitosis = "ukaiw.tymitosis";
    public const string SoleNemesis = "ukaiw.sole-nemesis";
    public const string LogEIDInfo = "ukaiw.dev.log-eid-info";

    public static int FriendCount = 0;

    public static bool IsCheatEnabled(string cheatID)
    {
        if (!Enabled)
        {
            return false;
        }
        
        return CheatsManager.Instance.GetCheatState(cheatID);
    }

    public static bool IsCheatDisabled(string cheatID)
    {
        return !IsCheatEnabled(cheatID);
    }

    public static void Initialize()
    {
        ScenesEvents.OnSceneWasLoaded += OnSceneWasLoaded;
        UpdateEvents.OnGUI += OnGUI;
    }

    
    private static void OnGUI()
    {
    }

    private static void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (CheatsManager.Instance == null)
        {
            return;
        }

        if (CheatsManager.Instance.GetCheatState(Cheats.RadiantAllEnemies))
        {
            OptionsManager.forceRadiance = true;
        }

        if (CheatsManager.Instance.GetCheatState(Cheats.SandAllEnemiesID))
        {
            OptionsManager.forceSand = true;
        }

        if (CheatsManager.Instance.GetCheatState(Cheats.BossBarAllEnemiesID))
        {
            OptionsManager.forceBossBars = true;
        }

        if (CheatsManager.Instance.GetCheatInstance<ToggleCheat>() != null)
        {
            return;
        } 

        CheatsManager.Instance.RegisterCheat(new HideCheatsStatus(), "meta");

        RegisterCheats();
    }

    public static bool IsHydraModeOn { get; private set; } = false;
    public static bool Enabled { get => (CheatsController.Instance?.cheatsEnabled).GetValueOrDefault(false); }

    private static void RegisterCheats()
    {
        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Override Cybergrind Starting Wave (unimplemented)", 
            Cheats.OverrideCybergrindStartingWaveID,
            onDisable: (cheat) =>
            {
                
            },
            onEnable: (cheat, manager) =>
            {
                
            }
        ), "CYBERGRIND");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Cybergrind Quick Restart", 
            Cheats.CybergrindQuickRestart,
            onDisable: (cheat) =>
            {
                
            },
            onEnable: (cheat, manager) =>
            {
                
            }
        ), "CYBERGRIND");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Cybergrind Challenger", 
            Cheats.CybergrindCheatRandomization,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "CYBERGRIND");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Force Next Wave", 
            "ukaiw.force-next-cybergrind-wave",
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
                CheatsManager.Instance.DisableCheat("ukaiw.force-next-cybergrind-wave");
                if (CybergrindAdditions.CybergrindActive && CybergrindAdditions.IsInCybergrind)
                {
                    EndlessGrid.Instance.GetComponent<ActivateNextWave>().deadEnemies = 99999;
                }
            }
        ), "CYBERGRIND");

        /*CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Cybergrind Shuffle", 
            Cheats.CybergrindShuffle,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "CYBERGRIND");*/

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Blood Fueled Enemies", 
            Cheats.BloodFueledEnemies,
            onDisable: (cheat) =>
            {
                
            },
            onEnable: (cheat, manager) =>
            {
                
            }
        ), "FAIRNESS AND EQUALITY");
        
        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Feedbackers for Everyone!", 
            Cheats.FeedbackerForAll,
            onDisable: (cheat) =>
            {
                
            },
            onEnable: (cheat, manager) =>
            {
                
            }
        ), "FAIRNESS AND EQUALITY");
        

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Give Enemies Friend(s)", 
            Cheats.GiveEnemiesFriends,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "THOUGHTFULNESS AND CARING");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Radiant All Enemies", 
            Cheats.RadiantAllEnemies,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "SELF SABOTAGE");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
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

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Give all Enemies Boss Bar", 
            Cheats.BossBarAllEnemiesID,
            onDisable: (cheat) =>
            {
                OptionsManager.forceBossBars = false;
            },
            onEnable: (cheat, manager) =>
            {
                OptionsManager.forceBossBars = true;
            }
        ), "SELF SABOTAGE");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Radiance For Thyself", 
            Cheats.GiveSelfRadiance,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "SELF CARE");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Hard Damage Rebalance (unimplemented)", 
            Cheats.HardDamageRebalance,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "EXPERIMENTATION");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Disable Stops", 
            Cheats.DisableStops,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "misc");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Disable Slowdown", 
            Cheats.DisableSlowdown,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "misc");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Immortality (unimplemented)", 
            Cheats.Immortality,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "SELF HATRED");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "ULTRASTOP (possible flashing lights!)", 
            Cheats.UltraStop,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "???");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Shortened Hitstop", 
            Cheats.ShortHitStop,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "misc");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Always Play Battle Music", 
            Cheats.AlwaysBattleMusic,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "music");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Play Clean Music with Battle Music", 
            Cheats.PlayCleanMusicWithBattle,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "music");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "mundanemurder Mode", 
            Cheats.MundaneMurder,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "???");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "No Corpses", 
            Cheats.NoCorpses,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "???");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Hydra Mode", 
            Cheats.HydraMode,
            onDisable: (cheat) =>
            {
                IsHydraModeOn = false;
            },
            onEnable: (cheat, manager) =>
            {
                IsHydraModeOn = true;
            }
        ), "???");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Hitstop On Heavy Hydra Load", 
            Cheats.HitstopOnHeavyHydraLoad,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "???");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Salty Enemies", 
            Cheats.SaltyEnemies,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "???");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Demanding Hell", 
            Cheats.DemandingHell,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
            }
        ), "???");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Self Conscience", 
            Cheats.SelfConscience,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
                
            }
        ), "???");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Heck Puppets", 
            Cheats.HeckPuppets,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
                
            }
        ), "???");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Bad Gyro", 
            Cheats.BadGyro,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
                
            }
        ), "???");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Tymitosis", 
            Cheats.Tymitosis,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
                
            }
        ), "???");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Stronger in Numbers", 
            Cheats.StrongerInNumbers,
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
                
            }
        ), "???");

        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
        "Log Eid Info On Start", 
        Cheats.LogEIDInfo,
        onDisable: (cheat) =>
        {
        },
        onEnable: (cheat, manager) =>
        {
        }
        ), "dev stuff");

        /*CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "Print the ALL!!!!", 
            "ukaiw.dev.print-all-children",
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
                CheatsManager.Instance.DisableCheat("ukaiw.dev.print-all-children");
                var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                var roots = scene.GetRootGameObjects();
                foreach (var root in roots)
                {
                    root.DebugPrintChildren();
                }
            }
        ), "dev stuff");*/

        /*CheatsManager.Instance.RegisterCheat(new ToggleCheat(
            "dev-button-0", 
            "ukaiw.dev.button-0",
            onDisable: (cheat) =>
            {
            },
            onEnable: (cheat, manager) =>
            {
                CheatsManager.Instance.DisableCheat("ukaiw.dev.button-0");
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
    }

    [HarmonyPatch(typeof(TeleportCheat), "Teleport")]
    static class TeleportCheatTeleportPatch
    {
        public static void Prefix(TeleportCheat __instance, Transform target)
        {
            if (!Cheats.Enabled)
            {
                //return; stopped working I noticed so trying commenting out this?
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