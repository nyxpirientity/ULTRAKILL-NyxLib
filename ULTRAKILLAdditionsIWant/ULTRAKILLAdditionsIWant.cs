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
using System.Collections.Generic;

namespace UKAIW
{
    public class ULTRAKILLAdditionsIWant : MelonMod
    {
        enum LevelQuickLoadState
        {
            Needed, AwaitingLoad, WaitingToReturn, Returning, Done
        }

        public int NumFixedUpdatesThisScene {get; private set; } = 0;

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


        public override void OnSceneWasLoaded(int buildIndex, string sceneName) // Runs when a Scene has Loaded and is passed the Scene's Build Index and Name.
        {
            Log.TraceExpectedInfo($"------------- New Scene Loaded '{SceneHelper.CurrentScene}:{buildIndex}' -------------");
            TryLog.Action(() => { ScenesEvents.OnSceneWasLoaded?.Invoke(buildIndex, sceneName); });
            NumFixedUpdatesThisScene = 0;
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName) // Runs when a Scene has Initialized and is passed the Scene's Build Index and Name.
        {
            Log.TraceExpectedInfo($"------------- Scene Initialized '{SceneHelper.CurrentScene}:{sceneName}:{buildIndex}' -------------");
            TryLog.Action(() => { ScenesEvents.OnSceneWasInitialized?.Invoke(buildIndex, sceneName); });
            NumFixedUpdatesThisScene = 0;
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            Log.TraceExpectedInfo($"------------- Scene Unloaded '{SceneHelper.CurrentScene}:{sceneName}:{buildIndex}' -------------");
            TryLog.Action(() => { ScenesEvents.OnSceneWasUnloaded?.Invoke(buildIndex, sceneName); });
            NumFixedUpdatesThisScene = 0;
        }

        public override void OnUpdate() // Runs once per frame.
        {
            TryLog.Action(() => { UpdateEvents.OnUpdate?.Invoke(); });

            if ((SceneHelper.PendingScene == null) && !QuickLoading)
            {
                QuickLoadLevel = null;
                foreach (var pair in QuickLoadStates)
                {
                    if (pair.Value is LevelQuickLoadState.Needed)
                    {
                        QuickLoadLevel = pair.Key;
                        break;
                    }
                }
                
                if (QuickLoadLevel != null)
                {
                    Log.TraceExpectedInfo($"Quickloading {QuickLoadLevel}");
                    SceneHelper.LoadScene(QuickLoadLevel);
                    QuickLoadStates[QuickLoadLevel] = LevelQuickLoadState.AwaitingLoad;
                    QuickLoading = true;
                }
            }

            if (SceneHelper.CurrentScene == QuickLoadLevel && (SceneHelper.PendingScene == null))
            {
                if (QuickLoadStates[QuickLoadLevel] is LevelQuickLoadState.AwaitingLoad)
                {
                    QuickLoadStates[QuickLoadLevel] = LevelQuickLoadState.WaitingToReturn;
                }
                else if (QuickLoadStates[QuickLoadLevel] is LevelQuickLoadState.WaitingToReturn)
                {
                    Log.TraceExpectedInfo($"{QuickLoadLevel} quick load done!");
                    SceneHelper.LoadPreviousScene();
                    QuickLoadStates[QuickLoadLevel] = LevelQuickLoadState.Done;
                    QuickLoadLevel = null;
                    QuickLoading = false;
                }
            }
        }

        Dictionary<string, LevelQuickLoadState> QuickLoadStates = new Dictionary<string, LevelQuickLoadState>
        {
            {"Level 0-E", LevelQuickLoadState.Needed}
        };

        bool QuickLoading = false;
        string QuickLoadLevel = null;
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
        }
    }
}