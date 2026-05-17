using System;
using System.Collections.Generic;
using System.Linq;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using Sandbox;
using Sandbox.Arm;
using ULTRAKILL.Cheats;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class EnemyPrefabDatabase : MonoBehaviour
    {
        public static EnemyPrefabDatabase Instance { get; private set; } = null;

        public static GameObject GetPrefab(EnemyType enemyType)
        {
            return Instance.prefabs.GetValueOrDefault(enemyType, null);
        } 

        public static EnemyComponents TrySpawnAt(EnemyType enemyType, Vector3 position, Quaternion rotation, Transform parent, bool autoActivate)
        {
            var prefab = GetPrefab(enemyType);
            
            var go = Instantiate(prefab, parent);
            
            var enemy = go.GetComponentInChildren<EnemyComponents>();

            go.SetActive(autoActivate);

            return enemy;
        }

        private FieldAccess<SpawnMenu, SpawnableObjectsDatabase> spawnableObjectsDbFA = new FieldAccess<SpawnMenu, SpawnableObjectsDatabase>("objects");
        private bool SpawnMenuBasedInitializationFinished = false;
        internal void SpawnMenuBasedInitialize()
        {
            if (SpawnMenuBasedInitializationFinished)
            {
                return;
            }          

            if (CanvasController.Instance == null)
            {
                return;
            }

            var spawnMenu = CanvasController.Instance.GetComponentInChildren<SpawnMenu>(includeInactive: true);

            if (spawnMenu == null)
            {
                return;
            }

            SpawnMenuBasedInitializationFinished = true;

            var spawnableObjectsDb = spawnableObjectsDbFA.GetValue(spawnMenu);
            
            if (spawnableObjectsDb == null)
            {
                return;
            }

            if (PrefabHolder == null)
            {
                PrefabHolder = new GameObject("EnemyPrefabsHolder");
                PrefabHolder.SetActive(false);
                PrefabHolder.transform.parent = transform;
                GameObject.DontDestroyOnLoad(PrefabHolder);
            }
            
            foreach (var enemyTypeGeneric in Enum.GetValues(typeof(EnemyType)))
            {
                FindAndAddEnemyViaDb(spawnableObjectsDb, (EnemyType)enemyTypeGeneric);
                Log.TraceExpectedInfo($"Trying to add enemy of type {(EnemyType)enemyTypeGeneric} prefab from the spawnableObjectsDb");
            }

            SpawnMenuBasedInitializationFinished = true;
            Log.TraceExpectedInfo($"Finished(?) adding enemy prefabs from the spawnableObjectsDb");
        }

        private void FindAndAddEnemyViaDb(SpawnableObjectsDatabase spawnableObjectsDb, EnemyType enemyType)
        {
            var go = spawnableObjectsDb.enemies.FirstOrDefault((obj) => obj.enemyType == enemyType)?.gameObject;
            
            if (go == null)
            {
                return;
            }

            var prefab = GameObject.Instantiate(go, PrefabHolder.transform);
            
            prefabs[enemyType] = prefab;
            
            var eid = prefab.GetComponentInChildren<EnemyIdentifier>();
            var enemyComp = eid.GetOrAddComponent<EnemyComponents>();
            var spawnableInst = eid.GetComponent<EnemySpawnableInstance>();
            
            GameObject.DontDestroyOnLoad(prefab);

            if (spawnableInst != null)
            {
                MonoBehaviour.Destroy(spawnableInst);
            }

            enemyComp.Setup();
        }

        protected void OnEnable()
        {
            if (Instance == null || !Instance.enabled)
            {
                Instance = this;
            }
        }

        protected void Start()
        {
            UpdateEvents.OnLateUpdate += () => 
            {
            };

            PlayerEvents.PreStart += (canceler, player) =>
            {
                SpawnMenuBasedInitialize();
            };
        }

        private GameObject PrefabHolder = null;
        private Dictionary<EnemyType, GameObject> prefabs = new Dictionary<EnemyType, GameObject>();    
    }
}