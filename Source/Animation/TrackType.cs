using System.IO;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib.AnimScript;

public enum KeyFrameType
{
    Bool, Float, Vec2, Vec3, Vec4, Quat
}

public static class TrackTypeMethods
{
    public static KeyFrameType? StringToTrackType(string str)
    {
        return str switch
        {
            "bool" => KeyFrameType.Bool,
            "float" => KeyFrameType.Float,
            "vec2" => KeyFrameType.Vec2,
            "vec3" => KeyFrameType.Vec3,
            "vec4" => KeyFrameType.Vec4,
            "quat" => KeyFrameType.Quat,
            _ => null,
        };
    }

    public static string TrackTypeToScriptString(KeyFrameType type)
    {
        return type switch
        {
            KeyFrameType.Bool => "bool",
            KeyFrameType.Float => "float",
            KeyFrameType.Vec2 => "vec2",
            KeyFrameType.Vec3 => "vec3",
            KeyFrameType.Vec4 => "vec4",
            KeyFrameType.Quat => "quat",
            _ => throw new System.InvalidOperationException(),
        };
    }
}