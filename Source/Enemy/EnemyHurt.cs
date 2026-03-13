using HarmonyLib;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    [HarmonyPatch(typeof(EnemyIdentifier), "DeliverDamage")]
    static class EidDeliverDamagePatch
    {
        public static void Prefix(EnemyIdentifier __instance, GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, bool tryForExplode, float critMultiplier = 0f, GameObject sourceWeapon = null, bool ignoreTotalDamageTakenMultiplier = false, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPreHurt(target, force, hitPoint, multiplier, critMultiplier, sourceWeapon, tryForExplode, ignoreTotalDamageTakenMultiplier, fromExplosion, typeof(EidDeliverDamagePatch));
        }

        public static void Postfix(EnemyIdentifier __instance, GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, bool tryForExplode, float critMultiplier = 0f, GameObject sourceWeapon = null, bool ignoreTotalDamageTakenMultiplier = false, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPostHurt(target, force, hitPoint, multiplier, critMultiplier, sourceWeapon, tryForExplode, ignoreTotalDamageTakenMultiplier, fromExplosion, typeof(EidDeliverDamagePatch));
        }
    }   

    [HarmonyPatch(typeof(Zombie), "GetHurt")]
    static class ZombieHurtPatch
    {
        public static void Prefix(Zombie __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPreHurt(target, force, null, multiplier, critMultiplier, sourceWeapon, false, false, fromExplosion, typeof(ZombieHurtPatch));
        }

        public static void Postfix(Zombie __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPostHurt(target, force, null, multiplier, critMultiplier, sourceWeapon, false, false, fromExplosion, typeof(ZombieHurtPatch));
        }
    }   

    [HarmonyPatch(typeof(Machine), "GetHurt")]
    static class MachineHurtPatch
    {
        public static void Prefix(Machine __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPreHurt(target, force, null, multiplier, critMultiplier, sourceWeapon, false, false, fromExplosion, typeof(MachineHurtPatch));
        }

        public static void Postfix(Machine __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPostHurt(target, force, null, multiplier, critMultiplier, sourceWeapon, false, false, fromExplosion, typeof(MachineHurtPatch));
        }
    }

    [HarmonyPatch(typeof(SpiderBody), "GetHurt")]
    static class SpiderBodyHurtPatch
    {
        public static void Prefix(SpiderBody __instance, GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, GameObject sourceWeapon)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPreHurt(target, force, hitPoint, multiplier, 1.0f, sourceWeapon, false, false, false, typeof(SpiderBodyHurtPatch));
        }

        public static void Postfix(SpiderBody __instance, GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, GameObject sourceWeapon)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPostHurt(target, force, hitPoint, multiplier, 1.0f, sourceWeapon, false, false, false, typeof(SpiderBodyHurtPatch));
        }
    }

    [HarmonyPatch(typeof(Statue), "GetHurt")]
    static class StatueHurtPatch
    {
        public static void Prefix(Statue __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, Vector3 hurtPos, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPreHurt(target, force, null, multiplier, critMultiplier, sourceWeapon, false, false, fromExplosion, typeof(StatueHurtPatch));
        }

        public static void Postfix(Statue __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, Vector3 hurtPos, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPostHurt(target, force, null, multiplier, critMultiplier, sourceWeapon, false, false, fromExplosion, typeof(StatueHurtPatch));
        }
    }

    [HarmonyPatch(typeof(Drone), "GetHurt")]
    static class DroneHurtPatch
    {
        public static void Prefix(Drone __instance, Vector3 force, float multiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPreHurt(__instance.gameObject, force, null, multiplier, 1.0f, sourceWeapon, false, false, fromExplosion, typeof(DroneHurtPatch));
        }

        public static void Postfix(Drone __instance, Vector3 force, float multiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyComponents>();

            enemy.NotifyOfPostHurt(__instance.gameObject, force, null, multiplier, 1.0f, sourceWeapon, false, false, fromExplosion, typeof(DroneHurtPatch));
        }
    }
}