
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets;

public static class JsonMaterial
{
    public static bool ApplyTo(Material material, ExternalAssetManager assets, string jsonText)
    {
        var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonText);

        if (json == null)
        {
            Log.Error($"Failed to parse json material due to json deserialize failure");
            return false;
        }

        if (json.TryGetValue("Keywords", out var genericKeywords))
        {
            if (genericKeywords is not JObject keywords)
            {
                Log.Error($"genericKeywords was a {genericKeywords.GetType()}?");
                return false;
            }

            foreach (var pair in keywords)
            {
                Log.Message($"{pair.Key} = {((bool?)pair.Value).GetValueOrDefault()}");

                if (((bool?)pair.Value).GetValueOrDefault())
                {
                    material.EnableKeyword(pair.Key);
                }
                else
                {
                    material.DisableKeyword(pair.Key);
                }
            }
        }

        if (json.TryGetValue("Properties", out var genericProps))
        {
            if (genericProps is not JObject props)
            {
                Log.Error($"genericProps was a {genericProps.GetType()}?");
                return false;
            }

            var propToType = Materials.SolvePropertyToTypeDictionary(material);

            foreach (var pair in props)
            {
                var propName = pair.Key;
                var propValue = pair.Value;

                if (!propToType.TryGetValue(propName, out var propType))
                {
                    Log.Warning($"Unknown material property '{propName}'");
                    continue;
                }

                switch (propType)
                {
                    case MaterialPropertyType.Float:
                        material.SetFloat(propName, ((float?)propValue).GetValueOrDefault());
                        Log.Message($"{propName} = {((float?)propValue).GetValueOrDefault()}");
                        break;
                    case MaterialPropertyType.Vector:
                        if (propValue is not JArray vecElems)
                        {
                            Log.Warning($"Failed to parse vector for material property '{propName}'");
                            continue;
                        }

                        Vector4 vecVal = default;

                        for (int i = 0; i < Math.Min(4, vecElems.Count); i++)
                        {
                            vecVal[i] = ((float?)vecElems[i]).GetValueOrDefault();
                            Log.Message($"{propName}[{i}] = {vecVal[i]}");
                        }
                        material.SetVector(propName, vecVal);
                        break;
                    case MaterialPropertyType.Texture:
                        material.SetTexture(propName, assets.GetAsset<TextureAsset>(propValue.ToString()).Texture);
                        break;
                    default:
                        Log.Warning($"incompatible material property '{propName}'");
                        continue;
                }
            }
        }
        else
        {
            Log.Warning($"No Properties entry in json material");
        }

        return true;
    }

    private static bool TryParseFloat(string str, out float value) => float.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
}