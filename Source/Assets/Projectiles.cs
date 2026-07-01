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
        public static PrefabAsset<Grenade> Core { get; private set; } = null;
        public static PrefabAsset<Grenade> PlayerRocket { get; private set; } = null;
        public static PrefabAsset<Projectile> Homing { get; private set; } = null;
        public static PrefabAsset<Projectile> Mortar { get; private set; } = null;

        [SerializeField] private GameObject _mortar = null;
        [SerializeField] private GameObject _homing = null;

        [SerializeField] private Grenade _playerRocket = null;
        [SerializeField] private Grenade _core = null;

        private void Awake()
        {
            SceneEvents.OnSceneStart += OnNewSceneStart;
            SpawnDbPicker.OnGettingPrefabs += GetSpawnDbPrefabs;
        }

        private void GetSpawnDbPrefabs(SpawnableObjectsDatabase db)
        {
            if (_mortar == null)
            {
                var hideousMass = db.enemies.First((spawnable) => spawnable.enemyType == EnemyType.HideousMass).gameObject.GetComponentInChildren<Mass>();

                if (hideousMass != null)
                {
                    _mortar = GameObject.Instantiate(hideousMass.explosiveProjectile, AssetsRoot.Holder);
                    _homing = GameObject.Instantiate(hideousMass.homingProjectile, AssetsRoot.Holder);
                }
                else
                {
                    Log.ExpectedInfo($"We'd like a a hideous mass in order to yoink it's projectile prefabs, but this scene \"{SceneHelper.CurrentScene}\" didn't have it yet!");
                }

                Homing = new PrefabAsset<Projectile>(_homing.GetComponentInChildren<Projectile>());
                Mortar = new PrefabAsset<Projectile>(_mortar.GetComponentInChildren<Projectile>());
            }
        }

        private void OnNewSceneStart(Scene scene, string levelName, string unitySceneName)
        {
            if (PlayerRocket == null && Gear.Firestarter != null)
            {
                var fs = Gear.Firestarter.ToAsset().GetComponent<RocketLauncher>();
                var ce = Gear.CoreEject.ToAsset().GetComponent<Shotgun>();

                _playerRocket = Instantiate(fs.rocket, AssetsRoot.Holder).GetComponentInChildren<Grenade>();
                _core = Instantiate(ce.grenade, AssetsRoot.Holder).GetComponentInChildren<Grenade>();

                PlayerRocket = new PrefabAsset<Grenade>(_playerRocket);
                Core = new PrefabAsset<Grenade>(_core);
            }
        }
    }
}