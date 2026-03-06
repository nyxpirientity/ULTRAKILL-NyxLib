using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace UKAIW
{
    [HarmonyPatch(typeof(RevolverBeam), "Start")]
    static class RevolverBeamStartPatch
    {
        public static void Prefix(RevolverBeam __instance)
        {
            __instance.GetOrAddComponent<ProjectileBoostTracker>();
        }

        public static void Postfix(RevolverBeam __instance)
        {
            
        }
    }

    [HarmonyPatch(typeof(RevolverBeam), "HitSomething")]
    static class RevolverBeamHitSomethingPatch
    {
        public static bool Prefix(RevolverBeam __instance, PhysicsCastResult hit)
        {
            if (Cheats.IsCheatDisabled(Cheats.FeedbackerForAll))
            {
                return true;
            }

            if (__instance.beamType == BeamType.Enemy || __instance.beamType == BeamType.MaliciousFace)
            {
                return true;
            }

            var boostTracker = __instance.GetComponent<ProjectileBoostTracker>();

            var parryability = boostTracker.NotifyContact();

            if ((hit.collider.attachedRigidbody ? hit.collider.attachedRigidbody.TryGetComponent<EnemyIdentifierIdentifier>(out var eidid) : hit.collider.TryGetComponent<EnemyIdentifierIdentifier>(out eidid)) && (bool)eidid.eid)
            {
                var eadd = eidid.eid.GetComponent<EnemyAdditions>();

                Assert.IsNotNull(eadd);

                if (eadd.Eid.Dead)
                {
                    return true;
                }

                if (parryability < 0.5f)
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

                var parryForce = feedbacker.SolveParryForce(hit.point, Vector3.one);
                
                feedbacker.ParryEffect();

                var counterBeamGo = GameObject.Instantiate(Assets.EnemyRevolverBullet);
                var counterBeam = counterBeamGo.GetComponent<Projectile>();
                var counterBeamBoostTracker = counterBeamGo.GetOrAddComponent<ProjectileBoostTracker>();
                counterBeamBoostTracker.CopyFrom(boostTracker);
                counterBeamBoostTracker.IncrementEnemyBoost();
                counterBeamGo.transform.position = hit.point;
                counterBeamGo.transform.rotation = Quaternion.LookRotation(parryForce);
                counterBeamGo.SetActive(true);
                
                var colliders = eadd.Colliders;
                counterBeamBoostTracker.IgnoreColliders = colliders;

                //counterBeam.safeEnemyType = eadd.Eid.enemyType;
                counterBeam.playerBullet = true;
                counterBeam.damage = __instance.damage * 25.0f;
                counterBeam.enemyDamageMultiplier = 1.0f / 25.0f;
                __instance.fake = true;
                return false;
            }

            return true;
        }

        public static void Postfix(RevolverBeam __instance, PhysicsCastResult hit)
        {
        }
    }

    [HarmonyPatch(typeof(RevolverBeam), "PiercingShotCheck")]
    static class RevolverBeamPiercingShotCheckPatch
    {
        private static FieldInfo _enemiesPiercedFi = typeof(RevolverBeam).GetField("enemiesPierced", BindingFlags.Instance | BindingFlags.NonPublic);

        public static bool Prefix(RevolverBeam __instance)
        {
            if (Cheats.IsCheatDisabled(Cheats.FeedbackerForAll))
            {
                return true;
            }

            if (__instance.beamType == BeamType.Enemy || __instance.beamType == BeamType.MaliciousFace)
            {
                return true;
            }

            var boostTracker = __instance.GetComponent<ProjectileBoostTracker>();

            var parryability = boostTracker.NotifyContact();

            int enemiesPierced = (int)_enemiesPiercedFi.GetValue(__instance);
            
            if (enemiesPierced != 0)
            {
                return true;
            }

            if (__instance.hitList.Count <= enemiesPierced)
            {
                return true;
            }

            var hit = __instance.hitList[enemiesPierced];
            
            if (hit.collider == null)
            {
                return true;
            }
            
            if ((hit.collider.attachedRigidbody ? hit.collider.attachedRigidbody.TryGetComponent<EnemyIdentifierIdentifier>(out var eidid) : hit.collider.TryGetComponent<EnemyIdentifierIdentifier>(out eidid)) && (bool)eidid.eid)
            {
                var eadd = eidid.eid.GetComponent<EnemyAdditions>();

                Assert.IsNotNull(eadd);

                if (eadd.Eid.Dead)
                {
                    return true;
                }

                if (parryability < 0.5f)
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

                var parryForce = feedbacker.SolveParryForce(hit.point, Vector3.one);
                
                feedbacker.ParryEffect();

                var counterBeamGo = GameObject.Instantiate(Assets.EnemyRevolverBullet);
                var counterBeam = counterBeamGo.GetComponent<Projectile>();
                var counterBeamBoostTracker = counterBeamGo.GetOrAddComponent<ProjectileBoostTracker>();
                counterBeamBoostTracker.CopyFrom(boostTracker);
                counterBeamBoostTracker.IncrementEnemyBoost();
                counterBeamGo.transform.position = hit.point;
                counterBeamGo.transform.rotation = Quaternion.LookRotation(parryForce);
                counterBeamGo.SetActive(true);
                
                var colliders = eadd.Colliders;
                counterBeamBoostTracker.IgnoreColliders = colliders;
                counterBeamBoostTracker.SafeEid = eadd.Eid;

                //counterBeam.safeEnemyType = eadd.Eid.enemyType;
                counterBeam.playerBullet = true;
                counterBeam.damage = __instance.damage * 25.0f;
                counterBeam.enemyDamageMultiplier = 1.0f / 25.0f;
                __instance.fake = true;
                _enemiesPiercedFi.SetValue(__instance, int.MaxValue);
                return true;
            }

            return true;
        }

        public static void Postfix(RevolverBeam __instance)
        {
        }
    }
}