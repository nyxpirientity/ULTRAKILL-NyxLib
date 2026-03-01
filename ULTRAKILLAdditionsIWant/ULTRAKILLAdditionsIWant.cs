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

        public override void OnInitializeMelon()
        {
            Options.Initialize();
            Log.TraceExpectedInfo($"Initialize called!");
            Assets.Load();
        }

        public override void OnLateInitializeMelon() // Runs after OnApplicationStart.
        {
            PlayerEvents.Initialize();
            Cheats.Initialize();
            Hydra.Initialize();
            Music.Initialize();
            LevelAdditionsManager.Initialize();
            CybergrindAdditions.Initialize();
            MundaneMurder.Initialize();
            BloodOptimizer.Initialize();
            QuickMsgPool.Initialize();
            Heck.Initialize();
            EnemyPrefabManager.Initialize();

            GameConsole.Console.Instance.onError += () =>
            {
                if (Options.ShowErrorNotification.Value)
                {
                    QuickMsgPool.DisplayQuickMsg($"AN ERROR HAS OCCURRED!", Color.red, 3.0f, Vector3.down * 100.0f, 42.0f, false);
                    QuickMsgPool.DisplayQuickMsg($"TIME: {DateTime.Now.Hour}:{DateTime.Now.Minute}", Color.red, 3.0f, Vector3.down * 200.0f, 32.0f, false);
                }
            };
        }


        public override void OnSceneWasLoaded(int buildIndex, string sceneName) // Runs when a Scene has Loaded and is passed the Scene's Build Index and Name.
        {
            Log.TraceExpectedInfo($"------------- New Scene Loaded '{SceneHelper.CurrentScene}:{buildIndex}' -------------");
            TryLog.Action(() => { ScenesEvents.OnSceneWasLoaded?.Invoke(buildIndex, sceneName); });
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName) // Runs when a Scene has Initialized and is passed the Scene's Build Index and Name.
        {
            Log.TraceExpectedInfo($"------------- Scene Initialized '{SceneHelper.CurrentScene}:{sceneName}:{buildIndex}' -------------");
            TryLog.Action(() => { ScenesEvents.OnSceneWasInitialized?.Invoke(buildIndex, sceneName); });
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            Log.TraceExpectedInfo($"------------- Scene Unloaded '{SceneHelper.CurrentScene}:{sceneName}:{buildIndex}' -------------");
            TryLog.Action(() => { ScenesEvents.OnSceneWasUnloaded?.Invoke(buildIndex, sceneName); });
        }

        public override void OnUpdate() // Runs once per frame.
        {
            TryLog.Action(() => { UpdateEvents.OnUpdate?.Invoke(); });

            TryFindQuickLoadLevel();

            if (SceneHelper.CurrentScene == QuickLoadLevel && (SceneHelper.PendingScene == null))
            {
                if (QuickLoadStates[QuickLoadLevel] is LevelQuickLoadState.AwaitingLoad)
                {
                    QuickLoadStates[QuickLoadLevel] = LevelQuickLoadState.WaitingToReturn;
                }
                else if (QuickLoadStates[QuickLoadLevel] is LevelQuickLoadState.WaitingToReturn)
                {
                    Log.TraceExpectedInfo($"{QuickLoadLevel} quick load done!");
                    QuickLoadStates[QuickLoadLevel] = LevelQuickLoadState.Done;
                    QuickLoadLevel = null;
                    QuickLoading = false;

                    if (!TryFindQuickLoadLevel())
                    {
                        SceneHelper.LoadScene(PreQuickLoadLevel);
                        PreQuickLoadLevel = null;
                        CurrentLevelIsFromQuickLoad = false;
                    }
                }
            }
        }

        private bool TryFindQuickLoadLevel()
        {
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
                    if (!CurrentLevelIsFromQuickLoad)
                    {
                        PreQuickLoadLevel = SceneHelper.CurrentScene;
                    }
                    Log.TraceExpectedInfo($"Quickloading {QuickLoadLevel}");
                    SceneHelper.LoadScene(QuickLoadLevel);
                    CurrentLevelIsFromQuickLoad = true;
                    QuickLoadStates[QuickLoadLevel] = LevelQuickLoadState.AwaitingLoad;
                    QuickLoading = true;
                    return true;
                }
            }
            
            return false;
        }

        Dictionary<string, LevelQuickLoadState> QuickLoadStates = new Dictionary<string, LevelQuickLoadState>
        {
            {"Level 0-E", LevelQuickLoadState.Needed},
            {"Level P-1", LevelQuickLoadState.Needed},
            {"Level P-2", LevelQuickLoadState.Needed},
            {"Endless", LevelQuickLoadState.Needed}
        };

        bool QuickLoading = false;
        bool CurrentLevelIsFromQuickLoad = false;
        string QuickLoadLevel = null;
        string PreQuickLoadLevel = null;
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