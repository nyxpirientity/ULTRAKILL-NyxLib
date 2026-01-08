using System;
using System.Collections.Generic;
using UKAIW;
using ULTRAKILL.Cheats;

public static class Cheats
{
    public const string OverrideCybergrindStartingWaveID = "ukaiw.cybergrind-start-wave-override";
    public const string CybergrindQuickRestartID = "ukaiw.cybergrind-quick-restart";
    public const string RadiantAllEnemiesID = "ukaiw.radiant-all-enemies";
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

    public static int FriendCount = 0;

    public static bool IsCheatEnabled(string cheatID)
    {
        return CheatsManager.Instance.GetCheatState(cheatID);
    }

    public static void Initialize()
    {
        ScenesEvents.OnSceneWasLoaded += OnSceneWasLoaded;
    }

    private static void OnSceneWasLoaded(int buildINdex, string sceneName)
    {
        if (CheatsManager.Instance == null)
        {
            return;
        }

        if (CheatsManager.Instance.GetCheatState(Cheats.RadiantAllEnemiesID))
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
        FriendCheat.Reset();
    }

    public static bool IsHydraModeOn { get; private set; } = false;

    private static void RegisterCheats()
    {
        CheatsManager.Instance.RegisterCheat(new ToggleCheat(
        "Override Cybergrind Starting Wave", 
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
        Cheats.CybergrindQuickRestartID,
        onDisable: (cheat) =>
        {
            
        },
        onEnable: (cheat, manager) =>
        {
            
        }
    ), "CYBERGRIND");

    CheatsManager.Instance.RegisterCheat(new ToggleCheat(
        "Radiant All Enemies", 
        Cheats.RadiantAllEnemiesID,
        onDisable: (cheat) =>
        {
            OptionsManager.forceRadiance = false;
        },
        onEnable: (cheat, manager) =>
        {
            OptionsManager.forceRadiance = false;
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
        "Hard Damage Rebalance", 
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
        "Immortality", 
        Cheats.Immortality,
        onDisable: (cheat) =>
        {
        },
        onEnable: (cheat, manager) =>
        {
        }
    ), "SELF HATRED");

    CheatsManager.Instance.RegisterCheat(new ToggleCheat(
        "ULTRASTOP", 
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
    }
}