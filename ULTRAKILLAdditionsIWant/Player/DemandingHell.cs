using System;
using MelonLoader;
using TMPro;
using UnityEngine;

namespace UKAIW
{
    public class DemandingHell : MonoBehaviour
    {
        protected void OnDestroy()
        {
            Player.PreHurt -= PlayerPreHurt;
        }

        protected void Start()
        {
            Player.PreHurt += PlayerPreHurt;
        }

        protected void FixedUpdate()
        {
            if (!Cheats.IsCheatEnabled(Cheats.DemandingHell))
            {
                return;
            }
            
            if (Time.timeSinceLevelLoad < 4.0f)
            {
                return;
            }
            
            if (OurHeatResistance != null && HeatResistance.Instance != null && HeatResistance.Instance != OurHeatResistance)
            {
                UnityEngine.Object.Destroy(OurHeatResistanceRootGo);
                OurHeatResistance = null;
            }
            else if (OurHeatResistance == null && HeatResistance.Instance == null)
            {
                OurHeatResistanceRootGo = UnityEngine.Object.Instantiate(Assets.HeatResistancePrefab, CanvasController.Instance.gameObject.transform);
                OurHeatResistanceRootGo.SetActive(true);
                OurHeatResistance = OurHeatResistanceRootGo.GetComponentInChildren<HeatResistance>(true);
                OurHeatResistance.enabled = true;
                OurHeatResistance.gameObject.SetActive(true);
                OurHeatResistanceRootGo.transform.SetAsFirstSibling();
                OurHeatResistanceFlavourText = OurHeatResistance.gameObject.transform.Find("Flavor Text").gameObject.GetComponent<TextMeshProUGUI>();
                OurHeatResistanceFlavourText.text = "YOU THINK YOU'RE SO GOOD? WELL YOU'D BETTER STAY MOVING, BLOOD BUCKET";
            }
            
            OurHeatResistance.speed = 100.0f;
            float heatResistanceRecovery = NewMovement.Instance.rb.velocity.magnitude * 2.2f;
            FieldPublisher<HeatResistance, float> heatResistance = new FieldPublisher<HeatResistance, float>(OurHeatResistance, "heatResistance");
            if (heatResistance.Value >= 0.1f || heatResistanceRecovery > OurHeatResistance.speed)
            {
                heatResistance.Value = Mathf.MoveTowards(heatResistance.Value, 100.0f, (Time.fixedDeltaTime * heatResistanceRecovery));
            }
        }

        private void PlayerPreHurt(NewMovement player, int damage, bool invincible, float scoreLossMultiplier, bool explosion, bool instablack, float hardDamageMultiplier, bool ignoreInvincibility)
        {

        }

        GameObject OurHeatResistanceRootGo = null;
        HeatResistance OurHeatResistance = null;

        public TextMeshProUGUI OurHeatResistanceFlavourText { get; private set; }
    }
}