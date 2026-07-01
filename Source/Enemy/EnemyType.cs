using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class EnemyTypeData
    {
        public string DisplayName { get; private set; }
        public string Name { get; private set; }
        public string UniqueName { get; private set; }
        public global::EnemyType? VanillaEnumValue { get; private set; }
        public bool IsVanilla { get; private set; } = false;

        public EnemyTypeData(string displayName, string name, string uniqueName, global::EnemyType? vanillaEnumValue = null, bool isVanilla = false)
        {
            DisplayName = displayName;
            Name = name;
            UniqueName = uniqueName;
            VanillaEnumValue = vanillaEnumValue;
            IsVanilla = isVanilla;
        }

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

    public static class VanillaEnemyType
    {
        public static EnemyTypeData BigJohnator { get; private set; }
        public static EnemyTypeData CancerousRodent { get; private set; }
        public static EnemyTypeData Centaur { get; private set; }
        public static EnemyTypeData Cerberus { get; private set; }
        public static EnemyTypeData Deathcatcher { get; private set; }
        public static EnemyTypeData Drone { get; private set; }
        public static EnemyTypeData Ferryman { get; private set; }
        public static EnemyTypeData Filth { get; private set; }
        public static EnemyTypeData FleshPanopticon { get; private set; }
        public static EnemyTypeData Gabriel { get; private set; }
        public static EnemyTypeData FleshPrison { get; private set; }
        public static EnemyTypeData GabrielSecond { get; private set; }
        public static EnemyTypeData Geryon { get; private set; }
        public static EnemyTypeData Gutterman { get; private set; }
        public static EnemyTypeData Guttertank { get; private set; }
        public static EnemyTypeData HideousMass { get; private set; }
        public static EnemyTypeData Idol { get; private set; }
        public static EnemyTypeData Leviathan { get; private set; }
        public static EnemyTypeData MaliciousFace { get; private set; }
        public static EnemyTypeData Mandalore { get; private set; }
        public static EnemyTypeData Mannequin { get; private set; }
        public static EnemyTypeData Mindflayer { get; private set; }
        public static EnemyTypeData Minos { get; private set; }
        public static EnemyTypeData MinosPrime { get; private set; }
        public static EnemyTypeData Minotaur { get; private set; }
        public static EnemyTypeData MirrorReaper { get; private set; }
        public static EnemyTypeData Power { get; private set; }
        public static EnemyTypeData Providence { get; private set; }
        public static EnemyTypeData Puppet { get; private set; }
        public static EnemyTypeData Schism { get; private set; }
        public static EnemyTypeData Sisyphus { get; private set; }
        public static EnemyTypeData SisyphusPrime { get; private set; }
        public static EnemyTypeData Soldier { get; private set; }
        public static EnemyTypeData Stalker { get; private set; }
        public static EnemyTypeData Stray { get; private set; }
        public static EnemyTypeData Streetcleaner { get; private set; }
        public static EnemyTypeData Swordsmachine { get; private set; }
        public static EnemyTypeData Turret { get; private set; }
        public static EnemyTypeData V2 { get; private set; }
        public static EnemyTypeData V2Second { get; private set; }
        public static EnemyTypeData VeryCancerousRodent { get; private set; }
        public static EnemyTypeData Virtue { get; private set; }
        public static EnemyTypeData Wicked { get; private set; }

        internal static void Initialize()
        {
            var etdb = EnemyTypeDB.Instance;
            var prefix = "nyxlib.vanilla-ultrakill.";

            foreach (var valueGeneric in Enum.GetValues(typeof(global::EnemyType)))
            {
                var enemyType = (global::EnemyType)valueGeneric;
                EnemyTypeData enemyTypeObj = null;

                switch (enemyType)
                {
                    case global::EnemyType.BigJohnator:
                        enemyTypeObj = new EnemyTypeData(displayName: "Big Johninator", name: "BigJohnator", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.BigJohnator, isVanilla: true);
                        BigJohnator = enemyTypeObj;
                        break;
                    case global::EnemyType.CancerousRodent:
                        enemyTypeObj = new EnemyTypeData(displayName: "Cancerous Rodent", name: "CancerousRodent", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.CancerousRodent, isVanilla: true);
                        CancerousRodent = enemyTypeObj;
                        break;
                    case global::EnemyType.Centaur:
                        enemyTypeObj = new EnemyTypeData(displayName: "Earth Mover", name: "Centaur", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Centaur, isVanilla: true);
                        Centaur = enemyTypeObj;
                        break;
                    case global::EnemyType.Cerberus:
                        enemyTypeObj = new EnemyTypeData(displayName: "Cerberus", name: "Cerberus", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Cerberus, isVanilla: true);
                        Cerberus = enemyTypeObj;
                        break;
                    case global::EnemyType.Deathcatcher:
                        enemyTypeObj = new EnemyTypeData(displayName: "Deathcatcher", name: "Deathcatcher", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Deathcatcher, isVanilla: true);
                        Deathcatcher = enemyTypeObj;
                        break;
                    case global::EnemyType.Drone:
                        enemyTypeObj = new EnemyTypeData(displayName: "Drone", name: "Drone", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Drone, isVanilla: true);
                        Drone = enemyTypeObj;
                        break;
                    case global::EnemyType.Ferryman:
                        enemyTypeObj = new EnemyTypeData(displayName: "Ferryman", name: "Ferryman", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Ferryman, isVanilla: true);
                        Ferryman = enemyTypeObj;
                        break;
                    case global::EnemyType.Filth:
                        enemyTypeObj = new EnemyTypeData(displayName: "Filth", name: "Filth", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Filth, isVanilla: true);
                        Filth = enemyTypeObj;
                        break;
                    case global::EnemyType.FleshPanopticon:
                        enemyTypeObj = new EnemyTypeData(displayName: "Flesh Panopticon", name: "FleshPanopticon", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.FleshPanopticon, isVanilla: true);
                        FleshPanopticon = enemyTypeObj;
                        break;
                    case global::EnemyType.FleshPrison:
                        enemyTypeObj = new EnemyTypeData(displayName: "Flesh Prison", name: "FleshPrison", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.FleshPrison, isVanilla: true);
                        FleshPrison = enemyTypeObj;
                        break;
                    case global::EnemyType.Gabriel:
                        enemyTypeObj = new EnemyTypeData(displayName: "Gabrie - Judge of Hell", name: "Gabriel", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Gabriel, isVanilla: true);
                        Gabriel = enemyTypeObj;
                        break;
                    case global::EnemyType.GabrielSecond:
                        enemyTypeObj = new EnemyTypeData(displayName: "Gabriel - Apostate of Hate", name: "GabrielSecond", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.GabrielSecond, isVanilla: true);
                        GabrielSecond = enemyTypeObj;
                        break;
                    case global::EnemyType.Geryon:
                        enemyTypeObj = new EnemyTypeData(displayName: "Geryon", name: "Geryon", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Geryon, isVanilla: true);
                        Geryon = enemyTypeObj;
                        break;
                    case global::EnemyType.Gutterman:
                        enemyTypeObj = new EnemyTypeData(displayName: "Gutterman", name: "Gutterman", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Gutterman, isVanilla: true);
                        Gutterman = enemyTypeObj;
                        break;
                    case global::EnemyType.Guttertank:
                        enemyTypeObj = new EnemyTypeData(displayName: "Guttertank", name: "Guttertank", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Guttertank, isVanilla: true);
                        Guttertank = enemyTypeObj;
                        break;
                    case global::EnemyType.HideousMass:
                        enemyTypeObj = new EnemyTypeData(displayName: "Hideous Mass", name: "HideousMass", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.HideousMass, isVanilla: true);
                        HideousMass = enemyTypeObj;
                        break;
                    case global::EnemyType.Idol:
                        enemyTypeObj = new EnemyTypeData(displayName: "Idol", name: "Idol", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Idol, isVanilla: true);
                        Idol = enemyTypeObj;
                        break;
                    case global::EnemyType.Leviathan:
                        enemyTypeObj = new EnemyTypeData(displayName: "Leviathan", name: "Leviathan", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Leviathan, isVanilla: true);
                        Leviathan = enemyTypeObj;
                        break;
                    case global::EnemyType.MaliciousFace:
                        enemyTypeObj = new EnemyTypeData(displayName: "Malicious Face", name: "MaliciousFace", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.MaliciousFace, isVanilla: true);
                        MaliciousFace = enemyTypeObj;
                        break;
                    case global::EnemyType.Mandalore:
                        enemyTypeObj = new EnemyTypeData(displayName: "Mandalore", name: "Mandalore", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Mandalore, isVanilla: true);
                        Mandalore = enemyTypeObj;
                        break;
                    case global::EnemyType.Mannequin:
                        enemyTypeObj = new EnemyTypeData(displayName: "Mannequin", name: "Mannequin", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Mannequin, isVanilla: true);
                        Mannequin = enemyTypeObj;
                        break;
                    case global::EnemyType.Mindflayer:
                        enemyTypeObj = new EnemyTypeData(displayName: "Mindflayer", name: "Mindflayer", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Mindflayer, isVanilla: true);
                        Mindflayer = enemyTypeObj;
                        break;
                    case global::EnemyType.Minos:
                        enemyTypeObj = new EnemyTypeData(displayName: "Corpse of King Minos", name: "Minos", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Minos, isVanilla: true);
                        Minos = enemyTypeObj;
                        break;
                    case global::EnemyType.MinosPrime:
                        enemyTypeObj = new EnemyTypeData(displayName: "Minos Prime", name: "MinosPrime", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.MinosPrime, isVanilla: true);
                        MinosPrime = enemyTypeObj;
                        break;
                    case global::EnemyType.Minotaur:
                        enemyTypeObj = new EnemyTypeData(displayName: "Minotaur", name: "Minotaur", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Minotaur, isVanilla: true);
                        Minotaur = enemyTypeObj;
                        break;
                    case global::EnemyType.MirrorReaper:
                        enemyTypeObj = new EnemyTypeData(displayName: "Mirror Reaper", name: "MirrorReaper", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.MirrorReaper, isVanilla: true);
                        MirrorReaper = enemyTypeObj;
                        break;
                    case global::EnemyType.Power:
                        enemyTypeObj = new EnemyTypeData(displayName: "Power", name: "Power", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Power, isVanilla: true);
                        Power = enemyTypeObj;
                        break;
                    case global::EnemyType.Providence:
                        enemyTypeObj = new EnemyTypeData(displayName: "Providence", name: "Providence", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Providence, isVanilla: true);
                        Providence = enemyTypeObj;
                        break;
                    case global::EnemyType.Puppet:
                        enemyTypeObj = new EnemyTypeData(displayName: "Puppet", name: "Puppet", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Puppet, isVanilla: true);
                        Puppet = enemyTypeObj;
                        break;
                    case global::EnemyType.Schism:
                        enemyTypeObj = new EnemyTypeData(displayName: "Schism", name: "Schism", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Schism, isVanilla: true);
                        Schism = enemyTypeObj;
                        break;
                    case global::EnemyType.Sisyphus:
                        enemyTypeObj = new EnemyTypeData(displayName: "Sisyphus", name: "Sisyphus", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Sisyphus, isVanilla: true);
                        Sisyphus = enemyTypeObj;
                        break;
                    case global::EnemyType.SisyphusPrime:
                        enemyTypeObj = new EnemyTypeData(displayName: "Sisyphus Prime", name: "SisyphusPrime", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.SisyphusPrime, isVanilla: true);
                        SisyphusPrime = enemyTypeObj;
                        break;
                    case global::EnemyType.Soldier:
                        enemyTypeObj = new EnemyTypeData(displayName: "Soldier", name: "Soldier", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Soldier, isVanilla: true);
                        Soldier = enemyTypeObj;
                        break;
                    case global::EnemyType.Stalker:
                        enemyTypeObj = new EnemyTypeData(displayName: "Stalker", name: "Stalker", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Stalker, isVanilla: true);
                        Stalker = enemyTypeObj;
                        break;
                    case global::EnemyType.Stray:
                        enemyTypeObj = new EnemyTypeData(displayName: "Stray", name: "Stray", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Stray, isVanilla: true);
                        Stray = enemyTypeObj;
                        break;
                    case global::EnemyType.Streetcleaner:
                        enemyTypeObj = new EnemyTypeData(displayName: "Street Cleaner", name: "Streetcleaner", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Streetcleaner, isVanilla: true);
                        Streetcleaner = enemyTypeObj;
                        break;
                    case global::EnemyType.Swordsmachine:
                        enemyTypeObj = new EnemyTypeData(displayName: "Swords Machine", name: "Swordsmachine", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Swordsmachine, isVanilla: true);
                        Swordsmachine = enemyTypeObj;
                        break;
                    case global::EnemyType.Turret:
                        enemyTypeObj = new EnemyTypeData(displayName: "Turret", name: "Turret", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Turret, isVanilla: true);
                        Turret = enemyTypeObj;
                        break;
                    case global::EnemyType.V2:
                        enemyTypeObj = new EnemyTypeData(displayName: "V2", name: "V2", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.V2, isVanilla: true);
                        V2 = enemyTypeObj;
                        break;
                    case global::EnemyType.V2Second:
                        enemyTypeObj = new EnemyTypeData(displayName: "V2... 2!", name: "V2Second", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.V2Second, isVanilla: true);
                        V2Second = enemyTypeObj;
                        break;
                    case global::EnemyType.VeryCancerousRodent:
                        enemyTypeObj = new EnemyTypeData(displayName: "Very Cancerous Rodent", name: "VeryCancerousRodent", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.VeryCancerousRodent, isVanilla: true);
                        VeryCancerousRodent = enemyTypeObj;
                        break;
                    case global::EnemyType.Virtue:
                        enemyTypeObj = new EnemyTypeData(displayName: "Virtue", name: "Virtue", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Virtue, isVanilla: true);
                        Virtue = enemyTypeObj;
                        break;
                    case global::EnemyType.Wicked:
                        enemyTypeObj = new EnemyTypeData(displayName: "Something Wicked", name: "Wicked", uniqueName: $"{prefix}.{enemyType}", global::EnemyType.Wicked, isVanilla: true);
                        Wicked = enemyTypeObj;
                        break;
                }

                etdb.RegisterType(enemyTypeObj);
            }
        }
    }
}