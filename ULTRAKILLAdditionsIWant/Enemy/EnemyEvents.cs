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

        // params: eid, enemyGo, target, force, multiplier, critMultiplier, sourceWeapon, fromExplosion, hitPoint
        public static Action<EnemyIdentifier, GameObject, GameObject, Vector3, float, float, GameObject, bool, Vector3?> PreHurt = null;
        // params: eid, enemyGo, target, force, multiplier, critMultiplier, sourceWeapon, fromExplosion, hitPoint
        public static Action<EnemyIdentifier, GameObject, GameObject, Vector3, float, float, GameObject, bool, Vector3?> PostHurt = null;
        
        public static Action<EnemyIdentifier, bool> PreDeath = null;
        public static Action<EnemyIdentifier, bool> PostDeath = null;
        public static Action<EnemyIdentifier> Death = null;
    }
}