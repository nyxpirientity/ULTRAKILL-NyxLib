using System;
using HarmonyLib;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

namespace UKAIW
{
    public static class EnemyEvents
    {
        public static Action<EnemyIdentifier, GameObject> PreStart = null;
        public static Action<EnemyIdentifier, GameObject> PostStart = null;
        public static Action<EnemyIdentifier, GameObject> PreDisabled = null;
        public static Action<EnemyIdentifier, GameObject> PreDestroy = null;

        // params: (EnemyAdditions eadd, GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
        public static Action<EnemyAdditions, GameObject, Vector3, Vector3?, float, bool, float, GameObject, bool, bool> PreHurt = null;
        // params: (EnemyAdditions eadd, GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
        public static Action<EnemyAdditions, GameObject, Vector3, Vector3?, float, bool, float, GameObject, bool, bool> PostHurt = null;
        
        public static Action<EnemyIdentifier, bool> PreDeath = null;
        public static Action<EnemyIdentifier, bool> PostDeath = null;
        public static Action<EnemyIdentifier> Death = null;
    }
}