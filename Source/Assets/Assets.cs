using System.IO;
using HarmonyLib;
using TMPro;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System;
using BepInEx;

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets
{
    [ConfigureSingleton(SingletonFlags.NoAutoInstance)]
    public class AssetsRoot : MonoSingleton<AssetsRoot>
    {
        public static Transform Holder => Instance.AssetHolder.transform;
        public GameObject AssetHolder { get; private set; } = null;

        private void Awake()
        {
            Log.ExpectedInfo("AssetsRoot Awakening!");
            AssetHolder = new GameObject();
            AssetHolder.transform.parent = transform;
            AssetHolder.name = "AssetHolder";
            AssetHolder.SetActive(false);

            SpawnDbPicker.Initialize();
            AssetPickingManager.Initialize();

            gameObject.AddComponent<Gear>();
            gameObject.AddComponent<HookPoints>();
            gameObject.AddComponent<Projectiles>();
            gameObject.AddComponent<UIElements>();
            gameObject.AddComponent<Explosions>();
        }
    }
}