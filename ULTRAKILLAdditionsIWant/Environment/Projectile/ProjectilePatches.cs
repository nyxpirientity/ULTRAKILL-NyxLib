using System;
using System.Linq;
using System.Reflection;
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

    [HarmonyPatch(typeof(Projectile), "Collided")]
    static class ProjectileCollidedPatch
    {
        static FieldInfo _activeFi = typeof(Projectile).GetField("active", BindingFlags.Instance | BindingFlags.NonPublic); 
        public static bool Prefix(Projectile __instance, Collider other)
        {
            if (!(bool)_activeFi.GetValue(__instance))
            {
                return true;
            }

            if (Cheats.IsCheatDisabled(Cheats.FeedbackerForAll))
            {
                return true;
            }

            var boostTracker = __instance.GetComponent<ProjectileBoostTracker>();

            var parryability = boostTracker.NotifyContact();

            EnemyIdentifierIdentifier eidid = null;

            if (!__instance.friendly && !__instance.hittingPlayer && other.gameObject.CompareTag("Player"))
            {
                return true;
            }
            else if (__instance.canHitCoin && other.gameObject.CompareTag("Coin"))
            {
                return true;
            }
            else if ((other.gameObject.CompareTag("Armor") && (__instance.friendly || !other.TryGetComponent(out eidid) || !eidid.eid || eidid.eid.enemyType != __instance.safeEnemyType)) || (__instance.boosted && other.gameObject.layer == 11 && other.gameObject.CompareTag("Body") && other.TryGetComponent(out eidid) && (bool)eidid.eid && eidid.eid.enemyType == EnemyType.MaliciousFace && !eidid.eid.isGasolined))
            {
                return true;
            }
            else if ((other.gameObject.CompareTag("Head") || other.gameObject.CompareTag("Body") || other.gameObject.CompareTag("Limb") || other.gameObject.CompareTag("EndLimb")) && !other.gameObject.CompareTag("Armor"))
            {
                eidid = other.gameObject.GetComponentInParent<EnemyIdentifierIdentifier>();
                
                EnemyIdentifier eid = null;

                if (eidid != null && eidid.eid != null)
                {
                    eid = eidid.eid;
                }

                if ((eid == null) || (__instance.alreadyHitEnemies.Count != 0 && __instance.alreadyHitEnemies.Contains(eid)) || ((eid.enemyType == __instance.safeEnemyType || EnemyIdentifier.CheckHurtException(__instance.safeEnemyType, eid.enemyType, __instance.targetHandle)) && (!__instance.friendly || eid.immuneToFriendlyFire) && !__instance.playerBullet && !__instance.parried))
                {
                    return true;
                }

                if (eid.Dead)
                {
                    return true;
                }

                var eadd = eid.GetComponent<EnemyAdditions>();

                Assert.IsNotNull(eadd);

                var feedbacker = eadd.Feedbacker;

                if (!feedbacker.Enabled)
                {
                    return true;
                }

                if (!feedbacker.ReadyToParry)
                {
                    return true;
                }

                if (__instance.unparryable || __instance.undeflectable)
                {
                    return true;
                }

                if (parryability < 0.5f)
                {
                    return true;
                }
                
                boostTracker.IncrementEnemyBoost();
                
                if (boostTracker.IgnoreColliders.Contains(other))
                {
                    return false;
                }

                var parryForce = feedbacker.SolveParryForce(__instance.transform.position, __instance.GetComponent<Rigidbody>().velocity);
                __instance.homingType = HomingType.None;
                __instance.transform.rotation = Quaternion.LookRotation(parryForce);
                feedbacker.ParryEffect();
                boostTracker.IgnoreColliders = eadd.Colliders;
                __instance.friendly = false;
                return false;
            }
            return true;
        }

        public static void Postfix(Projectile __instance, Collider other)
        {
        }
    }

}