using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

namespace UKAIW
{
    public class ProjectileBoostTracker : MonoBehaviour
    {
        public enum ProjectileCategory : byte
        {
            Null, RevolverShot, ChargedRevolverShot, BigRevolverShot, BigChargedRevolverShot, HellProjectile, HomingProjectile, Rocket, Grenade, EnemyRocket, EnemyGrenade
        }

        public bool LastBoostedByPlayer = false;
        public uint NumPlayerBoosts { get; private set; } = 0;
        public uint NumEnemyBoosts { get; private set; } = 0;
        public ProjectileCategory ProjectileType { get; private set; } = ProjectileCategory.Null;
        public IReadOnlyList<Collider> IgnoreColliders
        {
            get => _ignoreColliders;
            set
            {
                foreach (var otherCol in _ignoreColliders)
                {
                    if (otherCol == null)
                    {
                        continue;
                    }

                    foreach (var col in _colliders)
                    {
                        Physics.IgnoreCollision(otherCol, col, false);
                    }
                }
                
                _ignoreColliders = value.ToArray();

                foreach (var otherCol in _ignoreColliders)
                {
                    if (otherCol == null)
                    {
                        continue;
                    }

                    foreach (var col in _colliders)
                    {
                        Physics.IgnoreCollision(otherCol, col, true);
                    }
                }
            }
        }

        public void IncrementPlayerBoosts()
        {
            if (LastBoostedByPlayer && NumPlayerBoosts > 0)
            {
                return;
            }

            NumPlayerBoosts += 1;
            LastBoostedByPlayer = true;
            _creationProgressParryabilityDist = ParryabilityTracker.NotifyCreationProgress(GetHashCode());
            _creationProgressTime.UpdateToNow();
        }
        
        public void IncrementEnemyBoost()
        {
            if (!LastBoostedByPlayer && NumEnemyBoosts > 0)
            {
                return;
            }

            NumEnemyBoosts += 1;
            LastBoostedByPlayer = false;
            _creationProgressParryabilityDist = ParryabilityTracker.NotifyCreationProgress(GetHashCode());
            _creationProgressTime.UpdateToNow();
        }

        protected void Awake()
        {
            _colliders = GetComponentsInChildren<Collider>();

            if (ProjectileType == ProjectileCategory.Null)
            {
                _creationStartTime.UpdateToNow();
                _creationProgressTime.UpdateToNow();
            }

            // revolver beam changes this
            /*try 
            {
                Assert.IsNotNull(_colliders, "Projectile without a collider? Didn't know it existed!");
                Assert.IsFalse(_colliders.Length == 0, "Projectile without a collider? Didn't know it existed!");
            }
            catch (System.Exception e)
            {
                Log.Error($"Soft Error in ProjectileBoostTracker.Awake: {e}"); // don't want to block projectile awake and stuff if this fails
            } */
        }

        protected void Start()
        {
            if (ProjectileType == ProjectileCategory.Null)
            {
                if (TryGetComponent(out Grenade grenade))
                {
                    if (grenade.enemy)
                    {
                        if (grenade.rocket)
                        {
                            ProjectileType = ProjectileCategory.EnemyRocket;
                        }
                        else
                        {
                            ProjectileType = ProjectileCategory.EnemyGrenade;                    
                        }
                    }
                    else
                    {
                        if (grenade.rocket)
                        {
                            ProjectileType = ProjectileCategory.Rocket;
                        }
                        else
                        {
                            ProjectileType = ProjectileCategory.Grenade;                    
                        }
                    }
                }
                else if (TryGetComponent(out Projectile proj))
                {
                    if (proj.homingType == HomingType.None)
                    {
                        ProjectileType = ProjectileCategory.HellProjectile;                    
                    }
                    else
                    {
                        ProjectileType = ProjectileCategory.HomingProjectile;
                    }
                }
                else if (TryGetComponent(out RevolverBeam revolverBeam))
                {
                    ProjectileType = ProjectileCategory.RevolverShot;                    
                }

                _startParryabilityDist = ParryabilityTracker.NotifyCreationStart(GetHashCode());
                _creationStartTime.UpdateToNow();
            }

            _creationProgressTime.UpdateToNow();
        }

        protected void FixedUpdate()
        {
            
        }

        public double NotifyContact()
        {
            var contactDiffDist = ParryabilityTracker.NotifyContact(GetHashCode());

            double window = Math.Max(Math.Max(0.4 + (_creationStartTime.TimeSince * 0.25), 0.3 + (_creationProgressTime.TimeSince * 0.5)), 0.75);

            var diffDist = Math.Min(Math.Min(contactDiffDist, _startParryabilityDist), _creationProgressParryabilityDist);

            var parryability = Mathf.Clamp01(NyxMath.InverseNormalizeToRange((float)diffDist, (float)window / 2, (float)window));

            Log.TraceExpectedInfo($"ProjectileBoostTracker.NotifyContact called and is giving a window of {window}, a diffDist of {diffDist} and a contactDiffDist of {contactDiffDist}, resulting in a parryability of {parryability}");
            
            return parryability;
        }

        public override int GetHashCode()
        {
            byte playerBoostByte = ((byte)(Math.Min(NumPlayerBoosts, 15)));
            byte enemyBoostByte = (byte)Math.Min(NumEnemyBoosts, 15);

            if (BitConverter.IsLittleEndian)
            {
                enemyBoostByte <<= 4;
            }
            else
            {
                enemyBoostByte >>= 4;
            }

            byte boostByte = (byte)(playerBoostByte ^ enemyBoostByte);

            MelonLogger.Msg($"TEST PRINT FOR ProjectileBoostTracker.GetHashCode\nplayerBoostByte:{Convert.ToString(playerBoostByte, toBase: 2)}\nenemyBoostByte:{Convert.ToString(enemyBoostByte, toBase: 2)}\nboostByte:{Convert.ToString(boostByte, toBase: 2)}");
            return BitConverter.ToInt32(new byte[] { boostByte, (byte)ProjectileType, 0, 0}, 0);
        }

        public void CopyFrom(ProjectileBoostTracker other)
        {
            ProjectileType = other.ProjectileType;
            NumPlayerBoosts = other.NumPlayerBoosts;
            NumEnemyBoosts = other.NumEnemyBoosts;
            LastBoostedByPlayer = other.LastBoostedByPlayer;
            _startParryabilityDist = other._startParryabilityDist;
            _creationProgressTime = other._creationProgressTime;
            _creationStartTime = other._creationStartTime;
        }

        private double _startParryabilityDist = double.PositiveInfinity;
        private double _creationProgressParryabilityDist = double.PositiveInfinity;
        private FixedTimeStamp _creationStartTime = new FixedTimeStamp();
        private FixedTimeStamp _creationProgressTime = new FixedTimeStamp();
        private Collider[] _ignoreColliders = new Collider[0];
        private Collider[] _colliders = null;
    }
}