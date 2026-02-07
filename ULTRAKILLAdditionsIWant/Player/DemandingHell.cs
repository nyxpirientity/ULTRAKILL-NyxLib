using System;
using MelonLoader;
using TMPro;
using UnityEngine;

namespace UKAIW
{
    public class DemandingHell : MonoBehaviour
    {
        public NewMovement player { get; private set; } = null;
        public StyleHUD Shud { get; private set; } = null;


        private void EnemyPostHurt(EnemyIdentifier eid, GameObject enemyGo, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon, bool fromExplosion, Vector3? hitPoint)
        {
            if (OurHeatResistance != null)
            {
                if (eid.Dead)
                {
                    return;
                }
                
                FieldPublisher<HeatResistance, float> heatResistance = new FieldPublisher<HeatResistance, float>(OurHeatResistance, "heatResistance");

                StyleRanks styleRank = (StyleRanks)(Shud.rankIndex);
            
                OurHeatResistance.speed = 0.0f;

                float explosiveSizeScalar = -1.0f;
                float explosiveDmgScalar = -1.0f;

                switch (styleRank)
                {
                    case StyleRanks.Null:
                    case StyleRanks.Destructive:
                        explosiveSizeScalar = Options.DemandingHellDestructiveHeatResExplosiveSizeScalar.Value;
                        explosiveDmgScalar = Options.DemandingHellDestructiveHeatResExplosiveDmgScalar.Value;
                        break;
                    case StyleRanks.Chaotic:
                        explosiveSizeScalar = Options.DemandingHellChaoticHeatResExplosiveSizeScalar.Value;
                        explosiveDmgScalar = Options.DemandingHellChaoticHeatResExplosiveDmgScalar.Value;
                        break;
                    case StyleRanks.Brutal:
                        explosiveSizeScalar = Options.DemandingHellBrutalHeatResExplosiveSizeScalar.Value;
                        explosiveDmgScalar = Options.DemandingHellBrutalHeatResExplosiveDmgScalar.Value;
                        break;
                    case StyleRanks.Anarchic:
                        explosiveSizeScalar = Options.DemandingHellAnarchicHeatResExplosiveSizeScalar.Value;
                        explosiveDmgScalar = Options.DemandingHellAnarchicHeatResExplosiveDmgScalar.Value;
                        break;
                    case StyleRanks.Supreme:
                        explosiveSizeScalar = Options.DemandingHellSupremeHeatResExplosiveSizeScalar.Value;
                        explosiveDmgScalar = Options.DemandingHellSupremeHeatResExplosiveDmgScalar.Value;
                        break;
                    case StyleRanks.SSadistic:
                        explosiveSizeScalar = Options.DemandingHellSSadisticHeatResExplosiveSizeScalar.Value;
                        explosiveDmgScalar = Options.DemandingHellSSadisticHeatResExplosiveDmgScalar.Value;
                        break;
                    case StyleRanks.SSSensoredStorm:
                        explosiveSizeScalar = Options.DemandingHellSSSensoredStormHeatResExplosiveSizeScalar.Value;
                        explosiveDmgScalar = Options.DemandingHellSSSensoredStormHeatResExplosiveDmgScalar.Value;
                        break;
                    case StyleRanks.ULTRAKILL:
                        explosiveSizeScalar = Options.DemandingHellULTRAKILLHeatResExplosiveSizeScalar.Value;
                        explosiveDmgScalar = Options.DemandingHellULTRAKILLHeatResExplosiveDmgScalar.Value;
                        break;
                }

                if (heatResistance.Value <= 25.0f && !fromExplosion && explosiveSizeScalar > 0.0f)
                {
                    if (hitPoint == null)
                    {
                        hitPoint = eid.transform.position;
                    }

                    Assert.IsNotNull(Assets.ExplosionPrefab);

                    var explosionGo = UnityEngine.Object.Instantiate(Assets.ExplosionPrefab);
                    explosionGo.transform.position = hitPoint.Value;
                    var explosion = explosionGo.GetComponentInChildren<Explosion>();

                    explosion.damage = (int)(multiplier * explosiveDmgScalar);
                    explosion.enemy = false;
                    explosion.harmless = explosiveDmgScalar <= 0.0f;
                    explosion.lowQuality = false;
                    explosion.maxSize = multiplier * explosiveSizeScalar;
                    explosion.speed = explosion.maxSize;
                    explosion.enemyDamageMultiplier = 1.0f;
                    explosion.playerDamageOverride = 0;
                    explosion.ignite = true;
                    explosion.friendlyFire = false;
                    explosion.isFup = false;
                    explosion.hitterWeapon = "";
                    explosion.halved = false;
                    explosion.canHit = AffectedSubjects.EnemiesOnly;
                    explosion.originEnemy = null;
                    explosion.rocketExplosion = false;
                    explosion.toIgnore = new System.Collections.Generic.List<EnemyType>();
                    explosion.ultrabooster = false;
                    explosion.unblockable = false;
                    explosion.electric = false;

                    explosionGo.SetActive(true);
                }
            }
        }

