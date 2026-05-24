using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    // I know prefixing abstract classes is not really a thing, but EnemyType is taken up in the global namespace unfortunately by ULTRAKILL
    public abstract class AEnemyType
    {
        public abstract string ReadableName { get; }
        public abstract string Name { get; }
        public abstract string UniqueName { get; }
        public abstract global::EnemyType? VanillaEnumValue { get; }
        public abstract bool IsVanilla { get; }

        public override string ToString()
        {
            return Name;
        }

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

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    namespace EnemyTypes
    {
        public class VanillaEnemyType : AEnemyType
        {
            public VanillaEnemyType(string friendlyName, string name)
            {
                readableName = friendlyName;
                this.name = name;
                uniqueName = $"nyxlib.vanilla-ultrakill.{name}";
            }

            public VanillaEnemyType(string friendlyName, string name, global::EnemyType vanillaEnumValue)
            {
                readableName = friendlyName;
                this.name = name;
                uniqueName = $"NyxLib.Vanilla.ULTRAKILL.{name}";
                this.vanillaEnumValue = vanillaEnumValue;
            }

            public VanillaEnemyType(string friendlyName, string name, string uniqueName, global::EnemyType vanillaEnumValue)
            {
                readableName = friendlyName;
                this.name = name;
                this.uniqueName = uniqueName;
                this.vanillaEnumValue = vanillaEnumValue;
            }

            private string readableName = "???";
            private string name = "???";
            private string uniqueName = "???";
            private global::EnemyType? vanillaEnumValue = null;

            public override string ReadableName => readableName;
            public override string Name => name;
            public override string UniqueName => uniqueName;
            public override global::EnemyType? VanillaEnumValue => vanillaEnumValue;

            public override bool IsVanilla => true;

            public static AEnemyType BigJohnator { get; private set; }
            public static AEnemyType CancerousRodent { get; private set; }
            public static AEnemyType Centaur { get; private set; }
            public static AEnemyType Cerberus { get; private set; }
            public static AEnemyType Deathcatcher { get; private set; }
            public static AEnemyType Drone { get; private set; }
            public static AEnemyType Ferryman { get; private set; }
            public static AEnemyType Filth { get; private set; }
            public static AEnemyType FleshPanopticon { get; private set; }
            public static AEnemyType Gabriel { get; private set; }
            public static AEnemyType FleshPrison { get; private set; }
            public static AEnemyType GabrielSecond { get; private set; }
            public static AEnemyType Geryon { get; private set; }
            public static AEnemyType Gutterman { get; private set; }
            public static AEnemyType Guttertank { get; private set; }
            public static AEnemyType HideousMass { get; private set; }
            public static AEnemyType Idol { get; private set; }
            public static AEnemyType Leviathan { get; private set; }
            public static AEnemyType MaliciousFace { get; private set; }
            public static AEnemyType Mandalore { get; private set; }
            public static AEnemyType Mannequin { get; private set; }
            public static AEnemyType Mindflayer { get; private set; }
            public static AEnemyType Minos { get; private set; }
            public static AEnemyType MinosPrime { get; private set; }
            public static AEnemyType Minotaur { get; private set; }
            public static AEnemyType MirrorReaper { get; private set; }
            public static AEnemyType Power { get; private set; }
            public static AEnemyType Providence { get; private set; }
            public static AEnemyType Puppet { get; private set; }
            public static AEnemyType Schism { get; private set; }
            public static AEnemyType Sisyphus { get; private set; }
            public static AEnemyType SisyphusPrime { get; private set; }
            public static AEnemyType Soldier { get; private set; }
            public static AEnemyType Stalker { get; private set; }
            public static AEnemyType Stray { get; private set; }
            public static AEnemyType Streetcleaner { get; private set; }
            public static AEnemyType Swordsmachine { get; private set; }
            public static AEnemyType Turret { get; private set; }
            public static AEnemyType V2 { get; private set; }
            public static AEnemyType V2Second { get; private set; }
            public static AEnemyType VeryCancerousRodent { get; private set; }
            public static AEnemyType Virtue { get; private set; }
            public static AEnemyType Wicked { get; private set; }

            internal static void Initialize()
            {
                var etdb = EnemyTypeDB.Instance;

                foreach (var valueGeneric in Enum.GetValues(typeof(global::EnemyType)))
                {
                    var enemyType = (global::EnemyType)valueGeneric;
                    AEnemyType enemyTypeObj = null;

                    switch (enemyType)
                    {
                        case global::EnemyType.BigJohnator:
                            enemyTypeObj = new VanillaEnemyType("Big Johninator", "BigJohnator", global::EnemyType.BigJohnator);
                            BigJohnator = enemyTypeObj;
                            break;
                        case global::EnemyType.CancerousRodent:
                            enemyTypeObj = new VanillaEnemyType("Cancerous Rodent", "CancerousRodent", global::EnemyType.CancerousRodent);
                            CancerousRodent = enemyTypeObj;
                            break;
                        case global::EnemyType.Centaur:
                            enemyTypeObj = new VanillaEnemyType("Earth Mover", "Centaur", global::EnemyType.Centaur);
                            Centaur = enemyTypeObj;
                            break;
                        case global::EnemyType.Cerberus:
                            enemyTypeObj = new VanillaEnemyType("Cerberus", "Cerberus", global::EnemyType.Cerberus);
                            Cerberus = enemyTypeObj;
                            break;
                        case global::EnemyType.Deathcatcher:
                            enemyTypeObj = new VanillaEnemyType("Deathcatcher", "Deathcatcher", global::EnemyType.Deathcatcher);
                            Deathcatcher = enemyTypeObj;
                            break;
                        case global::EnemyType.Drone:
                            enemyTypeObj = new VanillaEnemyType("Drone", "Drone", global::EnemyType.Drone);
                            Drone = enemyTypeObj;
                            break;
                        case global::EnemyType.Ferryman:
                            enemyTypeObj = new VanillaEnemyType("Ferryman", "Ferryman", global::EnemyType.Ferryman);
                            Ferryman = enemyTypeObj;
                            break;
                        case global::EnemyType.Filth:
                            enemyTypeObj = new VanillaEnemyType("Filth", "Filth", global::EnemyType.Filth);
                            Filth = enemyTypeObj;
                            break;
                        case global::EnemyType.FleshPanopticon:
                            enemyTypeObj = new VanillaEnemyType("Flesh Panopticon", "FleshPanopticon", global::EnemyType.FleshPanopticon);
                            FleshPanopticon = enemyTypeObj;
                            break;
                        case global::EnemyType.FleshPrison:
                            enemyTypeObj = new VanillaEnemyType("Flesh Prison", "FleshPrison", global::EnemyType.FleshPrison);
                            FleshPrison = enemyTypeObj;
                            break;
                        case global::EnemyType.Gabriel:
                            enemyTypeObj = new VanillaEnemyType("Gabrie - Judge of Hell", "Gabriel", global::EnemyType.Gabriel);
                            Gabriel = enemyTypeObj;
                            break;
                        case global::EnemyType.GabrielSecond:
                            enemyTypeObj = new VanillaEnemyType("Gabriel - Apostate of Hate", "GabrielSecond", global::EnemyType.GabrielSecond);
                            GabrielSecond = enemyTypeObj;
                            break;
                        case global::EnemyType.Geryon:
                            enemyTypeObj = new VanillaEnemyType("Geryon", "Geryon", global::EnemyType.Geryon);
                            Geryon = enemyTypeObj;
                            break;
                        case global::EnemyType.Gutterman:
                            enemyTypeObj = new VanillaEnemyType("Gutterman", "Gutterman", global::EnemyType.Gutterman);
                            Gutterman = enemyTypeObj;
                            break;
                        case global::EnemyType.Guttertank:
                            enemyTypeObj = new VanillaEnemyType("Guttertank", "Guttertank", global::EnemyType.Guttertank);
                            Guttertank = enemyTypeObj;
                            break;
                        case global::EnemyType.HideousMass:
                            enemyTypeObj = new VanillaEnemyType("Hideous Mass", "HideousMass", global::EnemyType.HideousMass);
                            HideousMass = enemyTypeObj;
                            break;
                        case global::EnemyType.Idol:
                            enemyTypeObj = new VanillaEnemyType("Idol", "Idol", global::EnemyType.Idol);
                            Idol = enemyTypeObj;
                            break;
                        case global::EnemyType.Leviathan:
                            enemyTypeObj = new VanillaEnemyType("Leviathan", "Leviathan", global::EnemyType.Leviathan);
                            Leviathan = enemyTypeObj;
                            break;
                        case global::EnemyType.MaliciousFace:
                            enemyTypeObj = new VanillaEnemyType("Malicious Face", "MaliciousFace", global::EnemyType.MaliciousFace);
                            MaliciousFace = enemyTypeObj;
                            break;
                        case global::EnemyType.Mandalore:
                            enemyTypeObj = new VanillaEnemyType("Mandalore", "Mandalore", global::EnemyType.Mandalore);
                            Mandalore = enemyTypeObj;
                            break;
                        case global::EnemyType.Mannequin:
                            enemyTypeObj = new VanillaEnemyType("Mannequin", "Mannequin", global::EnemyType.Mannequin);
                            Mannequin = enemyTypeObj;
                            break;
                        case global::EnemyType.Mindflayer:
                            enemyTypeObj = new VanillaEnemyType("Mindflayer", "Mindflayer", global::EnemyType.Mindflayer);
                            Mindflayer = enemyTypeObj;
                            break;
                        case global::EnemyType.Minos:
                            enemyTypeObj = new VanillaEnemyType("Corpse of King Minos", "Minos", global::EnemyType.Minos);
                            Minos = enemyTypeObj;
                            break;
                        case global::EnemyType.MinosPrime:
                            enemyTypeObj = new VanillaEnemyType("Minos Prime", "MinosPrime", global::EnemyType.MinosPrime);
                            MinosPrime = enemyTypeObj;
                            break;
                        case global::EnemyType.Minotaur:
                            enemyTypeObj = new VanillaEnemyType("Minotaur", "Minotaur", global::EnemyType.Minotaur);
                            Minotaur = enemyTypeObj;
                            break;
                        case global::EnemyType.MirrorReaper:
                            enemyTypeObj = new VanillaEnemyType("Mirror Reaper", "MirrorReaper", global::EnemyType.MirrorReaper);
                            MirrorReaper = enemyTypeObj;
                            break;
                        case global::EnemyType.Power:
                            enemyTypeObj = new VanillaEnemyType("Power", "Power", global::EnemyType.Power);
                            Power = enemyTypeObj;
                            break;
                        case global::EnemyType.Providence:
                            enemyTypeObj = new VanillaEnemyType("Providence", "Providence", global::EnemyType.Providence);
                            Providence = enemyTypeObj;
                            break;
                        case global::EnemyType.Puppet:
                            enemyTypeObj = new VanillaEnemyType("Puppet", "Puppet", global::EnemyType.Puppet);
                            Puppet = enemyTypeObj;
                            break;
                        case global::EnemyType.Schism:
                            enemyTypeObj = new VanillaEnemyType("Schism", "Schism", global::EnemyType.Schism);
                            Schism = enemyTypeObj;
                            break;
                        case global::EnemyType.Sisyphus:
                            enemyTypeObj = new VanillaEnemyType("Sisyphus", "Sisyphus", global::EnemyType.Sisyphus);
                            Sisyphus = enemyTypeObj;
                            break;
                        case global::EnemyType.SisyphusPrime:
                            enemyTypeObj = new VanillaEnemyType("Sisyphus Prime", "SisyphusPrime", global::EnemyType.SisyphusPrime);
                            SisyphusPrime = enemyTypeObj;
                            break;
                        case global::EnemyType.Soldier:
                            enemyTypeObj = new VanillaEnemyType("Soldier", "Soldier", global::EnemyType.Soldier);
                            Soldier = enemyTypeObj;
                            break;
                        case global::EnemyType.Stalker:
                            enemyTypeObj = new VanillaEnemyType("Stalker", "Stalker", global::EnemyType.Stalker);
                            Stalker = enemyTypeObj;
                            break;
                        case global::EnemyType.Stray:
                            enemyTypeObj = new VanillaEnemyType("Stray", "Stray", global::EnemyType.Stray);
                            Stray = enemyTypeObj;
                            break;
                        case global::EnemyType.Streetcleaner:
                            enemyTypeObj = new VanillaEnemyType("Street Cleaner", "Streetcleaner", global::EnemyType.Streetcleaner);
                            Streetcleaner = enemyTypeObj;
                            break;
                        case global::EnemyType.Swordsmachine:
                            enemyTypeObj = new VanillaEnemyType("Swords Machine", "Swordsmachine", global::EnemyType.Swordsmachine);
                            Swordsmachine = enemyTypeObj;
                            break;
                        case global::EnemyType.Turret:
                            enemyTypeObj = new VanillaEnemyType("Turret", "Turret", global::EnemyType.Turret);
                            Turret = enemyTypeObj;
                            break;
                        case global::EnemyType.V2:
                            enemyTypeObj = new VanillaEnemyType("V2", "V2", global::EnemyType.V2);
                            V2 = enemyTypeObj;
                            break;
                        case global::EnemyType.V2Second:
                            enemyTypeObj = new VanillaEnemyType("V2... 2!", "V2Second", global::EnemyType.V2Second);
                            V2Second = enemyTypeObj;
                            break;
                        case global::EnemyType.VeryCancerousRodent:
                            enemyTypeObj = new VanillaEnemyType("Very Cancerous Rodent", "VeryCancerousRodent", global::EnemyType.VeryCancerousRodent);
                            VeryCancerousRodent = enemyTypeObj;
                            break;
                        case global::EnemyType.Virtue:
                            enemyTypeObj = new VanillaEnemyType("Virtue", "Virtue", global::EnemyType.Virtue);
                            Virtue = enemyTypeObj;
                            break;
                        case global::EnemyType.Wicked:
                            enemyTypeObj = new VanillaEnemyType("Something Wicked", "Wicked", global::EnemyType.Wicked);
                            Wicked = enemyTypeObj;
                            break;
                    }

                    etdb.RegisterType(enemyTypeObj);
                }
            }
        }
    }
}