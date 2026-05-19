using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class NailEvents
    {        
        public delegate void PreNailStartEventHandler(EventMethodCanceler canceler, Nail nail);
        public static event PreNailStartEventHandler PreNailStart;

        public delegate void PostNailStartEventHandler(EventMethodCancelInfo cancelInfo, Nail nail);
        public static event PostNailStartEventHandler PostNailStart;
    
        public delegate void PreNailHitEnemyEventHandler(EventMethodCanceler canceler, Nail nail, Transform other, EnemyIdentifierIdentifier eidid);
        public static event PreNailHitEnemyEventHandler PreNailHitEnemy;

        public delegate void PostNailHitEnemyEventHandler(EventMethodCancelInfo cancelInfo, Nail nail, Transform other, EnemyIdentifierIdentifier eidid);
        public static event PostNailHitEnemyEventHandler PostNailHitEnemy;    

        public delegate void PreNailTouchEnemyEventHandler(EventMethodCanceler canceler, Nail nail, Transform other);
        public static event PreNailTouchEnemyEventHandler PreNailTouchEnemy;

        public delegate void PostNailTouchEnemyEventHandler(EventMethodCancelInfo cancelInfo, Nail nail, Transform other);
        public static event PostNailTouchEnemyEventHandler PostNailTouchEnemy;

        [HarmonyPatch(typeof(Nail), "Start")]
        static class NailStartPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Nail __instance)
            {
                _cancellationTracker.Reset();
                PreNailStart?.Invoke(_cancellationTracker.GetCanceler(), __instance);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }
            
            public static void Postfix(Nail __instance)
            {
                PostNailStart?.Invoke(_cancellationTracker.GetCancelInfo(), __instance);
            }
        }

        [HarmonyPatch(typeof(Nail), "TouchEnemy")]
        public static class NailTouchEnemyPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Nail __instance, Transform other)
            {
                _cancellationTracker.Reset();
                PreNailTouchEnemy?.Invoke(_cancellationTracker.GetCanceler(), __instance, other);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }
            
            public static void Postfix(Nail __instance, Transform other)
            {
                PostNailTouchEnemy?.Invoke(_cancellationTracker.GetCancelInfo(), __instance, other);
            }
        }

        [HarmonyPatch(typeof(Nail), "HitEnemy")]
        static class NailHitEnemyPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Nail __instance, Transform other, EnemyIdentifierIdentifier eidid = null)
            {
                _cancellationTracker.Reset();
                PreNailHitEnemy?.Invoke(_cancellationTracker.GetCanceler(), __instance, other, eidid);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }
            
            public static void Postfix(Nail __instance, Transform other, EnemyIdentifierIdentifier eidid = null)
            {
                PostNailHitEnemy?.Invoke(_cancellationTracker.GetCancelInfo(), __instance, other, eidid);
            }
        }
    }
}