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
            switch (eid.enemyType)
            {
                case EnemyType.BigJohnator:
                    return EnemyGameplayRank.Ultraboss;
                case EnemyType.CancerousRodent:
                    return EnemyGameplayRank.Normal;
                case EnemyType.Centaur:
                    return EnemyGameplayRank.Boss;
                case EnemyType.Cerberus:
                    return eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Normal;
                case EnemyType.Drone:
                    return EnemyGameplayRank.Normal;
                case EnemyType.Ferryman:
                    return EnemyGameplayRank.Miniboss;
                case EnemyType.Filth:
                    return EnemyGameplayRank.Normal;
                case EnemyType.FleshPanopticon:
                    return EnemyGameplayRank.Ultraboss;
                case EnemyType.FleshPrison:
                    return EnemyGameplayRank.Ultraboss;
                case EnemyType.Gabriel:
                    return EnemyGameplayRank.Boss;
                case EnemyType.GabrielSecond:
                    return EnemyGameplayRank.Boss;
                case EnemyType.Gutterman:
                    return EnemyGameplayRank.Normal;
                case EnemyType.Guttertank:
                    return EnemyGameplayRank.Normal;
                case EnemyType.HideousMass:
                    return eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Miniboss;
                case EnemyType.Idol:
                    return EnemyGameplayRank.Normal;
                case EnemyType.Leviathan:
                    return EnemyGameplayRank.Boss;
                case EnemyType.MaliciousFace:
                    return eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Normal;
                case EnemyType.Mandalore:
                    return EnemyGameplayRank.Ultraboss;
                case EnemyType.Mannequin:
                    return EnemyGameplayRank.Normal;
                case EnemyType.Mindflayer:
                    return eid.isBoss ? EnemyGameplayRank.Ultraboss : EnemyGameplayRank.Boss;
                case EnemyType.Minos:
                    return EnemyGameplayRank.Ultraboss;
                case EnemyType.MinosPrime:
                    return EnemyGameplayRank.Ultraboss;
                case EnemyType.Minotaur:
                    return EnemyGameplayRank.Boss;
                case EnemyType.Puppet:
                    return EnemyGameplayRank.Normal;
                case EnemyType.Schism:
                    return EnemyGameplayRank.Normal;
                case EnemyType.Sisyphus:
                    return EnemyGameplayRank.Miniboss;
                case EnemyType.SisyphusPrime:
                    return EnemyGameplayRank.Ultraboss;
                case EnemyType.Soldier:
                /*
                * Likely an unexpected decision, but my reasoning is that they appear in layer 2, in which you can only have nail guns, slab revolvers, and shotguns
                * and they're explosion proof. Furthermore, unlike malicious faces, they immediately start running around rapidly upon instantiating.
                * As it is if they're considered normal, your only real counters is to parry them with a *direct* hit, 
                * to slowly pick them off with the punch, revolver shot, punch combo,
                * to slowly pick all of the clones off with nails, or to hope one somehow ends up off of the ground and them you can hit them with any explosion 
                * and it'll get them.
                * For that reason, it seems like a disproportionate difficulty increase.
                * Also, you can knock them off the ground with explosions... but only if it's non-damaging, for some ridiculous reason. 
                * Whilst it would be cool if that weren't the case, it is indeed the case unfortunately. 
                * So you cannot do a 'blast off of the ground with a proj boost then hit them with another explosive' move.
                * You *might* be able to get them all with a red explosion? Not sure though. Either way, I'd say that's both boring and a disproportionate 
                * difficulty increase.
                * And upon further testing, a regular shotgun fire at close range *sometimes* gets them all, alternatively, a close range shotgun parry
                * works fairly consistently. Still seems disproportionate, you get swarmed by these on the second layer so it seems unfun.
                * And I mean, the second layer also just is bad. So like, whatever it takes to make it last less time.
                */
                    return EnemyGameplayRank.Miniboss;
                case EnemyType.Stalker:
                    return EnemyGameplayRank.Normal;
                case EnemyType.Stray:
                    return EnemyGameplayRank.Normal;
                case EnemyType.Streetcleaner:
                    return EnemyGameplayRank.Normal;
                case EnemyType.Swordsmachine:
                    return eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Miniboss;
                case EnemyType.Turret:
                    return EnemyGameplayRank.Normal;
                case EnemyType.V2:
                    return EnemyGameplayRank.Boss;
                case EnemyType.V2Second:
                    return EnemyGameplayRank.Boss;
                case EnemyType.VeryCancerousRodent:
                    return EnemyGameplayRank.Boss;
                case EnemyType.Virtue:
                    return EnemyGameplayRank.Normal;
                case EnemyType.Wicked:
                    return EnemyGameplayRank.Ultraboss;
            }

            throw new NotImplementedException();
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