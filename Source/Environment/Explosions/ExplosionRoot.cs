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