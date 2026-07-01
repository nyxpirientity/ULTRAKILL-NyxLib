using System.Collections.Generic;
using System.Linq;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class ExplosionStartModifier : MonoBehaviour
    {
        public class ManagedExplosion
        {
            public Explosion Explosion = null;
            public float BaseMaxSize = 0.0f;
            public float BasePushScale = 0.0f;
            public float BaseSpeed = 0.0f;
            public float BaseEnemyDamageMultiplier = 0.0f;
            public int BasePlayerDamageOverride = 0;
            public int BaseDamage = 0;
        }

        ManagedExplosion[] _Explosions = null;

        public IReadOnlyCollection<ManagedExplosion> Explosions { get => _Explosions; }
        public IEnumerable<AudioSource> Audios { get; private set; }

        [SerializeField] private float _baseDamageOverride = -1.0f;
        [SerializeField] private sbyte _harmless = -1;
        public float? BaseDamageOverride
        {
            get
            {
                if (_baseDamageOverride >= 0.0f)
                {
                    return _baseDamageOverride;
                }

                return null;
            }

            set
            {
                _baseDamageOverride = value.GetValueOrDefault(-1.0f);
            }
        }
        public float ExplosionScale = 1.0f;
        public float ExplosionSpeedScale = 1.0f;
        public float ExplosionDamageScale = 1.0f;
        public float ExplosionEnemyDamageMultiplierScale = 1.0f;
        public float ExplosionPushScale = 1.0f;

        public bool ForceElectric = false;
        public bool? Harmless
        {
            get
            {
                if (_harmless >= 0)
                {
                    return _harmless > 0;
                }

                return null;
            }

            set
            {
                _harmless = value.HasValue ? (value.Value ? (sbyte)1 : (sbyte)0) : (sbyte)-1;
            }
        }

        protected void Awake()
        {
            var explosions = GetComponentsInChildren<Explosion>();
            _Explosions = new ManagedExplosion[explosions.Length];

            for (int i = 0; i < explosions.Length; i++)
            {
                Explosion explosion = explosions[i];
                var mExplosion = new ManagedExplosion();
                mExplosion.Explosion = explosion;
                mExplosion.BaseSpeed = explosion.speed;
                mExplosion.BaseMaxSize = explosion.maxSize;
                mExplosion.BaseDamage = explosion.damage;
                mExplosion.BasePushScale = explosion.pushForceMultiplier;
                mExplosion.BaseEnemyDamageMultiplier = explosion.enemyDamageMultiplier;
                mExplosion.BasePlayerDamageOverride = explosion.playerDamageOverride;
                _Explosions[i] = mExplosion;
            }

            Audios = GetComponentsInChildren<AudioSource>().Concat(GetComponents<AudioSource>());
        }

        protected void Start()
        {
            ApplyValues();

            foreach (var audio in Audios)
            {
                if (audio.loop)
                {
                    continue;
                }

                audio.Play();
            }
        }

        public void ApplyValues()
        {
            foreach (var explosion in _Explosions)
            {
                explosion.Explosion.maxSize = explosion.BaseMaxSize * ExplosionScale;
                explosion.Explosion.speed = explosion.BaseSpeed * ExplosionSpeedScale;
                explosion.Explosion.damage = Mathf.RoundToInt((BaseDamageOverride.GetValueOrDefault(explosion.BaseDamage)) * ExplosionDamageScale);
                explosion.Explosion.enemyDamageMultiplier = explosion.BaseEnemyDamageMultiplier * ExplosionEnemyDamageMultiplierScale;
                explosion.Explosion.playerDamageOverride = Mathf.RoundToInt(explosion.BasePlayerDamageOverride * ExplosionDamageScale);
                explosion.Explosion.electric = explosion.Explosion.electric || ForceElectric;
                explosion.Explosion.harmless = Harmless.GetValueOrDefault(explosion.Explosion.harmless);
                explosion.Explosion.pushForceMultiplier = explosion.BasePushScale * ExplosionPushScale;
            }
        }
    }
}