using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class ExplosionRoot : MonoBehaviour
    {
        public IReadOnlyCollection<Explosion> Explosions
        {
            get
            {
                if (_explosions == null || _explosions.Length == 0)
                {
                    FindExplosions();
                }

                return _explosions;
            }
        }

        public bool FriendlyFire { set => ForEachExplosion((e) => { e.friendlyFire = value; }); }
        public bool Enemy { set => ForEachExplosion((e) => { e.enemy = value; }); }
        public bool IsFup { set => ForEachExplosion((e) => { e.isFup = value; }); }
        public bool Ultrabooster { set => ForEachExplosion((e) => { e.ultrabooster = value; }); }
        public bool RocketExplosion { set => ForEachExplosion((e) => { e.rocketExplosion = value; }); }

        public bool Ignite { set => ForEachExplosion((e) => { e.enemy = value; }); }
        public bool Eletric { set => ForEachExplosion((e) => { e.electric = value; }); }

        public bool Unblockable { set => ForEachExplosion((e) => { e.unblockable = value; }); }
        public bool Halved { set => ForEachExplosion((e) => { e.halved = value; }); }

        public string HitterWeapon { set => ForEachExplosion((e) => { e.hitterWeapon = value; }); }
        public EnemyIdentifier OriginEid { set => ForEachExplosion((e) => { e.originEnemy = value; }); }

        public AffectedSubjects CanHit { set => ForEachExplosion((e) => { e.canHit = value; }); }
        public List<EnemyType> ToIgnore { set => ForEachExplosion((e) => { e.toIgnore = value; }); }

        public void SetMaxDamage(int maxDamage)
        {
            if (Explosions.Count == 0)
            {
                return;
            }

            int highestDamage = int.MinValue;

            foreach (var explosion in Explosions)
            {
                highestDamage = Math.Max(explosion.damage, highestDamage);
            }

            float scalar = maxDamage / highestDamage;

            ScaleDamage(scalar);
        }

        public void SetMaxSize(float maxSize)
        {
            if (Explosions.Count == 0)
            {
                return;
            }

            float highestVal = float.NegativeInfinity;

            foreach (var explosion in Explosions)
            {
                highestVal = Math.Max(explosion.maxSize, highestVal);
            }

            float scalar = maxSize / highestVal;

            ScaleSize(scalar);
        }

        public void SetMaxSpeed(float maxSpeed)
        {
            if (Explosions.Count == 0)
            {
                return;
            }

            float highestVal = GetMaxSpeed();

            float scalar = maxSpeed / highestVal;

            ScaleSpeed(scalar);
        }

        public float GetMaxSpeed()
        {
            float highestVal = float.NegativeInfinity;

            foreach (var explosion in Explosions)
            {
                highestVal = Math.Max(explosion.speed, highestVal);
            }

            return highestVal;
        }

        public void SetMaxEnemyDamageMultiplier(float maxVal)
        {
            if (Explosions.Count == 0)
            {
                return;
            }

            float highestVal = float.NegativeInfinity;

            foreach (var explosion in Explosions)
            {
                highestVal = Math.Max(explosion.enemyDamageMultiplier, highestVal);
            }

            float scalar = maxVal / highestVal;

            ScaleEnemyDamageMultiplier(scalar);
        }


        public void SetMaxPlayerDamageOverride(int maxVal)
        {
            if (Explosions.Count == 0)
            {
                return;
            }

            if (maxVal < 0)
            {
                ForEachExplosion((e) => { e.playerDamageOverride = Math.Min(maxVal, e.playerDamageOverride); });
                return;
            }

            int highestVal = int.MinValue;

            foreach (var explosion in Explosions)
            {
                highestVal = Math.Max(explosion.playerDamageOverride, highestVal);
            }

            float scalar = maxVal / highestVal;

            foreach (var explosion in Explosions)
            {
                explosion.playerDamageOverride = Mathf.RoundToInt(explosion.playerDamageOverride * scalar);
            }
        }

        public void ScaleSize(float scalar)
        {
            ForEachExplosion((e) => { e.maxSize *= scalar; });
        }

        public void ScaleSpeed(float scalar)
        {
            ForEachExplosion((e) => { e.speed *= scalar; });
        }

        public void ScaleSpeedAndSize(float speedScalar, float sizeScalar)
        {
            ForEachExplosion((e) => { e.speed *= speedScalar; e.maxSize *= sizeScalar; });
        }

        public void ScaleSpeedAndSize(float scalar)
        {
            ScaleSpeedAndSize(scalar, scalar);
        }

        public void MakeHarmless()
        {
            ForEachExplosion((e) => { e.harmless = true; });
        }

        public void ScalePushForce(float scalar)
        {
            ForEachExplosion((e) => { e.pushForceMultiplier *= scalar; });
        }

        public void ScaleDamage(float scalar)
        {
            ForEachExplosion((e) =>
            {
                e.damage = (int)((float)e.damage * scalar);
            });
        }

        public void ScaleEnemyDamageMultiplier(float scalar)
        {
            ForEachExplosion((e) =>
            {
                e.enemyDamageMultiplier *= scalar;
            });
        }

        public void ForEachExplosion(Action<Explosion> action)
        {
            foreach (var explosion in Explosions)
            {
                action(explosion);
            }
        }

        public void FindExplosions()
        {
            _explosions = GetComponentsInChildren<Explosion>();
        }

        [SerializeField] private Explosion[] _explosions = null;

        private void Awake()
        {
            FindExplosions();
        }
    }
}