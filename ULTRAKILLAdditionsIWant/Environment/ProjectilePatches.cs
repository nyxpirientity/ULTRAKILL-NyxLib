using System;
using HarmonyLib;
using UnityEngine;

namespace UKAIW
{
    [HarmonyPatch(typeof(Projectile), "Awake")]
    static class ProjectileAwakePatch
    {
        public static void Prefix(Projectile __instance)
        {

        }

        public static void Postfix(Projectile __instance)
        {
            if (__instance.gameObject.GetComponent<ProjectileAdditions>() != null)
            {
                return;
            }

            __instance.gameObject.AddComponent<ProjectileAdditions>();
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