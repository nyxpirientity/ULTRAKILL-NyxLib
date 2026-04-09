using System;
using UnityEngine;
using HarmonyLib;

namespace Nyxpiri.ULTRAKILL.NyxLib
{   
    
    [HarmonyPatch(typeof(SpiderBody), "Die", new Type[]{})]
    static class SpiderDiePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        
        public static bool Prefix(SpiderBody __instance)
        {
            return EidDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), false, typeof(SpiderDiePatch), _cancellationTracker);
        }
        
        public static void Postfix(SpiderBody __instance)
        {
            EidDeathPatch.PostDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), typeof(SpiderDiePatch), _cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(Enemy), "GoLimp", new Type[]{typeof(bool)})]
    static class EnemyLimpPatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

        public static bool Prefix(Enemy __instance, bool fromExplosion)
        {
            return EidDeathPatch.PreDeath(__instance.EID, false, typeof(EnemyLimpPatch), _cancellationTracker);
        }
        
        public static void Postfix(Enemy __instance, bool fromExplosion)
        {
            EidDeathPatch.PostDeath(__instance.EID, typeof(EnemyLimpPatch), _cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(Drone), "Death", new Type[]{typeof(bool)})]
    static class DroneDeathPatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        
        public static bool Prefix(Drone __instance, bool fromExplosion)
        {
            return EidDeathPatch.PreDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), false, typeof(DroneDeathPatch), _cancellationTracker);
        }
        
        public static void Postfix(Drone __instance, bool fromExplosion)
        {
            EidDeathPatch.PostDeath(__instance.gameObject.GetComponent<EnemyIdentifier>(), typeof(DroneDeathPatch), _cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(EnemyIdentifier), "InstaKill")]
    static class EnemyIdentifierInstakill
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        
        public static bool Prefix(EnemyIdentifier __instance)
        {
            _cancellationTracker.Reset();
            var enemy = __instance.GetComponent<EnemyComponents>();
            enemy.NullInvalid()?.TryCallPreDeath(_cancellationTracker.GetCanceler(), true, typeof(EnemyIdentifierInstakill));
            _cancellationTracker.TryInvokeReimplementation();
            return _cancellationTracker.ShouldRunMethod;
        }
        
        public static void Postfix(EnemyIdentifier __instance)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();
            enemy.NullInvalid()?.TryCallPostDeath(_cancellationTracker.GetCancelInfo(), typeof(EnemyIdentifierInstakill));
        }
    }

    [HarmonyPatch(typeof(EnemyIdentifier), "Explode")]
    static class EnemyIdentifierExplodePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        
        public static bool Prefix(EnemyIdentifier __instance)
        {
            _cancellationTracker.Reset();
            var enemy = __instance.GetComponent<EnemyComponents>();
            enemy.NullInvalid()?.TryCallPreDeath(_cancellationTracker.GetCanceler(), true, typeof(EnemyIdentifierExplodePatch));
            _cancellationTracker.TryInvokeReimplementation();
            return _cancellationTracker.ShouldRunMethod;
        }
        
        public static void Postfix(EnemyIdentifier __instance)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();
            enemy.NullInvalid()?.TryCallPostDeath(_cancellationTracker.GetCancelInfo(), typeof(EnemyIdentifierExplodePatch));
        }
    }


    [HarmonyPatch(typeof(EnemyIdentifier), "ProcessDeath", new Type[1]{typeof(bool)})]
    static class EidDeathPatch
    {
        public static GameObject[] ActivateOnDeath;
        public static bool CalledPreDeath = false;

        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

        public static bool PreDeath(EnemyIdentifier eid, bool instakill, object callerObj, EventMethodCancellationTracker cancellationTracker)
        {
            cancellationTracker.Reset();
            var enemy = eid.GetComponent<EnemyComponents>();
            enemy.NullInvalid()?.TryCallPreDeath(cancellationTracker.GetCanceler(), instakill, callerObj);
            cancellationTracker.TryInvokeReimplementation();
            return cancellationTracker.ShouldRunMethod;
        }

        public static void PostDeath(EnemyIdentifier eid, object callerObj, EventMethodCancellationTracker cancellationTracker)
        {
            var enemy = eid.GetComponent<EnemyComponents>();
            enemy.NullInvalid()?.TryCallPostDeath(cancellationTracker.GetCancelInfo(), callerObj);
        }

        public static bool Prefix(EnemyIdentifier __instance, bool fromExplosion)
        {
            _cancellationTracker.Reset();
            var enemy = __instance.GetComponent<EnemyComponents>();
            enemy.NullInvalid()?.TryCallPreDeath(_cancellationTracker.GetCanceler(), false, typeof(EidDeathPatch));
            _cancellationTracker.TryInvokeReimplementation();
            enemy.NullInvalid()?.TryCallDeath();
            return _cancellationTracker.ShouldRunMethod;
        }

        public static void Postfix(EnemyIdentifier __instance, bool fromExplosion)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();
            enemy.NullInvalid()?.TryCallPostDeath(_cancellationTracker.GetCancelInfo(), typeof(EidDeathPatch));
        }
    }
}