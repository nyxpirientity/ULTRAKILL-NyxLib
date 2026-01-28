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
                /*
                * the second layer also just is bad. So like, whatever it takes to make it last less time.
                */
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

    public static class EnemyEvents
    {
        public static Action<EnemyIdentifier, GameObject> PreStart = null;
        public static Action<EnemyIdentifier, GameObject> PostStart = null;
        public static Action<EnemyIdentifier, GameObject> PreDisabled = null;
        public static Action<EnemyIdentifier, GameObject> PreDestroy = null;
        
        // pre death that doesn't include instakill
        public static Action<EnemyIdentifier> PreNoIKDeath = null;
        // post death that doesn't include instakill
        public static Action<EnemyIdentifier> PostNoIKDeath = null;

        public static Action<EnemyIdentifier> DuringDeath = null;
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
}