using System;
using MelonLoader;
using UnityEngine;
using HarmonyLib;
using MelonLoader.Utils;
using System.IO;
using System.Globalization;
using ULTRAKILL.Cheats;
using System.Reflection;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Sandbox;
using UKAIW.Diagnostics.Debug;
using GameConsole.Commands;

namespace UKAIW
{
    public class ULTRAKILLAdditionsIWant : MelonMod
    {
        public override void OnInitializeMelon()
        {
            Options.Initialize();
            Log.TraceExpectedInfo($"Initialize called!");
            Assets.Load();
        }

        public override void OnLateInitializeMelon() // Runs after OnApplicationStart.
        {
            Player.Initialize();
            Cheats.Initialize();
            Hydra.Initialize();
            MusicAdditions.Initialize();
            LevelAdditionsManager.Initialize();
            CybergrindAdditions.Initialize();
            MundaneMurder.Initialize();
        }

        public override void OnSceneWasLoaded(int buildindex, string sceneName) // Runs when a Scene has Loaded and is passed the Scene's Build Index and Name.
        {
            TryLog.Action(() => { ScenesEvents.OnSceneWasLoaded?.Invoke(buildindex, sceneName); });
        }

        public override void OnSceneWasInitialized(int buildindex, string sceneName) // Runs when a Scene has Initialized and is passed the Scene's Build Index and Name.
        {
            TryLog.Action(() => { ScenesEvents.OnSceneWasInitialized?.Invoke(buildindex, sceneName); });
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            TryLog.Action(() => { ScenesEvents.OnSceneWasUnloaded?.Invoke(buildIndex, sceneName); });
        }

        public override void OnUpdate() // Runs once per frame.
        {
            TryLog.Action(() => { UpdateEvents.OnUpdate?.Invoke(); });
        }

        public override void OnFixedUpdate() // Can run multiple times per frame. Mostly used for Physics.
        {
            TryLog.Action(() => { UpdateEvents.OnFixedUpdate?.Invoke(); });
        }

        public override void OnLateUpdate() // Runs once per frame after OnUpdate and OnFixedUpdate have finished.
        {
            TryLog.Action(() => { UpdateEvents.OnLateUpdate?.Invoke(); });
        }

        public override void OnGUI() // Can run multiple times per frame. Mostly used for Unity's IMGUI.
        {
            TryLog.Action(() => { UpdateEvents.OnGUI?.Invoke(); });
        }

        public override void OnApplicationQuit() // Runs when the Game is told to Close.
        {

        }

        public override void OnPreferencesSaved() // Runs when Melon Preferences get saved.
        {
        }

        public override void OnPreferencesLoaded() // Runs when Melon Preferences get loaded.
        {
            Options.Reload();
        }
    }
}