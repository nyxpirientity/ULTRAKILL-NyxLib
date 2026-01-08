using System;
using HarmonyLib;
using UnityEngine;

public static class Cybergrind
{
    [HarmonyPatch(typeof(EndlessGrid), "OnTriggerEnter", new Type[] { typeof(Collider) })]
    static class CybergrindStartPatch
    {
        public static void Prefix(Collider other)
        {
            if (!((Component)(object)other).CompareTag("Player"))
            {
                return;
            }
        }

        public static void Postfix(Collider other)
        {
            if (!((Component)(object)other).CompareTag("Player"))
            {
                return;
            }
        }
    }
}