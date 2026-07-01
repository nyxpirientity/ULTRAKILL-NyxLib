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
    public static class AssetPickingManager
    {
        public static void AddAssetPicker<ObjectType>(Func<ObjectType, bool> pickerFunc) where ObjectType : UnityEngine.Object
        {
            Func<Scene, bool> picker = (scene) =>
            {
                var assetHolders = UnityEngine.Object.FindObjectsByType<ObjectType>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                List<ObjectType> assetHoldersComps = new List<ObjectType>();

                if (typeof(ObjectType).IsSubclassOf(typeof(Component)) || typeof(ObjectType) == typeof(Component))
                {
                    var gos = scene.GetRootGameObjects();

                    foreach (var go in gos)
                    {
                        assetHoldersComps.AddRange(go.GetComponentsInChildren<ObjectType>());
                    }
                }

                if (assetHoldersComps != null)
                {
                    foreach (var assetHolder in assetHoldersComps)
                    {
                        if (pickerFunc(assetHolder))
                        {
                            return true;
                        }
                    }
                }

                if (assetHolders != null)
                {
                    foreach (var assetHolder in assetHolders)
                    {
                        if (pickerFunc(assetHolder))
                        {
                            return true;
                        }
                    }
                }

                return false;
            };

            _assetPickers.Add(picker);
        }

        private static List<Func<Scene, bool>> _assetPickers = new List<Func<Scene, bool>>(64);

        private static void TryRunAssetPickers(Scene scene)
        {
            if (_assetPickers.Count == 0)
            {
                return;
            }

            for (int i = 0; i < _assetPickers.Count; i++)
            {
                Func<Scene, bool> picker = _assetPickers[i];

                try
                {
                    if (picker(scene))
                    {
                        _assetPickers.RemoveAt(i);
                        i -= 1;
                    }
                }
                catch (System.Exception e)
                {
                    Log.Error($"Caught {e.GetType()} whilst trying to execute an asset picker!\n{e}\n");
                }
            }

            if (_assetPickers.Count == 0)
            {
                Log.TraceExpectedInfo($"[Assets] Success! It seems! All asset pickers seem to be satisfied.");
            }
        }

        internal static void Initialize()
        {
            SceneEvents.OnSceneStart += OnSceneStart;
            LevelQuickLoader.OnQuickLoad += OnNewScene;
        }

        private static void OnSceneStart(Scene scene, string levelName, string unitySceneName)
        {
            OnNewScene(scene);
        }

        private static void OnNewScene(Scene scene)
        {
            TryRunAssetPickers(scene);
        }
    }
}