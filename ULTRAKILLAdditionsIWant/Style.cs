using System;
using MelonLoader;
using UnityEngine;
using HarmonyLib;
using MelonLoader.Utils;
using System.Reflection;
using System.Collections.Generic;
using UKAIW.Diagnostics.Debug;

namespace UKAIW
{
    public enum StyleRanks
    {
        Null = -1,
        Destructive = 0,
        Chaotic = 1,
        Brutal = 2,
        Anarchic = 3,
        Supreme = 4,
        SSadistic = 5,
        SSSensoredStorm = 6,
        ULTRAKILL = 7,
    }

    public static class Style
    {
        public static Action<StyleHUD, int, string, GameObject, EnemyIdentifier, int, string, string> AddPointsPrefix = null;
        public static Action<StyleHUD, int, string, GameObject, EnemyIdentifier, int, string, string> AddPointsPostfix = null;

        public static Action<StyleHUD, int> RemovePointsPrefix = null;
        public static Action<StyleHUD, int> RemovePointsPostfix = null;

        [HarmonyPatch(typeof(StyleHUD), "AddPoints")]
        static class AddPointsPatch
        {
            public static bool Prefix(StyleHUD __instance, int points, string pointID, GameObject sourceWeapon = null, EnemyIdentifier eid = null, int count = -1, string prefix = "", string postfix = "")
            {
                if (Cheats.IsCheatEnabled(Cheats.MundaneMurder))
                {
                    MundaneMurder.AddPointsPrefix(__instance, points, pointID, sourceWeapon, eid, count, prefix, postfix);
                    return false;
                }

                return true;
            }

            public static void Postfix(StyleHUD __instance, int points, string pointID, GameObject sourceWeapon = null, EnemyIdentifier eid = null, int count = -1, string prefix = "", string postfix = "")
            {

            }
        }

        [HarmonyPatch(typeof(StyleHUD), "RemovePoints")]
        static class RemovePointsPatch
        {
            public static void Prefix(StyleHUD __instance, int points)
            {

            }

            public static void Postfix(StyleHUD __instance, int points)
            {

            }
        }

        [HarmonyPatch(typeof(StyleHUD), "AscendRank")]
        static class AscendRankPatch
        {
            public static void Prefix(StyleHUD __instance)
            {

            }

            public static void Postfix(StyleHUD __instance)
            {

            }
        }

        [HarmonyPatch(typeof(StyleHUD), "DescendRank")]
        static class DescendRankPatch
        {
            public static void Prefix(StyleHUD __instance)
            {

            }

            public static void Postfix(StyleHUD __instance)
            {

            }
        }

        [HarmonyPatch(typeof(StyleHUD), "Update")]
        static class StyleUpdatePatch
        {
            public static bool Prefix(StyleHUD __instance)
            {
                if (Cheats.IsCheatEnabled(Cheats.MundaneMurder))
                {
                    MundaneMurder.OnStyleUpdate(__instance);
                    return false;
                }

                return true;
            }

            public static void Postfix(StyleHUD __instance)
            {

            }
        } 
    }
}