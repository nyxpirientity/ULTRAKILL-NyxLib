using System;
using UnityEngine;

namespace UKAIW
{
    public static class EnemyEvents
    {
        public static Action<EnemyAdditions> PreStart = null;
        public static Action<EnemyAdditions> PostStart = null;
        public static Action<EnemyAdditions> PreDisabled = null;
        public static Action<EnemyAdditions> PreDestroy = null;

        // params: (EnemyAdditions eadd, GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
        public static Action<EnemyAdditions, GameObject, Vector3, Vector3?, float, bool, float, GameObject, bool, bool> PreHurt = null;
        // params: (EnemyAdditions eadd, GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
        public static Action<EnemyAdditions, GameObject, Vector3, Vector3?, float, bool, float, GameObject, bool, bool> PostHurt = null;
        
        public static Action<EnemyAdditions, bool> PreDeath = null;
        public static Action<EnemyAdditions, bool> PostDeath = null;
        public static Action<EnemyAdditions> Death = null;
    }
}