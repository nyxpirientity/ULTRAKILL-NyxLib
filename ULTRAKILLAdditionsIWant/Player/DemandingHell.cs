using System;
using MelonLoader;
using SettingsMenu.Components.Pages;
using TMPro;
using UnityEngine;

namespace UKAIW
{
    public class DemandingHell : MonoBehaviour
    {
        public NewMovement player { get; private set; } = null;
        public StyleHUD Shud { get; private set; } = null;
        public FieldPublisher<NewMovement, float> AntiHpCooldown { get; private set; } = null;

        private void EnemyPostHurt(EnemyIdentifier eid, GameObject enemyGo, GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon, bool fromExplosion, Vector3? hitPoint)
        {
            if (!Cheats.IsCheatEnabled(Cheats.DemandingHell))
            {
                return;
            }

            if (sourceWeapon == null)
            {
                return;
            }
            
            if (eid.Dead)
            {
                return;
            }

            if (OurHeatResistance != null)
            {
                FieldPublisher<HeatResistance, float> heatResistance = new FieldPublisher<HeatResistance, float>(OurHeatResistance, "heatResistance");

                if (heatResistance.Value <= 20.0f && !fromExplosion)
                {
                    HeatResExplosion(multiplier, hitPoint.GetValueOrDefault(eid.transform.position), false, out _);
                }

                if (heatResistance.Value <= 35.0f)
                {
                    eid.StartBurning(3.0f);
                }
            }
        }