        protected void Start()
        {
            Player.PreHurt += PlayerPreHurt;
            EnemyEvents.PostHurt += EnemyPostHurt;
            player = NewMovement.Instance;
            Shud = StyleHUD.Instance;
        }

        protected void OnDestroy()
        {
            Player.PreHurt -= PlayerPreHurt;
            EnemyEvents.PostHurt -= EnemyPostHurt;
        }

        protected void FixedUpdate()
        {
            if (OurHeatResistance != null)
            {
                FieldPublisher<HeatResistance, float> heatResistance = new FieldPublisher<HeatResistance, float>(OurHeatResistance, "heatResistance");
                if (heatResistance.Value <= 50.0f )
                {
                    StainVoxelManager.Instance.TryIgniteAt(player.rb.transform.position);
                }
            }

            if (!Cheats.IsCheatEnabled(Cheats.DemandingHell))
            {
                if (OurHeatResistance.isActiveAndEnabled)
                {
                    OurHeatResistance.gameObject.SetActive(false);
                }

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
                OurHeatResistance = OurHeatResistanceRootGo.GetComponentInChildren<HeatResistance>(true);
                OurHeatResistance.enabled = true;
                OurHeatResistance.gameObject.SetActive(false);
                OurHeatResistanceRootGo.SetActive(true);
                OurHeatResistanceRootGo.transform.SetAsFirstSibling();
                OurHeatResistanceFlavourText = OurHeatResistance.gameObject.transform.Find("Flavor Text").gameObject.GetComponent<TextMeshProUGUI>();
                OurHeatResistanceFlavourText.text = "YOU THINK YOU'RE SO GOOD? WELL YOU'D BETTER KEEP MOVING, BLOOD BUCKET";
            }
            
            if (OurHeatResistance != null)
            {
                float heatResistanceRecovery = player.rb.velocity.magnitude;

                StyleRanks styleRank = (StyleRanks)(Shud.rankIndex);
                OurHeatResistance.speed = 0.0f;
                switch (styleRank)
                {
                    case StyleRanks.Null:
                    case StyleRanks.Destructive:
                        heatResistanceRecovery *= Options.DemandingHellDestructiveHeatResRecoveryEntry.Value;
                        OurHeatResistance.speed = Options.DemandingHellDestructiveHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.Chaotic:
                        heatResistanceRecovery *= Options.DemandingHellChaoticHeatResRecoveryEntry.Value;
                        OurHeatResistance.speed = Options.DemandingHellChaoticHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.Brutal:
                        heatResistanceRecovery *= Options.DemandingHellBrutalHeatResRecoveryEntry.Value;
                        OurHeatResistance.speed = Options.DemandingHellBrutalHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.Anarchic:
                        heatResistanceRecovery *= Options.DemandingHellAnarchicHeatResRecoveryEntry.Value;
                        OurHeatResistance.speed = Options.DemandingHellAnarchicHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.Supreme:
                        heatResistanceRecovery *= Options.DemandingHellSupremeHeatResRecoveryEntry.Value;
                        OurHeatResistance.speed = Options.DemandingHellSupremeHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.SSadistic:
                        heatResistanceRecovery *= Options.DemandingHellSSadisticHeatResRecoveryEntry.Value;
                        OurHeatResistance.speed = Options.DemandingHellSSadisticHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.SSSensoredStorm:
                        heatResistanceRecovery *= Options.DemandingHellSSSensoredStormHeatResRecoveryEntry.Value;
                        OurHeatResistance.speed = Options.DemandingHellSSSensoredStormHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.ULTRAKILL:
                        heatResistanceRecovery *= Options.DemandingHellULTRAKILLHeatResRecoveryEntry.Value;
                        OurHeatResistance.speed = Options.DemandingHellULTRAKILLHeatResDrainEntry.Value;
                        break;
                }

                if (OurHeatResistance.isActiveAndEnabled && OurHeatResistance.speed <= 0.0f)
                {
                    OurHeatResistance.gameObject.SetActive(false);
                }
                else if (!OurHeatResistance.isActiveAndEnabled && OurHeatResistance.speed > 0.0f)
                {
                    OurHeatResistance.gameObject.SetActive(true);
                }

                FieldPublisher<HeatResistance, float> heatResistance = new FieldPublisher<HeatResistance, float>(OurHeatResistance, "heatResistance");
                
                if (heatResistance.Value >= 0.1f || heatResistanceRecovery > OurHeatResistance.speed)
                {
                    heatResistance.Value = Mathf.MoveTowards(heatResistance.Value, 100.0f, (Time.fixedDeltaTime * heatResistanceRecovery));
                }
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