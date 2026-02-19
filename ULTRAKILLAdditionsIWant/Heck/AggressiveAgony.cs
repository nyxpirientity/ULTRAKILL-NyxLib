using System;
using System.Collections;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace UKAIW
{
    public class AggressiveAgony : MonoBehaviour
    {
        public Heck Heck { get; private set; } = null;
        public PainStore PainStore { get; private set; } = null;

        private GameObject PrefabHolder = null;

        private GameObject NoExplosionPrefab = null;
        private GameObject SmallExplosionPrefab = null;
        private GameObject ExplosionPrefab = null;

        public FixedTimeStamp MortarAttackTimestamp = new FixedTimeStamp();
        public FixedTimeStamp ULTRAMortarAttackTimestamp = new FixedTimeStamp();

        public FixedTimeStamp HomingAttackTimestamp = new FixedTimeStamp();
        public FixedTimeStamp ULTRAHomingAttackTimestamp = new FixedTimeStamp();

        public bool Enabled { get => Cheats.IsCheatEnabled(Cheats.AggressiveAgony); }
        public bool Disabled { get => !Enabled; }

        protected void Awake()
        {
            Heck = Heck.Instance;
            PainStore = Heck.Instance.PainStore;
        }

        protected void Start()
        {
            MortarAttackTimestamp.UpdateToNow();
            ULTRAMortarAttackTimestamp.UpdateToNow();

            HomingAttackTimestamp.UpdateToNow();
            ULTRAHomingAttackTimestamp.UpdateToNow();

            PrefabHolder = new GameObject("AggressiveAgonyPrefabHolder");
            PrefabHolder.transform.SetParent(transform);
            PrefabHolder.SetActive(false);

            NoExplosionPrefab = new GameObject("NoExplosion");
            NoExplosionPrefab.AddComponent<DestroyOnCheckpointRestart>();
            NoExplosionPrefab.transform.SetParent(PrefabHolder.transform);

            if (Assets.ExplosionPrefab == null)
            {
                return;
            }

            SmallExplosionPrefab = GameObject.Instantiate(Assets.ExplosionPrefab, PrefabHolder.transform);
            SmallExplosionPrefab.SetActive(true);
            var smallExplosion = SmallExplosionPrefab.GetComponent<ExplosionAdditions>();
            smallExplosion.ExplosionScale = 0.35f;
            smallExplosion.ExplosionSpeedScale = 0.35f;

            ExplosionPrefab = GameObject.Instantiate(Assets.ExplosionPrefab, PrefabHolder.transform);
            ExplosionPrefab.SetActive(true);
            var explosion = ExplosionPrefab.GetComponent<ExplosionAdditions>();
            explosion.ExplosionScale = 0.5f;
            explosion.ExplosionSpeedScale = 0.5f;
        }

        protected void Update()
        {
            
        }

        protected void FixedUpdate()
        {
            if (Disabled)
            {
                return;
            }
            
            float painCost = 0.0f;

            if (PainStore.Pain > 10.0f)
            {
                painCost += TryHomingAttack(PainStore.Pain - painCost);
            }

            if (PainStore.Pain > 20.0f)
            {
                painCost += TryMortarAttack(PainStore.Pain - painCost);
            }

            if (PainStore.Pain > 40.0f)
            {
                painCost += TryUltraHomingAttack(PainStore.Pain - painCost);
            }

            if (PainStore.Pain > 60.0f)
            {
                painCost += TryUltraMortarAttack(PainStore.Pain - painCost);
            }

            PainStore.AddPain(-painCost);
        }

        private float TryUltraMortarAttack(float remainingPain)
        {
            float waitTime = 24.0f / (1.0f + (PainStore.Pain / 100.0f));

            if (ULTRAMortarAttackTimestamp.TimeSince > waitTime)
            {
                ULTRAMortarAttackTimestamp.UpdateToNow();
                StartCoroutine(MortarAttack(true, 1.5f));
                return 22.0f;
            }

            return 0.0f;
        }

        private float TryUltraHomingAttack(float remainingPain)
        {
            float waitTime = 24.0f / (1.0f + (PainStore.Pain / 100.0f));

            if (ULTRAHomingAttackTimestamp.TimeSince > waitTime)
            {
                ULTRAHomingAttackTimestamp.UpdateToNow();
                StartCoroutine(HomingAttack(true, 8));
                return 18.0f;
            }

            return 0.0f;
        }

        private float TryMortarAttack(float remainingPain)
        {
            float waitTime = 14.0f / (1.0f + (PainStore.Pain / 100.0f));

            if (MortarAttackTimestamp.TimeSince > waitTime)
            {
                MortarAttackTimestamp.UpdateToNow();
                StartCoroutine(MortarAttack());
                return 10.0f;
            }

            return 0.0f;
        }

        private float TryHomingAttack(float remainingPain)
        {
            float waitTime = 14.0f / (1.0f + (PainStore.Pain / 100.0f));

            if (HomingAttackTimestamp.TimeSince > waitTime)
            {
                HomingAttackTimestamp.UpdateToNow();
                StartCoroutine(HomingAttack(false, 5));
                return 7.0f;
            }

            return 0.0f;
        }

        private IEnumerator HomingAttack(bool explosive = false, int numProjectiles = 1)
        {
            var player = NewMovement.Instance;
            List<Projectile> projectiles = new List<Projectile>(numProjectiles);
            Vector3 playerPos = player.transform.position;
            float originalExplosiveMaxSize = 0.0f;

            for (int i = 0; i < numProjectiles; i++)
            {
                Vector3 offset = (Vector3.forward + Vector3.right + (Vector3.up * 0.5f)).normalized * 10.0f;
                offset = Quaternion.Euler(0.0f, i * (360.0f / numProjectiles), 0.0f) * offset;

                GameObject projectileGo;

                if (explosive)
                {
                    projectileGo = GameObject.Instantiate(Assets.MortarPrefab, playerPos + offset, Quaternion.identity, null);
                    projectileGo.transform.localScale *= 0.35f;
                    projectileGo.GetComponent<Rigidbody>().useGravity = false;
                }
                else
                {
                    projectileGo = GameObject.Instantiate(Assets.HomingProjectilePrefab, playerPos + offset, Quaternion.identity, null);
                }

                var projectile = projectileGo.GetComponent<Projectile>();

                if (explosive)
                {
                    projectile.explosionEffect = NoExplosionPrefab;
                }

                projectile.homingType = HomingType.Instant;
                projectile.damage = 0;
                projectiles.Add(projectile);

                projectileGo.SetActive(true);

                yield return new UnityEngine.WaitForSeconds(0.5f / numProjectiles);
                
                if (projectile == null)
                {
                    continue;
                }

                projectile.GetComponent<AudioSource>().Stop();
            }

            yield return new UnityEngine.WaitForSeconds(0.2f);
            
            foreach (var projectile in projectiles)
            {
                if (projectile == null)
                {
                    continue;
                }
                
                projectile.homingType = HomingType.Instant;
                projectile.turningSpeedMultiplier = 50.0f; 
            }
            
            yield return new UnityEngine.WaitForSeconds(0.1f);

            foreach (var projectile in projectiles)
            {
                if (projectile == null)
                {
                    continue;
                }
                
                projectile.GetComponent<AudioSource>().Play();
                projectile.speed = 40.0f;
                projectile.turningSpeedMultiplier = 0.35f;
                projectile.damage = 35;
                
                if (explosive)
                {
                    projectile.explosionEffect = SmallExplosionPrefab;
                }
            }

            yield return new UnityEngine.WaitForSeconds(3.0f);

                        
            foreach (var projectile in projectiles)
            {
                if (projectile == null)
                {
                    continue;
                }

                projectile.Explode();
            }
        }

        private IEnumerator MortarAttack(bool bigExplosion = false, float numProjectilesMultiplier = 1.0f)
        {
            var player = NewMovement.Instance;
            int numProjectiles = (int)(4 * numProjectilesMultiplier);

            List<Projectile> projectiles = new List<Projectile>(numProjectiles);
            Vector3 playerPos = player.transform.position;
            
            for (int i = 0; i < numProjectiles; i++)
            {
                Vector3 offset = (Vector3.forward + Vector3.right).normalized * 6.0f;
                offset = Quaternion.Euler(0.0f, i * (360.0f / numProjectiles), 0.0f) * offset;

                var projectileGo = GameObject.Instantiate(Assets.MortarPrefab, (playerPos + offset) + Vector3.up * 2.0f, Quaternion.identity, null);
                projectileGo.transform.localScale *= 0.75f;
                var projectile = projectileGo.GetComponent<Projectile>();
                
                projectile.homingType = HomingType.HorizontalOnly;
                projectile.damage = 0;
                projectile.bigExplosion = bigExplosion;
                projectile.explosionEffect = NoExplosionPrefab;

                projectile.GetComponent<Rigidbody>().velocity = Vector3.up * 46.0f;
                projectiles.Add(projectile);

                projectileGo.SetActive(true);

                yield return new UnityEngine.WaitForSeconds(0.5f / numProjectiles);
            }

            yield return new UnityEngine.WaitForSeconds(0.4f);

            foreach (var projectile in projectiles)
            {
                if (projectile == null)
                {
                    continue;
                }
                
                projectile.GetComponent<AudioSource>().Play();
                projectile.damage = 25;
                projectile.explosionEffect = ExplosionPrefab;
            }
        }

        protected void OnDestroy()
        {
            
        }

        protected void OnEnable()
        {
            
        }
        
        protected void OnDisable()
        {
            
        }
    }
}