        private bool HeatResExplosion(float multiplier, Vector3 hitPoint, bool forceDontHitPlayer, out float explosiveSize)
        {
            StyleRanks styleRank = (StyleRanks)(Shud.rankIndex);

            float explosiveSizeBase = 0.0f;
            float explosiveSizeNormMin = 1000000000.0f;
            float explosiveSizeNormMax = 10000000000.0f;
            float explosiveDmgScalar = -1.0f;
            bool explosiveDamagePlayer = false;

            switch (styleRank)
            {
                case StyleRanks.Null:
                case StyleRanks.Destructive:
                    explosiveSizeBase = Options.DemandingHellDestructiveHeatResExplosiveSizeBase.Value;
                    explosiveSizeNormMin = Options.DemandingHellDestructiveHeatResExplosiveSizeNormMin.Value;
                    explosiveSizeNormMax = Options.DemandingHellDestructiveHeatResExplosiveSizeNormMax.Value;
                    explosiveDmgScalar = Options.DemandingHellDestructiveHeatResExplosiveDmgScalar.Value;
                    explosiveDamagePlayer = Options.DemandingHellDestructiveHeatResExplosiveDmgPlayer.Value;
                    break;
                case StyleRanks.Chaotic:
                    explosiveSizeBase = Options.DemandingHellChaoticHeatResExplosiveSizeBase.Value;
                    explosiveSizeNormMin = Options.DemandingHellChaoticHeatResExplosiveSizeNormMin.Value;
                    explosiveSizeNormMax = Options.DemandingHellChaoticHeatResExplosiveSizeNormMax.Value;
                    explosiveDmgScalar = Options.DemandingHellChaoticHeatResExplosiveDmgScalar.Value;
                    explosiveDamagePlayer = Options.DemandingHellChaoticHeatResExplosiveDmgPlayer.Value;
                    break;
                case StyleRanks.Brutal:
                    explosiveSizeBase = Options.DemandingHellBrutalHeatResExplosiveSizeBase.Value;
                    explosiveSizeNormMin = Options.DemandingHellBrutalHeatResExplosiveSizeNormMin.Value;
                    explosiveSizeNormMax = Options.DemandingHellBrutalHeatResExplosiveSizeNormMax.Value;
                    explosiveDmgScalar = Options.DemandingHellBrutalHeatResExplosiveDmgScalar.Value;
                    explosiveDamagePlayer = Options.DemandingHellBrutalHeatResExplosiveDmgPlayer.Value;
                    break;
                case StyleRanks.Anarchic:
                    explosiveSizeBase = Options.DemandingHellAnarchicHeatResExplosiveSizeBase.Value;
                    explosiveSizeNormMin = Options.DemandingHellAnarchicHeatResExplosiveSizeNormMin.Value;
                    explosiveSizeNormMax = Options.DemandingHellAnarchicHeatResExplosiveSizeNormMax.Value;
                    explosiveDmgScalar = Options.DemandingHellAnarchicHeatResExplosiveDmgScalar.Value;
                    explosiveDamagePlayer = Options.DemandingHellAnarchicHeatResExplosiveDmgPlayer.Value;
                    break;
                case StyleRanks.Supreme:
                    explosiveSizeBase = Options.DemandingHellAnarchicHeatResExplosiveSizeBase.Value;
                    explosiveSizeNormMin = Options.DemandingHellSupremeHeatResExplosiveSizeNormMin.Value;
                    explosiveSizeNormMax = Options.DemandingHellSupremeHeatResExplosiveSizeNormMax.Value;
                    explosiveDmgScalar = Options.DemandingHellSupremeHeatResExplosiveDmgScalar.Value;
                    explosiveDamagePlayer = Options.DemandingHellSupremeHeatResExplosiveDmgPlayer.Value;
                    break;
                case StyleRanks.SSadistic:
                    explosiveSizeBase = Options.DemandingHellSSadisticHeatResExplosiveSizeBase.Value;
                    explosiveSizeNormMin = Options.DemandingHellSSadisticHeatResExplosiveSizeNormMin.Value;
                    explosiveSizeNormMax = Options.DemandingHellSSadisticHeatResExplosiveSizeNormMax.Value;
                    explosiveDmgScalar = Options.DemandingHellSSadisticHeatResExplosiveDmgScalar.Value;
                    explosiveDamagePlayer = Options.DemandingHellSSadisticHeatResExplosiveDmgPlayer.Value;
                    break;
                case StyleRanks.SSSensoredStorm:
                    explosiveSizeBase = Options.DemandingHellSSSensoredStormHeatResExplosiveSizeBase.Value;
                    explosiveSizeNormMin = Options.DemandingHellSSSensoredStormHeatResExplosiveSizeNormMin.Value;
                    explosiveSizeNormMax = Options.DemandingHellSSSensoredStormHeatResExplosiveSizeNormMax.Value;
                    explosiveDmgScalar = Options.DemandingHellSSSensoredStormHeatResExplosiveDmgScalar.Value;
                    explosiveDamagePlayer = Options.DemandingHellSSSensoredStormHeatResExplosiveDmgPlayer.Value;
                    break;
                case StyleRanks.ULTRAKILL:
                    explosiveSizeBase = Options.DemandingHellULTRAKILLHeatResExplosiveSizeBase.Value;
                    explosiveSizeNormMin = Options.DemandingHellULTRAKILLHeatResExplosiveSizeNormMin.Value;
                    explosiveSizeNormMax = Options.DemandingHellULTRAKILLHeatResExplosiveSizeNormMax.Value;
                    explosiveDmgScalar = Options.DemandingHellULTRAKILLHeatResExplosiveDmgScalar.Value;
                    explosiveDamagePlayer = Options.DemandingHellULTRAKILLHeatResExplosiveDmgPlayer.Value;
                    break;
            }

            explosiveSize = explosiveSizeBase * Mathf.Max(0.0f, explosiveSizeNormMin <= explosiveSizeNormMax ? NyxMath.NormalizeToRange(multiplier, explosiveSizeNormMin, explosiveSizeNormMax) : NyxMath.InverseNormalizeToRange(multiplier, explosiveSizeNormMin, explosiveSizeNormMax));

            if (explosiveSize < 0.01f)
            {
                return false;
            }

            if (Assets.ExplosionPrefab == null)
            {
                return false;
            }

            var explosionGo = UnityEngine.Object.Instantiate(Assets.ExplosionPrefab);
            explosionGo.transform.position = hitPoint;
            var explosion = explosionGo.GetComponentInChildren<Explosion>();

            explosion.damage = (int)(multiplier * explosiveDmgScalar);
            explosion.enemy = false;
            explosion.harmless = explosiveDmgScalar <= 0.0f;
            explosion.lowQuality = false;
            explosion.maxSize = explosiveSize;
            explosion.speed = explosion.maxSize;
            explosion.enemyDamageMultiplier = 1.0f;
            explosion.playerDamageOverride = -1;
            explosion.ignite = true;
            explosion.friendlyFire = false;
            explosion.isFup = false;
            explosion.hitterWeapon = "";
            explosion.halved = false;
            explosion.canHit = explosiveDamagePlayer && !forceDontHitPlayer ? AffectedSubjects.All : AffectedSubjects.EnemiesOnly;
            explosion.originEnemy = null;
            explosion.rocketExplosion = false;
            explosion.toIgnore = new System.Collections.Generic.List<EnemyType>();
            explosion.ultrabooster = false;
            explosion.unblockable = false;
            explosion.electric = false;

            explosionGo.SetActive(true);
            return true;
        }

