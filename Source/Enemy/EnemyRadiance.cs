using System;
using System.Collections.Generic;
using HarmonyLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class EnemyRadiance : EnemyModifier
    {
        public EnemyRadiance()
        {
            RadiantAllExtBaseBuffDisallowRegTracker = new RegistrationTracker(() => { DisallowExternalBaseBuffs.Request(); return true; }, () => { DisallowExternalBaseBuffs.Unrequest(); return true; });
            RadiantAllExtSpeedBuffDisallowRegTracker = new RegistrationTracker(() => { DisallowExternalSpeedBuffs.Request(); return true; }, () => { DisallowExternalSpeedBuffs.Unrequest(); return true; });
            RadiantAllExtHealthBuffDisallowRegTracker = new RegistrationTracker(() => { DisallowExternalHealthBuffs.Request(); return true; }, () => { DisallowExternalHealthBuffs.Unrequest(); return true; });
            RadiantAllExtDamageBuffDisallowRegTracker = new RegistrationTracker(() => { DisallowExternalDamageBuffs.Request(); return true; }, () => { DisallowExternalDamageBuffs.Unrequest(); return true; });
        }

        public class Modifier
        {
            public enum CompositionTypes { Additive, Multiply, MultiplyAdded }

            public bool BaseEnabled = false;
            public bool SpeedEnabled = false;
            public bool HealthEnabled = false;
            public bool DamageEnabled = false;

            public CompositionTypes CompositionType = CompositionTypes.Additive;

            public float BaseMod = 0.0f;
            public float SpeedMod = 0.0f;
            public float DamageMod = 0.0f;
            public float HealthMod = 0.0f;
        }

        public RequestTracker DisallowExternalBaseBuffs = new RequestTracker();
        public RequestTracker DisallowExternalHealthBuffs = new RequestTracker();
        public RequestTracker DisallowExternalDamageBuffs = new RequestTracker();
        public RequestTracker DisallowExternalSpeedBuffs = new RequestTracker();

        EnemyIdentifier Eid = null;
        EnemyComponents Enemy = null;

        private static bool _expectingBuffCalls = false;

        [SerializeField] private bool _hasBuffedBefore = false;
        [SerializeField] private bool _hasBuffedSpeedBefore = false;
        [SerializeField] private bool _hasBuffedHealthBefore = false;
        [SerializeField] private bool _hasBuffedDamageBefore = false;

        [SerializeField] private bool _requestedSpeedBuff = false;
        [SerializeField] private bool _requestedHealthBuff = false;
        [SerializeField] private bool _requestedDamageBuff = false;

        [SerializeField] private bool _buffsBase = false;
        public bool BuffsBase => _buffsBase;

        [SerializeField] private bool _buffsSpeed = false;
        public bool BuffsSpeed => _buffsSpeed;

        [SerializeField] private bool _buffsDamage = false;
        public bool BuffsDamage => _buffsDamage;

        [SerializeField] private bool _buffsHealth = false;
        public bool BuffsHealth => _buffsHealth;

        [SerializeField] private float _addedBase = 0.0f;
        public float AddedBase { get => _addedBase; private set => _addedBase = value; }

        [SerializeField] private float _addedSpeed = 0.0f;
        public float AddedSpeed { get => _addedSpeed; private set => _addedSpeed = value; }

        [SerializeField] private float _addedDamage = 0.0f;
        public float AddedDamage { get => _addedDamage; private set => _addedDamage = value; }

        [SerializeField] private float _addedHealth = 0.0f;
        public float AddedHealth { get => _addedHealth; private set => _addedHealth = value; }

        public bool IsActive => !Eid.Dead && Cheats.Enabled && !_excluded && _started && (Enemy?.EidStarted).GetValueOrDefault(false);

        [SerializeField] private bool _wasPreviouslyBuffed = false;
        [SerializeField] private float _prevBaseBuff = 1.0f;
        [SerializeField] private float _prevHealthBuff = 1.0f;
        [SerializeField] private float _prevDamageBuff = 1.0f;
        [SerializeField] private float _prevSpeedBuff = 1.0f;

        [SerializeField] private float _externalBaseBuff = -1.0f;
        [SerializeField] private float _externalHealthBuff = -1.0f;
        [SerializeField] private float _externalDamageBuff = -1.0f;
        [SerializeField] private float _externalSpeedBuff = -1.0f;

        [SerializeField] private float _initialBaseBuff = -1.0f;
        [SerializeField] private float _initialHealthBuff = -1.0f;
        [SerializeField] private float _initialDamageBuff = -1.0f;
        [SerializeField] private float _initialSpeedBuff = -1.0f;

        public float ExternalBaseBuff => _externalBaseBuff != -1.0f ? _externalBaseBuff : _initialBaseBuff;
        public float ExternalHealthBuff => _externalHealthBuff != -1.0f ? _externalHealthBuff : _initialHealthBuff;
        public float ExternalDamageBuff => _externalDamageBuff != -1.0f ? _externalDamageBuff : _initialDamageBuff;
        public float ExternalSpeedBuff => _externalSpeedBuff != -1.0f ? _externalSpeedBuff : _initialSpeedBuff;

        public HashSet<Modifier> Modifiers = new HashSet<Modifier>(8);

        public bool BuffsAny => BuffsHealth || BuffsDamage || BuffsSpeed || BuffsBase;

        public int ExpectedSpeedBuffRequests => _requestedSpeedBuff ? 1 : 0;
        public int ExpectedHealthBuffRequests => _requestedHealthBuff ? 1 : 0;
        public int ExpectedDamageBuffRequests => _requestedDamageBuff ? 1 : 0;

        public bool IsExternallySpeedBuffed => (Eid.speedBuffRequests - ExpectedSpeedBuffRequests) > 0;
        public bool IsExternallyDamageBuffed => (Eid.damageBuffRequests - ExpectedDamageBuffRequests) > 0;
        public bool IsExternallyHealthBuffed => (Eid.healthBuffRequests - ExpectedHealthBuffRequests) > 0;

        public bool IsExternallyBuffed => IsExternallySpeedBuffed || IsExternallyDamageBuffed || IsExternallyHealthBuffed;

        public void AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);
        }

        private EnemyRadiance.Modifier RadiantAllMultiplierModifier = new EnemyRadiance.Modifier();
        private EnemyRadiance.Modifier RadiantAllAdditiveModifier = new EnemyRadiance.Modifier();
        private RegistrationTracker RadiantAllExtBaseBuffDisallowRegTracker = null;
        private RegistrationTracker RadiantAllExtSpeedBuffDisallowRegTracker = null;
        private RegistrationTracker RadiantAllExtHealthBuffDisallowRegTracker = null;
        private RegistrationTracker RadiantAllExtDamageBuffDisallowRegTracker = null;

        private EnemyRadiance.Modifier ExternalBuffModifier = new EnemyRadiance.Modifier();

        private bool _excluded = false;

        protected void FixedUpdate()
        {
            if (!Cheats.Enabled)
            {
                return;
            }

            if (!IsActive)
            {
                return;
            }

            if (Cheats.IsCheatEnabled(Cheats.RadiantAllEnemies))
            {
                RadiantAllMultiplierModifier.SpeedEnabled = Options.RadianceSpeed.Value;
                RadiantAllMultiplierModifier.DamageEnabled = Options.RadianceDamage.Value;
                RadiantAllMultiplierModifier.HealthEnabled = Options.RadianceHealth.Value;
                RadiantAllMultiplierModifier.BaseEnabled = true;
                RadiantAllMultiplierModifier.BaseMod = Options.RadianceTier.Value;
                RadiantAllMultiplierModifier.SpeedMod = Options.RadianceSpeedScalar.Value;
                RadiantAllMultiplierModifier.HealthMod = Options.RadianceHealthScalar.Value;
                RadiantAllMultiplierModifier.DamageMod = Options.RadianceDamageScalar.Value;
                RadiantAllMultiplierModifier.CompositionType = Modifier.CompositionTypes.MultiplyAdded;

                RadiantAllAdditiveModifier.SpeedEnabled = Options.RadianceSpeed.Value;
                RadiantAllAdditiveModifier.DamageEnabled = Options.RadianceDamage.Value;
                RadiantAllAdditiveModifier.HealthEnabled = Options.RadianceHealth.Value;
                RadiantAllAdditiveModifier.BaseEnabled = true;
                RadiantAllAdditiveModifier.BaseMod = 0.0f;
                RadiantAllAdditiveModifier.SpeedMod = ExternalSpeedBuff - 1.0f;
                RadiantAllAdditiveModifier.HealthMod = ExternalHealthBuff - 1.0f;
                RadiantAllAdditiveModifier.DamageMod = ExternalDamageBuff - 1.0f;
                RadiantAllAdditiveModifier.CompositionType = Modifier.CompositionTypes.Additive;

                if (Options.RadiantAllDisableExternalBaseRadiance.Value)
                {
                    RadiantAllExtBaseBuffDisallowRegTracker.Register();
                }
                else
                {
                    RadiantAllExtBaseBuffDisallowRegTracker.Unregister();
                }

                if (Options.RadiantAllDisableExternalSpeedRadiance.Value)
                {
                    RadiantAllExtSpeedBuffDisallowRegTracker.Register();
                }
                else
                {
                    RadiantAllExtSpeedBuffDisallowRegTracker.Unregister();
                }

                if (Options.RadiantAllDisableExternalDamageRadiance.Value)
                {
                    RadiantAllExtDamageBuffDisallowRegTracker.Register();
                }
                else
                {
                    RadiantAllExtDamageBuffDisallowRegTracker.Unregister();
                }

                if (Options.RadiantAllDisableExternalHealthRadiance.Value)
                {
                    RadiantAllExtHealthBuffDisallowRegTracker.Register();
                }
                else
                {
                    RadiantAllExtHealthBuffDisallowRegTracker.Unregister();
                }

                // we start with 1.0f so subtract that for simplicity (maybe simpler?)
            }
            else
            {
                RadiantAllMultiplierModifier.BaseEnabled = false;
                RadiantAllMultiplierModifier.SpeedEnabled = false;
                RadiantAllMultiplierModifier.DamageEnabled = false;
                RadiantAllMultiplierModifier.HealthEnabled = false;

                RadiantAllAdditiveModifier.BaseEnabled = false;
                RadiantAllAdditiveModifier.SpeedEnabled = false;
                RadiantAllAdditiveModifier.DamageEnabled = false;
                RadiantAllAdditiveModifier.HealthEnabled = false;

                RadiantAllExtBaseBuffDisallowRegTracker.Unregister();
                RadiantAllExtSpeedBuffDisallowRegTracker.Unregister();
                RadiantAllExtDamageBuffDisallowRegTracker.Unregister();
                RadiantAllExtHealthBuffDisallowRegTracker.Unregister();
            }

            float radianceTier = 1.0f;
            float healthValue = 1.0f;
            float speedValue = 1.0f;
            float damageValue = 1.0f;

            _buffsDamage = false;
            _buffsHealth = false;
            _buffsSpeed = false;
            _buffsBase = false;

            DisableExternalBuffMod();

            for (int i = 0; i < 4; i++)
            {
                bool solvingBuffs = i == 0;
                Modifier.CompositionTypes? currentComposition = null;
                switch (i)
                {
                    case 1:
                        currentComposition = Modifier.CompositionTypes.Additive;
                        break;
                    case 2:
                        currentComposition = Modifier.CompositionTypes.Multiply;
                        break;
                    case 3:
                        currentComposition = Modifier.CompositionTypes.MultiplyAdded;
                        break;
                }

                foreach (var modifier in Modifiers)
                {
                    if (solvingBuffs || currentComposition == Modifier.CompositionTypes.Additive)
                    {
                        _buffsDamage = BuffsDamage || modifier.DamageEnabled;
                        _buffsHealth = BuffsHealth || modifier.HealthEnabled;
                        _buffsSpeed = BuffsSpeed || modifier.SpeedEnabled;
                        _buffsBase = BuffsBase || modifier.BaseEnabled;
                    }

                    if (modifier.CompositionType != currentComposition || solvingBuffs)
                    {
                        continue;
                    }

                    Func<float, float, float> processRadVal = (radVal, mod) =>
                    {
                        switch (modifier.CompositionType)
                        {
                            case Modifier.CompositionTypes.Additive:
                                return radVal + mod;
                            case Modifier.CompositionTypes.Multiply:
                                return radVal * mod;
                            case Modifier.CompositionTypes.MultiplyAdded:
                                return 1.0f + (Mathf.Max(radVal - 1.0f, 0.0f) * mod);
                        }

                        throw new NotImplementedException();
                    };

                    if (modifier.BaseEnabled)
                    {
                        radianceTier = processRadVal(radianceTier, modifier.BaseMod);
                    }

                    if (modifier.HealthEnabled)
                    {
                        healthValue = processRadVal(healthValue, modifier.HealthMod);
                    }

                    if (modifier.SpeedEnabled)
                    {
                        speedValue = processRadVal(speedValue, modifier.SpeedMod);
                    }

                    if (modifier.DamageEnabled)
                    {
                        damageValue = processRadVal(damageValue, modifier.DamageMod);
                    }
                }

                if (!BuffsAny && !_hasBuffedBefore)
                {
                    return;
                }

                if (solvingBuffs)
                {
                    HandleExternalBuffs();
                }
            }

            if (BuffsBase)
            {
                AddBase(-AddedBase);

                AddBase(radianceTier - 1.0f);
            }
            else if (AddedBase != 0)
            {
                AddBase(-AddedBase);
            }

            if (BuffsSpeed)
            {
                AddSpeed(-AddedSpeed);

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
                AddDamage(-AddedDamage);

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
                AddHealth(-AddedHealth);

                RequestHealthBuff();
                AddHealth(healthValue - 1.0f);
            }
            else
            {
                AddHealth(-AddedHealth);
                UnrequestHealthBuff();
            }

            bool isBuffed = Eid.speedBuff || Eid.damageBuff || Eid.healthBuff;

            if ((isBuffed != _wasPreviouslyBuffed) || (Eid.radianceTier != _prevBaseBuff) || (Eid.speedBuffModifier != _prevSpeedBuff) || (Eid.damageBuffModifier != _prevDamageBuff) || (Eid.damageBuffModifier != _prevDamageBuff))
            {
                Eid.UpdateBuffs();
                if (Options.LogEnemyRadianceUpdates.Value && (!Options.LogEnemyRadianceUpdatesOnlyIfExternallyBuffed.Value || IsExternallyBuffed))
                {
                    Log.Message(
                        $"{this} updated radiance! radiance info: \nradianceTier: {Eid.radianceTier}" +
                        $"\nspeedBuff: {Eid.speedBuffModifier}\nhealthBuff: {Eid.healthBuffModifier}" +
                        $"\ndamageBuff: {Eid.damageBuffModifier}\nAddedBase: {AddedBase}" +
                        $"\nAddedSpeed: {AddedSpeed}\nspeedValue: {speedValue}\nAddedDamage: {AddedDamage}" +
                        $"\ndamageValue: {damageValue}\nAddedHealth: {AddedHealth}\nhealthValue: {healthValue}" +
                        $"\nIsExternallyBuffed: {IsExternallyBuffed}\n" +
                        $"IsExternallySpeedBuffed: {IsExternallySpeedBuffed}\n" +
                        $"_externalSpeedBuff: {_externalSpeedBuff}\n" +
                        $"IsExternallyDamageBuffed: {IsExternallyDamageBuffed}\n" +
                        $"_externalDamageBuff: {_externalDamageBuff}\n" +
                        $"IsExternallyHealthBuffed: {IsExternallyHealthBuffed}\n" +
                        $"_externalHealthBuff: {_externalHealthBuff}\n" +
                        $"ExpectedSpeedBuffRequests: {ExpectedSpeedBuffRequests}\n" +
                        $"ExpectedDamageBuffRequests: {ExpectedDamageBuffRequests}\n" +
                        $"ExpectedHealthBuffRequests: {ExpectedHealthBuffRequests}\n" +
                        $"_initialBaseBuff: {_initialBaseBuff}\n" +
                        $"_initialDamageBuff: {_initialDamageBuff}\n" +
                        $"_initialHealthBuff: {_initialHealthBuff}\n" +
                        $"_initialSpeedBuff: {_initialSpeedBuff}\n" +
                        $"ExternalBaseBuff (diff, public): {ExternalBaseBuff}\n" +
                        $"ExternalDamageBuff (diff, public): {ExternalDamageBuff}\n" +
                        $"ExternalHealthBuff (diff, public): {ExternalHealthBuff}\n" +
                        $"ExternalSpeedBuff (diff, public): {ExternalSpeedBuff}\n" +
                        $"HealthBuffRequests: {Eid.healthBuffRequests}\n" +
                        $"DamageBuffRequests: {Eid.damageBuffRequests}\n" +
                        $"SpeedBuffRequests: {Eid.speedBuffRequests}\n" +
                        $"ExternalBuffModifier.BaseEnabled: {ExternalBuffModifier.BaseEnabled}\n" +
                        $"ExternalBuffModifier.BaseMod: {ExternalBuffModifier.BaseMod}\n" +
                        $"ExternalBuffModifier.SpeedEnabled: {ExternalBuffModifier.SpeedEnabled}\n" +
                        $"ExternalBuffModifier.SpeedMod: {ExternalBuffModifier.SpeedMod}\n" +
                        $"ExternalBuffModifier.DamageEnabled: {ExternalBuffModifier.DamageEnabled}\n" +
                        $"ExternalBuffModifier.DamageMod: {ExternalBuffModifier.DamageMod}\n" +
                        $"ExternalBuffModifier.HealthEnabled: {ExternalBuffModifier.HealthEnabled}\n" +
                        $"ExternalBuffModifier.HealthMod: {ExternalBuffModifier.HealthMod}\n" +

                        $"RadiantAllAdditiveModifier.BaseEnabled: {RadiantAllAdditiveModifier.BaseEnabled}\n" +
                        $"RadiantAllAdditiveModifier.BaseMod: {RadiantAllAdditiveModifier.BaseMod}\n" +
                        $"RadiantAllAdditiveModifier.SpeedEnabled: {RadiantAllAdditiveModifier.SpeedEnabled}\n" +
                        $"RadiantAllAdditiveModifier.SpeedMod: {RadiantAllAdditiveModifier.SpeedMod}\n" +
                        $"RadiantAllAdditiveModifier.DamageEnabled: {RadiantAllAdditiveModifier.DamageEnabled}\n" +
                        $"RadiantAllAdditiveModifier.DamageMod: {RadiantAllAdditiveModifier.DamageMod}\n" +
                        $"RadiantAllAdditiveModifier.HealthEnabled: {RadiantAllAdditiveModifier.HealthEnabled}\n" +
                        $"RadiantAllAdditiveModifier.HealthMod: {RadiantAllAdditiveModifier.HealthMod}\n" +

                        $"RadiantAllMultiplierModifier.BaseEnabled: {RadiantAllMultiplierModifier.BaseEnabled}\n" +
                        $"RadiantAllMultiplierModifier.BaseMod: {RadiantAllMultiplierModifier.BaseMod}\n" +
                        $"RadiantAllMultiplierModifier.SpeedEnabled: {RadiantAllMultiplierModifier.SpeedEnabled}\n" +
                        $"RadiantAllMultiplierModifier.SpeedMod: {RadiantAllMultiplierModifier.SpeedMod}\n" +
                        $"RadiantAllMultiplierModifier.DamageEnabled: {RadiantAllMultiplierModifier.DamageEnabled}\n" +
                        $"RadiantAllMultiplierModifier.DamageMod: {RadiantAllMultiplierModifier.DamageMod}\n" +
                        $"RadiantAllMultiplierModifier.HealthEnabled: {RadiantAllMultiplierModifier.HealthEnabled}\n" +
                        $"RadiantAllMultiplierModifier.HealthMod: {RadiantAllMultiplierModifier.HealthMod}\n"
                    );
                }
            }

            _wasPreviouslyBuffed = isBuffed;
            _prevBaseBuff = Eid.radianceTier;
            _prevDamageBuff = Eid.damageBuffModifier;
            _prevHealthBuff = Eid.healthBuffModifier;
            _prevSpeedBuff = Eid.speedBuffModifier;
        }

        private void DisableExternalBuffMod()
        {
            ExternalBuffModifier.BaseEnabled = false;
            ExternalBuffModifier.DamageEnabled = false;
            ExternalBuffModifier.HealthEnabled = false;
            ExternalBuffModifier.DamageEnabled = false;
        }

        private void HandleExternalBuffs()
        {
            if (!Cheats.Enabled)
            {
                return;
            }

            if ((BuffsAny) && (Eid.radianceTier == 0.0f || !_hasBuffedBefore))
            {
                _hasBuffedBefore = true;
                _externalBaseBuff = Eid.radianceTier;

                if (_externalBaseBuff == 0.0f && !_hasBuffedBefore)
                {
                    _externalBaseBuff = 1.0f;
                }

                Eid.radianceTier = 1.0f;

                if (!_hasBuffedHealthBefore)
                {
                    PossiblyWarnOfCurrentMethod();
                    _externalHealthBuff = Eid.healthBuffModifier;
                    Eid.healthBuffModifier = 1.0f;
                }

                if (!_hasBuffedDamageBefore)
                {
                    PossiblyWarnOfCurrentMethod();
                    _externalDamageBuff = Eid.damageBuffModifier;
                    Eid.damageBuffModifier = 1.0f;
                }

                if (!_hasBuffedSpeedBefore)
                {
                    PossiblyWarnOfCurrentMethod();
                    _externalSpeedBuff = Eid.speedBuffModifier;
                    Eid.speedBuffModifier = 1.0f;
                }
            }

            if (BuffsHealth)
            {

            }

            if (BuffsDamage)
            {

            }

            if (BuffsSpeed)
            {

            }

            ExternalBuffModifier.BaseMod = _externalBaseBuff - 1.0f;

            if (IsExternallyBuffed)
            {
                ExternalBuffModifier.BaseEnabled = true && DisallowExternalBaseBuffs.Unrequested;
            }

            if (IsExternallyHealthBuffed)
            {
                ExternalBuffModifier.HealthEnabled = true && DisallowExternalHealthBuffs.Unrequested;
                ExternalBuffModifier.HealthMod = _externalHealthBuff - 1.0f;
            }

            if (IsExternallyDamageBuffed)
            {
                ExternalBuffModifier.DamageEnabled = true && DisallowExternalDamageBuffs.Unrequested;
                ExternalBuffModifier.DamageMod = _externalDamageBuff - 1.0f;
            }

            if (IsExternallySpeedBuffed)
            {
                ExternalBuffModifier.SpeedEnabled = true && DisallowExternalSpeedBuffs.Unrequested;
                ExternalBuffModifier.SpeedMod = _externalSpeedBuff - 1.0f;
            }
        }

        private void RequestHealthBuff()
        {
            if (_requestedHealthBuff)
            {
                return;
            }

            PossiblyWarnOfCurrentMethod();

            _hasBuffedHealthBefore = true;

            _expectingBuffCalls = true;
            LogBuffRequest("health", Eid.healthBuffRequests);
            Eid.HealthBuff();
            _expectingBuffCalls = false;

            _requestedHealthBuff = true;
        }

        private void UnrequestHealthBuff()
        {
            if (!_requestedHealthBuff)
            {
                return;
            }

            PossiblyWarnOfCurrentMethod();

            LogBuffUnrequest("health");
            Eid.HealthUnbuff();
            _requestedHealthBuff = false;
        }

        private void RequestSpeedBuff()
        {
            if (_requestedSpeedBuff)
            {
                return;
            }

            PossiblyWarnOfCurrentMethod();

            _hasBuffedSpeedBefore = true;

            _expectingBuffCalls = true;
            LogBuffRequest("speed", Eid.speedBuffRequests);
            Eid.SpeedBuff();
            _expectingBuffCalls = false;

            _requestedSpeedBuff = true;
        }

        private void UnrequestSpeedBuff()
        {
            if (!_requestedSpeedBuff)
            {
                return;
            }

            PossiblyWarnOfCurrentMethod();

            LogBuffUnrequest("speed");
            Eid.SpeedUnbuff();
            _requestedSpeedBuff = false;
        }

        private void RequestDamageBuff()
        {
            if (_requestedDamageBuff)
            {
                return;
            }

            PossiblyWarnOfCurrentMethod();

            _hasBuffedDamageBefore = true;

            _expectingBuffCalls = true;
            LogBuffRequest("damage", Eid.damageBuffRequests);
            Eid.DamageBuff();
            _expectingBuffCalls = false;

            _requestedDamageBuff = true;
        }

        private void UnrequestDamageBuff()
        {
            if (!_requestedDamageBuff)
            {
                return;
            }

            PossiblyWarnOfCurrentMethod();

            Eid.DamageUnbuff();
            LogBuffUnrequest("damage");
            _requestedDamageBuff = false;
        }

        private void AddSpeed(float amount)
        {
            PossiblyWarnOfCurrentMethod();
            Eid.speedBuffModifier += amount;
            AddedSpeed += amount;
        }

        private void AddDamage(float amount)
        {
            PossiblyWarnOfCurrentMethod();
            Eid.damageBuffModifier += amount;
            AddedDamage += amount;
        }

        private void AddHealth(float amount)
        {
            PossiblyWarnOfCurrentMethod();
            Eid.healthBuffModifier += amount;
            AddedHealth += amount;
        }

        private void AddBase(float amount)
        {
            PossiblyWarnOfCurrentMethod();
            Eid.radianceTier += amount;
            AddedBase += amount;
        }

        private void LogBuffRequest(string type, int buffRequests)
        {
            if (Options.LogEnemyRadianceBuffRequests.Value)
            {
                Log.Message($"{name} is about to request a {type} buff! current have {buffRequests} requests!");
            }
        }

        private void LogBuffUnrequest(string type)
        {
            if (Options.LogEnemyRadianceBuffRequests.Value)
            {
                Log.Message($"{name} UNREQUESTED a {type} buff!");
            }
        }

        private void Awake()
        {
            Eid = GetComponent<EnemyIdentifier>();
            Enemy = GetComponent<EnemyComponents>();

            if (!Cheats.Enabled)
            {
                return;
            }

        }

        private void Start()
        {
            RadiantAllMultiplierModifier = new EnemyRadiance.Modifier();
            RadiantAllAdditiveModifier = new EnemyRadiance.Modifier();
            ExternalBuffModifier = new EnemyRadiance.Modifier();
            AddModifier(RadiantAllAdditiveModifier);
            AddModifier(RadiantAllMultiplierModifier);
            AddModifier(ExternalBuffModifier);

            _initialBaseBuff = _initialBaseBuff < 0.0f ? (Eid.radianceTier == 0.0f ? 1.0f : Eid.radianceTier) : _initialBaseBuff;
            _initialHealthBuff = _initialHealthBuff < 0.0f ? Eid.healthBuffModifier : _initialHealthBuff;
            _initialSpeedBuff = _initialSpeedBuff < 0.0f ? Eid.speedBuffModifier : _initialSpeedBuff;
            _initialDamageBuff = _initialDamageBuff < 0.0f ? Eid.damageBuffModifier : _initialDamageBuff;

            _started = true;

            if (!Cheats.Enabled)
            {
                return;
            }

            // HACK(?): works around a weird quirk of how EnemyIdentifier was written, where on start if healthBuff, speedBuff, etc. are true it'll just blindly ++ the requests
            if (_requestedHealthBuff && Eid.healthBuff)
            {
                Eid.healthBuffRequests -= 1;
            }

            if (_requestedSpeedBuff && Eid.speedBuff)
            {
                Eid.speedBuffRequests -= 1;
            }

            if (_requestedDamageBuff && Eid.damageBuff)
            {
                Eid.damageBuffRequests -= 1;
            }

            PossiblyWarnOfCurrentMethod();
        }

        private bool _started = false;

        private static void PossiblyWarnOfCurrentMethod()
        {
            if (!Options.WarnOfEnemyRadianceUpdates.Value)
            {
                return;
            }

            Log.Warning($"Enemy Radiance Method Seemingly Called!\n    Stack:\n{StackDebug.GetStackString()}");
        }

        [HarmonyPatch(typeof(EnemyIdentifier), "BuffAll", new Type[] { })]
        private static class EidAllBuffPatch
        {
            public static void Prefix(EnemyIdentifier __instance)
            {
                if (!Cheats.Enabled)
                {
                    return;
                }


            }

            public static void Postfix(EnemyIdentifier __instance)
            {
                if (!Cheats.Enabled)
                {
                    return;
                }

                if (_expectingBuffCalls)
                {
                    return;
                }
            }
        }

        [HarmonyPatch(typeof(EnemyIdentifier), "HealthBuff", new Type[] { typeof(float) })]
        private static class EidHealthBuffPatch
        {
            public static void Prefix(EnemyIdentifier __instance, float modifier)
            {
                if (!Cheats.Enabled)
                {
                    return;
                }
            }

            public static void Postfix(EnemyIdentifier __instance, float modifier)
            {
                if (!Cheats.Enabled)
                {
                    return;
                }

                if (_expectingBuffCalls)
                {
                    return;
                }

                var rad = __instance.GetComponent<EnemyRadiance>();

                if (rad == null)
                {
                    return;
                }

                if (!rad._hasBuffedHealthBefore)
                {
                    return;
                }

                PossiblyWarnOfCurrentMethod();

                rad._externalHealthBuff = modifier;
                __instance.healthBuffModifier = rad._prevHealthBuff;
            }
        }

        [HarmonyPatch(typeof(EnemyIdentifier), "SpeedBuff", new Type[] { typeof(float) })]
        private static class EidSpeedBuffPatch
        {
            public static void Prefix(EnemyIdentifier __instance, float modifier)
            {
                if (!Cheats.Enabled)
                {
                    return;
                }


            }

            public static void Postfix(EnemyIdentifier __instance, float modifier)
            {
                if (!Cheats.Enabled)
                {
                    return;
                }

                if (_expectingBuffCalls)
                {
                    return;
                }

                var rad = __instance.GetComponent<EnemyRadiance>();

                if (rad == null)
                {
                    return;
                }

                if (!rad._hasBuffedSpeedBefore)
                {
                    return;
                }

                PossiblyWarnOfCurrentMethod();

                rad._externalSpeedBuff = modifier;
                __instance.speedBuffModifier = rad._prevSpeedBuff;
            }
        }

        [HarmonyPatch(typeof(EnemyIdentifier), "DamageBuff", new Type[] { typeof(float) })]
        private static class EidDamageBuffPatch
        {
            public static void Prefix(EnemyIdentifier __instance, float modifier)
            {
                if (!Cheats.Enabled)
                {
                    return;
                }


            }

            public static void Postfix(EnemyIdentifier __instance, float modifier)
            {
                if (!Cheats.Enabled)
                {
                    return;
                }

                if (_expectingBuffCalls)
                {
                    return;
                }

                var rad = __instance.GetComponent<EnemyRadiance>();

                if (rad == null)
                {
                    return;
                }

                if (!rad._hasBuffedDamageBefore)
                {
                    return;
                }

                PossiblyWarnOfCurrentMethod();

                rad._externalHealthBuff = modifier;
                __instance.damageBuffModifier = rad._prevDamageBuff;
            }
        }
    }
}