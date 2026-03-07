using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    [HarmonyPatch(typeof(Cannonball), "Start")]
    static class CannonballStartPatch
    {
        public static void Prefix(Cannonball __instance)
        {
            __instance.GetOrAddComponent<ProjectileBoostTracker>();
        }
        
        public static void Postfix(Cannonball __instance)
        {
            
        }
    }

    [HarmonyPatch(typeof(Cannonball), "Collide")]
    static class CannonballCollidePatch
    {
        private static readonly FieldInfo _checkingForBreakFi = AccessTools.Field(typeof(Cannonball), "checkingForBreak");

        public static bool Prefix(Cannonball __instance, Collider other)
        {
            Cannonball cannonball = __instance;
            Collider col = cannonball.GetComponent<Collider>();
            var boostTracker = cannonball.GetComponent<ProjectileBoostTracker>();

            Action failedParry = () =>
            {
                if (boostTracker.NumPlayerBoosts > 0 && boostTracker.NumEnemyBoosts > 0)
                {
                    StyleHUD.Instance.AddPoints(10, "<color=#00c3ff>VOLLEYBALL</color>");
                }
            };

            if (other.TryGetComponent<NewMovement>(out var _) && !boostTracker.LastBoostedByPlayer && boostTracker.HasBeenBoosted)
            {
                __instance.Explode();
                return false;
            }

            if ((cannonball.launched || cannonball.canBreakBeforeLaunched) && !other.isTrigger && (LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment) || (cannonball.launched && other.gameObject.layer == 0 && (!other.gameObject.CompareTag("Player") || !col.isTrigger))))
            {
                return true;
            }
            else
            {
                var checkingForBreak = (bool)(_checkingForBreakFi.GetValue(cannonball));

                if ((!cannonball.launched && !cannonball.physicsCannonball) || (other.gameObject.layer != 10 && other.gameObject.layer != 11 && other.gameObject.layer != 12) || checkingForBreak)
                {
                    return true;
                }

                if (!(other.attachedRigidbody ? other.attachedRigidbody.TryGetComponent<EnemyIdentifierIdentifier>(out var eidid) : other.TryGetComponent<EnemyIdentifierIdentifier>(out eidid)) || eidid.eid == null)
                {
                    return true;
                }
                
                var enemy = eidid.eid.GetComponent<EnemyComponents>();

                Assert.IsNotNull(enemy);

                if (enemy.Eid.Dead)
                {
                    return true;
                }                

                var feedbacker = enemy.Feedbacker;

                if (!feedbacker.Enabled)
                {
                    failedParry();
                    return true;
                }

                if (boostTracker.SafeEid == enemy.Eid)
                {
                    return false;
                }

                var parryability = boostTracker.NotifyContact();
                boostTracker.MarkCannotBeEnemyParried();

                if (!feedbacker.ReadyToParry)
                {
                    failedParry();
                    return true;
                }

                if ((parryability < 0.5f))
                {
                    failedParry();
                    return true;
                }

                var parryForce = enemy.Feedbacker.SolveParryForce(cannonball.transform.position, cannonball.Rigidbody.velocity);
                
                cannonball.Rigidbody.velocity = parryForce * cannonball.Rigidbody.velocity.magnitude;
                cannonball.Rigidbody.transform.rotation = Quaternion.LookRotation(parryForce);

                boostTracker.IncrementEnemyBoost();
                feedbacker.ParryEffect();
  
                boostTracker.IgnoreColliders = enemy.Colliders;
                boostTracker.SafeEid = enemy.Eid;
                cannonball.hitEnemies.Add(enemy.Eid);

                var v1 = NewMovement.Instance;
                Physics.IgnoreCollision(__instance.GetComponent<Collider>(), v1.playerCollider, false);

                return false;
            }
        }
        
        public static void Postfix(Cannonball __instance, Collider other)
        {
            
        }
    }
    [HarmonyPatch(typeof(Cannonball), "OnTriggerEnter")]
    static class CannonballOnTriggerEnterPatch
    {
        public static void Prefix(Cannonball __instance, Collider other)
        {
        }
        
        public static void Postfix(Cannonball __instance, Collider other)
        {
            
        }
    }

    [HarmonyPatch(typeof(Cannonball), "Launch")]
    static class CannonballLaunchPatch
    {
        public static void Prefix(Cannonball __instance)
        {
            __instance.GetComponent<ProjectileBoostTracker>().IncrementPlayerBoosts();
        }
        
        public static void Postfix(Cannonball __instance)
        {
            
        }
    }
}
