using System;
using System.Reflection;
using HarmonyLib;
using ULTRAKILL.Portal;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class GrenadeEvents
    {
        public delegate void PreGrenadeStartEventHandler(EventMethodCanceler canceler, Grenade grenade);
        public static event PreGrenadeStartEventHandler PreGrenadeStart;

        public delegate void PostGrenadeStartEventHandler(EventMethodCancelInfo cancelInfo, Grenade grenade);
        public static event PostGrenadeStartEventHandler PostGrenadeStart;
    
        public delegate void PreGrenadeBeamEventHandler(EventMethodCanceler canceler, Grenade grenade, Vector3 targetPoint, GameObject newSourceWeapon = null);
        public static event PreGrenadeBeamEventHandler PreGrenadeBeam;

        public delegate void PostGrenadeBeamEventHandler(EventMethodCancelInfo cancelInfo, Grenade grenade, Vector3 targetPoint, GameObject newSourceWeapon = null);
        public static event PostGrenadeBeamEventHandler PostGrenadeBeam;

        public delegate void PreGrenadeCollisionEventHandler(EventMethodCanceler canceler, Grenade grenade, Collider other, Vector3 velocity);
        public static event PreGrenadeCollisionEventHandler PreGrenadeCollision;

        public delegate void PostGrenadeCollisionEventHandler(EventMethodCancelInfo cancelInfo, Grenade grenade, Collider other, Vector3 velocity);
        public static event PostGrenadeCollisionEventHandler PostGrenadeCollision;

        public delegate void PreGrenadeExplodeEventHandler(EventMethodCanceler canceler, Grenade grenade, bool big, bool harmless, bool super, float sizeMultiplier, bool ultrabooster, GameObject exploderWeapon, bool fup);
        public static event PreGrenadeExplodeEventHandler PreGrenadeExplode;

        public delegate void PostGrenadeExplodeEventHandler(EventMethodCancelInfo cancelInfo, Grenade grenade, bool big, bool harmless, bool super, float sizeMultiplier, bool ultrabooster, GameObject exploderWeapon, bool fup);
        public static event PostGrenadeExplodeEventHandler PostGrenadeExplode;
        
        [HarmonyPatch(typeof(Grenade), "Start")]
        static class GrenadeStartPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Grenade __instance)
            {
                _cancellationTracker.Reset();
                PreGrenadeStart?.Invoke(_cancellationTracker.GetCanceler(), __instance);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }
            
            public static void Postfix(Grenade __instance)
            {
                PostGrenadeStart?.Invoke(_cancellationTracker.GetCancelInfo(), __instance);
            }
        } 

        [HarmonyPatch(typeof(Grenade), "Explode", new Type[]{typeof(bool), typeof(bool), typeof(bool), typeof(float), typeof(bool), typeof(GameObject), typeof(bool)})]
        static class GrenadeExplodePatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Grenade __instance, bool big = false, bool harmless = false, bool super = false, float sizeMultiplier = 1f, bool ultrabooster = false, GameObject exploderWeapon = null, bool fup = false)
            {
                _cancellationTracker.Reset();
                PreGrenadeExplode?.Invoke(_cancellationTracker.GetCanceler(), __instance, big, harmless, super, sizeMultiplier, ultrabooster, exploderWeapon, fup);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }
            
            public static void Postfix(Grenade __instance, bool big = false, bool harmless = false, bool super = false, float sizeMultiplier = 1f, bool ultrabooster = false, GameObject exploderWeapon = null, bool fup = false)
            {
                PostGrenadeExplode?.Invoke(_cancellationTracker.GetCancelInfo(), __instance, big, harmless, super, sizeMultiplier, ultrabooster, exploderWeapon, fup);
            }
        }


        [HarmonyPatch(typeof(Grenade), "GrenadeBeam")]
        static class GrenadeBeamPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Grenade __instance, Vector3 targetPoint, GameObject newSourceWeapon = null)
            {
                _cancellationTracker.Reset();
                PreGrenadeBeam?.Invoke(_cancellationTracker.GetCanceler(), __instance, targetPoint, newSourceWeapon);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }
            
            public static void Postfix(Grenade __instance, Vector3 targetPoint, GameObject newSourceWeapon = null)
            {
                PostGrenadeBeam?.Invoke(_cancellationTracker.GetCancelInfo(), __instance, targetPoint, newSourceWeapon);
            }
        }


        [HarmonyPatch(typeof(Grenade), "Collision", new Type[]{typeof(Collider), typeof(Vector3)})]
        static class GrenadeCollisionPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Grenade __instance, Collider other, Vector3 velocity)
            {
                _cancellationTracker.Reset();
                PreGrenadeCollision?.Invoke(_cancellationTracker.GetCanceler(), __instance, other, velocity);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }

            public static void Postfix(Grenade __instance, Collider other, Vector3 velocity)
            {
                PostGrenadeCollision?.Invoke(_cancellationTracker.GetCancelInfo(), __instance, other, velocity);
            }
        }
    }
}