using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using ULTRAKILL.Portal;
using UnityEngine;

namespace UKAIW
{
    [HarmonyPatch(typeof(Nail), "Start")]
    static class NailStartPatch
    {
        public static void Prefix(Nail __instance)
        {
            __instance.GetOrAddComponent<ProjectileBoostTracker>();
        }
        
        public static void Postfix(Nail __instance)
        {
        }
    }

    [HarmonyPatch(typeof(Nail), "FixedUpdate")]
    static class NailFixedUpdatePatch
    {
        public static void Prefix(Nail __instance)
        {
            if (!__instance.sawblade && !__instance.chainsaw)
            {
                return;
            }

            var boostTracker = __instance.GetComponent<ProjectileBoostTracker>();

            boostTracker.PreNailFixedUpdate();
        }
        
        public static void Postfix(Nail __instance)
        {
        }
    }

    [HarmonyPatch(typeof(Nail), "HitEnemy")]
    static class NailHitEnemyPatch
    {
        static FieldInfo sameEnemyHitCooldownFi = AccessTools.Field(typeof(Nail), "sameEnemyHitCooldown");
        static FieldInfo currentHitEnemyFi =AccessTools.Field(typeof(Nail), "currentHitEnemy");
        static FieldInfo hitLimbsFi = AccessTools.Field(typeof(Nail), "hitLimbs");

        public static bool Prefix(Nail __instance, Transform other, EnemyIdentifierIdentifier eidid = null)
        {
            Nail nail = __instance;
            
            if (!nail.chainsaw && !nail.sawblade)
            {
                return true;
            }

            if (nail.magnets.Count > 0)
            {
                return true;
            }
            
            var sameEnemyHitCooldown = (float)sameEnemyHitCooldownFi.GetValue(nail);
            var currentHitEnemy = (EnemyIdentifier)currentHitEnemyFi.GetValue(nail);
            var hitLimbs = (List<Transform>)hitLimbsFi.GetValue(nail);

            if ((eidid == null && !other.TryGetComponent<EnemyIdentifierIdentifier>(out eidid)) || !eidid.eid || (nail.enemy && eidid != null && eidid.eid != null && eidid.eid.enemyType == nail.safeEnemyType) || (nail.sawblade && ((sameEnemyHitCooldown > 0f && currentHitEnemy != null && currentHitEnemy == eidid.eid) || hitLimbs.Contains(other))))
            {
                return true;
            }

            Assert.IsNotNull(eidid);
            Assert.IsNotNull(eidid.eid);

            var eadd = eidid.eid.GetComponent<EnemyAdditions>();
            
            Assert.IsNotNull(eadd);

            if (eadd.Eid.Dead)
            {
                return true;
            }                

            var feedbacker = eadd.Feedbacker;

            if (!feedbacker.Enabled)
            {
                return true;
            }

            var boostTracker = nail.GetComponent<ProjectileBoostTracker>();

            if (boostTracker.SafeEid == eadd.Eid)
            {
                return false;
            }

            var parryability = boostTracker.NotifyContact();
            boostTracker.MarkCannotBeEnemyParried();

            if (!feedbacker.ReadyToParry)
            {
                return true;
            }

            if ((parryability < 0.5f))
            {
                return true;
            }

            var parryForce = eadd.Feedbacker.SolveParryForce(nail.transform.position, nail.rb.velocity);
            
            nail.rb.velocity = parryForce * nail.rb.velocity.magnitude;
            nail.rb.transform.rotation = Quaternion.LookRotation(parryForce);

            boostTracker.IncrementEnemyBoost();
            feedbacker.ParryEffect();

            boostTracker.IgnoreColliders = eadd.Colliders;
            boostTracker.SafeEid = eadd.Eid;
            nail.enemy = true;
            nail.gameObject.layer = 2;

            var v1 = NewMovement.Instance;
            
            foreach (var col in boostTracker.Colliders)
            {
                Physics.IgnoreCollision(col, v1.playerCollider, false);
            }

            return false;
        }
        
        public static void Postfix(Nail __instance, Transform other, EnemyIdentifierIdentifier eidid = null)
        {
        }
    }
}