using System;
using UnityEngine;
using HarmonyLib;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public enum StyleRanks
    {
        Null = -1,
        Destructive = 0,
        Chaotic = 1,
        Brutal = 2,
        Anarchic = 3,
        Supreme = 4,
        SSadistic = 5,
        SSSensoredStorm = 6,
        ULTRAKILL = 7,
    }

    public static class Style
    {
        public static int NumStyleRanks = 8;
        
        public delegate void PreAddPointsEventHandler(EventMethodCanceler canceler, StyleHUD shud, int points, string pointID, GameObject sourceWeapon = null, EnemyIdentifier eid = null, int count = -1, string prefix = "", string postfix = "");
        public static event PreAddPointsEventHandler PreAddPoints;

        public delegate void PostAddPointsEventHandler(EventMethodCancelInfo cancelled, StyleHUD shud, int points, string pointID, GameObject sourceWeapon = null, EnemyIdentifier eid = null, int count = -1, string prefix = "", string postfix = "");
        public static event PostAddPointsEventHandler PostAddPoints;
        
        public delegate void PreRemovePointsEventHandler(EventMethodCanceler canceler, StyleHUD shud, int points);
        public static event PreRemovePointsEventHandler PreRemovePoints;

        public delegate void PostRemovePointsEventHandler(EventMethodCancelInfo cancelled, StyleHUD shud, int points);
        public static event PostRemovePointsEventHandler PostRemovePoints;
        
        public delegate void PreShudUpdateEventHandler(EventMethodCanceler canceler, StyleHUD shud);
        public static event PreShudUpdateEventHandler PreShudUpdate;

        public delegate void PostShudUpdateEventHandler(EventMethodCancelInfo cancelled, StyleHUD shud);
        public static event PostShudUpdateEventHandler PostShudUpdate;

        public static StyleRanks GetStyleRank(this StyleHUD self)
        {
            return (StyleRanks)self.rankIndex;
        }

        [HarmonyPatch(typeof(StyleHUD), "AddPoints")]
        static class AddPointsPatch
        {
            private static EventMethodCancellationTracker CancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(StyleHUD __instance, int points, string pointID, GameObject sourceWeapon = null, EnemyIdentifier eid = null, int count = -1, string prefix = "", string postfix = "")
            {
                CancellationTracker.Reset();

                PreAddPoints?.Invoke(CancellationTracker.GetCanceler(), __instance, points, pointID, sourceWeapon, eid, count, prefix, postfix);
                
                CancellationTracker.TryInvokeReimplementation();

                return !CancellationTracker.Cancelled;
            }

            public static void Postfix(StyleHUD __instance, int points, string pointID, GameObject sourceWeapon = null, EnemyIdentifier eid = null, int count = -1, string prefix = "", string postfix = "")
            {
                PostAddPoints?.Invoke(CancellationTracker.GetCancelInfo(), __instance, points, pointID, sourceWeapon, eid, count, prefix, postfix);
            }
        }

        [HarmonyPatch(typeof(StyleHUD), "RemovePoints")]
        static class RemovePointsPatch
        {
            private static EventMethodCancellationTracker CancellationTracker = new EventMethodCancellationTracker();
            
            public static bool Prefix(StyleHUD __instance, int points)
            {
                CancellationTracker.Reset();

                PreRemovePoints?.Invoke(CancellationTracker.GetCanceler(), __instance, points);

                CancellationTracker.TryInvokeReimplementation();
                
                return !CancellationTracker.Cancelled;
            }

            public static void Postfix(StyleHUD __instance, int points)
            {
                PostRemovePoints?.Invoke(CancellationTracker.GetCancelInfo(), __instance, points);
            }
        }

        [HarmonyPatch(typeof(StyleHUD), "AscendRank")]
        static class AscendRankPatch
        {
            public static void Prefix(StyleHUD __instance)
            {

            }

            public static void Postfix(StyleHUD __instance)
            {

            }
        }

        [HarmonyPatch(typeof(StyleHUD), "DescendRank")]
        static class DescendRankPatch
        {
            public static void Prefix(StyleHUD __instance)
            {

            }

            public static void Postfix(StyleHUD __instance)
            {

            }
        }

        [HarmonyPatch(typeof(StyleHUD), "Update")]
        static class StyleUpdatePatch
        {
            private static EventMethodCancellationTracker CancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(StyleHUD __instance)
            {
                CancellationTracker.Reset();

                PreShudUpdate?.Invoke(CancellationTracker.GetCanceler(), __instance);

                CancellationTracker.TryInvokeReimplementation();

                return !CancellationTracker.Cancelled;
            }

            public static void Postfix(StyleHUD __instance)
            {
                PostShudUpdate?.Invoke(CancellationTracker.GetCancelInfo(), __instance);
            }
        } 
    }
}