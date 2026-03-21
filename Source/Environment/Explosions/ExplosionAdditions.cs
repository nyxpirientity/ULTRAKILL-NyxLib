using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class ExplosionAdditions : MonoBehaviour
    {
        public class ManagedExplosion
        {
            public Explosion Explosion = null;
            public float BaseMaxSize = 0.0f;
            public float BaseSpeed = 0.0f;
            public int BasePlayerDamageOverride = 0;
            public int BaseDamage = 0;
        }

        ManagedExplosion[] _Explosions = null;

        public IReadOnlyCollection<ManagedExplosion> Explosions { get => _Explosions; }
        public IEnumerable<AudioSource> Audios { get; private set; }

        public float ExplosionScale = 1.0f;
        public float ExplosionSpeedScale = 1.0f;
        public float ExplosionDamageScale = 1.0f;

        public bool ForceElectric = false;

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
                explosion.Explosion.damage = Mathf.RoundToInt(explosion.BaseDamage * ExplosionDamageScale);
                explosion.Explosion.playerDamageOverride = Mathf.RoundToInt(explosion.BasePlayerDamageOverride * ExplosionDamageScale);
                explosion.Explosion.electric = explosion.Explosion.electric || ForceElectric;
            }
        }
    }
}