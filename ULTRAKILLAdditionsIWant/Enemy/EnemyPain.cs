using System;
using MelonLoader;
using UnityEngine;

namespace UKAIW
{
    public class EnemyPain : MonoBehaviour
    {
        /* 
        * by nature, the initial defaults I'm writing for this class as I write this comment are a little lore heavy and I don't really give a darn about ULTRAKILL lore at all so uhhhhhhhh
        * either way it'll probably eventually be modified to suit balance instead unless it ends up actually being balanced via my best understanding of the lore being applied
        */
        public EnemyAdditions Eadd { get; private set; } = null;

        public float ActivePhysicalPain = 0.0f;
        public float PhysicalSensitivity = 1.0f;

        public float ActiveMentalPain = 0.0f;
        public float ActiveHardMentalPain = 0.0f;
        public float MentalSensitivity = 1.0f;

        protected void Awake()
        {
            Eadd = GetComponent<EnemyAdditions>();
        }

        protected void Start()
        {
            SpeciesType = Eadd.Eid.GetSpeciesType();
            SpeciesRank = Eadd.Eid.GetSpeciesRank();
            
            Eadd.PreHurt += OnPreHurt;
            Eadd.PostDeath += PostDeath;

            Eadd.PreDeath += OnPreDeath;
        }

        private void OnPreDeath(bool instakill)
        {
            if (Eadd.Eid.enemyType == EnemyType.Mindflayer)
            {
                Heck.Instance.PainStore.AddPain(5.0f);
            }
        }

        protected void OnEnable()
        {
            TryListenForDeath();
        }

        protected void OnDisable()
        {
            TryStopListeningForDeath();
        }

        private bool ListeningForDeath = false;
        private EnemySpeciesType SpeciesType;
        private EnemySpeciesRank SpeciesRank;

        private void PostDeath(bool instakilled)
        {
            TryStopListeningForDeath();
        }

        private void TryListenForDeath()
        {
            if (ListeningForDeath)
            {
                return;
            }
            
            if ((Eadd.NullInvalid()?.Eid.NullInvalid()?.Dead).GetValueOrDefault(true))
            {
                return;
            }

            EnemyEvents.Death += OnAnyEnemyDeath;
            ListeningForDeath = true;
        }

        private void TryStopListeningForDeath()
        {
            if (!ListeningForDeath)
            {
                return;
            }

            EnemyEvents.Death -= OnAnyEnemyDeath;
            ListeningForDeath = false;
        }

        private void OnAnyEnemyDeath(EnemyIdentifier otherEid)
        {
            if (SpeciesType is EnemySpeciesType.OrganicMachine)
            {
                return;
            }

            var otherSpeciesType = otherEid.GetSpeciesType();
            var otherSpeciesRank = otherEid.GetSpeciesRank();

            float compassion = 0.0f;
            float concern = 0.0f;

            if (Eadd.Eid.enemyType is EnemyType.Filth || Eadd.Eid.enemyType is EnemyType.Stray || Eadd.Eid.enemyType is EnemyType.Schism || Eadd.Eid.enemyType is EnemyType.Soldier)
            {
                if (otherEid.enemyType is EnemyType.Filth || otherEid.enemyType is EnemyType.Stray || otherEid.enemyType is EnemyType.Schism || otherEid.enemyType is EnemyType.Soldier)
                {
                    compassion += 0.25f; // they don't attack each other even with enemies attack enemies on soooooo
                }   
            }

            float lesserConcernScale = 0.0f;
            float greaterConcernScale = 0.0f;
            float supremeConcernScale = 0.0f;
            float primeConcernScale = 0.0f;

            switch (SpeciesRank)
            {
                case EnemySpeciesRank.NotApplicable:
                    lesserConcernScale = 0.0f;
                    greaterConcernScale = 0.0f;
                    supremeConcernScale = 0.0f;
                    primeConcernScale = 0.0f;
                    break;
                case EnemySpeciesRank.Lesser:
                    lesserConcernScale = 0.25f;
                    greaterConcernScale = 0.35f;
                    supremeConcernScale = 0.5f;
                    primeConcernScale = 0.65f;
                    break;
                case EnemySpeciesRank.Greater:
                    lesserConcernScale = 0.05f;
                    greaterConcernScale = 0.5f;
                    supremeConcernScale = 0.75f;
                    primeConcernScale = 1.0f;
                    break;
                case EnemySpeciesRank.Supreme:
                    lesserConcernScale = 0.01f;
                    greaterConcernScale = 0.2f;
                    supremeConcernScale = 0.5f;
                    primeConcernScale = 0.75f;
                    break;
                case EnemySpeciesRank.Prime:
                    lesserConcernScale = 0.0f;
                    greaterConcernScale = 0.025f;
                    supremeConcernScale = 0.05f;
                    primeConcernScale = 0.25f;
                    break;
            }

            if (SpeciesType is EnemySpeciesType.Machine && otherSpeciesType is EnemySpeciesType.Machine)
            {
                switch (otherSpeciesRank)
                {
                    case EnemySpeciesRank.NotApplicable:
                        break;
                    case EnemySpeciesRank.Lesser:
                        concern += lesserConcernScale * 0.25f;
                        break;
                    case EnemySpeciesRank.Greater:
                        concern += greaterConcernScale * 0.5f;
                        break;
                    case EnemySpeciesRank.Supreme:
                        concern += supremeConcernScale;
                        break;
                    case EnemySpeciesRank.Prime:
                        concern += float.PositiveInfinity;
                        break;
                }
            }
            else
            {
                switch (otherSpeciesRank)
                {
                    case EnemySpeciesRank.NotApplicable:
                        break;
                    case EnemySpeciesRank.Lesser:
                        concern += lesserConcernScale;
                        break;
                    case EnemySpeciesRank.Greater:
                        concern += greaterConcernScale;
                        break;
                    case EnemySpeciesRank.Supreme:
                        concern += supremeConcernScale;
                        break;
                    case EnemySpeciesRank.Prime:
                        concern += primeConcernScale;
                        break;
                }
            }

            var mentalPain = (concern + compassion) * MentalSensitivity;
            MentalSensitivity = Mathf.Max(0.2f, MentalSensitivity - mentalPain);
            ActiveMentalPain += mentalPain;
            ActiveHardMentalPain += mentalPain * 0.2f;
        }

