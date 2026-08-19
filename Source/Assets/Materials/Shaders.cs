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
public class Shaders : MonoSingleton<Shaders>
{
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
        Shaders.StandardLit = gunColorGetter.coloredMaterials[0].shader;
        Shaders.StandardUnlit = Projectiles.Core.DirectPrefab.GetComponentInChildren<Renderer>().sharedMaterial.shader;
    }

    public static Shader StandardLit { get; internal set; } = null;
    public static Shader StandardUnlit { get; internal set; } = null;
}