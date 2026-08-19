using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nyxpiri.ULTRAKILL.NyxLib;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib.AnimScript;

public static class VariantAnimMath
{
    public static object Lerp(KeyFrameType type, object from, object to, float alpha, float power)
    {
        return _lerpFuncs[type]?.Invoke(from, to, alpha, power);
    }

    public static object Compose(KeyFrameType type, object a, object b)
    {
        return _composeFuncs[type]?.Invoke(a, b);
    }

    public static object Scale(KeyFrameType type, object a, float scalar)
    {
        return _lerpFuncs[type]?.Invoke(null, a, scalar, 1.0f);
    }

    private delegate object AnimLerpFunc(object from, object to, float alpha, float power);
    private static Dictionary<KeyFrameType, AnimLerpFunc> _lerpFuncs = new() {
        { KeyFrameType.Bool, BoolLerp },
        { KeyFrameType.Float, FloatLerp },
        { KeyFrameType.Vec2, Vec2Lerp },
        { KeyFrameType.Vec3, Vec3Lerp },
        { KeyFrameType.Vec4, Vec4Lerp },
        { KeyFrameType.Quat, QuatLerp },
    };

    private delegate object AnimComposeFunc(object a, object b);
    private static Dictionary<KeyFrameType, AnimComposeFunc> _composeFuncs = new() {
      { KeyFrameType.Bool, BoolCompose },
      { KeyFrameType.Float, FloatCompose },
      { KeyFrameType.Vec2, Vec2Compose },
      { KeyFrameType.Vec3, Vec3Compose },
      { KeyFrameType.Vec4, Vec4Compose },
      { KeyFrameType.Quat, QuatCompose },
    };

    public static object BoolCompose(object a, object b)
    {
        a ??= false;
        b ??= false;

        Assert.IsTrue(a is bool);
        Assert.IsTrue(b is bool);

        return (bool)a || (bool)b && !((bool)a && (bool)b);
    }

    public static object FloatCompose(object a, object b)
    {
        a ??= 0.0f;
        b ??= 0.0f;

        Assert.IsTrue(a is float);
        Assert.IsTrue(b is float);

        return (float)a + (float)b;
    }

    public static object Vec2Compose(object a, object b)
    {
        a ??= Vector2.zero;
        b ??= Vector2.zero;

        Assert.IsTrue(a is Vector2);
        Assert.IsTrue(b is Vector2);

        return (Vector2)a + (Vector2)b;
    }

    public static object Vec3Compose(object a, object b)
    {
        a ??= Vector3.zero;
        b ??= Vector3.zero;

        Assert.IsTrue(a is Vector3);
        Assert.IsTrue(b is Vector3);

        return (Vector3)a + (Vector3)b;
    }

    public static object Vec4Compose(object a, object b)
    {
        a ??= Vector4.zero;
        b ??= Vector4.zero;

        Assert.IsTrue(a is Vector4);
        Assert.IsTrue(b is Vector4);

        return (Vector4)a + (Vector4)b;
    }

    public static object QuatCompose(object a, object b)
    {
        a ??= Quaternion.identity;
        b ??= Quaternion.identity;

        Assert.IsTrue(a is Quaternion);
        Assert.IsTrue(b is Quaternion);

        return (Quaternion)a * (Quaternion)b;
    }

    public static object BoolLerp(object from, object to, float alpha, float power)
    {
        if (from == null)
        {
            from = false;
        }

        to ??= false;

        Assert.IsTrue(from is bool);
        Assert.IsTrue(to is bool);

        return Mathf.LerpUnclamped((bool)from ? 1.0f : 0.0f, (bool)to ? 1.0f : 0.0f, Mathf.Pow(alpha, power)) > 0.5f;
    }

    public static object FloatLerp(object from, object to, float alpha, float power)
    {
        if (from == null)
        {
            from = 0.0f;
        }

        to ??= 0.0f;

        Assert.IsTrue(from is float);
        Assert.IsTrue(to is float);

        return Mathf.LerpUnclamped((float)from, (float)to, Mathf.Pow(alpha, power));
    }

    public static object Vec2Lerp(object from, object to, float alpha, float power)
    {
        if (from == null)
        {
            from = Vector2.zero;
        }

        to ??= Vector2.zero;

        Assert.IsTrue(from is Vector2);
        Assert.IsTrue(to is Vector2);

        return Vector2.LerpUnclamped((Vector2)from, (Vector2)to, Mathf.Pow(alpha, power));
    }

    public static object Vec3Lerp(object from, object to, float alpha, float power)
    {
        if (from == null)
        {
            from = Vector3.zero;
        }

        to ??= Vector3.zero;

        Assert.IsTrue(from is Vector3);
        Assert.IsTrue(to is Vector3);

        return Vector3.LerpUnclamped((Vector3)from, (Vector3)to, Mathf.Pow(alpha, power));
    }

    public static object Vec4Lerp(object from, object to, float alpha, float power)
    {
        if (from == null)
        {
            from = Vector4.zero;
        }

        to ??= Vector4.zero;

        Assert.IsTrue(from is Vector4);
        Assert.IsTrue(to is Vector4);

        return Vector4.LerpUnclamped((Vector4)from, (Vector4)to, Mathf.Pow(alpha, power));
    }

    public static object QuatLerp(object from, object to, float alpha, float power)
    {
        if (from == null)
        {
            from = Quaternion.identity;
        }

        to ??= Quaternion.identity;

        Assert.IsTrue(from is Quaternion);
        Assert.IsTrue(to is Quaternion);

        return Quaternion.SlerpUnclamped((Quaternion)from, (Quaternion)to, Mathf.Pow(alpha, power));
    }
}