using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace UKAIW
{
    [HarmonyPatch(typeof(Punch), "ParryProjectile")]
    static class PunchParryProjectilePatch
    {
        public static void Prefix(Punch __instance, Projectile proj)
        {
            if (Cheats.Enabled)
            {
                var boostTracker = proj.GetComponent<ProjectileBoostTracker>();
                if (boostTracker != null)
                {
                    boostTracker.IncrementPlayerBoosts();
                    
                    if (boostTracker.NumPlayerBoosts > 1)
                    {
                        proj.speed *= 0.55f; // player parry boosts speed by 2x, so this counteracts it
                    }

                    if (boostTracker.NumEnemyBoosts > 0 && (boostTracker.ProjectileType == ProjectileBoostTracker.ProjectileCategory.RevolverShot || boostTracker.ProjectileType == ProjectileBoostTracker.ProjectileCategory.PlayerProjectile))
                    {
                        StyleHUD.Instance.AddPoints(10, "<color=#26ff00>PARRY PONG</color>");
                    }

                }
            }
        }

        public static void Postfix(Punch __instance, Projectile proj)
        {
        }
    }
}