using System.IO;
using HarmonyLib;
using TMPro;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Rendering;

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets;

[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class Textures : MonoSingleton<Textures>
{
    public static Texture CubeMapStudio06 => Instance._cubeMapStudio06;

    private void Awake()
    {
        SceneEvents.OnSceneStart += OnNewSceneStart;
    }

    private void OnNewSceneStart(Scene scene, string levelName, string unitySceneName)
    {
        if (!Gear.AssetsLoaded)
        {
            return;
        }

        var gunColorGetter = Gear.Piercer.DirectPrefab.GetComponentInChildren<GunColorGetter>();
        _cubeMapStudio06 = gunColorGetter.coloredMaterials[0].GetTexture("_CubeTex");
    }

    private Texture _cubeMapStudio06 = null;
}