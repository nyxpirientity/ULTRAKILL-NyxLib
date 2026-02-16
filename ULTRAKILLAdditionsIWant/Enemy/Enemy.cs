using System;
using HarmonyLib;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

namespace UKAIW
{
    public enum EnemyGameplayRank
    {
        Normal, Miniboss, Boss, Ultraboss
    }

    public static class EnemyUtils
    {
        public static int NumGameplayRanks = 4;

        public static bool TryEnrage(this EnemyIdentifier eid)
        {
            if (eid.Dead)
            {
                return false;
            }
            
            switch (eid.enemyType)
            {
                case EnemyType.Swordsmachine:
                    eid.GetComponent<SwordsMachine>()?.Enrage();
                    return true;
                case EnemyType.Cerberus:
                    eid.GetComponent<StatueBoss>()?.Enrage();
                    return true;
                case EnemyType.Virtue:
                case EnemyType.Drone:
                    eid.GetComponent<Drone>()?.Enrage();
                    return true;
                case EnemyType.V2:
                    eid.GetComponent<V2>()?.Enrage();
                    return true;
                case EnemyType.Mindflayer:
                    eid.GetComponent<Mindflayer>()?.Enrage();
                    return true;
                case EnemyType.HideousMass:
                    if (!(eid.GetComponent<Mass>()?.GetComponentInChildren<EnemySimplifier>()?.enraged).GetValueOrDefault(true))
                    {
                        eid.GetComponent<Mass>()?.Enrage();
                        return true;
                    }
                    return false;
                case EnemyType.MaliciousFace:
                    eid.GetComponent<SpiderBody>()?.Enrage();
                    return true;
                case EnemyType.Gutterman:
                    if (!eid.dead)
                    {
                        eid.GetComponent<Gutterman>()?.Enrage();
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        public static bool TryUnenrage(this EnemyIdentifier eid)
        {
            if (eid.Dead)
            {
                return false;
            }

            switch (eid.enemyType)
            {
                case EnemyType.Swordsmachine:
                    eid.GetComponent<SwordsMachine>()?.UnEnrage();
                    return true;
                case EnemyType.Cerberus:
                    eid.GetComponent<StatueBoss>()?.UnEnrage();
                    return true;
                case EnemyType.Virtue:
                case EnemyType.Drone:
                    eid.GetComponent<Drone>()?.UnEnrage();
                    return true;
                case EnemyType.V2:
                    eid.GetComponent<V2>()?.UnEnrage();
                    return true;
                case EnemyType.Mindflayer:
                    eid.GetComponent<Mindflayer>()?.UnEnrage();
                    return true;
                case EnemyType.MaliciousFace:
                    eid.GetComponent<SpiderBody>()?.UnEnrage();
                    return true;
                case EnemyType.Gutterman:
                    if (!eid.dead)
                    {
                        eid.GetComponent<Gutterman>()?.UnEnrage();
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        /* GetHurt would be more consistent but JUST incase they add GetHurt, I'll go with what I usually call it.*/
        public static void ApplyDamage(this EnemyIdentifier eid, Vector3 force, Vector3 hitPoint, float multiplier, float critMultiplier, GameObject sourceWeapon, bool fromExplosion)
        {
            eid.DeliverDamage(eid.gameObject, force, hitPoint, multiplier, false, critMultiplier, sourceWeapon, false, fromExplosion);
        }

        public static EnemyGameplayRank GetEnemyGameplayRank(EnemyIdentifier eid)
        {
            return eid.enemyType switch
            {
                EnemyType.BigJohnator => EnemyGameplayRank.Ultraboss,
                EnemyType.CancerousRodent => EnemyGameplayRank.Normal,
                EnemyType.Centaur => EnemyGameplayRank.Boss,
                EnemyType.Cerberus => eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Normal,
                EnemyType.Drone => EnemyGameplayRank.Normal,
                EnemyType.Ferryman => EnemyGameplayRank.Miniboss,
                EnemyType.Filth => EnemyGameplayRank.Normal,
                EnemyType.FleshPanopticon => EnemyGameplayRank.Ultraboss,
                EnemyType.FleshPrison => EnemyGameplayRank.Ultraboss,
                EnemyType.Gabriel => EnemyGameplayRank.Boss,
                EnemyType.GabrielSecond => EnemyGameplayRank.Boss,
                EnemyType.Gutterman => EnemyGameplayRank.Normal,
                EnemyType.Guttertank => EnemyGameplayRank.Normal,
                EnemyType.HideousMass => eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Miniboss,
                EnemyType.Idol => EnemyGameplayRank.Normal,
                EnemyType.Leviathan => EnemyGameplayRank.Boss,
                EnemyType.MaliciousFace => eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Normal,
                EnemyType.Mandalore => EnemyGameplayRank.Ultraboss,
                EnemyType.Mannequin => EnemyGameplayRank.Normal,
                EnemyType.Mindflayer => eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Miniboss,
                EnemyType.Minos => EnemyGameplayRank.Ultraboss,
                EnemyType.MinosPrime => EnemyGameplayRank.Ultraboss,
                EnemyType.Minotaur => EnemyGameplayRank.Boss,
                EnemyType.Puppet => EnemyGameplayRank.Normal,
                EnemyType.Schism => EnemyGameplayRank.Normal,
                EnemyType.Sisyphus => EnemyGameplayRank.Miniboss,
                EnemyType.SisyphusPrime => EnemyGameplayRank.Ultraboss,
                EnemyType.Soldier => EnemyGameplayRank.Normal,
                EnemyType.Stalker => EnemyGameplayRank.Normal,
                EnemyType.Stray => EnemyGameplayRank.Normal,
                EnemyType.Streetcleaner => EnemyGameplayRank.Normal,
                EnemyType.Swordsmachine => eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Miniboss,
                EnemyType.Turret => EnemyGameplayRank.Normal,
                EnemyType.V2 => EnemyGameplayRank.Boss,
                EnemyType.V2Second => EnemyGameplayRank.Boss,
                EnemyType.VeryCancerousRodent => EnemyGameplayRank.Boss,
                EnemyType.Virtue => EnemyGameplayRank.Normal,
                EnemyType.Wicked => EnemyGameplayRank.Ultraboss,
                _ => throw new NotImplementedException(),
            };
        }

        public static Bounds SolveEnemyBounds(this GameObject enemyGo)
        {
            Bounds bounds = new Bounds();
            var mainCollider = enemyGo.GetComponent<Collider>();
            var colliders = enemyGo.GetComponents<Collider>().AddRangeToArray(enemyGo.GetComponentsInChildren<Collider>());

            if (mainCollider != null)
            {
                bounds = mainCollider.bounds;
            }
            
            if (bounds.extents.magnitude > 2.0f)
            {
                return bounds;
            }
            else
            {
                foreach (var collider in colliders)
                {
                    Vector3 boundsMin = bounds.min;
                    Vector3 boundsMax = bounds.max;
                    
                    if (boundsMin.x > collider.bounds.min.x)
                    {
                        boundsMin.x = collider.bounds.min.x;
                    }
                    
                    if (boundsMin.y > collider.bounds.min.y)
                    {
                        boundsMin.y = collider.bounds.min.y;
                    }

                    if (boundsMin.z > collider.bounds.min.z)
                    {
                        boundsMin.z = collider.bounds.min.z;
                    }                
                
                    if (boundsMax.x < collider.bounds.max.x)
                    {
                        boundsMax.x = collider.bounds.max.x;
                    }
                    
                    if (boundsMax.y < collider.bounds.max.y)
                    {
                        boundsMax.y = collider.bounds.max.y;
                    }

                    if (boundsMax.z < collider.bounds.max.z)
                    {
                        boundsMax.z = collider.bounds.max.z;
                    }

                    bounds.SetMinMax(boundsMin, boundsMax);
                }

                return bounds;
            }
        }
    }

    [HarmonyPatch(typeof(EnemyIdentifier), "Awake")]
    static class EnemyPreSpawnPatch
    {
        public static void Prefix(EnemyIdentifier __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;
            
            if (enemyGo.GetComponent<EnemyAdditions>() != null)
            {
                return;
            }

            try
            {
                var eadd = enemyGo.AddComponent<EnemyAdditions>();
                eadd.SetupMods();
            }
            catch (Exception e)
            {
                Log.Error($"{e}");
            }
        }

        public static void Postfix(EnemyIdentifier __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;

        }
    }

    [HarmonyPatch(typeof(EnemyIdentifier), "Start")]
    static class EnemyStartPatch
    {
        public static void Prefix(EnemyIdentifier __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PreStart?.Invoke(enemy, enemyGo);});
        }

        public static void Postfix(EnemyIdentifier __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PostStart?.Invoke(enemy, enemyGo);});
            
            if (Cheats.IsCheatEnabled(Cheats.LogEIDInfo))
            {
                enemyGo.transform.parent.gameObject.DebugPrintChildren();
            }
        }
    }

    [HarmonyPatch(typeof(EnemyIdentifier), "OnDisable")]
    static class EnemyDisablePatch
    {
        public static void Prefix(EnemyIdentifier __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PreDisabled?.Invoke(enemy, enemyGo);});
        }

