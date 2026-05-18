using System;
using HarmonyLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
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

        public static bool IsEnraged(this EnemyIdentifier eid)
        {
            if (eid.Dead)
            {
                return false;
            }
            
            var enrage = eid.GetComponent<IEnrage>();

            return (enrage?.isEnraged).GetValueOrDefault(false);
        }

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

            if (!(enrage is null))
            {
                enrage.Enrage();
                return true;
            }

            switch (eid.enemyType)
            {
                case global::EnemyType.Swordsmachine:
                    eid.GetComponent<SwordsMachine>()?.Enrage();
                    return true;
                case global::EnemyType.Cerberus:
                    eid.GetComponent<StatueBoss>()?.Enrage();
                    return true;
                case global::EnemyType.Virtue:
                case global::EnemyType.Drone:
                    eid.GetComponent<Drone>()?.Enrage();
                    return true;
                case global::EnemyType.V2:
                    eid.GetComponent<V2>()?.Enrage();
                    return true;
                case global::EnemyType.Mindflayer:
                    eid.GetComponent<Mindflayer>()?.Enrage();
                    return true;
                case global::EnemyType.HideousMass:
                    if (!(eid.GetComponent<Mass>()?.GetComponentInChildren<EnemySimplifier>()?.enraged).GetValueOrDefault(true))
                    {
                        eid.GetComponent<Mass>()?.Enrage();
                        return true;
                    }
                    return false;
                case global::EnemyType.MaliciousFace:
                    eid.GetComponent<SpiderBody>()?.Enrage();
                    return true;
                case global::EnemyType.Gutterman:
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
                case global::EnemyType.Swordsmachine:
                    eid.GetComponent<SwordsMachine>()?.UnEnrage();
                    return true;
                case global::EnemyType.Cerberus:
                    eid.GetComponent<StatueBoss>()?.UnEnrage();
                    return true;
                case global::EnemyType.Virtue:
                case global::EnemyType.Drone:
                    eid.GetComponent<Drone>()?.UnEnrage();
                    return true;
                case global::EnemyType.V2:
                    eid.GetComponent<V2>()?.UnEnrage();
                    return true;
                case global::EnemyType.Mindflayer:
                    eid.GetComponent<Mindflayer>()?.UnEnrage();
                    return true;
                case global::EnemyType.MaliciousFace:
                    eid.GetComponent<SpiderBody>()?.UnEnrage();
                    return true;
                case global::EnemyType.Gutterman:
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

        public static EnemyGameplayRank GetEnemyGameplayRank(this EnemyIdentifier eid)
        {
            return eid.enemyType switch
            {
                global::EnemyType.BigJohnator => EnemyGameplayRank.Ultraboss,
                global::EnemyType.CancerousRodent => EnemyGameplayRank.Normal,
                global::EnemyType.Centaur => EnemyGameplayRank.Boss,
                global::EnemyType.Cerberus => eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Normal,
                global::EnemyType.Drone => EnemyGameplayRank.Normal,
                global::EnemyType.Ferryman => EnemyGameplayRank.Miniboss,
                global::EnemyType.Filth => EnemyGameplayRank.Normal,
                global::EnemyType.FleshPanopticon => EnemyGameplayRank.Ultraboss,
                global::EnemyType.FleshPrison => EnemyGameplayRank.Ultraboss,
                global::EnemyType.Gabriel => EnemyGameplayRank.Boss,
                global::EnemyType.GabrielSecond => EnemyGameplayRank.Boss,
                global::EnemyType.Gutterman => EnemyGameplayRank.Normal,
                global::EnemyType.Guttertank => EnemyGameplayRank.Normal,
                global::EnemyType.HideousMass => eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Miniboss,
                global::EnemyType.Idol => EnemyGameplayRank.Normal,
                global::EnemyType.Leviathan => EnemyGameplayRank.Boss,
                global::EnemyType.MaliciousFace => eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Normal,
                global::EnemyType.Mandalore => EnemyGameplayRank.Ultraboss,
                global::EnemyType.Mannequin => EnemyGameplayRank.Normal,
                global::EnemyType.Mindflayer => eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Miniboss,
                global::EnemyType.Minos => EnemyGameplayRank.Ultraboss,
                global::EnemyType.MinosPrime => EnemyGameplayRank.Ultraboss,
                global::EnemyType.Minotaur => EnemyGameplayRank.Boss,
                global::EnemyType.Puppet => EnemyGameplayRank.Normal,
                global::EnemyType.Schism => EnemyGameplayRank.Normal,
                global::EnemyType.Sisyphus => EnemyGameplayRank.Miniboss,
                global::EnemyType.SisyphusPrime => EnemyGameplayRank.Ultraboss,
                global::EnemyType.Soldier => EnemyGameplayRank.Normal,
                global::EnemyType.Stalker => EnemyGameplayRank.Normal,
                global::EnemyType.Stray => EnemyGameplayRank.Normal,
                global::EnemyType.Streetcleaner => EnemyGameplayRank.Normal,
                global::EnemyType.Swordsmachine => eid.isBoss ? EnemyGameplayRank.Boss : EnemyGameplayRank.Miniboss,
                global::EnemyType.Turret => EnemyGameplayRank.Normal,
                global::EnemyType.V2 => EnemyGameplayRank.Boss,
                global::EnemyType.V2Second => EnemyGameplayRank.Boss,
                global::EnemyType.VeryCancerousRodent => EnemyGameplayRank.Boss,
                global::EnemyType.Virtue => EnemyGameplayRank.Normal,
                global::EnemyType.Wicked => EnemyGameplayRank.Ultraboss,
                global::EnemyType.Providence => EnemyGameplayRank.Normal,
                global::EnemyType.Power => EnemyGameplayRank.Normal,
                global::EnemyType.MirrorReaper => EnemyGameplayRank.Miniboss,
                global::EnemyType.Geryon => EnemyGameplayRank.Boss,
                global::EnemyType.Deathcatcher => EnemyGameplayRank.Normal,
                _ => throw new NotImplementedException(),
            };
        }

        public static EnemySpeciesType GetSpeciesType(this global::EnemyType enemyType)
        {
            return enemyType switch
            {
                global::EnemyType.Filth => EnemySpeciesType.Husk,
                global::EnemyType.BigJohnator => EnemySpeciesType.UltraUnknown,
                global::EnemyType.CancerousRodent => EnemySpeciesType.UltraUnknown,
                global::EnemyType.Centaur => EnemySpeciesType.Machine,
                global::EnemyType.Cerberus => EnemySpeciesType.Demon,
                global::EnemyType.Drone => EnemySpeciesType.Machine,
                global::EnemyType.Ferryman => EnemySpeciesType.Husk,
                global::EnemyType.FleshPanopticon => EnemySpeciesType.OrganicMachine,
                global::EnemyType.FleshPrison => EnemySpeciesType.OrganicMachine,
                global::EnemyType.Gabriel => EnemySpeciesType.Angel,
                global::EnemyType.GabrielSecond => EnemySpeciesType.Angel,
                global::EnemyType.Gutterman => EnemySpeciesType.Machine,
                global::EnemyType.Guttertank => EnemySpeciesType.Machine,
                global::EnemyType.HideousMass => EnemySpeciesType.Demon,
                global::EnemyType.Idol => EnemySpeciesType.Demon,
                global::EnemyType.Leviathan => EnemySpeciesType.Demon,
                global::EnemyType.MaliciousFace => EnemySpeciesType.Demon,
                global::EnemyType.Mandalore => EnemySpeciesType.UltraUnknown,
                global::EnemyType.Mannequin => EnemySpeciesType.Demon,
                global::EnemyType.Mindflayer => EnemySpeciesType.Machine,
                global::EnemyType.Minos => EnemySpeciesType.Husk,
                global::EnemyType.MinosPrime => EnemySpeciesType.Soul,
                global::EnemyType.Minotaur => EnemySpeciesType.Demon,
                global::EnemyType.Puppet => EnemySpeciesType.Puppet,
                global::EnemyType.Schism => EnemySpeciesType.Husk,
                global::EnemyType.Sisyphus => EnemySpeciesType.Husk,
                global::EnemyType.SisyphusPrime => EnemySpeciesType.Soul,
                global::EnemyType.Soldier => EnemySpeciesType.Husk,
                global::EnemyType.Stalker => EnemySpeciesType.Husk,
                global::EnemyType.Stray => EnemySpeciesType.Husk,
                global::EnemyType.Streetcleaner => EnemySpeciesType.Machine,
                global::EnemyType.Swordsmachine => EnemySpeciesType.Machine,
                global::EnemyType.Turret => EnemySpeciesType.Machine,
                global::EnemyType.V2 => EnemySpeciesType.Machine,
                global::EnemyType.V2Second => EnemySpeciesType.Machine,
                global::EnemyType.VeryCancerousRodent => EnemySpeciesType.UltraUnknown,
                global::EnemyType.Virtue => EnemySpeciesType.Angel,
                global::EnemyType.Providence => EnemySpeciesType.Angel,
                global::EnemyType.Power => EnemySpeciesType.Angel,
                global::EnemyType.MirrorReaper => EnemySpeciesType.Husk,
                global::EnemyType.Geryon => EnemySpeciesType.Demon,
                global::EnemyType.Deathcatcher => EnemySpeciesType.Demon,
                global::EnemyType.Wicked => EnemySpeciesType.Unknown,
                _ => throw new NotImplementedException(),
            };
        }

        public static EnemySpeciesRank GetSpeciesRank(this global::EnemyType enemyType)
        {
            return enemyType switch
            {
                global::EnemyType.BigJohnator => EnemySpeciesRank.NotApplicable,
                global::EnemyType.CancerousRodent => EnemySpeciesRank.Supreme,
                global::EnemyType.Centaur => EnemySpeciesRank.Supreme,
                global::EnemyType.Cerberus => EnemySpeciesRank.Lesser,
                global::EnemyType.Drone => EnemySpeciesRank.Lesser,
                global::EnemyType.Ferryman => EnemySpeciesRank.Supreme,
                global::EnemyType.Filth => EnemySpeciesRank.Lesser,
                global::EnemyType.FleshPanopticon => EnemySpeciesRank.Greater,
                global::EnemyType.FleshPrison => EnemySpeciesRank.Lesser,
                global::EnemyType.Gabriel => EnemySpeciesRank.Supreme,
                global::EnemyType.GabrielSecond => EnemySpeciesRank.Supreme,
                global::EnemyType.Gutterman => EnemySpeciesRank.Greater,
                global::EnemyType.Guttertank => EnemySpeciesRank.Greater,
                global::EnemyType.HideousMass => EnemySpeciesRank.Greater,
                global::EnemyType.Idol => EnemySpeciesRank.Lesser,
                global::EnemyType.Leviathan => EnemySpeciesRank.Supreme,
                global::EnemyType.MaliciousFace => EnemySpeciesRank.Lesser,
                global::EnemyType.Mandalore => EnemySpeciesRank.NotApplicable,
                global::EnemyType.Mannequin => EnemySpeciesRank.Lesser,
                global::EnemyType.Mindflayer => EnemySpeciesRank.Greater,
                global::EnemyType.Minos => EnemySpeciesRank.Supreme,
                global::EnemyType.MinosPrime => EnemySpeciesRank.Prime,
                global::EnemyType.Minotaur => EnemySpeciesRank.Supreme,
                global::EnemyType.Puppet => EnemySpeciesRank.NotApplicable,
                global::EnemyType.Schism => EnemySpeciesRank.Greater,
                global::EnemyType.Sisyphus => EnemySpeciesRank.Supreme,
                global::EnemyType.SisyphusPrime => EnemySpeciesRank.Prime,
                global::EnemyType.Soldier => EnemySpeciesRank.Greater,
                global::EnemyType.Stalker => EnemySpeciesRank.Lesser,
                global::EnemyType.Stray => EnemySpeciesRank.Lesser,
                global::EnemyType.Streetcleaner => EnemySpeciesRank.Lesser,
                global::EnemyType.Swordsmachine => EnemySpeciesRank.Greater,
                global::EnemyType.Turret => EnemySpeciesRank.Greater,
                global::EnemyType.V2 => EnemySpeciesRank.Supreme,
                global::EnemyType.V2Second => EnemySpeciesRank.Supreme,
                global::EnemyType.VeryCancerousRodent => EnemySpeciesRank.Prime,
                global::EnemyType.Virtue => EnemySpeciesRank.Lesser,
                global::EnemyType.MirrorReaper => EnemySpeciesRank.Supreme,
                global::EnemyType.Deathcatcher => EnemySpeciesRank.Lesser,
                global::EnemyType.Geryon => EnemySpeciesRank.Supreme,
                global::EnemyType.Providence => EnemySpeciesRank.Lesser,
                global::EnemyType.Power => EnemySpeciesRank.Greater,
                global::EnemyType.Wicked => EnemySpeciesRank.NotApplicable,
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

        public static Bounds SolveEnemyBounds(this EnemyComponents enemy)
        {
            return SolveEnemyBounds(enemy.gameObject);    
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
            var eid = __instance;
            var eidGo = eid.gameObject;
            
            try
            {
                var enemy = eidGo.GetOrAddComponent<EnemyComponents>();
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

            TryLog.Action(() => { EnemyEvents.PreStart?.Invoke(enemy.GetComponent<EnemyComponents>()); });
        }

        public static void Postfix(EnemyIdentifier __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;

            var ec = enemy.GetComponent<EnemyComponents>();
            TryLog.Action(() => {EnemyEvents.PostStart?.Invoke(ec);});
            
            if (Cheats.IsCheatEnabled(Cheats.LogEIDInfo))
            {
                ec.RootGameObject.DebugPrintChildren();
            }

            ec.EidStarted = true;

            if (Options.LogEnemyTypeOnStart.Value)
            {
                Log.Message($"{enemyGo.name}: enemy type is: {enemy.enemyType}");
            }
        }
    }

    [HarmonyPatch(typeof(EnemyComponents), "OnDisable")]
    static class EnemyDisablePatch
    {
        public static void Prefix(EnemyComponents __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PreDisabled?.Invoke(__instance);});
        }

        public static void Postfix(EnemyComponents __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;
        }
    }

    [HarmonyPatch(typeof(EnemyComponents), "OnDestroy")]
    static class EnemyDestroyPatch
    {
        public static void Prefix(EnemyComponents __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;

            TryLog.Action(() => {EnemyEvents.PreDestroy?.Invoke(__instance);});
        }

        public static void Postfix(EnemyComponents __instance)
        {
            var enemy = __instance;
            var enemyGo = enemy.gameObject;
        }
    }
}