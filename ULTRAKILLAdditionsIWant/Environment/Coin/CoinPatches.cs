using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace UKAIW
{
    [HarmonyPatch(typeof(Coin), nameof(Coin.ReflectRevolver))]
    static class CoinReflectRevolverPatch
    {
        static void DeliverDamageReplacement(EnemyIdentifier eid, GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, bool tryForExplode, float critMultiplier = 0f, GameObject sourceWeapon = null, bool ignoreTotalDamageTakenMultiplier = false, bool fromExplosion = false)
        {
            Action deliverThatDamage = () =>
            {
                eid.DeliverDamage(target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon, ignoreTotalDamageTakenMultiplier, fromExplosion);  
            };

            if (Cheats.IsCheatDisabled(Cheats.FeedbackerForAll))
            {
                deliverThatDamage();
                return;
            }
            
            var coin = _currentCoin;

            var boostTracker = coin.GetComponent<ProjectileBoostTracker>();

            var parryability = boostTracker.NotifyContact();

            var eadd = eid.GetComponent<EnemyAdditions>();

            Assert.IsNotNull(eadd);

            if (eadd.Eid.Dead)
            {
                deliverThatDamage();
                return;
            }

            if (parryability < 0.5f)
            {
                deliverThatDamage();
                return;
            }

            var feedbacker = eadd.Feedbacker;

            if (!feedbacker.Enabled)
            {
                deliverThatDamage();
                return;
            }

            if (!feedbacker.ReadyToParry)
            {
                deliverThatDamage();
                return;
            }

            var parryForce = feedbacker.SolveParryForce(hitPoint, Vector3.zero);
            
            feedbacker.ParryEffect();
            var coinMeshF = coin.GetComponentInChildren<MeshFilter>();
            var coinMeshR = coin.GetComponentInChildren<MeshRenderer>();
            
            var counterBeamGo = GameObject.Instantiate(Assets.EnemyRevolverBullet);
            var counterBeam = counterBeamGo.GetComponent<Projectile>();
            var counterBeamBoostTracker = counterBeamGo.GetOrAddComponent<ProjectileBoostTracker>();
            counterBeam.GetComponentInChildren<MeshFilter>().mesh = coinMeshF.mesh;
            counterBeam.GetComponentInChildren<MeshRenderer>().material = coinMeshR.material;
            counterBeamBoostTracker.CopyFrom(boostTracker);
            counterBeamBoostTracker.IncrementEnemyBoost();
            counterBeamGo.transform.position = hitPoint;
            counterBeamGo.transform.rotation = Quaternion.LookRotation(parryForce);
            counterBeamGo.SetActive(true);
            
            var colliders = eadd.Colliders;
            counterBeamBoostTracker.IgnoreColliders = colliders;
            counterBeamBoostTracker.SafeEid = eid;

            //counterBeam.safeEnemyType = eadd.Eid.enemyType;
            counterBeam.playerBullet = true;
            counterBeam.damage = coin.power * 5.0f;
            counterBeam.enemyDamageMultiplier = 1.0f / 5.0f;
            UnityEngine.Object.Destroy(coin.gameObject);
            return;
        }

        private static Coin _currentCoin = null;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instr in instructions)
            {
                if (instr.Calls(typeof(EnemyIdentifier).GetMethod(nameof(EnemyIdentifier.DeliverDamage))))
                {
                    instr.operand = typeof(CoinReflectRevolverPatch).GetMethod(nameof(DeliverDamageReplacement), BindingFlags.Static | BindingFlags.NonPublic);
                }

                yield return instr;
            }
        }
        
        public static void Prefix(Coin __instance)
        {
            _currentCoin = __instance;
        }

        public static void Postfix(Coin __instance)
        {
            _currentCoin = null;
        }
    }

    [HarmonyPatch(typeof(Coin), "Awake")]
    static class CoinAwakePatch
    {
        public static void Prefix(Coin __instance)
        {
            __instance.GetOrAddComponent<ProjectileBoostTracker>();
        }

        public static void Postfix(Coin __instance)
        {
        }
    }

    [HarmonyPatch(typeof(Coin), nameof(Coin.Punchflection))]
    static class CoinPunchflectionPatch
    {
        static void DeliverDamageReplacement(EnemyIdentifier eid, GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, bool tryForExplode, float critMultiplier = 0f, GameObject sourceWeapon = null, bool ignoreTotalDamageTakenMultiplier = false, bool fromExplosion = false)
        {
            Action deliverThatDamage = () =>
            {
                eid.DeliverDamage(target, force, hitPoint, multiplier, tryForExplode, critMultiplier, sourceWeapon, ignoreTotalDamageTakenMultiplier, fromExplosion);  
            };

            if (Cheats.IsCheatDisabled(Cheats.FeedbackerForAll))
            {
                deliverThatDamage();
                return;
            }
            
            var coin = _currentCoin;

            var boostTracker = coin.GetComponent<ProjectileBoostTracker>();

            boostTracker.CoinPunched = true;

            var parryability = boostTracker.NotifyContact();

            var eadd = eid.GetComponent<EnemyAdditions>();

            Assert.IsNotNull(eadd);

            if (eadd.Eid.Dead)
            {
                deliverThatDamage();
                return;
            }

            if (parryability < 0.5f)
            {
                deliverThatDamage();
                return;
            }

            var feedbacker = eadd.Feedbacker;

            if (!feedbacker.Enabled)
            {
                deliverThatDamage();
                return;
            }

            if (!feedbacker.ReadyToParry)
            {
                deliverThatDamage();
                return;
            }

            var parryForce = feedbacker.SolveParryForce(hitPoint, Vector3.zero);
            
            feedbacker.ParryEffect();
            var coinMeshF = coin.GetComponentInChildren<MeshFilter>();
            var coinMeshR = coin.GetComponentInChildren<MeshRenderer>();
            
            var counterBeamGo = GameObject.Instantiate(Assets.EnemyRevolverBullet);
            var counterBeam = counterBeamGo.GetComponent<Projectile>();
            var counterBeamBoostTracker = counterBeamGo.GetOrAddComponent<ProjectileBoostTracker>();
            counterBeam.GetComponentInChildren<MeshFilter>().mesh = coinMeshF.mesh;
            counterBeam.GetComponentInChildren<MeshRenderer>().material = coinMeshR.material;
            counterBeamBoostTracker.CopyFrom(boostTracker);
            counterBeamBoostTracker.IncrementEnemyBoost();
            counterBeamGo.transform.position = hitPoint;
            counterBeamGo.transform.rotation = Quaternion.LookRotation(parryForce);
            counterBeamGo.SetActive(true);
            
            var colliders = eadd.Colliders;
            counterBeamBoostTracker.IgnoreColliders = colliders;
            counterBeamBoostTracker.SafeEid = eid;

            //counterBeam.safeEnemyType = eadd.Eid.enemyType;
            counterBeam.playerBullet = true;
            counterBeam.damage = coin.power * 5.0f;
            counterBeam.enemyDamageMultiplier = 1.0f / 5.0f;
            coin.GetDeleted();
            return;
        }

        private static Coin _currentCoin = null;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instr in instructions)
            {
                if (instr.Calls(typeof(EnemyIdentifier).GetMethod(nameof(EnemyIdentifier.DeliverDamage))))
                {
                    instr.operand = typeof(CoinPunchflectionPatch).GetMethod(nameof(DeliverDamageReplacement), BindingFlags.Static | BindingFlags.NonPublic);
                }

                yield return instr;
            }
        }

        public static void Prefix(Coin __instance)
        {
            _currentCoin = __instance;
        }

        public static void Postfix(Coin __instance)
        {
            _currentCoin = null;
        }
    }
}