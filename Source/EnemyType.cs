using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public interface IEnemyType
    {
        public string ReadableName { get; }
        public string Name { get; }
        public string UniqueName { get; }
        public EnemyType? VanillaEnumValue { get; }
        public bool IsVanilla { get; }
    }

    public class EnemyTypeDB : MonoSingleton<EnemyTypeDB>
    {
        public void RegisterType(IEnemyType enemyType)
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

        public IReadOnlyList<IEnemyType> GetValues()
        {
            return types;
        }

        public IEnemyType GetVanillaType(EnemyType vanillaEnumType)
        {
            return vanillaTypeDict[vanillaEnumType];
        }

        protected void OnEnable()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }

        private IEnemyType[] types = new IEnemyType[0];
        private HashSet<IEnemyType> hashTypes = new HashSet<IEnemyType>();
        private Dictionary<string, IEnemyType> dictTypes = new Dictionary<string, IEnemyType>();
        private Dictionary<EnemyType, IEnemyType> vanillaTypeDict = new Dictionary<EnemyType, IEnemyType>();
    }

    namespace EnemyTypes
    {
        public class VanillaEnemyType : IEnemyType
        {
            public VanillaEnemyType(string friendlyName, string name)
            {
                ReadableName = friendlyName;
                Name = name;
                UniqueName = $"nyxlib.vanilla-ultrakill.{name}";
            }

            public VanillaEnemyType(string friendlyName, string name, EnemyType vanillaEnumValue)
            {
                ReadableName = friendlyName;
                Name = name;
                UniqueName = $"nyxlib.vanilla-ultrakill.{name}";
                VanillaEnumValue = vanillaEnumValue;
            }

            public VanillaEnemyType(string friendlyName, string name, string uniqueName, EnemyType vanillaEnumValue)
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
            public EnemyType? VanillaEnumValue { get; private set; } = null;

            internal static void Initialize()
            {
                var etdb = EnemyTypeDB.Instance;

                foreach (var valueGeneric in Enum.GetValues(typeof(EnemyType)))
                {
                    var enemyType = (EnemyType)valueGeneric;
                    IEnemyType enemyTypeObj = null;

                    switch (enemyType)
                    {
                        case EnemyType.BigJohnator:
                            enemyTypeObj = new VanillaEnemyType("Big Johninator", "big-johnator", EnemyType.BigJohnator);
                            break;
                        case EnemyType.CancerousRodent:
                            enemyTypeObj = new VanillaEnemyType("Cancerous Rodent", "cancerous-rodent", EnemyType.CancerousRodent);
                            break;
                        case EnemyType.Centaur:
                            enemyTypeObj = new VanillaEnemyType("Earth Mover", "centaur", EnemyType.Centaur);
                            break;
                        case EnemyType.Cerberus:
                            enemyTypeObj = new VanillaEnemyType("Cerberus", "cerberus", EnemyType.Cerberus);
                            break;
                        case EnemyType.Deathcatcher:
                            enemyTypeObj = new VanillaEnemyType("Deathcatcher", "deathcatcher", EnemyType.Deathcatcher);
                            break;
                        case EnemyType.Drone:
                            enemyTypeObj = new VanillaEnemyType("Drone", "drone", EnemyType.Drone);
                            break;
                        case EnemyType.Ferryman:
                            enemyTypeObj = new VanillaEnemyType("Ferryman", "ferryman", EnemyType.Ferryman);
                            break;
                        case EnemyType.Filth:
                            enemyTypeObj = new VanillaEnemyType("Filth", "filth", EnemyType.Filth);
                            break;
                        case EnemyType.FleshPanopticon:
                            enemyTypeObj = new VanillaEnemyType("Flesh Panopticon", "flesh-panopticon", EnemyType.FleshPanopticon);
                            break;
                        case EnemyType.FleshPrison:
                            enemyTypeObj = new VanillaEnemyType("Flesh Prison", "flesh-prison", EnemyType.FleshPrison);
                            break;
                        case EnemyType.Gabriel:
                            enemyTypeObj = new VanillaEnemyType("Gabrie - Judge of Hell", "gabriel", EnemyType.Gabriel);
                            break;
                        case EnemyType.GabrielSecond:
                            enemyTypeObj = new VanillaEnemyType("Gabriel - Apostate of Hate", "gabriel-second", EnemyType.GabrielSecond);
                            break;
                        case EnemyType.Geryon:
                            enemyTypeObj = new VanillaEnemyType("Geryon", "geryon", EnemyType.Geryon);
                            break;
                        case EnemyType.Gutterman:
                            enemyTypeObj = new VanillaEnemyType("Gutterman", "gutterman", EnemyType.Gutterman);
                            break;
                        case EnemyType.Guttertank:
                            enemyTypeObj = new VanillaEnemyType("Guttertank", "guttertank", EnemyType.Guttertank);
                            break;
                        case EnemyType.HideousMass:
                            enemyTypeObj = new VanillaEnemyType("Hideous Mass", "hideous-mass", EnemyType.HideousMass);
                            break;
                        case EnemyType.Idol:
                            enemyTypeObj = new VanillaEnemyType("Idol", "idol", EnemyType.Idol);
                            break;
                        case EnemyType.Leviathan:
                            enemyTypeObj = new VanillaEnemyType("Leviathan", "leviathan", EnemyType.Leviathan);
                            break;
                        case EnemyType.MaliciousFace:
                            enemyTypeObj = new VanillaEnemyType("Malicious Face", "malicious-face", EnemyType.MaliciousFace);
                            break;
                        case EnemyType.Mandalore:
                            enemyTypeObj = new VanillaEnemyType("Mandalore", "mandalore", EnemyType.Mandalore);
                            break;
                        case EnemyType.Mannequin:
                            enemyTypeObj = new VanillaEnemyType("Mannequin", "mannequin", EnemyType.Mannequin);
                            break;
                        case EnemyType.Mindflayer:
                            enemyTypeObj = new VanillaEnemyType("Mindflayer", "mindflayer", EnemyType.Mindflayer);
                            break;
                        case EnemyType.Minos:
                            enemyTypeObj = new VanillaEnemyType("Corpse of King Minos", "minos", EnemyType.Minos);
                            break;
                        case EnemyType.MinosPrime:
                            enemyTypeObj = new VanillaEnemyType("Minos Prime", "minos-prime", EnemyType.MinosPrime);
                            break;
                        case EnemyType.Minotaur:
                            enemyTypeObj = new VanillaEnemyType("Minotaur", "minotaur", EnemyType.Minotaur);
                            break;
                        case EnemyType.MirrorReaper:
                            enemyTypeObj = new VanillaEnemyType("Mirror Reaper", "mirror-reaper", EnemyType.MirrorReaper);
                            break;
                        case EnemyType.Power:
                            enemyTypeObj = new VanillaEnemyType("Power", "power", EnemyType.Power);
                            break;
                        case EnemyType.Providence:
                            enemyTypeObj = new VanillaEnemyType("Providence", "providence", EnemyType.Providence);
                            break;
                        case EnemyType.Puppet:
                            enemyTypeObj = new VanillaEnemyType("Puppet", "puppet", EnemyType.Puppet);
                            break;
                        case EnemyType.Schism:
                            enemyTypeObj = new VanillaEnemyType("Schism", "schism", EnemyType.Schism);
                            break;
                        case EnemyType.Sisyphus:
                            enemyTypeObj = new VanillaEnemyType("Sisyphus", "sisyphus", EnemyType.Sisyphus);
                            break;
                        case EnemyType.SisyphusPrime:
                            enemyTypeObj = new VanillaEnemyType("Sisyphus Prime", "sisyphus-prime", EnemyType.SisyphusPrime);
                            break;
                        case EnemyType.Soldier:
                            enemyTypeObj = new VanillaEnemyType("Soldier", "soldier", EnemyType.Soldier);
                            break;
                        case EnemyType.Stalker:
                            enemyTypeObj = new VanillaEnemyType("Stalker", "stalker", EnemyType.Stalker);
                            break;
                        case EnemyType.Stray:
                            enemyTypeObj = new VanillaEnemyType("Stray", "stray", EnemyType.Stray);
                            break;
                        case EnemyType.Streetcleaner:
                            enemyTypeObj = new VanillaEnemyType("Street Cleaner", "streetcleaner", EnemyType.Streetcleaner);
                            break;
                        case EnemyType.Swordsmachine:
                            enemyTypeObj = new VanillaEnemyType("Swords Machine", "swordsmachine", EnemyType.Swordsmachine);
                            break;
                        case EnemyType.Turret:
                            enemyTypeObj = new VanillaEnemyType("Turret", "turret", EnemyType.Turret);
                            break;
                        case EnemyType.V2:
                            enemyTypeObj = new VanillaEnemyType("V2", "v2", EnemyType.V2);
                            break;
                        case EnemyType.V2Second:
                            enemyTypeObj = new VanillaEnemyType("V2... 2!", "v2-second", EnemyType.V2Second);
                            break;
                        case EnemyType.VeryCancerousRodent:
                            enemyTypeObj = new VanillaEnemyType("Very Cancerous Rodent", "very-cancerous-rodent", EnemyType.VeryCancerousRodent);
                            break;
                        case EnemyType.Virtue:
                            enemyTypeObj = new VanillaEnemyType("Virtue", "virtue", EnemyType.Virtue);
                            break;
                        case EnemyType.Wicked:
                            enemyTypeObj = new VanillaEnemyType("Something Wicked", "wicked", EnemyType.Wicked);
                            break;
                    }

                    etdb.RegisterType(enemyTypeObj);
                }
            }
        }
    }
}