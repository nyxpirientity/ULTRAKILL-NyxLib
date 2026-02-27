using System;
using HarmonyLib;
using MelonLoader;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

namespace UKAIW
{
    public enum EnemyGameplayRank
    {
        Normal, Miniboss, Boss, Ultraboss
    }

    public enum EnemySpeciesType
    {
        Husk, Machine, Demon, Angel, Soul, OrganicMachine, Puppet, Unknown, UltraUnknown
    }

    public enum EnemySpeciesRank
    {
        NotApplicable, Lesser, Greater, Supreme, Prime
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
            
            var enrage = eid.GetComponent<IEnrage>();

            if ((enrage?.isEnraged).GetValueOrDefault(false))
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
                EnemyType.Providence => EnemyGameplayRank.Normal,
                EnemyType.Power => EnemyGameplayRank.Normal,
                EnemyType.MirrorReaper => EnemyGameplayRank.Miniboss,
                EnemyType.Geryon => EnemyGameplayRank.Boss,
                EnemyType.Deathcatcher => EnemyGameplayRank.Normal,
                _ => throw new NotImplementedException(),
            };
        }

        public static EnemySpeciesType GetSpeciesType(this EnemyType enemyType)
        {
            return enemyType switch
            {
                EnemyType.Filth => EnemySpeciesType.Husk,
                EnemyType.BigJohnator => EnemySpeciesType.UltraUnknown,
                EnemyType.CancerousRodent => EnemySpeciesType.UltraUnknown,
                EnemyType.Centaur => EnemySpeciesType.Machine,
                EnemyType.Cerberus => EnemySpeciesType.Demon,
                EnemyType.Drone => EnemySpeciesType.Machine,
                EnemyType.Ferryman => EnemySpeciesType.Husk,
                EnemyType.FleshPanopticon => EnemySpeciesType.OrganicMachine,
                EnemyType.FleshPrison => EnemySpeciesType.OrganicMachine,
                EnemyType.Gabriel => EnemySpeciesType.Angel,
                EnemyType.GabrielSecond => EnemySpeciesType.Angel,
                EnemyType.Gutterman => EnemySpeciesType.Machine,
                EnemyType.Guttertank => EnemySpeciesType.Machine,
                EnemyType.HideousMass => EnemySpeciesType.Demon,
                EnemyType.Idol => EnemySpeciesType.Demon,
                EnemyType.Leviathan => EnemySpeciesType.Demon,
                EnemyType.MaliciousFace => EnemySpeciesType.Demon,
                EnemyType.Mandalore => EnemySpeciesType.UltraUnknown,
                EnemyType.Mannequin => EnemySpeciesType.Demon,
                EnemyType.Mindflayer => EnemySpeciesType.Machine,
                EnemyType.Minos => EnemySpeciesType.Husk,
                EnemyType.MinosPrime => EnemySpeciesType.Soul,
                EnemyType.Minotaur => EnemySpeciesType.Demon,
                EnemyType.Puppet => EnemySpeciesType.Puppet,
                EnemyType.Schism => EnemySpeciesType.Husk,
                EnemyType.Sisyphus => EnemySpeciesType.Husk,
                EnemyType.SisyphusPrime => EnemySpeciesType.Soul,
                EnemyType.Soldier => EnemySpeciesType.Husk,
                EnemyType.Stalker => EnemySpeciesType.Husk,
                EnemyType.Stray => EnemySpeciesType.Husk,
                EnemyType.Streetcleaner => EnemySpeciesType.Machine,
                EnemyType.Swordsmachine => EnemySpeciesType.Machine,
                EnemyType.Turret => EnemySpeciesType.Machine,
                EnemyType.V2 => EnemySpeciesType.Machine,
                EnemyType.V2Second => EnemySpeciesType.Machine,
                EnemyType.VeryCancerousRodent => EnemySpeciesType.UltraUnknown,
                EnemyType.Virtue => EnemySpeciesType.Angel,
                EnemyType.Providence => EnemySpeciesType.Angel,
                EnemyType.Power => EnemySpeciesType.Angel,
                EnemyType.MirrorReaper => EnemySpeciesType.Husk,
                EnemyType.Geryon => EnemySpeciesType.Demon,
                EnemyType.Deathcatcher => EnemySpeciesType.Demon,
                EnemyType.Wicked => EnemySpeciesType.Unknown,
                _ => throw new NotImplementedException(),
            };
        }

        public static EnemySpeciesRank GetSpeciesRank(this EnemyType enemyType)
        {
            return enemyType switch
            {
                EnemyType.BigJohnator => EnemySpeciesRank.NotApplicable,
                EnemyType.CancerousRodent => EnemySpeciesRank.Supreme,
                EnemyType.Centaur => EnemySpeciesRank.Supreme,
                EnemyType.Cerberus => EnemySpeciesRank.Lesser,
                EnemyType.Drone => EnemySpeciesRank.Lesser,
                EnemyType.Ferryman => EnemySpeciesRank.Supreme,
                EnemyType.Filth => EnemySpeciesRank.Lesser,
                EnemyType.FleshPanopticon => EnemySpeciesRank.Greater,
                EnemyType.FleshPrison => EnemySpeciesRank.Lesser,
                EnemyType.Gabriel => EnemySpeciesRank.Supreme,
                EnemyType.GabrielSecond => EnemySpeciesRank.Supreme,
                EnemyType.Gutterman => EnemySpeciesRank.Greater,
                EnemyType.Guttertank => EnemySpeciesRank.Greater,
                EnemyType.HideousMass => EnemySpeciesRank.Greater,
                EnemyType.Idol => EnemySpeciesRank.Lesser,
                EnemyType.Leviathan => EnemySpeciesRank.Supreme,
                EnemyType.MaliciousFace => EnemySpeciesRank.Lesser,
                EnemyType.Mandalore => EnemySpeciesRank.NotApplicable,
                EnemyType.Mannequin => EnemySpeciesRank.Lesser,
                EnemyType.Mindflayer => EnemySpeciesRank.Greater,
                EnemyType.Minos => EnemySpeciesRank.Supreme,
                EnemyType.MinosPrime => EnemySpeciesRank.Prime,
                EnemyType.Minotaur => EnemySpeciesRank.Supreme,
                EnemyType.Puppet => EnemySpeciesRank.NotApplicable,
                EnemyType.Schism => EnemySpeciesRank.Greater,
                EnemyType.Sisyphus => EnemySpeciesRank.Supreme,
                EnemyType.SisyphusPrime => EnemySpeciesRank.Prime,
                EnemyType.Soldier => EnemySpeciesRank.Greater,
                EnemyType.Stalker => EnemySpeciesRank.Lesser,
                EnemyType.Stray => EnemySpeciesRank.Lesser,
                EnemyType.Streetcleaner => EnemySpeciesRank.Lesser,
                EnemyType.Swordsmachine => EnemySpeciesRank.Greater,
                EnemyType.Turret => EnemySpeciesRank.Greater,
                EnemyType.V2 => EnemySpeciesRank.Supreme,
                EnemyType.V2Second => EnemySpeciesRank.Supreme,
                EnemyType.VeryCancerousRodent => EnemySpeciesRank.Prime,
                EnemyType.Virtue => EnemySpeciesRank.Lesser,
                EnemyType.MirrorReaper => EnemySpeciesRank.Supreme,
                EnemyType.Deathcatcher => EnemySpeciesRank.Lesser,
                EnemyType.Geryon => EnemySpeciesRank.Supreme,
                EnemyType.Providence => EnemySpeciesRank.Lesser,
                EnemyType.Power => EnemySpeciesRank.Greater,
                EnemyType.Wicked => EnemySpeciesRank.NotApplicable,
                _ => throw new NotImplementedException(),
            };
        }

        public static EnemySpeciesRank GetSpeciesRank(this EnemyIdentifier eid)
        {
            return eid.enemyType.GetSpeciesRank();
        }

        public static EnemySpeciesType GetSpeciesType(this EnemyIdentifier eid)
        {
            return eid.enemyType.GetSpeciesType();
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

    [HarmonyPatch(typeof(Enemy), "Awake")]
    static class EnemyPreSpawnPatch
    {
        public static void Prefix(Enemy __instance)
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

    [HarmonyPatch(typeof(Enemy), "Start")]
    static class EnemyStartPatch
    {
        public static void Prefix(Enemy __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => { EnemyEvents.PreStart?.Invoke(enemy, enemyGo); });
        }

        public static void Postfix(Enemy __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PostStart?.Invoke(enemy, enemyGo);});
            
            if (Cheats.IsCheatEnabled(Cheats.LogEIDInfo))
            {
                enemyGo.GetComponent<EnemyAdditions>().RootGameObject.DebugPrintChildren();
            }

            if (Options.LogEnemyTypeOnStart.Value)
            {
                MelonLogger.Msg($"{enemyGo.name}: enemy type is: {enemy.EID.enemyType}");
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

            TryLog.Action(() => {EnemyEvents.PreDisabled?.Invoke(enemy.GetComponent<Enemy>(), enemyGo);});
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

            TryLog.Action(() => {EnemyEvents.PreDestroy?.Invoke(enemy.GetComponent<Enemy>(), enemyGo);});
        }

        public static void Postfix(EnemyIdentifier __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;
        }
    }
}