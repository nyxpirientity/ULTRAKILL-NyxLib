using System;
using UnityEngine;

namespace UKAIW
{
    public static class EnemyFeedbackerDelegate
    {
        public static void Initialize()
        {
            EnemyEvents.PreHurt += PreEnemyHurt;
            EnemyEvents.PostHurt += PostEnemyHurt;
        }

        private static void PreEnemyHurt(EnemyIdentifier eid, GameObject go, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon, bool fromExplosion, Vector3? hitPoint)
        {
            var eadd = go.GetComponent<EnemyAdditions>();

            eadd.Feedbacker.PreEnemyHurt(eid, go, target, force, multiplier, critMultiplier, sourceWeapon, fromExplosion, hitPoint);
        }

        private static void PostEnemyHurt(EnemyIdentifier eid, GameObject go, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon, bool fromExplosion, Vector3? hitPoint)
        {
            var eadd = go.GetComponent<EnemyAdditions>();

            eadd.Feedbacker.PostEnemyHurt(eid, go, target, force, multiplier, critMultiplier, sourceWeapon, fromExplosion, hitPoint);
        }
    }

    public class EnemyFeedbacker : MonoBehaviour
    {
        protected void Start()
        {
        }

        protected void Update()
        {
        }

        protected void FixedUpdate()
        {
        }

        protected void OnDestroy()
        {
        }

        internal void PreEnemyHurt(EnemyIdentifier eid, GameObject go, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon, bool fromExplosion, Vector3? hitPoint)
        {
        }

        internal void PostEnemyHurt(EnemyIdentifier eid, GameObject go, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon, bool fromExplosion, Vector3? hitPoint)
        {
        }
    }
}