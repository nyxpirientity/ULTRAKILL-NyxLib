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
        Radiance.Modifier RadianceModifier = new Radiance.Modifier();
        bool PlayedEnrageSound = false;
        float enrageSoundTimer = -1.0f;
        float EnrageSoundCooldown = -1.0f;

        protected void FixedUpdate()
        {
            if (Cheats.IsCheatEnabled(Cheats.SaltyEnemies))
            {
                if (Eid.enemyType == EnemyType.Puppet && Eid.Dead)
                {
                    return;
                }
                
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
                            if (!Eid.dead)
                            {
                                Eid.GetComponent<Gutterman>()?.Enrage();
                            }
                            break;
                        default:
                            radienceTier = Options.ULTRAKILLNoEnrageRadianceTier;
                            enrageSoundTimer = UnityEngine.Random.value % 0.3f;
                            break;
                    }
                }
                else
                {
                    PlayedEnrageSound = false;
                    enrageSoundTimer = -1.0f;
                }

                if (radienceTier <= 0.001f)
                {
                    UnrequestBuffs();
                    return;
                }
                
                if (!Eid.dead)
                {
                    RequestBuffs(radienceTier);
                }
            }
            else
            {
                UnrequestBuffs();
            }
        }

        private void TryPlayHuskEnrageSound(float pitch = 1.0f, float volume = 1.0f)
        {
            if (PlayedEnrageSound)
            {
                return;
            }

            if (EnrageSoundCooldown >= 0.0f)
            {
                PlayedEnrageSound = true;
                return;
            }

            if (Assets.HuskEnrageSound_0 != null)
            {
                Log.TraceExpectedInfo($"'[SaltyEnemy] playing husk enrage sound!");
                var audioGo = GameObject.Instantiate(Assets.HuskEnrageSound_0, transform);
                audioGo.GetComponent<AudioSource>().volume *= volume;
                audioGo.GetComponent<AudioSource>().pitch = 0.3f * pitch;
                audioGo.GetComponent<AudioDistortionFilter>().distortionLevel = 0.5f;
                audioGo.SetActive(true);
                PlayedEnrageSound = true;

                EnrageSoundCooldown = 10.0f;
            }
            else
            {
                Log.UnlikelyInfo($"'[SaltyEnemy] Tried to play husk enrage sound but we haven't cached it yet");
            }
        }

        private void TryPlayMachineEnrageSound(float pitch = 1.0f, float volume = 1.0f)
        {
            if (PlayedEnrageSound)
            {
                return;
            }

            if (EnrageSoundCooldown >= 0.0f)
            {
                PlayedEnrageSound = true;
                return;
            }
            
            if (Assets.MachineEnrageSound_0 != null)
            {
                Log.TraceExpectedInfo($"'[SaltyEnemy] playing machine enrage sound!");
                var audioGo = GameObject.Instantiate(Assets.MachineEnrageSound_0, transform);
                var audioSource = audioGo.GetComponent<AudioSource>();
                audioSource.volume *= volume;
                switch (Eid.enemyType)
                {
                    case EnemyType.Streetcleaner:
                    audioSource.clip = Eid.machine.scream;
                    break;
                    default:
                    var randVal = (int)(UnityEngine.Random.value * 100.0f) % 2;
                    if (randVal == 0)
                    {
                        audioSource.pitch = 2f * pitch;
                    }
                    else if (randVal == 1)
                    {
                        audioSource.clip = Eid.machine.scream;
                        audioSource.pitch = pitch;
                    }
                    break;
                }
                audioGo.SetActive(true);
                PlayedEnrageSound = true;
                EnrageSoundCooldown = 10.0f;
            }
            else
            {
                Log.UnlikelyInfo($"'[SaltyEnemy] Tried to play machine enrage sound but we haven't cached it yet");
            }
        }


        private void RequestBuffs(float radienceTier)
        {
            RadianceModifier.BaseEnabled = true;
            RadianceModifier.SpeedEnabled = Options.SaltEffectSpeed;
            RadianceModifier.HealthEnabled = Options.SaltEffectHealth;
            RadianceModifier.DamageEnabled = Options.SaltEffectDamage;

            RadianceModifier.BaseMod = 0.0f;
            RadianceModifier.DamageMod = radienceTier;
            RadianceModifier.SpeedMod = radienceTier;
            RadianceModifier.HealthMod = radienceTier;
        }

        private void UnrequestBuffs()
        {
            RadianceModifier.BaseEnabled = false;
            RadianceModifier.SpeedEnabled = false;
            RadianceModifier.HealthEnabled = false;
            RadianceModifier.DamageEnabled = false;
        }

        protected void Start()
        {
            Eadd = GetComponent<EnemyAdditions>();
            Eid = Eadd.Eid;
            Eadd.EnemyRadiance.AddModifier(RadianceModifier);
            RadianceModifier.SpeedEnabled = false;
            RadianceModifier.HealthEnabled = false;
            RadianceModifier.DamageEnabled = false;
            RadianceModifier.Multiplier = false;
        }

        protected void Update()
        {
            EnrageSoundCooldown -= Time.deltaTime;

            if (enrageSoundTimer > 0.0f)
            {
                enrageSoundTimer -= Time.deltaTime;

                if (enrageSoundTimer <= 0.0f)
                {
                    enrageSoundTimer = -1.0f;
                    
                    switch (Eid.enemyClass)
                    {
                        case EnemyClass.Husk:
                            TryPlayHuskEnrageSound((UnityEngine.Random.value % 0.3f) + 0.9f, Eid.bigEnemy ? 0.8f : 0.6f);
                            break;
                        case EnemyClass.Machine:
                            TryPlayMachineEnrageSound(Eid.bigEnemy ? 1.05f : 1.3f, Eid.bigEnemy ? 1.4f : 1.1f);
                            break;
                        case EnemyClass.Demon:
                            TryPlayHuskEnrageSound((UnityEngine.Random.value % 0.3f) + 0.9f, Eid.bigEnemy ? 0.85f : 0.7f);
                            break;
                        case EnemyClass.Divine:
                            break;
                        case EnemyClass.Other:
                            break;
                    }
                }
            }
        }
    }
}