using System;
using System.Collections.Generic;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine.SceneManagement;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class LevelQuickLoader
    {
        public static void AddQuickLoadLevel(string levelName)
        {
            if (_instance == null)
            {
                _preInitQueue.Enqueue(levelName);
                return;
            }

            _instance.QuickLoadLevel(levelName);
        }

        private static ILevelQuickLoader _instance = null;

        private static Queue<string> _preInitQueue = new Queue<string>();

        public static event ILevelQuickLoader.OnQuickLoadEventHandler OnQuickLoad;

        internal static void Initialize()
        {
            if (Options.DisableQuickLoad.Value)
            {
                Log.Warning($"LevelQuickLoader disabled due to DisableGameInitLevelQuickLoad in config file being true...");
                return;
            }

            Log.DebugInfo($"LevelQuickLoader constructing ILevelQuickLoader based on options...");

            switch (Options.LevelQuickLoaderType.Value)
            {
                case Options.LevelQuickLoaderTypes.Default:
                    _instance = new SimpleLevelQuickLoader();
                    break;
                case Options.LevelQuickLoaderTypes.Simple:
                    _instance = new SimpleLevelQuickLoader();
                    break;
                case Options.LevelQuickLoaderTypes.Additive:
                    _instance = new AdditiveLevelQuickLoader();
                    break;
            }

            foreach (var levelName in _preInitQueue)
            {
                AddQuickLoadLevel(levelName);
            }

            _preInitQueue = null;
            _instance.PreInitQueueAdded();
            _instance.OnQuickLoad += OnQuickLoad;
        }
    }

    public interface ILevelQuickLoader
    {
        public void QuickLoadLevel(string levelName);

        public delegate void OnQuickLoadEventHandler(Scene scene);
        public event OnQuickLoadEventHandler OnQuickLoad;

        internal void PreInitQueueAdded();
    }
}