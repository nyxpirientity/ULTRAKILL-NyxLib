using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class EnemyBloodFuel : EnemyModifier
    {
        public EnemyComponents Enemy { get; private set; } = null;

        protected void OnDestroy()
        {
            PlayerEvents.PreHurt -= PlayerPreHurt;
        }

        protected void Start()
        {
            PlayerEvents.PreHurt += PlayerPreHurt;
            Enemy = GetComponent<EnemyComponents>();
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

                Enemy.Health = Mathf.Min(Enemy.InitialHealth, Enemy.Health + heal);
            }
        }
    }
}