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
public class Materials : MonoSingleton<Materials>
{
    public static Material CreateLitMaterial()
    {
        var material = new Material(Shaders.StandardLit);

        material.SetTexture(MaterialProperties.Textures.CubeTex, Textures.CubeMapStudio06);

        return material;
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
}
