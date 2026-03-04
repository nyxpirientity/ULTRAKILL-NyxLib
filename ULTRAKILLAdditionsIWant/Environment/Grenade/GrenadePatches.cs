using System;
using System.Reflection;
using HarmonyLib;
using ULTRAKILL.Portal;
using UnityEngine;

namespace UKAIW
{
    [HarmonyPatch(typeof(Grenade), "Start")]
    static class GrenadeStartPatch
    {
        public static void Prefix(Grenade __instance)
        {
            ProjectileBoostTracker boostTracker = __instance.GetOrAddComponent<ProjectileBoostTracker>();
        }
        
        public static void Postfix(Grenade __instance)
        {
            
        }
    }

    [HarmonyPatch(typeof(Grenade), "GrenadeBeam")]
    static class GrenadeBeamPatch
    {
        private static FieldInfo grenadeBeamFi = typeof(Grenade).GetField("grenadeBeam", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void Prefix(Grenade __instance, Vector3 targetPoint, GameObject newSourceWeapon = null)
        {
            var grenadeBeamPrefab = (RevolverBeam)(grenadeBeamFi.GetValue(__instance));
            var boostTracker = grenadeBeamPrefab.gameObject.AddComponent<ProjectileBoostTracker>();
            boostTracker.CopyFrom(__instance.GetComponent<ProjectileBoostTracker>());
        }
        
        public static void Postfix(Grenade __instance, Vector3 targetPoint, GameObject newSourceWeapon = null)
        {
            var grenadeBeamPrefab = (RevolverBeam)grenadeBeamFi.GetValue(__instance);
            UnityEngine.Object.Destroy(grenadeBeamPrefab.gameObject.GetComponent<ProjectileBoostTracker>());
        }
    }


    [HarmonyPatch(typeof(Grenade), "Collision", new Type[]{typeof(Collider), typeof(Vector3)})]
    static class GrenadeCollisionPatch
    {
        public static bool Prefix(Grenade __instance, Collider other, Vector3 velocity)
        {
            if (Cheats.IsCheatDisabled(Cheats.FeedbackerForAll))
            {
                return true;
            }

            var boostTracker = __instance.GetComponent<ProjectileBoostTracker>();

            var parryability = boostTracker.NotifyContact();

            if (other.TryGetComponent<PortalAwarePlayerColliderClone>(out var _) || __instance.IsExploded() || (!__instance.enemy && other.CompareTag("Player")) || other.gameObject.layer == 14 || other.gameObject.layer == 20)
            {
                return true;
            }
            
            if ((other.gameObject.layer == 11 || other.gameObject.layer == 10) && (other.attachedRigidbody ? other.attachedRigidbody.TryGetComponent<EnemyIdentifierIdentifier>(out var eidid) : other.TryGetComponent<EnemyIdentifierIdentifier>(out eidid)) && (bool)eidid.eid)
            {
                var eadd = eidid.eid.GetComponent<EnemyAdditions>();

                Assert.IsNotNull(eadd);

                if (eadd.Eid.Dead)
                {
                    return true;
                }

                if (__instance.ignoreEnemyType.Count > 0 && __instance.ignoreEnemyType.Contains(eadd.Eid.enemyType))
                {
                    return true;
                }

                var feedbacker = eadd.Feedbacker;

                if (!feedbacker.Enabled)
                {
                    return true;
                }

                if (!feedbacker.ReadyToParry)
                {
                    return true;
                }

                if ((parryability < 0.5f))
                {
                    return true;
                }
                
                var parryForce = eadd.Feedbacker.SolveParryForce(__instance.transform.position, __instance.rb.velocity);
                
                if (__instance.rocket)
                {
                    __instance.rb.velocity = parryForce * __instance.rb.velocity.magnitude;
                    __instance.rb.rotation = Quaternion.LookRotation(parryForce);
                }
                else
                {
                    var vel = (parryForce * __instance.rb.velocity.magnitude * 5.0f);

                    if (vel.magnitude > 80.0f)
                    {
                        vel = vel.normalized * 80.0f;
                    }

                    __instance.rb.velocity = vel;
                }

                __instance.enemy = true;
                boostTracker.IncrementEnemyBoost();
                feedbacker.ParryEffect();

                boostTracker.IgnoreColliders = eadd.Colliders;

                var v1 = NewMovement.Instance;
                Physics.IgnoreCollision(__instance.GetComponent<Collider>(), v1.playerCollider, false);

                return false;
            }

            return true;
        }

        public static void Postfix(Grenade __instance, Collider other, Vector3 velocity)
        {
        }
    }
}