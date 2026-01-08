using System;
using MelonLoader;
using UnityEngine;
using HarmonyLib;
using MelonLoader.Utils;
using System.Reflection;
using System.Collections.Generic;
using UKAIW.Diagnostics.Debug;
using ULTRAKILL.Cheats;
using UnityEngine.UI;

namespace UKAIW
{   
    public static class EnemyDeathEvents
    {
        public static Action<EnemyIdentifier> PreDeath = null;
        public static Action<EnemyIdentifier> PostDeath = null;
    }

    [HarmonyPatch(typeof(SpiderBody), "Die", new Type[]{})]
    static class SpiderDiePatch
    {
        public static void Prefix(SpiderBody __instance)
        {
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>());
        }
        
        public static void Postfix(SpiderBody __instance)
        {
            EnemyDeathPatch.PostDeath(__instance.gameObject.GetComponent<EnemyIdentifier>());
        }
    }

    [HarmonyPatch(typeof(Zombie), "GoLimp", new Type[]{})]
    static class ZombieLimpPatch
    {
        public static void Prefix(Zombie __instance)
        {
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>());
        }
        
        public static void Postfix(Zombie __instance)
        {
            EnemyDeathPatch.PostDeath(__instance.gameObject.GetComponent<EnemyIdentifier>());
        }
    }

    [HarmonyPatch(typeof(Statue), "GoLimp", new Type[]{})]
    static class StatueLimpPatch
    {
        public static void Prefix(Statue __instance)
        {
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>());
        }
        
        public static void Postfix(Statue __instance)
        {
            EnemyDeathPatch.PostDeath(__instance.gameObject.GetComponent<EnemyIdentifier>());
        }
    }

    [HarmonyPatch(typeof(Machine), "GoLimp", new Type[]{typeof(bool)})]
    static class MachineLimpPatch
    {
        public static void Prefix(Machine __instance, bool fromExplosion)
        {
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>());
        }
        
        public static void Postfix(Machine __instance, bool fromExplosion)
        {
            EnemyDeathPatch.PostDeath(__instance.gameObject.GetComponent<EnemyIdentifier>());
        }
    }

    [HarmonyPatch(typeof(Drone), "Death", new Type[]{typeof(bool)})]
    static class DroneDeathPatch
    {
        public static void Prefix(Drone __instance, bool fromExplosion)
        {
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>());
        }
        
        public static void Postfix(Drone __instance, bool fromExplosion)
        {
            EnemyDeathPatch.PostDeath(__instance.gameObject.GetComponent<EnemyIdentifier>());
        }
    }

    [HarmonyPatch(typeof(EnemyIdentifier), "InstaKill")]
    static class EnemyIdentifierInstakill
    {
        public static void Prefix(EnemyIdentifier __instance)
        {
           // EnemyDeathPatch.PreDeath(__instance);
        }
        
        public static void Postfix(EnemyIdentifier __instance)
        {
            //EnemyDeathPatch.PostDeath(__instance);
        }
    }


    [HarmonyPatch(typeof(EnemyIdentifier), "Death", new Type[1]{typeof(bool)})]
    static class EnemyDeathPatch
    {
        public static GameObject[] ActivateOnDeath;
        public static bool CalledPreDeath = false;

        public static void PreDeath(EnemyIdentifier eid)
        {
            if (eid.dead)
            {
                CalledPreDeath = false;
                return;
            }

            try
            {
                EnemyDeathEvents.PreDeath?.Invoke(eid);
            }
            catch (System.Exception e)
            {
                Log.Error($"Enemy PreDeath error :c {e}");                
            }
            CalledPreDeath = true;
        }

        public static void PostDeath(EnemyIdentifier eid)
        {
            if (!CalledPreDeath)
            {
                return;
            }
            
            try
            {
                EnemyDeathEvents.PostDeath?.Invoke(eid);
            }
            catch (System.Exception e)
            {
                Log.Error($"Enemy PreDeath error :c {e}");                
            }
        }

        public static void Prefix(EnemyIdentifier __instance, bool fromExplosion)
        {
            if (__instance.dead)
            {
                return;
            }

            ActivateOnDeath = __instance.activateOnDeath;

            if (Cheats.IsCheatEnabled(Cheats.NoCorpses))
            {
                __instance.puppet = true;
            }
        }

        public static void Postfix(EnemyIdentifier __instance, bool fromExplosion)
        {
            if (ActivateOnDeath.Length == 0)
            {
                return;
            }

            if (Cheats.IsCheatEnabled(Cheats.NoCorpses))
            {
                foreach (var go in ActivateOnDeath)
                {
                    UnityEngine.Object.Destroy(go);
                }
            }

            ActivateOnDeath = new GameObject[0];
        }
    }
}