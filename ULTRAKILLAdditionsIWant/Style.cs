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
    [HarmonyPatch(typeof(StyleHUD), "AddPoints")]
    static class AddPointsPatch
    {
        public static void Prefix(StyleHUD __instance, int points, string pointID, GameObject sourceWeapon = null, EnemyIdentifier eid = null, int count = -1, string prefix = "", string postfix = "")
        {

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

    [HarmonyPatch(typeof(StyleHUD), "Update")]
    static class MeterUpdatePatch
    {
        public static void Prefix(StyleHUD __instance)
        {

        }

        public static void EnableMundaneMurderIcon(StyleHUD shud)
        {
            shud.rankImage.canvasRenderer.SetTexture(Assets.MundaneMurderIcon);
            shud.rankImage.canvasRenderer.DisableRectClipping();
        }

        public static void DisableMundaneMurderIcon(StyleHUD shud)
        {
            shud.rankImage.useSpriteMesh = true;
        }

        public static bool IsMundaneMurderIconEnabled(StyleHUD shud)
        {
            return shud.rankImage.mainTexture == Assets.MundaneMurderIcon;
        }

        public static void Postfix(StyleHUD __instance)
        {

        }
    }

}