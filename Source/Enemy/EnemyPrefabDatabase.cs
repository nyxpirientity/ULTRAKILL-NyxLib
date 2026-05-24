using System;
using System.Collections.Generic;
using System.Linq;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using Nyxpiri.ULTRAKILL.NyxLib.EnemyTypes;
using Sandbox;
using Sandbox.Arm;
using ULTRAKILL.Cheats;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class EnemyPrefabDatabase : MonoBehaviour
    {
        public static EnemyPrefabDatabase Instance { get; private set; } = null;

        public static GameObject GetPrefab(AEnemyType enemyType)
        {
            return Instance.prefabs.GetValueOrDefault(enemyType, null);
        }

        public static GameObject GetPrefab(global::EnemyType enemyType)
        {
            return Instance.prefabs.GetValueOrDefault(EnemyTypeDB.Instance.GetVanillaType(enemyType), null);
        }

        public static GameObject TrySpawnAt(global::EnemyType enemyType, Vector3 position, Quaternion rotation, Transform parent, bool autoActivate)
        {
            return TrySpawnAt(EnemyTypeDB.Instance.GetVanillaType(enemyType), position, rotation, parent, autoActivate);
        }

        public static GameObject TrySpawnAt(AEnemyType enemyType, Vector3 position, Quaternion rotation, Transform parent, bool autoActivate)
        {
            var prefab = GetPrefab(enemyType);

            var go = Instantiate(prefab, parent);

            go.transform.position = position;
            go.transform.rotation = rotation;

            go.SetActive(autoActivate);

            return go;
        }

        public void RegisterPrefab(AEnemyType enemyType, GameObject prefab)
        {
            prefabs[enemyType] = prefab;
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

            var spawnableObjectsDb = spawnableObjectsDbFA.GetValue(spawnMenu);

            if (spawnableObjectsDb == null)
            {
                return;
            }

            SpawnMenuBasedInitializationFinished = true;

            if (PrefabHolder == null)
            {
                PrefabHolder = new GameObject("EnemyPrefabsHolder");
                PrefabHolder.SetActive(false);
                PrefabHolder.transform.parent = transform;
                GameObject.DontDestroyOnLoad(PrefabHolder);
            }

            foreach (var enemyTypeGeneric in Enum.GetValues(typeof(global::EnemyType)))
            {
                FindAndAddEnemyViaDb(spawnableObjectsDb, (global::EnemyType)enemyTypeGeneric);
                Log.TraceExpectedInfo($"Trying to add enemy of type {((global::EnemyType)enemyTypeGeneric)} prefab from the spawnableObjectsDb");
            }

            SpawnMenuBasedInitializationFinished = true;
            Log.TraceExpectedInfo($"Finished(?) adding enemy prefabs from the spawnableObjectsDb");
        }

        private void FindAndAddEnemyViaDb(SpawnableObjectsDatabase spawnableObjectsDb, global::EnemyType enemyType)
        {
            var go = spawnableObjectsDb.enemies.FirstOrDefault((obj) => obj.enemyType == enemyType)?.gameObject;

            if (go == null)
            {
                return;
            }

            var prefab = GameObject.Instantiate(go, PrefabHolder.transform);

            RegisterPrefab(EnemyTypeDB.Instance.GetVanillaType(enemyType), prefab);

            var eid = prefab.GetComponentInChildren<EnemyIdentifier>();
            var enemyComp = eid.GetOrAddComponent<EnemyComponents>();
            var gce = prefab.GetComponentInChildren<GroundCheckEnemy>();

            if (gce != null)
            {
                gce.cols.Clear();
            }

            enemyComp.IsMarkedDontDestroyOnLoad = true;
            //var spawnableInst = eid.GetComponent<EnemySpawnableInstance>();

            GameObject.DontDestroyOnLoad(prefab);

            //if (spawnableInst != null)
            //{
            //    MonoBehaviour.Destroy(spawnableInst);
            //}

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
        private Dictionary<AEnemyType, GameObject> prefabs = new Dictionary<AEnemyType, GameObject>();
    }
}