using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class RevolverBeamEvents
    {
        public delegate void PreRevolverBeamStartEventHandler(EventMethodCanceler canceler, RevolverBeam revolverBeam);
        public static event PreRevolverBeamStartEventHandler PreRevolverBeamStart;

        public delegate void PostRevolverBeamStartEventHandler(EventMethodCancelInfo cancelInfo, RevolverBeam revolverBeam);
        public static event PostRevolverBeamStartEventHandler PostRevolverBeamStart;
    
        public delegate void PreRevolverBeamHitSomethingEventHandler(EventMethodCanceler canceler, RevolverBeam revolverBeam, PhysicsCastResult hit);
        public static event PreRevolverBeamHitSomethingEventHandler PreRevolverBeamHitSomething;

        public delegate void PostRevolverBeamHitSomethingEventHandler(EventMethodCancelInfo cancelInfo, RevolverBeam revolverBeam, PhysicsCastResult hit);
        public static event PostRevolverBeamHitSomethingEventHandler PostRevolverBeamHitSomething;
    
        public delegate void PreRevolverBeamPiercingShotCheckEventHandler(EventMethodCanceler canceler, RevolverBeam revolverBeam);
        public static event PreRevolverBeamPiercingShotCheckEventHandler PreRevolverBeamPiercingShotCheck;

        public delegate void PostRevolverBeamPiercingShotCheckEventHandler(EventMethodCancelInfo cancelInfo, RevolverBeam revolverBeam);
        public static event PostRevolverBeamPiercingShotCheckEventHandler PostRevolverBeamPiercingShotCheck;

        [HarmonyPatch(typeof(RevolverBeam), "Start")]
        static class RevolverBeamStartPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(RevolverBeam __instance)
            {
                _cancellationTracker.Reset();
                PreRevolverBeamStart?.Invoke(_cancellationTracker.GetCanceler(), __instance);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }

            public static void Postfix(RevolverBeam __instance)
            {
                PostRevolverBeamStart?.Invoke(_cancellationTracker.GetCancelInfo(), __instance);
            }
        }

        [HarmonyPatch(typeof(RevolverBeam), "HitSomething")]
        static class RevolverBeamHitSomethingPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(RevolverBeam __instance, PhysicsCastResult hit)
            {
                _cancellationTracker.Reset();
                PreRevolverBeamHitSomething?.Invoke(_cancellationTracker.GetCanceler(), __instance, hit);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }

            public static void Postfix(RevolverBeam __instance, PhysicsCastResult hit)
            {
                PostRevolverBeamHitSomething?.Invoke(_cancellationTracker.GetCancelInfo(), __instance, hit);
            }
        }

        [HarmonyPatch(typeof(RevolverBeam), "PiercingShotCheck")]
        static class RevolverBeamPiercingShotCheckPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(RevolverBeam __instance)
            {
                _cancellationTracker.Reset();
                PreRevolverBeamPiercingShotCheck?.Invoke(_cancellationTracker.GetCanceler(), __instance);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }

            public static void Postfix(RevolverBeam __instance)
            {
                PostRevolverBeamPiercingShotCheck?.Invoke(_cancellationTracker.GetCancelInfo(), __instance);
            }
        }
    }
}