using System;
using System.Collections.Generic;
using System.Linq;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class EnemyTypeDB : MonoSingleton<EnemyTypeDB>
    {
        public void RegisterType(EnemyTypeData enemyType)
        {
            // TODO: error handling for unique name conflicts (really just give a more straight forward error)
            dictTypes.Add(enemyType.UniqueName, enemyType);
            hashTypes.Add(enemyType);
            types = hashTypes.ToArray();

            Log.TraceExpectedInfo($"[EnemyTypeDB]: Registered AEnemyType {enemyType}");

            if (enemyType.VanillaEnumValue != null && !vanillaTypeDict.ContainsKey(enemyType.VanillaEnumValue.Value))
            {
                vanillaTypeDict.Add(enemyType.VanillaEnumValue.Value, enemyType);
            }
        }

        public IReadOnlyList<EnemyTypeData> GetValues()
        {
            return types;
        }

        public EnemyTypeData GetVanillaType(global::EnemyType vanillaEnumType)
        {
            return vanillaTypeDict[vanillaEnumType];
        }

        protected void OnEnable()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }

        private EnemyTypeData[] types = new EnemyTypeData[0];
        private HashSet<EnemyTypeData> hashTypes = new HashSet<EnemyTypeData>();
        private Dictionary<string, EnemyTypeData> dictTypes = new Dictionary<string, EnemyTypeData>();
        private Dictionary<global::EnemyType, EnemyTypeData> vanillaTypeDict = new Dictionary<global::EnemyType, EnemyTypeData>();
    }
}