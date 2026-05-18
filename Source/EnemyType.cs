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
            if (enemyType.VanillaEnumValue != null)
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
        }
    }
}