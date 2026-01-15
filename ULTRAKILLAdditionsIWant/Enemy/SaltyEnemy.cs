using System;
using System.Reflection;
using MelonLoader;
using Sandbox;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

namespace UKAIW
{
    public class SaltyEnemy : MonoBehaviour
    {
        EnemyIdentifier Eid = null;
        EnemyAdditions Eadd = null;
        bool RequestedSpeedBuff = false;
        bool RequestedHealthBuff = false;
        bool RequestedDamageBuff = false;

        protected void FixedUpdate()
        {
            if (Cheats.IsCheatEnabled(Cheats.SaltyEnemies))
            {
                var prefabEid = Eadd.PrefabMod.Prefab.GetComponent<EnemyIdentifier>() ?? Eadd.PrefabMod.Prefab.GetComponentInChildren<EnemyIdentifier>();
                var radienceTier = prefabEid.hasRadianceEffected ? prefabEid.radianceTier : 0.0f;
                int rankIndex = StyleHUD.Instance.rankIndex;
                StyleRanks rank = (StyleRanks)(rankIndex);
                radienceTier += rank switch
                {
                    StyleRanks.Destructive => Options.DestructiveRadianceTier,
                    StyleRanks.Chaotic => Options.ChaoticRadianceTier,
                    StyleRanks.Brutal => Options.BrutalRadianceTier,
                    StyleRanks.Anarchic => Options.AnarchicRadianceTier,
                    StyleRanks.Supreme => Options.SupremeRadianceTier,
                    StyleRanks.SSadistic => Options.SSadisticRadianceTier,
                    StyleRanks.SSSensoredStorm => Options.SSSensoredStormRadianceTier,
                    StyleRanks.ULTRAKILL => Options.ULTRAKILLRadianceTier,
                    _ => throw new Exception("A great sadness has struck the city."),
                };

                if (radienceTier <= 0.01f)
                {
                    UnrequestBuffs();
                    return;
                }
                
                RequestBuffs(radienceTier);
                
                if (rank is StyleRanks.ULTRAKILL)
                {
                    switch (Eid.enemyType)
                    {
                        case EnemyType.Swordsmachine:
                            Eid.GetComponent<SwordsMachine>()?.Enrage();
                            break;
                        case EnemyType.Cerberus:
                            Eid.GetComponent<StatueBoss>()?.Enrage();
                            break;
                        case EnemyType.Virtue:
                        case EnemyType.Drone:
                            Eid.GetComponent<Drone>()?.Enrage();
                            break;
                        case EnemyType.V2:
                            Eid.GetComponent<V2>()?.Enrage();
                            break;
                        case EnemyType.Mindflayer:
                            Eid.GetComponent<Mindflayer>()?.Enrage();
                            break;
                        case EnemyType.HideousMass:
                            if (!(Eid.GetComponent<Mass>()?.GetComponentInChildren<EnemySimplifier>()?.enraged).GetValueOrDefault(true))
                            {
                                Eid.GetComponent<Mass>()?.Enrage();
                            }
                            break;
                        case EnemyType.MaliciousFace:
                            Eid.GetComponent<SpiderBody>()?.Enrage();
                            break;
                        case EnemyType.Gutterman:
                            Eid.GetComponent<Gutterman>()?.Enrage();
                            break;
                    }
                }
            }
            else
            {
                UnrequestBuffs();
            }
        }

        private void RequestBuffs(float radienceTier)
        {
            if (Options.SaltEffectSpeed)
            {
                Eid.speedBuffModifier = radienceTier;
            }

            if (Options.SaltEffectDamage)
            {
                Eid.damageBuffModifier = radienceTier;
            }

            if (Options.SaltEffectHealth)
            {
                Eid.healthBuffModifier = radienceTier;
            }

            if (Options.SaltEffectSpeed && !RequestedSpeedBuff)
            {
                Eid.SpeedBuff(radienceTier);                
                RequestedSpeedBuff = true;
            }

            if (Options.SaltEffectHealth && !RequestedHealthBuff)
            {
                Eid.HealthBuff(radienceTier);                
                RequestedHealthBuff = true;
            }

            if (Options.SaltEffectDamage && !RequestedDamageBuff)
            {
                Eid.DamageBuff(radienceTier);                
                RequestedDamageBuff = true;
            }

            Eid.UpdateBuffs();
            MethodInfo updateModifiersFI = typeof(EnemyIdentifier).GetMethod("UpdateModifiers", BindingFlags.NonPublic | BindingFlags.Instance);
            updateModifiersFI.Invoke(Eid, null);
        }

        private void UnrequestBuffs()
        {
            if (Options.SaltEffectSpeed && RequestedSpeedBuff)
            {
                Eid.SpeedUnbuff();                
                RequestedSpeedBuff = false;
            }

            if (Options.SaltEffectHealth && RequestedHealthBuff)
            {
                Eid.HealthUnbuff();                
                RequestedHealthBuff = false;
            }

            if (Options.SaltEffectDamage && RequestedDamageBuff)
            {
                Eid.DamageUnbuff();          
      
                RequestedDamageBuff = false;
            }
        }

        protected void Start()
        {
            Eadd = GetComponent<EnemyAdditions>();
            Eid = Eadd.Eid;
        }
    }
}