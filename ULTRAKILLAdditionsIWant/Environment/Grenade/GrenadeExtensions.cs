using System;
using System.Reflection;
using HarmonyLib;
using ULTRAKILL.Portal;
using UnityEngine;

namespace UKAIW
{
    public static class GrenadeExtensions
    {
        private static FieldInfo _explodedFi = typeof(Grenade).GetField("exploded", BindingFlags.Instance | BindingFlags.NonPublic);
        public static bool IsExploded(this Grenade self)
        {
            return (bool)_explodedFi.GetValue(self);
        }
    }
}