using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class CannonballEvents
    {
        public delegate void PreCannonballStartEventHandler(EventMethodCanceler canceler, Cannonball cannonball);
        public static event PreCannonballStartEventHandler PreCannonballStart;

        public delegate void PostCannonballStartEventHandler(EventMethodCancelInfo cancelInfo, Cannonball cannonball);
        public static event PostCannonballStartEventHandler PostCannonballStart;

        public delegate void PreCannonballCollideEventHandler(EventMethodCanceler canceler, Cannonball cannonball, Collider other);
        public static event PreCannonballCollideEventHandler PreCannonballCollide;
        
        public delegate void PostCannonballCollideEventHandler(EventMethodCancelInfo cancelInfo, Cannonball cannonball, Collider other);
        public static event PostCannonballCollideEventHandler PostCannonballCollide;

        public delegate void PreCannonballLaunchEventHandler(EventMethodCanceler canceler, Cannonball cannonball);
        public static event PreCannonballLaunchEventHandler PreCannonballLaunch;
        
        public delegate void PostCannonballLaunchEventHandler(EventMethodCancelInfo cancelInfo, Cannonball cannonball);
        public static event PostCannonballLaunchEventHandler PostCannonballLaunch;

        [HarmonyPatch(typeof(Cannonball), "Start")]
        static class CannonballStartPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Cannonball __instance)
            {
                _cancellationTracker.Reset();
                PreCannonballStart?.Invoke(_cancellationTracker.GetCanceler(), __instance);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }
            
            public static void Postfix(Cannonball __instance)
            {
                PostCannonballStart?.Invoke(_cancellationTracker.GetCancelInfo(), __instance);
            }
        }

        [HarmonyPatch(typeof(Cannonball), "Collide")]
        static class CannonballCollidePatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Cannonball __instance, Collider other)
            {
                _cancellationTracker.Reset();
                PreCannonballCollide?.Invoke(_cancellationTracker.GetCanceler(), __instance, other);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }
            
            public static void Postfix(Cannonball __instance, Collider other)
            {
                PostCannonballCollide?.Invoke(_cancellationTracker.GetCancelInfo(), __instance, other);
            }
        }

        [HarmonyPatch(typeof(Cannonball), "Launch")]
        static class CannonballLaunchPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
            
            public static bool Prefix(Cannonball __instance)
            {
                _cancellationTracker.Reset();
                PreCannonballLaunch?.Invoke(_cancellationTracker.GetCanceler(), __instance);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }
            
            public static void Postfix(Cannonball __instance)
            {
                PostCannonballLaunch?.Invoke(_cancellationTracker.GetCancelInfo(), __instance);
            }
        }
    }
}
