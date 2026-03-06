using UnityEngine;

namespace UKAIW
{
    public class EnemyFeedbacker : EnemyModifier
    {
        private EnemyAdditions _eadd = null;
        public bool Enabled { get => Cheats.IsCheatEnabled(Cheats.FeedbackerForAll); }
        public bool ReadyToParry 
        { 
            get
            {
                return LastParryTimestamp.TimeSince >= ParryCooldown && Stamina >= ParryCost && Enabled;
            } 
        }

        public float ParryCost { get => 0.36f; }
        public float ParryCooldown { get => 0.1f; }
        public FixedTimeStamp LastParryTimestamp;
        public float Stamina { get; private set; } = 0;

        protected void Awake()
        {
            _eadd = GetComponent<EnemyAdditions>();
            
            Assert.IsNotNull(_eadd);

            _eadd.PreHurt += PreHurt;
            _eadd.PostHurt += PostHurt;
        }

        private void PreHurt(GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
        {
            if (!Enabled)
            {
                return;
            }

        }

        private void PostHurt(GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
        {
            if (!Enabled)
            {
                return;
            }

        }

        public void ParryEffect()
        {
            Assert.IsTrue(ReadyToParry, "EnemyFeedbacker.ParryEffect called when not ReadyToParry?");

            if (Options.HitstopOnEnemyParry.Value)
            {
                TimeScale.Controller.ParryFlash();
            }
            else
            {
                TimeScale.ModDisableHitstop = true;
                TimeScale.Controller.ParryFlash();
                TimeScale.ModDisableHitstop = false;
            }

            Stamina -= ParryCost;

            LastParryTimestamp.UpdateToNow();
        }

        protected void Start()
        {
            
        }

        protected void Update()
        {
        }

        protected void FixedUpdate()
        {
            Stamina = Mathf.MoveTowards(Stamina, 1.0f, (Time.fixedDeltaTime / 1.6f) * 0.25f);
        }

        protected void OnDestroy()
        {
        }

        public Vector3 SolveParryForce(Vector3 projectilePosition, Vector3 projectileVelocity)
        {
            var v1 = NewMovement.Instance;

            var direction = ((v1.HeadPosition) - projectilePosition).normalized;

            return direction;
        }
    }
}