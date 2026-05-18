using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public abstract class EnemyType
    {
        public string ReadableName { get; }
        public string Name { get; }
        public string UniqueName { get; }
        public global::EnemyType? VanillaEnumValue { get; }
        public bool IsVanilla { get; }

        public override bool Equals(object obj)
        {
            if (obj is global::EnemyType)
            {
                var vanillaEnemyType = (global::EnemyType)obj;

                if (VanillaEnumValue == vanillaEnemyType)
                {
                    return true;
                }
            }
            
            return base.Equals (obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class EnemyTypeDB : MonoSingleton<EnemyTypeDB>
    {
        public void RegisterType(EnemyType enemyType)
        {
            // TODO: error handling for unique name conflicts (really just give a more straight forward error)
            dictTypes.Add(enemyType.UniqueName, enemyType);
            hashTypes.Add(enemyType);
            types = hashTypes.ToArray();
            if (enemyType.VanillaEnumValue != null && !vanillaTypeDict.ContainsKey(enemyType.VanillaEnumValue.Value))
            {
                vanillaTypeDict.Add(enemyType.VanillaEnumValue.Value, enemyType);
            }
        }

        public IReadOnlyList<EnemyType> GetValues()
        {
            return types;
        }

        public EnemyType GetVanillaType(global::EnemyType vanillaEnumType)
        {
            return vanillaTypeDict[vanillaEnumType];
        }

        protected void OnEnable()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }

        private EnemyType[] types = new EnemyType[0];
        private HashSet<EnemyType> hashTypes = new HashSet<EnemyType>();
        private Dictionary<string, EnemyType> dictTypes = new Dictionary<string, EnemyType>();
        private Dictionary<global::EnemyType, EnemyType> vanillaTypeDict = new Dictionary<global::EnemyType, EnemyType>();
    }

    namespace EnemyTypes
    {
        public class VanillaEnemyType : EnemyType
        {
            public VanillaEnemyType(string friendlyName, string name)
            {
                ReadableName = friendlyName;
                Name = name;
                UniqueName = $"nyxlib.vanilla-ultrakill.{name}";
            }

            public VanillaEnemyType(string friendlyName, string name, global::EnemyType vanillaEnumValue)
            {
                ReadableName = friendlyName;
                Name = name;
                UniqueName = $"nyxlib.vanilla-ultrakill.{name}";
                VanillaEnumValue = vanillaEnumValue;
            }

            public VanillaEnemyType(string friendlyName, string name, string uniqueName, global::EnemyType vanillaEnumValue)
            {
                ReadableName = friendlyName;
                Name = name;
                UniqueName = uniqueName;
                VanillaEnumValue = vanillaEnumValue;
            }

            public string ReadableName { get; private set; } = "???";
            public string Name { get; private set; } = "???";
            public string UniqueName { get; private set; } = "???";

            public bool IsVanilla => true;
            public global::EnemyType? VanillaEnumValue { get; private set; } = null;

            internal static void Initialize()
            {
                var etdb = EnemyTypeDB.Instance;

                foreach (var valueGeneric in Enum.GetValues(typeof(global::EnemyType)))
                {
                    var enemyType = (global::EnemyType)valueGeneric;
                    EnemyType enemyTypeObj = null;

                    switch (enemyType)
                    {
                        case global::EnemyType.BigJohnator:
                            enemyTypeObj = new VanillaEnemyType("Big Johninator", "big-johnator", global::EnemyType.BigJohnator);
                            break;
                        case global::EnemyType.CancerousRodent:
                            enemyTypeObj = new VanillaEnemyType("Cancerous Rodent", "cancerous-rodent", global::EnemyType.CancerousRodent);
                            break;
                        case global::EnemyType.Centaur:
                            enemyTypeObj = new VanillaEnemyType("Earth Mover", "centaur", global::EnemyType.Centaur);
                            break;
                        case global::EnemyType.Cerberus:
                            enemyTypeObj = new VanillaEnemyType("Cerberus", "cerberus", global::EnemyType.Cerberus);
                            break;
                        case global::EnemyType.Deathcatcher:
                            enemyTypeObj = new VanillaEnemyType("Deathcatcher", "deathcatcher", global::EnemyType.Deathcatcher);
                            break;
                        case global::EnemyType.Drone:
                            enemyTypeObj = new VanillaEnemyType("Drone", "drone", global::EnemyType.Drone);
                            break;
                        case global::EnemyType.Ferryman:
                            enemyTypeObj = new VanillaEnemyType("Ferryman", "ferryman", global::EnemyType.Ferryman);
                            break;
                        case global::EnemyType.Filth:
                            enemyTypeObj = new VanillaEnemyType("Filth", "filth", global::EnemyType.Filth);
                            break;
                        case global::EnemyType.FleshPanopticon:
                            enemyTypeObj = new VanillaEnemyType("Flesh Panopticon", "flesh-panopticon", global::EnemyType.FleshPanopticon);
                            break;
                        case global::EnemyType.FleshPrison:
                            enemyTypeObj = new VanillaEnemyType("Flesh Prison", "flesh-prison", global::EnemyType.FleshPrison);
                            break;
                        case global::EnemyType.Gabriel:
                            enemyTypeObj = new VanillaEnemyType("Gabrie - Judge of Hell", "gabriel", global::EnemyType.Gabriel);
                            break;
                        case global::EnemyType.GabrielSecond:
                            enemyTypeObj = new VanillaEnemyType("Gabriel - Apostate of Hate", "gabriel-second", global::EnemyType.GabrielSecond);
                            break;
                        case global::EnemyType.Geryon:
                            enemyTypeObj = new VanillaEnemyType("Geryon", "geryon", global::EnemyType.Geryon);
                            break;
                        case global::EnemyType.Gutterman:
                            enemyTypeObj = new VanillaEnemyType("Gutterman", "gutterman", global::EnemyType.Gutterman);
                            break;
                        case global::EnemyType.Guttertank:
                            enemyTypeObj = new VanillaEnemyType("Guttertank", "guttertank", global::EnemyType.Guttertank);
                            break;
                        case global::EnemyType.HideousMass:
                            enemyTypeObj = new VanillaEnemyType("Hideous Mass", "hideous-mass", global::EnemyType.HideousMass);
                            break;
                        case global::EnemyType.Idol:
                            enemyTypeObj = new VanillaEnemyType("Idol", "idol", global::EnemyType.Idol);
                            break;
                        case global::EnemyType.Leviathan:
                            enemyTypeObj = new VanillaEnemyType("Leviathan", "leviathan", global::EnemyType.Leviathan);
                            break;
                        case global::EnemyType.MaliciousFace:
                            enemyTypeObj = new VanillaEnemyType("Malicious Face", "malicious-face", global::EnemyType.MaliciousFace);
                            break;
                        case global::EnemyType.Mandalore:
                            enemyTypeObj = new VanillaEnemyType("Mandalore", "mandalore", global::EnemyType.Mandalore);
                            break;
                        case global::EnemyType.Mannequin:
                            enemyTypeObj = new VanillaEnemyType("Mannequin", "mannequin", global::EnemyType.Mannequin);
                            break;
                        case global::EnemyType.Mindflayer:
                            enemyTypeObj = new VanillaEnemyType("Mindflayer", "mindflayer", global::EnemyType.Mindflayer);
                            break;
                        case global::EnemyType.Minos:
                            enemyTypeObj = new VanillaEnemyType("Corpse of King Minos", "minos", global::EnemyType.Minos);
                            break;
                        case global::EnemyType.MinosPrime:
                            enemyTypeObj = new VanillaEnemyType("Minos Prime", "minos-prime", global::EnemyType.MinosPrime);
                            break;
                        case global::EnemyType.Minotaur:
                            enemyTypeObj = new VanillaEnemyType("Minotaur", "minotaur", global::EnemyType.Minotaur);
                            break;
                        case global::EnemyType.MirrorReaper:
                            enemyTypeObj = new VanillaEnemyType("Mirror Reaper", "mirror-reaper", global::EnemyType.MirrorReaper);
                            break;
                        case global::EnemyType.Power:
                            enemyTypeObj = new VanillaEnemyType("Power", "power", global::EnemyType.Power);
                            break;
                        case global::EnemyType.Providence:
                            enemyTypeObj = new VanillaEnemyType("Providence", "providence", global::EnemyType.Providence);
                            break;
                        case global::EnemyType.Puppet:
                            enemyTypeObj = new VanillaEnemyType("Puppet", "puppet", global::EnemyType.Puppet);
                            break;
                        case global::EnemyType.Schism:
                            enemyTypeObj = new VanillaEnemyType("Schism", "schism", global::EnemyType.Schism);
                            break;
                        case global::EnemyType.Sisyphus:
                            enemyTypeObj = new VanillaEnemyType("Sisyphus", "sisyphus", global::EnemyType.Sisyphus);
                            break;
                        case global::EnemyType.SisyphusPrime:
                            enemyTypeObj = new VanillaEnemyType("Sisyphus Prime", "sisyphus-prime", global::EnemyType.SisyphusPrime);
                            break;
                        case global::EnemyType.Soldier:
                            enemyTypeObj = new VanillaEnemyType("Soldier", "soldier", global::EnemyType.Soldier);
                            break;
                        case global::EnemyType.Stalker:
                            enemyTypeObj = new VanillaEnemyType("Stalker", "stalker", global::EnemyType.Stalker);
                            break;
                        case global::EnemyType.Stray:
                            enemyTypeObj = new VanillaEnemyType("Stray", "stray", global::EnemyType.Stray);
                            break;
                        case global::EnemyType.Streetcleaner:
                            enemyTypeObj = new VanillaEnemyType("Street Cleaner", "streetcleaner", global::EnemyType.Streetcleaner);
                            break;
                        case global::EnemyType.Swordsmachine:
                            enemyTypeObj = new VanillaEnemyType("Swords Machine", "swordsmachine", global::EnemyType.Swordsmachine);
                            break;
                        case global::EnemyType.Turret:
                            enemyTypeObj = new VanillaEnemyType("Turret", "turret", global::EnemyType.Turret);
                            break;
                        case global::EnemyType.V2:
                            enemyTypeObj = new VanillaEnemyType("V2", "v2", global::EnemyType.V2);
                            break;
                        case global::EnemyType.V2Second:
                            enemyTypeObj = new VanillaEnemyType("V2... 2!", "v2-second", global::EnemyType.V2Second);
                            break;
                        case global::EnemyType.VeryCancerousRodent:
                            enemyTypeObj = new VanillaEnemyType("Very Cancerous Rodent", "very-cancerous-rodent", global::EnemyType.VeryCancerousRodent);
                            break;
                        case global::EnemyType.Virtue:
                            enemyTypeObj = new VanillaEnemyType("Virtue", "virtue", global::EnemyType.Virtue);
                            break;
                        case global::EnemyType.Wicked:
                            enemyTypeObj = new VanillaEnemyType("Something Wicked", "wicked", global::EnemyType.Wicked);
                            break;
                    }

                    etdb.RegisterType(enemyTypeObj);
                }
            }
        }
    }
}