        protected void Start()
        {
            Player.PostHurt += PlayerPostHurt;
            EnemyEvents.PostHurt += EnemyPostHurt;
            player = NewMovement.Instance;
            Shud = StyleHUD.Instance;
            AntiHpCooldown = new FieldPublisher<NewMovement, float>(player, "antiHpCooldown");
        }

        protected void OnDestroy()
        {
            Player.PostHurt -= PlayerPostHurt;
            EnemyEvents.PostHurt -= EnemyPostHurt;
        }

        float CurrentHeatResistance = 0.0f;
        float HeatResistanceDrain = 0.0f;
        float HeatResAntiHpCooldown = 0.0f;
        float HeatResHurtTimer = -1.0f;
        float TimeSinceLastHeatResActivation = 10.0f;
        float HeatResRankDescensionTimer = 5.0f;
        float HeatResRankDescensionTimerMax = 5.0f;

        public void ResetHeatRestRankDescensionTimer()
        {
            HeatResRankDescensionTimer = HeatResRankDescensionTimerMax;
        }

        protected void FixedUpdate()
        {
            if (!Cheats.IsCheatEnabled(Cheats.DemandingHell))
            {
                if (OurHeatResistance.isActiveAndEnabled)
                {
                    OurHeatResistance.gameObject.SetActive(false);
                }

                return;
            }
            
            if (OurHeatResistance != null)
            {
                FieldPublisher<HeatResistance, float> heatResistance = new FieldPublisher<HeatResistance, float>(OurHeatResistance, "heatResistance");
                if (heatResistance.Value <= 50.0f && OurHeatResistance.isActiveAndEnabled)
                {
                    StainVoxelManager.Instance.TryIgniteAt(player.rb.transform.position);
                }
            }

            if (Time.timeSinceLevelLoad < 1.0f)
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
                
                //OurHeatResistance.gameObject.DebugPrintChildren();
                OurHeatResistanceFlavourText = OurHeatResistance.gameObject.transform.Find("Flavor Text").gameObject.GetComponent<TextMeshProUGUI>();
                HeatResLabel = OurHeatResistance.gameObject.transform.Find("Meter/Label").gameObject.GetComponent<TextMeshProUGUI>();
                HeatResFlashingText = OurHeatResistance.gameObject.transform.Find("Warning").gameObject.GetComponent<TextMeshProUGUI>();

                OurHeatResistanceFlavourText.text = "YOU THINK YOU'RE SO GOOD? WELL YOU'D BETTER KEEP MOVING, BLOOD BUCKET";
                FieldPublisher<HeatResistance, GameObject> hurtingSound = new FieldPublisher<HeatResistance, GameObject>(OurHeatResistance, "hurtingSound");
                DefaultHurtingSoundPitch = hurtingSound.Value.GetComponent<AudioSource>().pitch;
            }
            
            TimeSinceLastHeatResActivation += Time.fixedDeltaTime;

