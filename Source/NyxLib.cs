using System;
using BepInEx;
using UnityEngine;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using HarmonyLib;
using System.IO;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    [BepInPlugin("nyxpiri.ultrakill.nyxlib", "NyxLib", "0.0.1")]
    [BepInProcess("ULTRAKILL.exe")]
    public class NyxLib : BaseUnityPlugin
    {
        protected void Awake()
        {
            Harmony.CreateAndPatchAll(System.Reflection.Assembly.GetAssembly(typeof(NyxLib)));
            Log.Logger = Logger;
            Options.Config = Config;
            Options.Initialize();

            if (!File.Exists(Config.ConfigFilePath))
            {
                Config.Save();
            }
            
            Log.TraceExpectedInfo($"Awake called!");
            Assets.Initialize();
            Cheats.Initialize();
            Log.TraceExpectedInfo($"Awake finished!");
        }

        protected void OnDestroy()
        {
        }

        protected void Start()
        {
            Log.TraceExpectedInfo($"Start called!");
            Music.Initialize();
            LevelAdditionsManager.Initialize();
            Cybergrind.Initialize();
            QuickMsgPool.Initialize();
            Heck.Initialize();
            EnemyPrefabManager.Initialize();
            LevelQuickLoader.Initialize();

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
            Log.TraceExpectedInfo($"Start finished!");
        }

        public void OnSceneWasLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadSceneMode) // Runs when a Scene has Loaded and is passed the Scene's Build Index and Name.
        {
            Log.TraceExpectedInfo($"------------- New Scene Loaded '{SceneHelper.CurrentScene}' -------------");
            TryLog.Action(() => { ScenesEvents.NotifySceneWasLoaded(scene, SceneHelper.CurrentScene, scene.name); });
        }

        private string _prevScene = SceneHelper.CurrentScene;
        public void OnSceneWasUnloaded(UnityEngine.SceneManagement.Scene scene)
        {
            Log.TraceExpectedInfo($"------------- Scene Unloaded '{_prevScene}' -------------");
            TryLog.Action(() => { ScenesEvents.NotifySceneWasUnloaded(scene, _prevScene, scene.name); });
            _prevScene = SceneHelper.CurrentScene;
        }

        protected void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                Config.Reload();
            }
        }

        protected void Update()
        {
            TryLog.Action(() => { UpdateEvents.NotifyUpdate(); });
        }

        protected void FixedUpdate()
        {
            TryLog.Action(() => { UpdateEvents.NotifyFixedUpdate(); });
        }

        protected void LateUpdate() 
        {
            TryLog.Action(() => { UpdateEvents.NotifyLateUpdate(); });
        }
    }
}