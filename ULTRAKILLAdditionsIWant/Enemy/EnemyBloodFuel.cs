using System;
using MelonLoader;
using UnityEngine;

namespace UKAIW
{
    public class EnemyBloodFuel : EnemyModifier
    {
        protected void OnDestroy()
        {
            PlayerEvents.PreHurt -= PlayerPreHurt;
        }

        protected void Start()
        {
            PlayerEvents.PreHurt += PlayerPreHurt;
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
                var eadd = GetComponent<EnemyAdditions>();
                
                if (eadd == null)
                {
                    Destroy(this);
                    return;
                }

                float heal = (damage * normalizedDist);
                heal *= Options.BloodFuelEnemiesHealScalar;

                eadd.Health = Mathf.Min(eadd.PrefabStore.Prefab.GetComponent<EnemyAdditions>().Health, eadd.Health + heal);
            }
        }
    }
}