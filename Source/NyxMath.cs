using System;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class NyxMath
    {
        public static float NormalizeToRange(float a, float minimum, float maximum)
        {
            if (maximum == minimum)
            {
                return 1.0f;
            }

            a -= minimum;
            a /= maximum - minimum;

            return a;
        }

        public static float InverseNormalizeToRange(float a, float minimum, float maximum)
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
        public static float EaseInterpTo(float a, float b, float decay, float delta)
        {
            return b + ((a - b) * Mathf.Exp(-decay * delta));
        }
            
        public static Vector2 EaseInterpTo(Vector2 a, Vector2 b, float decay, float delta)
        {
            return b + ((a - b) * Mathf.Exp(-decay * delta));
        }

        public static Vector3 EaseInterpTo(Vector3 a, Vector3 b, float decay, float delta)
        {
            return b + ((a - b) * Mathf.Exp(-decay * delta));
        }

        public static double EaseInterpTo(double a, double b, double decay, double delta)
        {
            return b + ((a - b) * Math.Exp(-decay * delta));
        }
    }
}