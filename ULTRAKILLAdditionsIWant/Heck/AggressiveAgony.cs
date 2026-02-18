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
            float waitTime = 12.0f;

            if (ULTRAMortarAttackTimestamp.TimeSince > waitTime)
            {
                ULTRAMortarAttackTimestamp.UpdateToNow();
                return 1.0f;
            }

            return 0.0f;
        }

        private float TryUltraHomingAttack(float remainingPain)
        {
            float waitTime = 10.0f;

            if (ULTRAHomingAttackTimestamp.TimeSince > waitTime)
            {
                ULTRAHomingAttackTimestamp.UpdateToNow();
                return 1.0f;
            }

            return 0.0f;
        }

        private float TryMortarAttack(float remainingPain)
        {
            float waitTime = 6.0f;

            if (MortarAttackTimestamp.TimeSince > waitTime)
            {
                MortarAttackTimestamp.UpdateToNow();
                return 0.4f;
            }

            return 0.0f;
        }

        private float TryHomingAttack(float remainingPain)
        {
            float waitTime = 8.0f;

            if (HomingAttackTimestamp.TimeSince > waitTime)
            {
                HomingAttackTimestamp.UpdateToNow();
                StartCoroutine(HomingAttack());
                return 5.0f;
            }

            return 0.0f;
        }

        private IEnumerator HomingAttack()
        {
            var player = NewMovement.Instance;
            int numProjectiles = 6;
            List<Projectile> projectiles = new List<Projectile>(numProjectiles);
            Vector3 playerPos = player.transform.position;
            for (int i = 0; i < numProjectiles; i++)
            {
                Vector3 offset = (Vector3.forward + Vector3.right + Vector3.up).normalized * 16.0f;
                offset = Quaternion.Euler(0.0f, i * (360.0f / numProjectiles), 0.0f) * offset;

                var projectileGo = GameObject.Instantiate(Assets.HomingProjectilePrefab, playerPos+ offset, Quaternion.identity, null);
                var projectile = projectileGo.GetComponent<Projectile>();
                
                projectile.homingType = HomingType.None;
                projectiles.Add(projectile);

                projectileGo.SetActive(true);

                yield return new UnityEngine.WaitForSeconds(0.5f / numProjectiles);
                projectile.GetComponent<AudioSource>().Stop();
            }

            yield return new UnityEngine.WaitForSeconds(0.3f);
            
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
            }

            yield return new UnityEngine.WaitForSeconds(1.5f);

                        
            foreach (var projectile in projectiles)
            {
                if (projectile == null)
                {
                    continue;
                }

                projectile.Explode();
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