using System;
using System.Collections.Generic;
using MelonLoader;
using SettingsMenu.Components.Pages;
using TMPro;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

namespace UKAIW
{
    [Serializable]
    public class HeckPuppetLeader : MonoBehaviour
    {
        public class ManagedHeckPuppet
        {
            public HeckPuppet HeckPuppet { get; private set;} = null;
            public SceneTimeStamp DeathTimestamp = new SceneTimeStamp();
            public GameObject PuppetRootGo { get; private set; } = null;
            public EnemyAdditions PuppetEad { get; private set; } = null;
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
                return !((HeckPuppet?.Eid?.Dead).GetValueOrDefault(true));
            }

            private static ulong NextHeckPuppetID = 0;
            internal void Spawn(Vector3 position, Quaternion rotation, HeckPuppetLeader leader, StyleRanks styleRank, Options.HeckPuppetStyleEntry.HeckPuppetOptions options)
            {   
                HeckPuppetID = NextHeckPuppetID;
                PuppetRootGo = UnityEngine.Object.Instantiate(leader.Ead.PrefabMod.Prefab, leader.Ead.RootGameObject.transform.parent);
                PuppetRootGo.transform.position = position;
                PuppetRootGo.transform.rotation = rotation;
                PuppetEad = PuppetRootGo.GetComponent<EnemyAdditions>() ?? PuppetRootGo.GetComponentInChildren<EnemyAdditions>();
                
                Assert.IsNotNull(PuppetEad);
                Assert.IsNotNull(PuppetEad.gameObject);
            
                PuppetEad.gameObject.GetComponent<HeckPuppetLeader>().enabled = false;
                PuppetEad.MarkAsUniquelySolo();
                PuppetEid = PuppetEad.gameObject.GetComponent<EnemyIdentifier>();
                PuppetEid.spawnIn = false;
                PuppetRootGo.SetActive(true);
                PuppetEad.gameObject.SetActive(true);
                
                PuppetEad.Health = (Mathf.Min(options.MaxHeckPuppetHealth.Value, leader.Eid.Health * options.HeckPuppetHealthScalar.Value));
                PuppetEad.EnemyRadiance.AddModifier(new Radiance.Modifier() 
                {
                    HealthEnabled = options.HeckPuppetHealthBuffScalar.Value >= 0.0f,
                    HealthMod = options.HeckPuppetHealthBuffScalar.Value,
                    DamageEnabled = options.HeckPuppetDamageBuffScalar.Value >= 0.0f,
                    DamageMod = options.HeckPuppetDamageBuffScalar.Value,
                    SpeedEnabled = options.HeckPuppetSpeedBuffScalar.Value >= 0.0f,
                    SpeedMod = options.HeckPuppetSpeedBuffScalar.Value,
                    Multiplier = true,
                });
                
                HeckPuppet = PuppetEad.gameObject.AddComponent<HeckPuppet>();
                HeckPuppet.Leader = leader;
                HeckPuppet.GameplayRank = leader.GameplayRank;
                HeckPuppet.StyleRank = styleRank;
                HeckPuppet.HeckPuppetID = NextHeckPuppetID;
                PuppetEid.dontCountAsKills = true;
                PuppetEid.puppet = true;
                PuppetEid.timeSinceSpawned = 0.0f;
            }
        }

        public StyleHUD Shud = null;
        public EnemyAdditions Ead = null;
        public EnemyIdentifier Eid = null;

        public EnemyGameplayRank GameplayRank = EnemyGameplayRank.Ultraboss;

        internal Dictionary<StyleRanks, List<ManagedHeckPuppet>> Puppets = new System.Collections.Generic.Dictionary<StyleRanks, List<ManagedHeckPuppet>>();

        private bool _ExcludedFromHeckPuppetCheat = false;
        public bool ExcludedFromHeckPuppetCheat { get => _ExcludedFromHeckPuppetCheat || (Eid?.puppet).GetValueOrDefault(true); }

        protected void Awake()
        {
            Ead = GetComponent<EnemyAdditions>();
            Eid = Ead.Eid;
        }

        protected void Start()
        {
            if (Eid.enemyType == EnemyType.Idol)
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
                            puppet.HeckPuppet?.Eid.InstaKill();
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
                            puppet.HeckPuppet?.Eid.InstaKill();
                            puppet.DeathTimestamp.UpdateToNow();
                        }
                    }

                    continue;
                }

                var options = Options.HeckPuppetsStyleEntries[styleRank].HeckPuppetsOptions[GameplayRank];
                int intendedNumPuppets = options.NumHeckPuppets.Value;
                
                if (puppets.Count > intendedNumPuppets)
                {
                    for (int i = intendedNumPuppets; i < puppets.Count; i++)
                    {
                        if (puppets[i].IsAlive())
                        {
                            var puppet = puppets[i];
                            puppet.HeckPuppet.PrevDead = true;
                            puppet.HeckPuppet.Eid.InstaKill();
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
    }
}