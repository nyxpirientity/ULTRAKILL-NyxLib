using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Nyxpiri.ULTRAKILL.NyxLib;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class Cybergrind
    {
        // calling EndlessGrid.Instance after endless grid has been not null once but is now null seems to cause lots of lag for some reason.
        public static EndlessGrid EndlessGrid
        {
            get
            {
                if (_endlessGrid != null)
                {
                    return _endlessGrid;
                }

                _endlessGrid = EndlessGrid.Instance;
                return _endlessGrid;
            }

            set
            {
                _endlessGrid = value;
            }
        }
        private static EndlessGrid _endlessGrid = null;

        public delegate void CybergrindPreBeginEventHandler(EventMethodCanceler canceler, EndlessGrid endlessGrid);
        public static event CybergrindPreBeginEventHandler PreCybergrindBegin;
        public delegate void CybergrindPostBeginEventHandler(EventMethodCancelInfo cancelInfo, EndlessGrid endlessGrid);
        public static event CybergrindPostBeginEventHandler PostCybergrindBegin;

        public delegate void CybergrindPreNextWaveEventHandler(EventMethodCanceler canceler, EndlessGrid endlessGrid);
        public static event CybergrindPreNextWaveEventHandler PreCybergrindNextWave;
        public delegate void CybergrindPostNextWaveEventHandler(EventMethodCancelInfo cancelInfo, EndlessGrid endlessGrid);
        public static event CybergrindPostNextWaveEventHandler PostCybergrindNextWave;

        [HarmonyPatch(typeof(EndlessGrid), "OnTriggerEnter", new Type[] { typeof(Collider) })]
        static class CybergrindStartPatch
        {

            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
            
            public static bool Prefix(EndlessGrid __instance, Collider other)
            {
                if (!other.CompareTag("Player"))
                {
                    return true;
                }

                _cancellationTracker.Reset();
                PreCybergrindBegin?.Invoke(_cancellationTracker.GetCanceler(), __instance);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }

            public static void Postfix(EndlessGrid __instance, Collider other)
            {
                if (!other.CompareTag("Player"))
                {
                    return;
                }

                PostCybergrindBegin?.Invoke(_cancellationTracker.GetCancelInfo(), __instance);

                IsActive = true;
            }
        }

        [HarmonyPatch(typeof(EndlessGrid), "Start")]
        static class EndlessGridStartPatch
        {
            public static void Prefix(EndlessGrid __instance)
            {
            }

            public static void Postfix(EndlessGrid __instance)
            {
                EndlessGrid = __instance;
            }
        }

        [HarmonyPatch(typeof(EndlessGrid), "NextWave", null)]
        static class EndlessGridNextWavePatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(EndlessGrid __instance)
            {
                _cancellationTracker.Reset();
                PreCybergrindNextWave?.Invoke(_cancellationTracker.GetCanceler(), __instance);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }

            public static void Postfix(EndlessGrid __instance)
            {
                PostCybergrindNextWave?.Invoke(_cancellationTracker.GetCancelInfo(), __instance);
            }
        }

        public static void Initialize()
        {
            UpdateEvents.OnFixedUpdate += OnFixedUpdate;
            ScenesEvents.OnSceneWasLoaded += OnSceneWasLoaded;
            ScenesEvents.OnSceneWasUnloaded += OnSceneWasUnloaded;
        }

        private static void OnSceneWasUnloaded(Scene scene, string levelName, string unitySceneName)
        {
            IsActive = false;
        }

        private static void OnSceneWasLoaded(Scene scene, string levelName, string unitySceneName)
        {
            IsActive = false;
        }

        private static void OnFixedUpdate()
        {
        }

        public static bool IsActive { get; private set; } = false;
        public static bool IsInCybergrindLevel { get => EndlessGrid != null; }
    }
}