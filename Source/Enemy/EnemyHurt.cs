using HarmonyLib;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    [HarmonyPatch(typeof(EnemyIdentifier), "DeliverDamage")]
    static class EidDeliverDamagePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();

        public static bool Prefix(EnemyIdentifier __instance, GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, bool tryForExplode, float critMultiplier = 0f, GameObject sourceWeapon = null, bool ignoreTotalDamageTakenMultiplier = false, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            _cancellationTracker.Reset();
            enemy.NotifyOfPreHurt(_cancellationTracker.GetCanceler(), target, force, hitPoint, multiplier, critMultiplier, sourceWeapon, tryForExplode, ignoreTotalDamageTakenMultiplier, fromExplosion, typeof(EidDeliverDamagePatch));
            _cancellationTracker.TryInvokeReimplementation();
            return !_cancellationTracker.Cancelled;
        }

        public static void Postfix(EnemyIdentifier __instance, GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, bool tryForExplode, float critMultiplier = 0f, GameObject sourceWeapon = null, bool ignoreTotalDamageTakenMultiplier = false, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPostHurt(_cancellationTracker.GetCancelInfo(), target, force, hitPoint, multiplier, critMultiplier, sourceWeapon, tryForExplode, ignoreTotalDamageTakenMultiplier, fromExplosion, typeof(EidDeliverDamagePatch));
        }
    }   

    [HarmonyPatch(typeof(Zombie), "GetHurt")]
    static class ZombieHurtPatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static bool Prefix(Zombie __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            _cancellationTracker.Reset();
            enemy.NotifyOfPreHurt(_cancellationTracker.GetCanceler(), target, force, null, multiplier, critMultiplier, sourceWeapon, false, false, fromExplosion, typeof(ZombieHurtPatch));
            _cancellationTracker.TryInvokeReimplementation();
            return !_cancellationTracker.Cancelled;
        }

        public static void Postfix(Zombie __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPostHurt(_cancellationTracker.GetCancelInfo(), target, force, null, multiplier, critMultiplier, sourceWeapon, false, false, fromExplosion, typeof(ZombieHurtPatch));
        }
    }   

    [HarmonyPatch(typeof(Machine), "GetHurt")]
    static class MachineHurtPatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static bool Prefix(Machine __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            _cancellationTracker.Reset();
            enemy.NotifyOfPreHurt(_cancellationTracker.GetCanceler(), target, force, null, multiplier, critMultiplier, sourceWeapon, false, false, fromExplosion, typeof(MachineHurtPatch));
            _cancellationTracker.TryInvokeReimplementation();
            return !_cancellationTracker.Cancelled;
        }

        public static void Postfix(Machine __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPostHurt(_cancellationTracker.GetCancelInfo(), target, force, null, multiplier, critMultiplier, sourceWeapon, false, false, fromExplosion, typeof(MachineHurtPatch));
        }
    }

    [HarmonyPatch(typeof(SpiderBody), "GetHurt")]
    static class SpiderBodyHurtPatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static bool Prefix(SpiderBody __instance, GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, GameObject sourceWeapon)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            _cancellationTracker.Reset();
            enemy.NotifyOfPreHurt(_cancellationTracker.GetCanceler(), target, force, hitPoint, multiplier, 1.0f, sourceWeapon, false, false, false, typeof(SpiderBodyHurtPatch));
            _cancellationTracker.TryInvokeReimplementation();
            return !_cancellationTracker.Cancelled;
        }

        public static void Postfix(SpiderBody __instance, GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, GameObject sourceWeapon)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPostHurt(_cancellationTracker.GetCancelInfo(), target, force, hitPoint, multiplier, 1.0f, sourceWeapon, false, false, false, typeof(SpiderBodyHurtPatch));
        }
    }

    [HarmonyPatch(typeof(Statue), "GetHurt")]
    static class StatueHurtPatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static bool Prefix(Statue __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, Vector3 hurtPos, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            _cancellationTracker.Reset();
            enemy.NotifyOfPreHurt(_cancellationTracker.GetCanceler(), target, force, null, multiplier, critMultiplier, sourceWeapon, false, false, fromExplosion, typeof(StatueHurtPatch));
            _cancellationTracker.TryInvokeReimplementation();
            return !_cancellationTracker.Cancelled;
        }

        public static void Postfix(Statue __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, Vector3 hurtPos, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPostHurt(_cancellationTracker.GetCancelInfo(), target, force, null, multiplier, critMultiplier, sourceWeapon, false, false, fromExplosion, typeof(StatueHurtPatch));
        }
    }

    [HarmonyPatch(typeof(Drone), "GetHurt")]
    static class DroneHurtPatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static bool Prefix(Drone __instance, Vector3 force, float multiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            _cancellationTracker.Reset();
            enemy.NotifyOfPreHurt(_cancellationTracker.GetCanceler(), __instance.gameObject, force, null, multiplier, 1.0f, sourceWeapon, false, false, fromExplosion, typeof(DroneHurtPatch));
            _cancellationTracker.TryInvokeReimplementation();
            return !_cancellationTracker.Cancelled;
        }

        public static void Postfix(Drone __instance, Vector3 force, float multiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPostHurt(_cancellationTracker.GetCancelInfo(), __instance.gameObject, force, null, multiplier, 1.0f, sourceWeapon, false, false, fromExplosion, typeof(DroneHurtPatch));
        }
    }
}