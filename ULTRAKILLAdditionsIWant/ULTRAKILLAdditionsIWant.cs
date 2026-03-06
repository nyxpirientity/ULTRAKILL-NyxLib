using System;
using BepInEx;
using UnityEngine;
using UKAIW.Diagnostics.Debug;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace UKAIW
{
    [BepInPlugin("com.nyxpiri.bepinex.plugins.ultrakill.ukaiw", "UKAIW", "0.0.0.1")]
    [BepInProcess("ULTRAKILL.exe")]
    public class ULTRAKILLAdditionsIWant : BaseUnityPlugin
    {
        enum LevelQuickLoadState
        {
            Needed, AwaitingLoad, WaitingToReturn, Returning, Done
        }

        protected void Awake()
        {
            Log.Logger = Logger;
            Options.Config = Config;
            Options.Initialize();
            Log.TraceExpectedInfo($"Initialize called!");
            Assets.Load();
        }

        protected void Start()
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
            ParryabilityTracker.Initialize();

            if (Options.DisableQuickLoad.Value)
            {
                QuickLoadStates.Clear();
            }

            GameConsole.Console.Instance.onError += () =>
            {
                if (Options.ShowErrorNotification.Value)
                {
                    QuickMsgPool.DisplayQuickMsg($"AN ERROR HAS OCCURRED!", Color.red, 3.0f, Vector3.down * 100.0f, 42.0f, false);
                    QuickMsgPool.DisplayQuickMsg($"TIME: {DateTime.Now.Hour}:{DateTime.Now.Minute}", Color.red, 3.0f, Vector3.down * 200.0f, 32.0f, false);
                }
            };

            SceneManager.sceneLoaded += OnSceneWasLoaded;
            SceneManager.sceneUnloaded += OnSceneWasUnloaded;
        }


        public void OnSceneWasLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadSceneMode) // Runs when a Scene has Loaded and is passed the Scene's Build Index and Name.
        {
            Log.TraceExpectedInfo($"------------- New Scene Loaded '{SceneHelper.CurrentScene}' -------------");
            TryLog.Action(() => { ScenesEvents.OnSceneWasLoaded?.Invoke(scene, SceneHelper.CurrentScene); });
        }

        public void OnSceneWasUnloaded(UnityEngine.SceneManagement.Scene scene)
        {
            Log.TraceExpectedInfo($"------------- Scene Unloaded '{SceneHelper.CurrentScene}' -------------");
            TryLog.Action(() => { ScenesEvents.OnSceneWasUnloaded?.Invoke(scene, SceneHelper.CurrentScene); });
        }

        protected void OnUpdate()
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
            {"Level 4-4", LevelQuickLoadState.Needed},
            {"Level 0-E", LevelQuickLoadState.Needed},
            {"Level P-1", LevelQuickLoadState.Needed},
            {"Level P-2", LevelQuickLoadState.Needed},
            {"Endless", LevelQuickLoadState.Needed}
        };

        bool QuickLoading = false;
        bool CurrentLevelIsFromQuickLoad = false;
        string QuickLoadLevel = null;
        string PreQuickLoadLevel = null;
        protected void OnFixedUpdate()
        {
            TryLog.Action(() => { UpdateEvents.OnFixedUpdate?.Invoke(); });
        }

        protected void OnLateUpdate() 
        {
            TryLog.Action(() => { UpdateEvents.OnLateUpdate?.Invoke(); });
        }
    }
}