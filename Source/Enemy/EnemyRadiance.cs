using System.Collections.Generic;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class EnemyRadiance : EnemyModifier
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
        EnemyComponents Enemy = null;

        [SerializeField] private bool _requestedSpeedBuff = false;
        [SerializeField] private bool _requestedHealthBuff = false;
        [SerializeField] private bool _requestedDamageBuff = false;

        public bool BuffsBase { get; private set; } = false;
        public bool BuffsSpeed { get; private set; } = false;
        public bool BuffsDamage { get; private set; } = false;
        public bool BuffsHealth { get; private set; } = false;

        [SerializeField] private float _addedBase = 0.0f;
        public float AddedBase{ get => _addedBase; private set => _addedBase = value; }

        [SerializeField] private float _addedSpeed = 0.0f;
        public float AddedSpeed { get => _addedSpeed; private set => _addedSpeed = value; }

        [SerializeField] private float _addedDamage = 0.0f;
        public float AddedDamage { get => _addedDamage; private set => _addedDamage = value; }

        [SerializeField] private float _addedHealth = 0.0f;
        public float AddedHealth { get => _addedHealth; private set => _addedHealth = value; }

        public HashSet<Modifier> Modifiers = new HashSet<Modifier>(8);

        public void AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);
        }

        private EnemyRadiance.Modifier RadiantAllModifier = new EnemyRadiance.Modifier();

        protected void FixedUpdate()
        {
            if (Eid.Dead)
            {
                return;
            }
            
            if (!Cheats.Enabled)
            {
                return;
            }

            if (Cheats.IsCheatEnabled(Cheats.RadiantAllEnemies))
            {
                RadiantAllModifier.SpeedEnabled = Options.RadianceAllSpeedTier >= 0.0f;
                RadiantAllModifier.DamageEnabled = Options.RadianceAllDamageTier >= 0.0f;
                RadiantAllModifier.HealthEnabled = Options.RadianceAllHealthTier >= 0.0f;
                RadiantAllModifier.BaseEnabled = Options.RadianceAllTier >= 0.0f;
                RadiantAllModifier.BaseMod = Options.RadianceAllTier - 1.0f;
                RadiantAllModifier.SpeedMod = Options.RadianceAllSpeedTier - 1.0f;
                RadiantAllModifier.HealthMod = Options.RadianceAllHealthTier - 1.0f;
                RadiantAllModifier.DamageMod = Options.RadianceAllDamageTier - 1.0f;
                // we start with 1.0f so subtract that for simplicity (maybe simpler?)
            }
            else
            {
                RadiantAllModifier.BaseEnabled = false;
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

            // todo: add first, then multiply

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

            if ((BuffsHealth || BuffsDamage || BuffsSpeed) && Eid.radianceTier == 0.0f)
            {
                Eid.radianceTier = 1.0f;
            }
            
            if (BuffsBase)
            {
                if (AddedBase > 0)
                {
                    AddBase(-AddedBase);
                }
                
                AddBase(radianceTier - 1.0f);
            }
            else if (AddedBase > 0)
            {
                AddBase(-AddedSpeed);
            }

            if (BuffsSpeed)
            {
                if (AddedSpeed > 0)
                {
                    AddSpeed(-AddedSpeed);
                }
                
                RequestSpeedBuff();
                AddSpeed(speedValue - 1.0f);
            }
            else
            {
                AddSpeed(-AddedSpeed);
                UnrequestSpeedBuff();
            }

            if (BuffsDamage)
            {
                if (AddedDamage > 0)
                {
                    AddDamage(-AddedDamage);
                }

                RequestDamageBuff();
                AddDamage(damageValue - 1.0f);
            }
            else
            {
                AddDamage(-AddedDamage);
                UnrequestDamageBuff();
            }

            if (BuffsHealth)
            {
                if (AddedHealth > 0)
                {
                    AddHealth(-AddedHealth);
                }

                RequestHealthBuff();
                AddHealth(healthValue - 1.0f);
            }
            else
            {
                AddHealth(-AddedHealth);
                UnrequestHealthBuff();
            }

            //Log.Message($"{this}: radianceTier: {Eid.radianceTier}\nspeedBuff: {Eid.speedBuffModifier}\nhealthBuff: {Eid.healthBuffModifier}\ndamageBuff: {Eid.damageBuffModifier}\n");
        }

        private void RequestHealthBuff()
        {
            if (_requestedHealthBuff)
            {
                return;
            }

            Eid.HealthBuff();
            _requestedHealthBuff = true;
        }

        private void UnrequestHealthBuff()
        {
            if (!_requestedHealthBuff)
            {
                return;
            }

            Eid.HealthUnbuff();
            _requestedHealthBuff = false;
        }
        
        private void RequestSpeedBuff()
        {
            if (_requestedSpeedBuff)
            {
                return;
            }

            Eid.SpeedBuff();
            _requestedSpeedBuff = true;
        }

        private void UnrequestSpeedBuff()
        {
            if (!_requestedSpeedBuff)
            {
                return;
            }

            Eid.SpeedUnbuff();
            _requestedSpeedBuff = false;
        }

        private void RequestDamageBuff()
        {
            if (_requestedDamageBuff)
            {
                return;
            }

            Eid.DamageBuff();
            _requestedDamageBuff = true;
        }

        private void UnrequestDamageBuff()
        {
            if (!_requestedDamageBuff)
            {
                return;
            }
            
            Eid.DamageUnbuff();
            _requestedDamageBuff = false;
        }

        private void AddSpeed(float amount)
        {
            Eid.speedBuffModifier += amount;
            AddedSpeed += amount;
        }

        private void AddDamage(float amount)
        {
            Eid.damageBuffModifier += amount;
            AddedDamage += amount;
        }

        private void AddHealth(float amount)
        {
            Eid.healthBuffModifier += amount;
            AddedHealth += amount;
        }

        private void AddBase(float amount)
        {
            Eid.radianceTier += amount;
            AddedBase += amount;
        }

        private void Awake()
        {
            Eid = GetComponent<EnemyIdentifier>();
            Enemy = GetComponent<EnemyComponents>();
            
            if (!Eid.speedBuff)
            {
                Eid.speedBuffModifier = 1.0f;
            }

            if (!Eid.damageBuff)
            {
                Eid.damageBuffModifier = 1.0f;
            }

            if (!Eid.healthBuff)
            {
                Eid.healthBuffModifier = 1.0f;
            }
        }

        private void Start()
        {
            RadiantAllModifier = new EnemyRadiance.Modifier();
            AddModifier(RadiantAllModifier);
        }
    }
}