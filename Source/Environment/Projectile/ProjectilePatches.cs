using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class ProjectileEvents
    {
        public delegate void PreProjectileAwakeEventHandler(EventMethodCanceler canceler, Projectile projectile);
        public static event PreProjectileAwakeEventHandler PreProjectileAwake;

        public delegate void PostProjectileAwakeEventHandler(EventMethodCancelInfo cancelInfo, Projectile projectile);
        public static event PostProjectileAwakeEventHandler PostProjectileAwake;
    
        public delegate void PreProjectileCollidedEventHandler(EventMethodCanceler canceler, Projectile projectile, Collider other);
        public static event PreProjectileCollidedEventHandler PreProjectileCollided;

        public delegate void PostProjectileCollidedEventHandler(EventMethodCancelInfo cancelInfo, Projectile projectile, Collider other);
        public static event PostProjectileCollidedEventHandler PostProjectileCollided;

        [HarmonyPatch(typeof(Projectile), "Awake")]
        static class ProjectileAwakePatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Projectile __instance)
            {
                _cancellationTracker.Reset();
                PreProjectileAwake?.Invoke(_cancellationTracker.GetCanceler(), __instance);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }

            public static void Postfix(Projectile __instance)
            {
                PostProjectileAwake?.Invoke(_cancellationTracker.GetCancelInfo(), __instance);
                __instance.gameObject.GetOrAddComponent<ProjectileAdditions>();
            }
        }

        [HarmonyPatch(typeof(Projectile), "Collided")]
        static class ProjectileCollidedPatch
        {
            private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

            public static bool Prefix(Projectile __instance, Collider other)
            {
                _cancellationTracker.Reset();
                PreProjectileCollided?.Invoke(_cancellationTracker.GetCanceler(), __instance, other);
                _cancellationTracker.TryInvokeReimplementation();
                return !_cancellationTracker.Cancelled;
            }

            public static void Postfix(Projectile __instance, Collider other)
            {
                PostProjectileCollided?.Invoke(_cancellationTracker.GetCancelInfo(), __instance, other);
            }
        }

        [HarmonyPatch(typeof(Projectile), "Explode")]
        static class ProjectileExplodePatch
        {
            public static void Prefix(Projectile __instance)
            {
                var additions = __instance.GetComponent<ProjectileAdditions>();
                
                additions.InvokePreExplode(false);
            }

            public static void Postfix(Projectile __instance)
            {
                var additions = __instance.GetComponent<ProjectileAdditions>();

                additions.InvokePostExplode(false);
            }
        }

        [HarmonyPatch(typeof(Projectile), "CreateExplosionEffect")]
        static class ProjectileCreateExplosionEffectPatch
        {
            public static void Prefix(Projectile __instance)
            {
                var additions = __instance.GetComponent<ProjectileAdditions>();

                additions.InvokePreExplode(true);
            }

            public static void Postfix(Projectile __instance)
            {
                var additions = __instance.GetComponent<ProjectileAdditions>();

                additions.InvokePostExplode(true);
            }
        }
    }
}