        public static void Postfix(EnemyIdentifier __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;
        }
    }

    [HarmonyPatch(typeof(EnemyAdditions), "OnDestroy")]
    static class EnemyDestroyPatch
    {
        public static void Prefix(EnemyIdentifier __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PreDestroy?.Invoke(enemy, enemyGo);});
        }

        public static void Postfix(EnemyIdentifier __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;
        }
    }

    [HarmonyPatch(typeof(Zombie), "GetHurt")]
    static class ZombieHurtPatch
    {
        public static void Prefix(Zombie __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyIdentifier>();
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PreHurt?.Invoke(enemy, enemyGo, target, force, multiplier, critMultiplier, sourceWeapon, fromExplosion, null); });
        }

        public static void Postfix(Zombie __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyIdentifier>();
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PostHurt?.Invoke(enemy, enemyGo, target, force, multiplier, critMultiplier, sourceWeapon, fromExplosion, null); });
        }
    }   

    [HarmonyPatch(typeof(Machine), "GetHurt")]
    static class MachineHurtPatch
    {
        public static void Prefix(Machine __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyIdentifier>();
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PreHurt?.Invoke(enemy, enemyGo, target, force, multiplier, critMultiplier, sourceWeapon, fromExplosion, null); });
        }

        public static void Postfix(Machine __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyIdentifier>();
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PostHurt?.Invoke(enemy, enemyGo, target, force, multiplier, critMultiplier, sourceWeapon, fromExplosion, null); });
        }
    }

    [HarmonyPatch(typeof(SpiderBody), "GetHurt")]
    static class SpiderBodyHurtPatch
    {
        public static void Prefix(SpiderBody __instance, GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, GameObject sourceWeapon)
        {
            var enemy = __instance.GetComponent<EnemyIdentifier>();
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PreHurt?.Invoke(enemy, enemyGo, target, force, multiplier, 1.0f, sourceWeapon, false, null); });
        }

        public static void Postfix(SpiderBody __instance, GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, GameObject sourceWeapon)
        {
            var enemy = __instance.GetComponent<EnemyIdentifier>();
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PostHurt?.Invoke(enemy, enemyGo, target, force, multiplier, 1.0f, sourceWeapon, false, null); });
        }
    }

    [HarmonyPatch(typeof(Statue), "GetHurt")]
    static class StatueHurtPatch
    {
        public static void Prefix(Statue __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, Vector3 hurtPos, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyIdentifier>();
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PreHurt?.Invoke(enemy, enemyGo, target, force, multiplier, critMultiplier, sourceWeapon, fromExplosion, hurtPos); });
        }

        public static void Postfix(Statue __instance, GameObject target, Vector3 force, float multiplier, float critMultiplier, Vector3 hurtPos, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyIdentifier>();
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PostHurt?.Invoke(enemy, enemyGo, target, force, multiplier, critMultiplier, sourceWeapon, fromExplosion, hurtPos); });
        }
    }

    [HarmonyPatch(typeof(Drone), "GetHurt")]
    static class DroneHurtPatch
    {
        public static void Prefix(Drone __instance, Vector3 force, float multiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyIdentifier>();
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PreHurt?.Invoke(enemy, enemyGo, null, force, multiplier, 1.0f, sourceWeapon, fromExplosion, null); });
        }

        public static void Postfix(Drone __instance, Vector3 force, float multiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
        {
            var enemy = __instance.GetComponent<EnemyIdentifier>();
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PostHurt?.Invoke(enemy, enemyGo, null, force, multiplier, 1.0f, sourceWeapon, fromExplosion, null); });
        }
    }
}