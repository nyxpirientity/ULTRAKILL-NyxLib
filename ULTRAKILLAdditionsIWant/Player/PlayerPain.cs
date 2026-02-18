
using System;
using UnityEngine;

namespace UKAIW
{
    public class PlayerPain : MonoBehaviour
    {
        protected void Start()
        {
            
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
            Heck.Instance.PainStore.AddPain(processedDamage);
        }
    }
}