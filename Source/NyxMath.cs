using System;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public static class NyxMath
{
    public static float NormalizeToRange(this float a, float minimum, float maximum)
    {
        if (maximum == minimum)
        {
            return 1.0f;
        }

        a -= minimum;
        a /= maximum - minimum;

        return a;
    }

    public static float Snapped(this float a, float snapSize)
    {
        return Mathf.Round(a / snapSize) * snapSize;
    }

    public static float SnappedFloor(this float a, float snapSize)
    {
        return Mathf.Floor(a / snapSize) * snapSize;
    }

    public static float SnappedCeil(this float a, float snapSize)
    {
        return Mathf.Ceil(a / snapSize) * snapSize;
    }

    public static float InverseNormalizeToRange(this float a, float minimum, float maximum)
    {
        if (maximum == minimum)
        {
            return 0.0f;
        }

        a -= maximum;
        a /= minimum - maximum;

        return a;
    }

    // ExpDecay functions courtesy of Freya Holmér https://www.youtube.com/watch?v=LSNQuFEDOyQ (preferred over Lerp for ease interps because it's much more framerate independent)
    public static float EaseInterpTo(this float a, float b, float decay, float delta)
    {
        return b + ((a - b) * Mathf.Exp(-decay * delta));
    }

    public static Vector2 EaseInterpTo(this Vector2 a, Vector2 b, float decay, float delta)
    {
        return b + ((a - b) * Mathf.Exp(-decay * delta));
    }

    public static Vector3 EaseInterpTo(this Vector3 a, Vector3 b, float decay, float delta)
    {
        return b + ((a - b) * Mathf.Exp(-decay * delta));
    }

    public static double EaseInterpTo(this double a, double b, double decay, double delta)
    {
        return b + ((a - b) * Math.Exp(-decay * delta));
    }

    public static Quaternion InterpTo(this Quaternion from, Quaternion to, float speed, float delta)
    {
        float angularDist = Quaternion.Angle(from, to);

        if (angularDist <= speed * (float)delta || angularDist is float.NaN)
        {
            return to;
        }

        return Quaternion.Slerp(from, to, ((Math.Min(speed * delta, angularDist)) / angularDist));
    }

    public static bool Coincident(this Vector3 a, Vector3 b, float threshold) => a.sqrMagnitude == 0.0f || b.sqrMagnitude == 0.0f ? (a == b || threshold >= 2.0f) : Vector3.Distance(a.normalized, b.normalized) < threshold;

    public static Vector3 CoincidentProject(this Vector3 a, Vector3 onto)
    {
        var val = Vector3.Project(a, onto);

        if (val.Coincident(onto, 1.0f))
        {
            return val;
        }

        return Vector3.zero;
    }

    public static Vector3 Towards(this Vector3 from, Vector3 to, Vector3 fallback = default)
    {
        var val = (to - from);

        if (val.sqrMagnitude == 0.0f)
        {
            return fallback;
        }

        return val.normalized;
    }
}