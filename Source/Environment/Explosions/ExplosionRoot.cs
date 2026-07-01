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

        public void SetFriendlyFire(bool friendlyFire)
        {
            ForEachExplosion((e) => { e.friendlyFire = friendlyFire; });
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