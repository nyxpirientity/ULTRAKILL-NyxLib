using UnityEngine;

namespace UKAIW
{
    public class PlayerPain : MonoBehaviour
    {
        NewMovement Player = null;

        protected void Start()
        {
            Player = NewMovement.Instance;
        }

        protected void FixedUpdate()
        {
            var painValue = Mathf.Lerp(0.0f, -1.0f, NyxMath.NormalizeToRange(Player.hp, -100.0f, 100.0f));
            Heck.Instance.PainStore.AddPain((float)painValue * Time.fixedDeltaTime);
        }

        protected void OnEnable()
        {
            PlayerEvents.PostHurt += PostPlayerHurt;
        }

        protected void OnDisable()
        {
            PlayerEvents.PostHurt -= PostPlayerHurt;
        }

        private void PostPlayerHurt(NewMovement nm, int processedDamage, bool invincible, float scoreLossMultiplier, bool explosion, bool instablack, float hardDamageMultiplier, bool ignoreInvincibility)
        {
            Heck.Instance.PainStore.AddPain((float)processedDamage * 0.01f);
        }
    }
}