using System;
using MelonLoader;
using UnityEngine;

namespace UKAIW
{
    public class EnemyBloodFuel : EnemyModifier
    {
        public EnemyAdditions Eadd { get; private set; } = null;

        protected void OnDestroy()
        {
            PlayerEvents.PreHurt -= PlayerPreHurt;
        }

        protected void Start()
        {
            PlayerEvents.PreHurt += PlayerPreHurt;
            Eadd = GetComponent<EnemyAdditions>();
        }

        private void PlayerPreHurt(NewMovement player, int damage, bool invincible, float scoreLossMultiplier, bool explosion, bool instablack, float hardDamageMultiplier, bool ignoreInvincibility)
        {
            if (Cheats.IsCheatEnabled(Cheats.BloodFueledEnemies))
            {
                var playerPos = player.rb.transform.position;
                var pos = transform.position;

                var dist = Vector3.Distance(playerPos, pos);

                float maxDist = damage / Options.BloodFuelEnemiesDistDivisor;

                float normalizedDist = 1.0f - Mathf.Min(1.0f, dist / maxDist);
                
                float heal = (damage * normalizedDist);
                heal *= Options.BloodFuelEnemiesHealScalar;

                Eadd.Health = Mathf.Min(Eadd.InitialHealth, Eadd.Health + heal);
            }
        }
    }
}