using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class ProjectileParryGainsModifier : MonoBehaviour
    {
        public float ParryHealthGain = 100.0f;
        public float ParryStaminaGain = 300.0f;
        public float ParryPunchStaminaGain = 0.0f;
        private Projectile _projectile;

        protected void Awake()
        {
            _projectile = GetComponent<Projectile>();        
        }

        protected void OnEnable()
        {
            PlayerPunchEvents.PreParryProjectile += PreParryProjectile;
        }

        protected void OnDisable()
        {
            PlayerPunchEvents.PreParryProjectile -= PreParryProjectile;
        }

        private void PreParryProjectile(EventMethodCanceler canceler, Punch punch, Projectile proj)
        {
            if (!Cheats.Enabled)
            {
                return;
            }

            if (proj != _projectile)
            {
                return;
            }

            proj.playerBullet = true;
            var v1 = NewMovement.Instance;
            v1.GetHealth((int)ParryHealthGain, false, false, true);
            WeaponCharges.Instance.punchStamina = Mathf.Min(MonoSingleton<WeaponCharges>.Instance.punchStamina + ParryPunchStaminaGain, Mathf.Max(2.0f, MonoSingleton<WeaponCharges>.Instance.punchStamina));
            
            if (v1.boostCharge + ParryStaminaGain >= 300.0f)
            {
                v1.FullStamina();
            }
            else
            {
                v1.boostCharge += ParryStaminaGain;                
            }
        }
    }
}