            if (OurHeatResistance != null)
            {
                float heatResistanceRecovery = player.rb.velocity.magnitude;

                StyleRanks styleRank = (StyleRanks)(Shud.rankIndex);
                OurHeatResistance.speed = 0.0f;
                HeatResistanceDrain = 0.0f;
                switch (styleRank)
                {
                    case StyleRanks.Null:
                    case StyleRanks.Destructive:
                        heatResistanceRecovery *= Options.DemandingHellDestructiveHeatResRecoveryEntry.Value;
                        HeatResistanceDrain = Options.DemandingHellDestructiveHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.Chaotic:
                        heatResistanceRecovery *= Options.DemandingHellChaoticHeatResRecoveryEntry.Value;
                        HeatResistanceDrain = Options.DemandingHellChaoticHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.Brutal:
                        heatResistanceRecovery *= Options.DemandingHellBrutalHeatResRecoveryEntry.Value;
                        HeatResistanceDrain = Options.DemandingHellBrutalHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.Anarchic:
                        heatResistanceRecovery *= Options.DemandingHellAnarchicHeatResRecoveryEntry.Value;
                        HeatResistanceDrain = Options.DemandingHellAnarchicHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.Supreme:
                        heatResistanceRecovery *= Options.DemandingHellSupremeHeatResRecoveryEntry.Value;
                        HeatResistanceDrain = Options.DemandingHellSupremeHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.SSadistic:
                        heatResistanceRecovery *= Options.DemandingHellSSadisticHeatResRecoveryEntry.Value;
                        HeatResistanceDrain = Options.DemandingHellSSadisticHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.SSSensoredStorm:
                        heatResistanceRecovery *= Options.DemandingHellSSSensoredStormHeatResRecoveryEntry.Value;
                        HeatResistanceDrain = Options.DemandingHellSSSensoredStormHeatResDrainEntry.Value;
                        break;
                    case StyleRanks.ULTRAKILL:
                        heatResistanceRecovery *= Options.DemandingHellULTRAKILLHeatResRecoveryEntry.Value;
                        HeatResistanceDrain = Options.DemandingHellULTRAKILLHeatResDrainEntry.Value;
                        break;
                }

                if (OurHeatResistance.isActiveAndEnabled && HeatResistanceDrain <= 0.0f)
                {
                    OurHeatResistance.gameObject.SetActive(false);
                }
                else if (!OurHeatResistance.isActiveAndEnabled && HeatResistanceDrain > 0.0f)
                {
                    OurHeatResistance.gameObject.SetActive(true);
                    HeatResHurtTimer = 1.25f;

                    if (TimeSinceLastHeatResActivation > 5.0f)
                    {
                        Shud.AddPoints(75, "ON FIRE", null, null, -1, "", "");
                    }

                    TimeSinceLastHeatResActivation = 0.0f;
                }

                int heatResHurtDmg = 6;
                heatResHurtDmg = Mathf.RoundToInt((float)heatResHurtDmg * NyxMath.InverseNormalizeToRange(CurrentHeatResistance, 0.0f, 100.0f));
                if (HeatResistanceDrain > 0.0f && player.hp > 25 && CurrentHeatResistance < 25.0f && HeatResHurtTimer <= 0.0f)
                {
                    player.GetHurt(Math.Min(Math.Max((player.hp - (20 + heatResHurtDmg)), 0), heatResHurtDmg), false, 0.0f, false, false, 0.0f, false);
                    HeatResHurtTimer = 2.0f;
                }

                HeatResHurtTimer -= Time.fixedDeltaTime;

                FieldPublisher<HeatResistance, float> appliedHeatResistance = new FieldPublisher<HeatResistance, float>(OurHeatResistance, "heatResistance");
                FieldPublisher<HeatResistance, GameObject> hurtingSound = new FieldPublisher<HeatResistance, GameObject>(OurHeatResistance, "hurtingSound");
                
                if (CurrentHeatResistance <= 0.0f)
                {
                    CurrentHeatResistance = Mathf.MoveTowards(CurrentHeatResistance, -100.0f, (Time.fixedDeltaTime * (HeatResistanceDrain * 0.5f)));
                }
                else
                {
                    CurrentHeatResistance = Mathf.MoveTowards(CurrentHeatResistance, 0.0f, (Time.fixedDeltaTime * (HeatResistanceDrain)));
                }

                if (CurrentHeatResistance >= 0.1f || heatResistanceRecovery > HeatResistanceDrain)
                {
                    CurrentHeatResistance = Mathf.MoveTowards(CurrentHeatResistance, 100.0f, (Time.fixedDeltaTime * heatResistanceRecovery));
                }

                HeatResLabel.text = $"HEAT RESISTANCE - {CurrentHeatResistance:F1}%";

                appliedHeatResistance.Value = Mathf.Max(CurrentHeatResistance, 0.0f);

                if (CurrentHeatResistance < -95.0f)
                {
                    HeatResAntiHpCooldown += Time.fixedDeltaTime * 4.0f;
                    player.ForceAntiHP((float)12.5f * Time.fixedDeltaTime, silent: true, dontOverwriteHp: false, addToCooldown: true, stopInstaHeal: true);
                    hurtingSound.Value.GetComponent<AudioSource>().pitch = DefaultHurtingSoundPitch + 0.4f + UnityEngine.Random.Range(0.0f, 0.1f);
                    HeatResFlashingText.text = $"E{(char)UnityEngine.Random.Range(33, 96)}{(char)UnityEngine.Random.Range(33, 96)}{(char)UnityEngine.Random.Range(33, 96)}R{(char)UnityEngine.Random.Range(33, 96)}{(char)UnityEngine.Random.Range(33, 96)}R{(char)UnityEngine.Random.Range(33, 96)}{(char)UnityEngine.Random.Range(33, 96)}O{(char)UnityEngine.Random.Range(33, 96)}{(char)UnityEngine.Random.Range(33, 96)}R{(char)UnityEngine.Random.Range(33, 96)}";
                    HeatResRankDescensionTimer += Time.fixedDeltaTime * -2.0f;
                }
                else if (CurrentHeatResistance < -50.0f)
                {
                    HeatResAntiHpCooldown += Time.fixedDeltaTime * 2.5f;
                    player.ForceAntiHP((float)5f * Time.fixedDeltaTime, silent: true, dontOverwriteHp: false, addToCooldown: true, stopInstaHeal: true);
                    hurtingSound.Value.GetComponent<AudioSource>().pitch = DefaultHurtingSoundPitch + 0.1f;
                    HeatResFlashingText.text = "CRITICAL";
                    HeatResRankDescensionTimer += Time.fixedDeltaTime * -0.75f;
                }
                else if (CurrentHeatResistance <= 0.0f)
                {
                    HeatResAntiHpCooldown += Time.fixedDeltaTime * 1.0f;
                    hurtingSound.Value.GetComponent<AudioSource>().pitch = DefaultHurtingSoundPitch;
                    HeatResFlashingText.text = "WARNING:";
                    HeatResRankDescensionTimer += Time.fixedDeltaTime * 0.75f;
                }
                else
                {
                    hurtingSound.Value.GetComponent<AudioSource>().pitch = DefaultHurtingSoundPitch;
                    HeatResFlashingText.text = "WARNING:";
                    HeatResRankDescensionTimer += Time.fixedDeltaTime * 1.5f;
                }

                HeatResRankDescensionTimer = Mathf.Min(HeatResRankDescensionTimer, HeatResRankDescensionTimerMax);

                if (HeatResRankDescensionTimer <= 0.0f)
                {
                    Shud.DescendRank();
                    ResetHeatRestRankDescensionTimer();
                }
            }
        }
        
