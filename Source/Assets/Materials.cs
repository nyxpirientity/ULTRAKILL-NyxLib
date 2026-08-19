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

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets;

[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class Materials : MonoSingleton<Materials>
{
    public class Properties
    {
        public const string MainTex = "_MainTex";
        public const string Color = "_Color";
        public const string EmissiveColor = "_EmissiveColor";
        public const string Opacity = "_Opacity";
        public const string CustomColor1 = "_CustomColor1";
        public const string CustomColor2 = "_CustomColor2";
        public const string CustomColor3 = "_CustomColor3";
        public const string IDTex = "_IDTex";
    }

    public static Material CreateMaterial(Texture mainTex, Color color, Color emissiveColor, Texture idTex, Color customColor1, Color customColor2, Color customColor3, float opacity)
    {
        var mat = Material.Instantiate(Instance._basicMaterial0);

        mat.SetTexture(Properties.MainTex, mainTex);
        mat.SetColor(Properties.Color, color);
        mat.SetFloat(Properties.Opacity, opacity);
        mat.SetColor(Properties.EmissiveColor, emissiveColor);
        mat.SetColor(Properties.CustomColor1, customColor1);
        mat.SetColor(Properties.CustomColor2, customColor2);
        mat.SetColor(Properties.CustomColor3, customColor3);
        mat.SetTexture(Properties.IDTex, idTex);

        return mat;
    }

    public static Dictionary<string, MaterialPropertyType> SolvePropertyToTypeDictionary(Material mat)
    {
        Dictionary<string, MaterialPropertyType> propToType = [];

        var addProps = (string[] props, MaterialPropertyType type) =>
        {
            foreach (var prop in props)
            {
                propToType[prop] = type;
            }
        };

        addProps(mat.GetPropertyNames(MaterialPropertyType.Float), MaterialPropertyType.Float);
        addProps(mat.GetPropertyNames(MaterialPropertyType.Texture), MaterialPropertyType.Texture);
        addProps(mat.GetPropertyNames(MaterialPropertyType.Vector), MaterialPropertyType.Vector);

        return propToType;
    }

    private Material _basicMaterial0 = null;

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

        _basicMaterial0 = Material.Instantiate(Gear.Piercer.DirectPrefab.GetComponentInChildren<SkinnedMeshRenderer>().material);
    }
}