using System.IO;
using HarmonyLib;
using TMPro;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System;

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets
{
    public static class SpawnDbPicker
    {
        public delegate void OnGettingPrefabsEventHandler(SpawnableObjectsDatabase db);
        public static event OnGettingPrefabsEventHandler OnGettingPrefabs;

        internal static void Initialize()
        {
            SceneEvents.OnSceneLoad += OnSceneWasLoaded;
            LevelQuickLoader.OnQuickLoad += OnNewScene;
        }

        private static void OnSceneWasLoaded(Scene scene, string levelName, string unitySceneName)
        {
            OnNewScene(scene);
        }

        private static void OnNewScene(Scene scene)
        {
            TryGetSpawnMenuPrefabs();
        }

        private static FieldAccess<SpawnMenu, SpawnableObjectsDatabase> spawnableObjectsDbFA = new FieldAccess<SpawnMenu, SpawnableObjectsDatabase>("objects");
        private static bool _spawnMenuPrefabsGotten = false;
        private static void TryGetSpawnMenuPrefabs()
        {
            var db = TryGetSpawnableObjectsDb();

            if (db == null)
            {
                return;
            }

            OnGettingPrefabs?.Invoke(db);
            _spawnMenuPrefabsGotten = true;
        }

        public static SpawnMenu TryGetSpawnMenu()
        {
            if (CanvasController.Instance == null)
            {
                return null;
            }

            return CanvasController.Instance.GetComponentInChildren<SpawnMenu>(includeInactive: true);
        }

        public static SpawnableObjectsDatabase TryGetSpawnableObjectsDb()
        {
            SpawnMenu spawnMenu = TryGetSpawnMenu();

            if (spawnMenu == null)
            {
                return null;
            }

            return spawnableObjectsDbFA.GetValue(spawnMenu);
        }
    }
}