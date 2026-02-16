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
    
    // TODO: changes made instakills with hydramod still let enemies split, this is not intended (at least, not currently intended..)
    [HarmonyPatch(typeof(SpiderBody), "Die", new Type[]{})]
    static class SpiderDiePatch
    {
        public static void Prefix(SpiderBody __instance)
        {
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), false);
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
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), false);
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
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), false);
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
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), false);
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
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), false);
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
            var eadd = __instance.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPreDeath(true);
        }
        
        public static void Postfix(EnemyIdentifier __instance)
        {
            var eadd = __instance.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPostDeath();
        }
    }

    [HarmonyPatch(typeof(EnemyIdentifier), "Explode")]
    static class EnemyIdentifierExplodePatch
    {
        public static void Prefix(EnemyIdentifier __instance)
        {
            var eadd = __instance.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPreDeath(true);
        }
        
        public static void Postfix(EnemyIdentifier __instance)
        {
            var eadd = __instance.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPostDeath();
        }
    }


    [HarmonyPatch(typeof(EnemyIdentifier), "Death", new Type[1]{typeof(bool)})]
    static class EnemyDeathPatch
    {
        public static GameObject[] ActivateOnDeath;
        public static bool CalledPreDeath = false;

        public static void PreDeath(EnemyIdentifier eid, bool instakill)
        {
            var eadd = eid.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPreDeath(instakill);
        }

        public static void PostDeath(EnemyIdentifier eid)
        {
            var eadd = eid.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPostDeath();
        }

        public static void Prefix(EnemyIdentifier __instance, bool fromExplosion)
        {
            var eadd = __instance.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPreDeath(false);
            eadd.NullInvalid()?.TryCallDeath();

            if (Cheats.IsCheatEnabled(Cheats.NoCorpses))
            {
                __instance.puppet = true;
            }
        }

        public static void Postfix(EnemyIdentifier __instance, bool fromExplosion)
        {
            var eadd = __instance.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPostDeath();
        }
    }
}