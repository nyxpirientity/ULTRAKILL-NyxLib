using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class CoinEvents
    {
        public delegate void PreCoinAwakeEventHandler(EventMethodCanceler canceler, Coin coin);
        public static event PreCoinAwakeEventHandler PreCoinAwake;

        public delegate void PostCoinAwakeEventHandler(EventMethodCancelInfo cancelInfo, Coin coin);
        public static event PostCoinAwakeEventHandler PostCoinAwake;
    
        public delegate void PreCoinPunchflectionEventHandler(EventMethodCanceler canceler, Coin coin);
        public static event PreCoinPunchflectionEventHandler PreCoinPunchflection;

        public delegate void PostCoinPunchflectionEventHandler(EventMethodCancelInfo cancelInfo, Coin coin);
        public static event PostCoinPunchflectionEventHandler PostCoinPunchflection;

        public delegate void PreCoinReflectRevolverEventHandler(EventMethodCanceler canceler, Coin coin);
        public static event PreCoinReflectRevolverEventHandler PreCoinReflectRevolver;

        public delegate void PostCoinReflectRevolverEventHandler(EventMethodCancelInfo cancelInfo, Coin coin);
        public static event PostCoinReflectRevolverEventHandler PostCoinReflectRevolver;

        [HarmonyPatch(typeof(Coin), "Awake")]
        static class CoinAwakePatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Coin __instance)
            {
                _cancellationTracker.Reset();
                PreCoinAwake?.Invoke(_cancellationTracker.GetCanceler(), __instance);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }

            public static void Postfix(Coin __instance)
            {
                PostCoinAwake?.Invoke(_cancellationTracker.GetCancelInfo(), __instance);
            }
        }

        [HarmonyPatch(typeof(Coin), nameof(Coin.ReflectRevolver))]
        static class CoinReflectRevolverPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
            
            public static bool Prefix(Coin __instance)
            {
                _cancellationTracker.Reset();
                PreCoinReflectRevolver?.Invoke(_cancellationTracker.GetCanceler(), __instance);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }

            public static void Postfix(Coin __instance)
            {
                PostCoinReflectRevolver?.Invoke(_cancellationTracker.GetCancelInfo(), __instance);
            }
        }

        [HarmonyPatch(typeof(Coin), nameof(Coin.Punchflection))]
        static class CoinPunchflectionPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Coin __instance)
            {
                _cancellationTracker.Reset();
                PreCoinPunchflection?.Invoke(_cancellationTracker.GetCanceler(), __instance);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }

            public static void Postfix(Coin __instance)
            {
                PostCoinPunchflection?.Invoke(_cancellationTracker.GetCancelInfo(), __instance);
            }
        }
    }
}