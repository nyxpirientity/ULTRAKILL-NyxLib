using System;
using HarmonyLib;
using UnityEngine;

namespace UKAIW
{
    [HarmonyPatch(typeof(ActivateNextWave), "AddDeadEnemy")]
    static class CybergrindStartPatch
    {
        public static void Prefix()
        {
        }

        public static void Postfix()
        {
        }
    }
}