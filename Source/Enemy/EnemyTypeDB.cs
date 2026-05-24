using System;
using System.Collections.Generic;
using System.Linq;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class EnemyTypeDB : MonoSingleton<EnemyTypeDB>
    {
        public void RegisterType(AEnemyType enemyType)
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

        public IReadOnlyList<AEnemyType> GetValues()
        {
            return types;
        }

        public AEnemyType GetVanillaType(global::EnemyType vanillaEnumType)
        {
            return vanillaTypeDict[vanillaEnumType];
        }

        protected void OnEnable()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }

        private AEnemyType[] types = new AEnemyType[0];
        private HashSet<AEnemyType> hashTypes = new HashSet<AEnemyType>();
        private Dictionary<string, AEnemyType> dictTypes = new Dictionary<string, AEnemyType>();
        private Dictionary<global::EnemyType, AEnemyType> vanillaTypeDict = new Dictionary<global::EnemyType, AEnemyType>();
    }
}