        protected void LateUpdate()
        {
            if (!Cheats.IsCheatEnabled(Cheats.DemandingHell))
            {
                return;
            }

            if (OurHeatResistance != null)
            {
                //FieldPublisher<HeatResistance, float> heatResistance = new FieldPublisher<HeatResistance, float>(OurHeatResistance, "heatResistance");
                
                //if (HeatResAntiHpCooldown > 0.0f && heatResistance.Value >= 0.0f || !OurHeatResistance.isActiveAndEnabled)
                //{
                    //AntiHpCooldown.Value = Mathf.Max(1.0f, Mathf.Min(HeatResAntiHpCooldown, 15.0f));
                    //HeatResAntiHpCooldown = -1.0f;
                //}
            }
        }

        private void PlayerPostHurt(NewMovement player, int damage, bool invincible, float scoreLossMultiplier, bool explosion, bool instablack, float hardDamageMultiplier, bool ignoreInvincibility)
        {
            if (!Cheats.IsCheatEnabled(Cheats.DemandingHell))
            {
                return;
            }

            if (damage <= 8)
            {
                return;
            }

            if (OurHeatResistance != null && !explosion && scoreLossMultiplier > 0.0f)
            {
                if (CurrentHeatResistance <= -10.0f)
                {
                    HeatResExplosion(damage * 0.25f, player.rb.transform.position, true, out float explosiveSize);
                    player.GetHurt(Mathf.RoundToInt(damage * 0.35f), false, 0.0f, true, false, 0.35f, true);
                    player.Launch(Vector3.up, explosiveSize * 2.5f, true);
                }
            }
        }

        GameObject OurHeatResistanceRootGo = null;
        HeatResistance OurHeatResistance = null;

        public TextMeshProUGUI OurHeatResistanceFlavourText { get; private set; }
        public TextMeshProUGUI HeatResLabel { get; private set; }
        public TextMeshProUGUI HeatResFlashingText { get; private set; }
        public float DefaultHurtingSoundPitch { get; private set; }
    }
}