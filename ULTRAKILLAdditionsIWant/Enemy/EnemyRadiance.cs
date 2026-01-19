using System.Collections.Generic;
using System.Reflection;
using MelonLoader;
using UnityEngine;

namespace UKAIW
{
    public class Radiance : MonoBehaviour
    {
        public class Modifier
        {
            public bool BaseEnabled = false;
            public bool SpeedEnabled = false;
            public bool HealthEnabled = false;
            public bool DamageEnabled = false;
            public bool Multiplier = false;
            public float BaseMod = 0.0f;
            public float SpeedMod = 0.0f;
            public float DamageMod = 0.0f;
            public float HealthMod = 0.0f;
        }
    
        EnemyIdentifier Eid = null;
        EnemyAdditions Eadd = null;

        bool RequestedSpeedBuff = false;
        bool RequestedHealthBuff = false;
        bool RequestedDamageBuff = false;

        public bool BuffsBase { get; private set; } = false;
        public bool BuffsSpeed { get; private set; } = false;
        public bool BuffsDamage { get; private set; } = false;
        public bool BuffsHealth { get; private set; } = false;

        public HashSet<Modifier> Modifiers = new HashSet<Modifier>(8);

        public void AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);
        }

        private Radiance.Modifier RadiantAllModifier = new Radiance.Modifier();

        protected void FixedUpdate()
        {
            if (Cheats.IsCheatEnabled(Cheats.RadiantAllEnemies))
            {
                RadiantAllModifier.SpeedEnabled = Options.RadianceAllSpeedTier >= 0.0f;
                RadiantAllModifier.DamageEnabled = Options.RadianceAllDamageTier >= 0.0f;
                RadiantAllModifier.HealthEnabled = Options.RadianceAllHealthTier >= 0.0f;
                RadiantAllModifier.BaseMod = Options.RadianceAllTier;
                RadiantAllModifier.SpeedMod = Options.RadianceAllSpeedTier;
                RadiantAllModifier.HealthMod = Options.RadianceAllHealthTier;
                RadiantAllModifier.DamageMod = Options.RadianceAllDamageTier;
            }
            else
            {
                RadiantAllModifier.SpeedEnabled = false;
                RadiantAllModifier.DamageEnabled = false;
                RadiantAllModifier.HealthEnabled = false;
            }

            float radianceTier = 1.0f;
            float healthValue = 1.0f;
            float speedValue = 1.0f;
            float damageValue = 1.0f;

            BuffsDamage = false;
            BuffsHealth = false;
            BuffsSpeed = false;
            BuffsBase = false;

            foreach (var modifier in Modifiers)
            {
                BuffsDamage = BuffsDamage || modifier.DamageEnabled;
                BuffsHealth = BuffsHealth || modifier.HealthEnabled;
                BuffsSpeed = BuffsSpeed || modifier.SpeedEnabled;
                BuffsBase = BuffsBase || modifier.BaseEnabled;

                if (modifier.BaseEnabled)
                {
                    radianceTier = modifier.Multiplier ? radianceTier * modifier.BaseMod : radianceTier + modifier.BaseMod;
                }

                if (modifier.HealthEnabled)
                {
                    healthValue = modifier.Multiplier ? healthValue * modifier.HealthMod : healthValue + modifier.HealthMod;
                }
                
                if (modifier.SpeedEnabled)
                {
                    speedValue = modifier.Multiplier ? speedValue * modifier.SpeedMod : speedValue + modifier.SpeedMod;
                }

                if (modifier.DamageEnabled)
                {
                    damageValue = modifier.Multiplier ? damageValue * modifier.DamageMod : damageValue + modifier.DamageMod;
                }
            }

            if (BuffsDamage && !RequestedDamageBuff)
            {
                Eid.DamageBuff();
                RequestedDamageBuff = true;
            }
            else if (!BuffsDamage && RequestedDamageBuff)
            {
                Eid.DamageUnbuff();
                RequestedDamageBuff = false;
            }

            if (BuffsHealth && !RequestedHealthBuff)
            {
                Eid.HealthBuff();
                RequestedHealthBuff = true;
            }
            else if (!BuffsHealth && RequestedHealthBuff)
            {
                Eid.HealthUnbuff();
                RequestedHealthBuff = false;
            }

            if (BuffsSpeed && !RequestedSpeedBuff)
            {
                Eid.SpeedBuff();
                RequestedSpeedBuff = true;
            }
            else if (!BuffsSpeed && RequestedSpeedBuff)
            {
                Eid.SpeedUnbuff();
                RequestedSpeedBuff = false;
            }

            if (BuffsBase)
            {
                Eid.radianceTier = radianceTier;
            }

            if (BuffsSpeed)
            {
                Eid.speedBuffModifier = speedValue;
            }

            if (BuffsDamage)
            {
                Eid.damageBuffModifier = damageValue;
            }

            if (BuffsHealth)
            {
                Eid.healthBuffModifier = healthValue;
            }
            
            Eid.UpdateBuffs();
            MethodInfo updateModifiersFI = typeof(EnemyIdentifier).GetMethod("UpdateModifiers", BindingFlags.NonPublic | BindingFlags.Instance);
            updateModifiersFI.Invoke(Eid, null);
        }

        private void Start()
        {
            Eid = GetComponent<EnemyIdentifier>();
            Eadd = GetComponent<EnemyAdditions>();
            
            RadiantAllModifier = new Radiance.Modifier();
            AddModifier(RadiantAllModifier);
        }
    }
}