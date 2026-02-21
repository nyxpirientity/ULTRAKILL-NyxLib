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
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), false, typeof(SpiderDiePatch));
        }
        
        public static void Postfix(SpiderBody __instance)
        {
            EnemyDeathPatch.PostDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), typeof(SpiderDiePatch));
        }
    }

    [HarmonyPatch(typeof(Zombie), "GoLimp", new Type[]{})]
    static class ZombieLimpPatch
    {
        public static void Prefix(Zombie __instance)
        {
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), false, typeof(ZombieLimpPatch));
        }
        
        public static void Postfix(Zombie __instance)
        {
            EnemyDeathPatch.PostDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), typeof(ZombieLimpPatch));
        }
    }

    [HarmonyPatch(typeof(Statue), "GoLimp", new Type[]{})]
    static class StatueLimpPatch
    {
        public static void Prefix(Statue __instance)
        {
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), false, typeof(StatueLimpPatch));
        }
        
        public static void Postfix(Statue __instance)
        {
            EnemyDeathPatch.PostDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), typeof(StatueLimpPatch));
        }
    }

    [HarmonyPatch(typeof(Machine), "GoLimp", new Type[]{typeof(bool)})]
    static class MachineLimpPatch
    {
        public static void Prefix(Machine __instance, bool fromExplosion)
        {
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), false, typeof(MachineLimpPatch));
        }
        
        public static void Postfix(Machine __instance, bool fromExplosion)
        {
            EnemyDeathPatch.PostDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), typeof(MachineLimpPatch));
        }
    }

    [HarmonyPatch(typeof(Drone), "Death", new Type[]{typeof(bool)})]
    static class DroneDeathPatch
    {
        public static void Prefix(Drone __instance, bool fromExplosion)
        {
            EnemyDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), false, typeof(DroneDeathPatch));
        }
        
        public static void Postfix(Drone __instance, bool fromExplosion)
        {
            EnemyDeathPatch.PostDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), typeof(DroneDeathPatch));
        }
    }

    [HarmonyPatch(typeof(EnemyIdentifier), "InstaKill")]
    static class EnemyIdentifierInstakill
    {
        public static void Prefix(EnemyIdentifier __instance)
        {
            var eadd = __instance.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPreDeath(true, typeof(EnemyIdentifierInstakill));
        }
        
        public static void Postfix(EnemyIdentifier __instance)
        {
            var eadd = __instance.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPostDeath(typeof(EnemyIdentifierInstakill));
        }
    }

    [HarmonyPatch(typeof(EnemyIdentifier), "Explode")]
    static class EnemyIdentifierExplodePatch
    {
        public static void Prefix(EnemyIdentifier __instance)
        {
            var eadd = __instance.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPreDeath(true, typeof(EnemyIdentifierExplodePatch));
        }
        
        public static void Postfix(EnemyIdentifier __instance)
        {
            var eadd = __instance.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPostDeath(typeof(EnemyIdentifierExplodePatch));
        }
    }


    [HarmonyPatch(typeof(EnemyIdentifier), "Death", new Type[1]{typeof(bool)})]
    static class EnemyDeathPatch
    {
        public static GameObject[] ActivateOnDeath;
        public static bool CalledPreDeath = false;

        public static void PreDeath(EnemyIdentifier eid, bool instakill, object callerObj)
        {
            var eadd = eid.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPreDeath(instakill, callerObj);
        }

        public static void PostDeath(EnemyIdentifier eid, object callerObj)
        {
            var eadd = eid.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPostDeath(callerObj);
        }

        public static void Prefix(EnemyIdentifier __instance, bool fromExplosion)
        {
            var eadd = __instance.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPreDeath(false, typeof(EnemyDeathPatch));
            eadd.NullInvalid()?.TryCallDeath();

            if (Cheats.IsCheatEnabled(Cheats.NoCorpses))
            {
                __instance.puppet = true;
            }
        }

        public static void Postfix(EnemyIdentifier __instance, bool fromExplosion)
        {
            var eadd = __instance.GetComponent<EnemyAdditions>();
            eadd.NullInvalid()?.TryCallPostDeath(typeof(EnemyDeathPatch));
        }
    }
}