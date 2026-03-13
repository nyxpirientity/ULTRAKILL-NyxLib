using System;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class EnemyEvents
    {
        public static Action<EnemyComponents> PreStart = null;
        public static Action<EnemyComponents> PostStart = null;
        public static Action<EnemyComponents> PreDisabled = null;
        public static Action<EnemyComponents> PreDestroy = null;

        // params: (EnemyAdditions enemy, GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
        public static Action<EnemyComponents, GameObject, Vector3, Vector3?, float, bool, float, GameObject, bool, bool> PreHurt = null;
        // params: (EnemyAdditions enemy, GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
        public static Action<EnemyComponents, GameObject, Vector3, Vector3?, float, bool, float, GameObject, bool, bool> PostHurt = null;
        
        public static Action<EnemyComponents, bool> PreDeath = null;
        public static Action<EnemyComponents, bool> PostDeath = null;
        public static Action<EnemyComponents> Death = null;
    }
}