using System;
using HarmonyLib;

namespace UKAIW
{
    [HarmonyPatch(typeof(CheckPoint), "ActivateCheckPoint", new Type[]{})]
    static class CheckpointGetPatch
    {
        public static void Prefix(CheckPoint __instance)
        {
        }
        
        public static void Postfix(CheckPoint __instance)
        {
        }
    }
}