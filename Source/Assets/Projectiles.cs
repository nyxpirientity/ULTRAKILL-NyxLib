using System.IO;
using HarmonyLib;
using TMPro;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets
{
    [ConfigureSingleton(SingletonFlags.NoAutoInstance)]
    public class Projectiles : MonoSingleton<Projectiles>
    {
        public static PrefabAsset<Grenade> Core { get; private set; } = new PrefabAsset<Grenade>(() => Instance?._core);
        public static PrefabAsset<Grenade> PlayerRocket { get; private set; } = new PrefabAsset<Grenade>(() => Instance?._playerRocket);

        public static PrefabAsset<Projectile> Homing { get; private set; } = new PrefabAsset<Projectile>(() => Instance?._homing);
        public static PrefabAsset<Projectile> Mortar { get; private set; } = new PrefabAsset<Projectile>(() => Instance?._mortar);

        [SerializeField] private Projectile _mortar = null;
        [SerializeField] private Projectile _homing = null;

        [SerializeField] private Grenade _playerRocket = null;
        [SerializeField] private Grenade _core = null;

        private void Awake()
        {
            SceneEvents.OnSceneStart += OnNewSceneStart;
            SpawnDbPicker.OnGettingPrefabs += GetSpawnDbPrefabs;
        }

        private void GetSpawnDbPrefabs(SpawnableObjectsDatabase db)
        {
            Log.ExpectedInfo($"Getting spawndb projectiles...");
            if (_mortar == null)
            {
                var hideousMass = db.enemies.First((spawnable) => spawnable.enemyType == EnemyType.HideousMass).gameObject.GetComponentInChildren<Mass>();

                if (hideousMass != null)
                {
                    _mortar = GameObject.Instantiate(hideousMass.explosiveProjectile, AssetsRoot.Holder).GetComponent<Projectile>();
                    _homing = GameObject.Instantiate(hideousMass.homingProjectile, AssetsRoot.Holder).GetComponent<Projectile>();
                }
                else
                {
                    Log.ExpectedInfo($"We'd like a a hideous mass in order to yoink it's projectile prefabs, but this scene \"{SceneHelper.CurrentScene}\" didn't have it yet!");
                }
            }
        }

        private void OnNewSceneStart(Scene scene, string levelName, string unitySceneName)
        {
            if (PlayerRocket == null && Gear.Firestarter != null)
            {
                var fs = Gear.Firestarter.DirectPrefab.GetComponent<RocketLauncher>();
                var ce = Gear.CoreEject.DirectPrefab.GetComponent<Shotgun>();

                _playerRocket = Instantiate(fs.rocket, AssetsRoot.Holder).GetComponentInChildren<Grenade>();
                _core = Instantiate(ce.grenade, AssetsRoot.Holder).GetComponentInChildren<Grenade>();
            }
        }
    }
}