
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
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
            return false;
        }

        if (json.TryGetValue("Properties", out var genericProps) && genericProps is Dictionary<string, object> props)
        {
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
                        if (!TryParseFloat(propValue.ToString(), out var floatVal))
                        {
                            continue;
                        }

                        material.SetFloat(propName, floatVal);
                        break;
                    case MaterialPropertyType.Vector:
                        if (propValue is not List<object> vecElems)
                        {
                            continue;
                        }

                        Vector4 vecVal = default;

                        for (int i = 0; i < Math.Min(4, vecElems.Count); i++)
                        {
                            if (vecElems[i] is float v1)
                            {
                                vecVal[i] = v1;
                            }
                            else if (vecElems[i] is double v2)
                            {
                                vecVal[i] = (float)v2;
                            }
                            else
                            {
                                Log.Warning($"material property '{propName}' is a vector but was set with something other than a float/double it seems");
                                continue;
                            }
                        }
                        break;
                    case MaterialPropertyType.Texture:
                        material.SetTexture(propName, assets.GetAsset<TextureAsset>(propValue.ToString()).Texture);
                        break;
                    default:
                        continue;
                }
            }
        }

        return true;
    }

    private static bool TryParseFloat(string str, out float value) => float.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
}