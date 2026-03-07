using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    [Serializable]
    public class HeckPuppetLeader : EnemyModifier
    {
        public class ManagedHeckPuppet
        {
            public HeckPuppet HeckPuppet { get; private set;} = null;
            public SceneTimeStamp DeathTimestamp = new SceneTimeStamp();
            public GameObject PuppetRootGo { get; private set; } = null;
            public EnemyComponents PuppetEad { get; private set; } = null;
            public EnemyIdentifier PuppetEid { get; private set; } = null;
            public ulong HeckPuppetID = 0;

            internal void Nullify()
            {
                HeckPuppet = null;
                PuppetRootGo = null;
                PuppetEad = null;
                PuppetEid = null;
            }

            internal bool IsAlive()
            {
                return !((HeckPuppet.NullInvalid()?.Eid.NullInvalid()?.Dead).GetValueOrDefault(true));
            }

            private static ulong NextHeckPuppetID = 0;
            internal void Spawn(Vector3 position, Quaternion rotation, HeckPuppetLeader leader, StyleRanks styleRank, Options.HeckPuppetStyleEntry.HeckPuppetOptions options)
            {   
                HeckPuppetID = NextHeckPuppetID;
                PuppetRootGo = leader.Enemy.PrefabStore.Instances.GetNewInstance();
                PuppetRootGo.transform.position = position + UnityEngine.Random.onUnitSphere * 2.5f;
                PuppetRootGo.transform.rotation = rotation;
                PuppetEad = PuppetRootGo.GetComponent<EnemyComponents>() ?? PuppetRootGo.GetComponentInChildren<EnemyComponents>();
                PuppetRootGo.name = $"{PuppetRootGo.name} (UKAIW.HeckPuppet)";
                Assert.IsNotNull(PuppetEad);
                Assert.IsNotNull(PuppetEad.gameObject);
                
                PuppetEad.gameObject.GetComponent<HeckPuppetLeader>().enabled = false;
                PuppetEad.MarkAsUniquelySolo();
                PuppetEid = PuppetEad.gameObject.GetComponent<EnemyIdentifier>();
                PuppetEid.spawnIn = false;
                PuppetRootGo.SetActive(true);
                PuppetEad.gameObject.SetActive(true);
                
                float speedBuff = options.HeckPuppetSpeedBuffScalar.Value;

                if (leader.Eid.enemyType == EnemyType.Virtue) // probably should be more configurable in the future but virtues seem quite disproportionately powerful.
                {
                    speedBuff *= 0.4f;
                }

                PuppetEad.Health = (Mathf.Min(options.MaxHeckPuppetHealth.Value, leader.Eid.Health * options.HeckPuppetHealthScalar.Value));
                var radianceMod = new EnemyRadiance.Modifier() 
                {
                    HealthEnabled = options.HeckPuppetHealthBuffScalar.Value >= 0.0f,
                    HealthMod = options.HeckPuppetHealthBuffScalar.Value,
                    DamageEnabled = options.HeckPuppetDamageBuffScalar.Value >= 0.0f,
                    DamageMod = options.HeckPuppetDamageBuffScalar.Value,
                    SpeedEnabled = speedBuff >= 0.0f,
                    SpeedMod = options.HeckPuppetSpeedBuffScalar.Value,
                    Multiplier = true,
                };

                PuppetEad.Radiance.AddModifier(radianceMod);
                
                HeckPuppet = PuppetEad.gameObject.AddComponent<HeckPuppet>();
                HeckPuppet.RadianceMod = radianceMod;
                HeckPuppet.Leader = leader;
                HeckPuppet.GameplayRank = leader.GameplayRank;
                HeckPuppet.StyleRank = styleRank;
                HeckPuppet.HeckPuppetID = NextHeckPuppetID;
                PuppetEid.dontCountAsKills = true;
                PuppetEid.timeSinceSpawned = 0.0f;
            }
        }

        public StyleHUD Shud = null;
        public EnemyComponents Enemy = null;
        public EnemyIdentifier Eid = null;

        public EnemyGameplayRank GameplayRank = EnemyGameplayRank.Ultraboss;

        internal Dictionary<StyleRanks, List<ManagedHeckPuppet>> Puppets = new System.Collections.Generic.Dictionary<StyleRanks, List<ManagedHeckPuppet>>();

        private bool _ExcludedFromHeckPuppetCheat = false;
        public bool ExcludedFromHeckPuppetCheat { get => _ExcludedFromHeckPuppetCheat || (Eid.NullInvalid()?.puppet).GetValueOrDefault(true) || Eid.NullInvalid()?.enemyType == EnemyType.Wicked; }

        protected void Awake()
        {
            Enemy = GetComponent<EnemyComponents>();
            Eid = Enemy.Eid;
        }

        protected void Start()
        {
            /* 
               Wicked excluded due to the earthmover being a wicked (???), Idol excluded because there's very little gameplay benefit and when instakilled they
               explode, which sucks, earthmover brain excluded because just in case, V2 and V2Second excluded because... go in sandbox and spawn a V2, make them
               puppeted, in my experience it breaks. Make them radiant and set their speed to 0.5 (which usually slows enemies down), watch as they
               suddenly gain super speed. Set it to 0 and watch them fling outside of the map, simply put, the game doesn't seem to work well enough in my
               experimentation.
               Sisyphus (Insurrectionists, not prime) excluded because they just seem.. broken for some reason. Needs more diagnosing OR this is just how it is
            */
            if (Eid.enemyType == EnemyType.Sisyphus || Eid.enemyType == EnemyType.Idol || Eid.gameObject.name.Contains("rain", StringComparison.OrdinalIgnoreCase) || Eid.gameObject.name.Contains("Mainframe", StringComparison.OrdinalIgnoreCase) || Eid.enemyType == EnemyType.Wicked || Eid.enemyType == EnemyType.V2 || Eid.enemyType == EnemyType.V2Second)
            {
                _ExcludedFromHeckPuppetCheat = true;
            }

            for (StyleRanks styleRank = 0; (int)styleRank < Style.NumStyleRanks; styleRank++)
            {
                var mhp = new List<ManagedHeckPuppet>();
                Puppets[styleRank] = mhp;
            }

            GameplayRank = EnemyUtils.GetEnemyGameplayRank(Eid);               

            Shud = StyleHUD.Instance;
        }

        protected void FixedUpdate()
        {
            if (Cheats.IsCheatDisabled(Cheats.HeckPuppets) || ExcludedFromHeckPuppetCheat)
            {
                return;
            }

            for (StyleRanks styleRank = 0; (int)styleRank < Style.NumStyleRanks; styleRank++)
            {
                List<ManagedHeckPuppet> puppets = Puppets[styleRank];
                
                if (styleRank > Shud.GetStyleRank())
                {
                    foreach (var puppet in puppets)
                    {
                        if (puppet.IsAlive())
                        {
                            puppet.HeckPuppet.PrevDead = true;
                            puppet.HeckPuppet.NullInvalid()?.InstaKill();
                            puppet.DeathTimestamp.TimeStamp = 0.0;
                        }
                    }

                    continue;
                }

                if (Eid.Dead)
                {
                    foreach (var puppet in puppets)
                    {
                        if (puppet.IsAlive())
                        {
                            puppet.HeckPuppet.PrevDead = true;
                            puppet.HeckPuppet?.InstaKill();
                            puppet.DeathTimestamp.UpdateToNow();
                        }
                    }

                    continue;
                }


                EnemyGameplayRank qualifierGameplayRank = GameplayRank;
                
                if (Eid.enemyType == EnemyType.Virtue)
                {
                    qualifierGameplayRank = EnemyGameplayRank.Boss;
                }

                var options = Options.HeckPuppetsStyleEntries[styleRank].HeckPuppetsOptions[qualifierGameplayRank];
                int intendedNumPuppets = options.NumHeckPuppets.Value;
                
                if (puppets.Count > intendedNumPuppets)
                {
                    for (int i = intendedNumPuppets; i < puppets.Count; i++)
                    {
                        if (puppets[i].IsAlive())
                        {
                            var puppet = puppets[i];
                            puppet.HeckPuppet.PrevDead = true;
                            puppet.HeckPuppet.InstaKill();
                            puppet.DeathTimestamp.UpdateToNow();
                        }
                    }
                    
                    puppets.RemoveRange(intendedNumPuppets, puppets.Count - intendedNumPuppets);
                }
                else if (puppets.Count < intendedNumPuppets)
                {
                    for (int i = puppets.Count; i < intendedNumPuppets; i++)
                    {
                        ManagedHeckPuppet managedHeckPuppet = new ManagedHeckPuppet();
                        managedHeckPuppet.DeathTimestamp.TimeStamp = 0.0;
                        puppets.Add(managedHeckPuppet);
                    }
                }

                foreach (var puppet in puppets)
                {
                    if (puppet.IsAlive())
                    {
                        continue;
                    }
                    
                    if (puppet.DeathTimestamp.TimeSince < 7.0)
                    {
                        continue;
                    }

                    bool isMalFace = Eid.enemyType == EnemyType.MaliciousFace;
                    puppet.Nullify();
                    puppet.Spawn(isMalFace ? transform.parent.position : transform.position, isMalFace ? transform.parent.rotation : transform.rotation, this, styleRank, options);
                }
            }
        }

        internal void NotifyPuppetDeath(HeckPuppet puppet, ulong heckPuppetID)
        {
            var puppets = Puppets[puppet.StyleRank];
            ManagedHeckPuppet mHeckPuppet = puppets.Find((a) => heckPuppetID == a.HeckPuppetID ? true : false );
            mHeckPuppet?.DeathTimestamp.UpdateToNow();
            mHeckPuppet?.Nullify();
        }

        protected void OnDestroy()
        {
            foreach (var styleRankPuppets in Puppets.Values)
            {
                foreach (var puppet in styleRankPuppets)
                {
                    if (puppet?.HeckPuppet == null)
                    {
                        continue;
                    }

                    if (puppet.HeckPuppet.Eid.Dead)
                    {
                        continue;
                    }

                    puppet.HeckPuppet.GivePoints = false;
                    GameObject.Destroy(puppet.HeckPuppet.GetComponent<EnemyComponents>().RootGameObject);
                }
            }
        }
    }
}