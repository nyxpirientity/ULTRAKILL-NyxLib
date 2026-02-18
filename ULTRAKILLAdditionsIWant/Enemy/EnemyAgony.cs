using System;
using UnityEngine;

namespace UKAIW
{
    public class EnemyAgony : MonoBehaviour
    {
        public EnemyAdditions Eadd { get; private set; } = null;

        protected void Awake()
        {
            Eadd = GetComponent<EnemyAdditions>();
        }

        protected void Start()
        {
            Eadd.PreHurt += OnPreHurt;
        }

        private void OnPreHurt(GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
        {
        }
    }
}