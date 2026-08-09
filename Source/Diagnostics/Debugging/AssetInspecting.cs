using System;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public static class AssetInspecting
{
    public static void DebugPrintMaterialProperties(this Material material)
    {
        foreach (var type in Enum.GetValues(typeof(MaterialPropertyType)))
        {
            foreach (var name in material.GetPropertyNames((MaterialPropertyType)type))
            {
                Log.Message($"Material Debug! There's a {type} named {name}");
            }
        }
    }
}