        protected void FixedUpdate()
        {
            ActivePhysicalPain = Mathf.MoveTowards(ActivePhysicalPain, -0.05f, Time.fixedDeltaTime * 0.15f);
            PhysicalSensitivity = Mathf.MoveTowards(PhysicalSensitivity, 1.0f, Time.fixedDeltaTime * 3.0f);

            ActiveMentalPain = Mathf.MoveTowards(ActiveMentalPain, ActiveHardMentalPain, Time.fixedDeltaTime * 0.25f);
            ActiveHardMentalPain = Mathf.MoveTowards(ActiveHardMentalPain, -0.05f, Time.fixedDeltaTime * 0.1f);
            MentalSensitivity = Mathf.MoveTowards(MentalSensitivity, 1.0f, Time.fixedDeltaTime * 3.0f);

            if (Eadd.Eid.Dead)
            {
                ActivePhysicalPain = 0.0f;
                ActiveMentalPain = 0.0f;
                return;
            }

            if (Eadd.Eid.zombie != null)
            {
                var fallSpeed = new FieldPublisher<Zombie, float>(Eadd.Eid.zombie, "fallSpeed");
                var fallTime = new FieldPublisher<Zombie, float>(Eadd.Eid.zombie, "fallTime");
                if (fallTime.Value > 0.05f && Eadd.Eid.zombie.GetComponent<Rigidbody>().velocity.y < fallSpeed.Value)
                {
                    ActiveMentalPain += Time.fixedDeltaTime;
                }
            }
            else if (Eadd.Eid.machine != null)
            {
                var fallSpeed = new FieldPublisher<Machine, float>(Eadd.Eid.machine, "fallSpeed");
                var fallTime = new FieldPublisher<Machine, float>(Eadd.Eid.machine, "fallTime");

                if (fallTime.Value > 0.05f && Eadd.Eid.machine.GetComponent<Rigidbody>().velocity.y < fallSpeed.Value)
                {
                    ActiveMentalPain += Time.fixedDeltaTime * 1.5f;
                }
            }
            
            ActivePhysicalPain = Mathf.Min(ActivePhysicalPain, 2.0f);
            ActiveMentalPain = Mathf.Min(ActiveMentalPain, 2.0f);
            ActiveHardMentalPain = Mathf.Min(ActiveHardMentalPain, 2.0f);

            switch (Eadd.Eid.enemyType)
            {
                // they've probably ascended beyond the concepts of pain, mild annoyance for them at the worst.
                case EnemyType.VeryCancerousRodent:
                case EnemyType.CancerousRodent:
                case EnemyType.Mandalore:
                    ActivePhysicalPain = -1.0f;
                    ActiveMentalPain = -1.0f;
                break;
                case EnemyType.SisyphusPrime:
                    ActiveMentalPain = 0.0f;
                    break;
                default:
                    break;
            }

            Heck.Instance.PainStore.AddPain((ActivePhysicalPain + ActiveMentalPain) * Time.fixedDeltaTime);
        }

        private void OnPreHurt(GameObject target, Vector3 force, Vector3? hitPoint, float multiplier, bool tryForExplode, float critMultiplier, GameObject sourceWeapon, bool ignoreTotalDamageTakenMultiplier, bool fromExplosion)
        {
            if (!ListeningForDeath)
            {
                return;
            }

            float pain = ((Mathf.Pow(multiplier + (multiplier * critMultiplier), 0.5f) * PhysicalSensitivity) / (Mathf.Pow(Eadd.StartingHealth, 0.5f))) * 2.0f;

            PhysicalSensitivity = Mathf.Max(0.2f, PhysicalSensitivity - (pain * 0.5f));

            ActivePhysicalPain += pain;
        }
    }
}