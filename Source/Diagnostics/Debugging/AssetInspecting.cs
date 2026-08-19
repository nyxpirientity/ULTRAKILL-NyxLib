using System;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public static class AssetInspecting
{
    public static void DebugPrintMaterialProperties(this Material material)
    {
        Log.Message($"----------========== Material Debug Print of properties for {material.name} (shader: {material.shader.name}) start! ==========----------");

        foreach (var type in Enum.GetValues(typeof(MaterialPropertyType)))
        {
            foreach (var name in material.GetPropertyNames((MaterialPropertyType)type))
            {
                string value = null;
                switch ((MaterialPropertyType)type)
                {
                    case MaterialPropertyType.Float:
                        value = material.GetFloat(name).ToString();
                        break;
                    case MaterialPropertyType.Int:
                        value = material.GetInt(name).ToString();
                        break;
                    case MaterialPropertyType.Vector:
                        value = material.GetVector(name).ToString();
                        break;
                    case MaterialPropertyType.Matrix:
                        value = material.GetMatrix(name).ToString();
                        break;
                    case MaterialPropertyType.Texture:
                        value = (material.GetTexture(name)?.name) ?? "null";
                        break;
                    case MaterialPropertyType.ConstantBuffer:
                        value = $"{material.GetConstantBuffer(name)}";
                        break;
                    case MaterialPropertyType.ComputeBuffer:
                        value = $"{material.GetBuffer(name)}";
                        break;
                }

                Log.Message($"Material Debug! There's a {type} named {name}, with a value of {value}");
            }
        }

        Log.Message($"----------========== Material Debug Print of properties for {material.name} END! ==========----------");
    }

    public static void DebugPrintMaterialKeywords(this Material material)
    {
        Log.Message($"----------========== Material Debug Print of keywords for {material.name} (shader: {material.shader.name}) start! ==========----------");

        foreach (var keyword in material.shader.keywordSpace.keywordNames)
        {
            if (material.IsKeywordEnabled(keyword))
            {
                Log.Message($"{keyword} is ENABLED");
            }
            else
            {
                Log.Message($"{keyword} is DISABLED");
            }
        }

        Log.Message($"----------========== Material Debug Print of keywords for {material.name} END! ==========----------");
    }
}