using System;
using System.Text;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public static class AssetInspecting
{
    public static void DebugPrintMaterialProperties(this Material material)
    {
        Log.Message($"----------========== Material Debug Print of properties for {material.name} (shader: {material.shader.name}) start! ==========----------");
        StringBuilder formattedBuilder = new StringBuilder();
        foreach (var type in Enum.GetValues(typeof(MaterialPropertyType)))
        {
            foreach (var name in material.GetPropertyNames((MaterialPropertyType)type))
            {
                string value = null;
                string formattedValue = null;
                switch ((MaterialPropertyType)type)
                {
                    case MaterialPropertyType.Float:
                        value = material.GetFloat(name).ToString();
                        formattedValue = value;
                        break;
                    case MaterialPropertyType.Int:
                        value = material.GetInt(name).ToString();
                        formattedValue = value;
                        break;
                    case MaterialPropertyType.Vector:
                        var vec = material.GetVector(name);
                        value = vec.ToString();
                        formattedValue = $"[{vec.x}, {vec.y}, {vec.z}, {vec.w}]";
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

                if (formattedValue != null)
                {
                    formattedBuilder.AppendLine($"\"{name}\": {formattedValue},");
                }
            }
        }

        formattedBuilder.Remove(formattedBuilder.Length - 2, 1);

        Log.Message($"------ FORMATTED VERSION START! -------");
        Log.Message($"{formattedBuilder}");
        Log.Message($"------ FORMATTED VERSION END! -------");

        Log.Message($"----------========== Material Debug Print of properties for {material.name} END! ==========----------");
    }

    public static void DebugPrintMaterialKeywords(this Material material)
    {
        Log.Message($"----------========== Material Debug Print of keywords for {material.name} (shader: {material.shader.name}) start! ==========----------");

        var kc = material.shader.keywordSpace.keywordNames.Length;
        for (int i = 0; i < kc; i++)
        {
            string keyword = material.shader.keywordSpace.keywordNames[i];
            if (material.IsKeywordEnabled(keyword))
            {
                Log.Message($"\"{keyword}\": true{(i == kc - 1 ? "" : ",")}");
            }
            else
            {
                Log.Message($"\"{keyword}\": false{(i == kc - 1 ? "" : ",")}");
            }
        }

        Log.Message($"----------========== Material Debug Print of keywords for {material.name} END! ==========----------");
    }

    public static void DebugPrintMaterialShaderPasses(this Material material)
    {
        Log.Message($"----------========== Material Debug Print of passes for {material.name} (shader: {material.shader.name}) start! ==========----------");

        for (int i = 0; i < material.passCount; i++)
        {
            var pass = material.GetPassName(i);
            if (material.GetShaderPassEnabled(pass))
            {
                Log.Message($"{pass} [{i}] is ENABLED");
            }
            else
            {
                Log.Message($"{pass} [{i}] is DISABLED");
            }
        }

        Log.Message($"{material.shader.passCount} shader passes");

        Log.Message($"----------========== Material Debug Print of passes for {material.name} END! ==========----------");
    }
}