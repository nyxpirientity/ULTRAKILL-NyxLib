using System.IO;
using HarmonyLib;
using TMPro;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System;

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets
{
    [ConfigureSingleton(SingletonFlags.NoAutoInstance)]
    public class Explosions : MonoSingleton<Explosions>
    {
        public static PrefabAsset<ExplosionRoot> Harmless { get; private set; } = new PrefabAsset<ExplosionRoot>(() => Instance?._harmless);
        public static PrefabAsset<ExplosionRoot> Normal { get; private set; } = new PrefabAsset<ExplosionRoot>(() => Instance?._normal);
        public static PrefabAsset<ExplosionRoot> Super { get; private set; } = new PrefabAsset<ExplosionRoot>(() => Instance?._super);

        [SerializeField] private ExplosionRoot _harmless = null;
        [SerializeField] private ExplosionRoot _normal = null;
        [SerializeField] private ExplosionRoot _super = null;

        private void Awake()
        {
            SceneEvents.OnSceneStart += OnNewSceneStart;
        }

        private void OnNewSceneStart(Scene scene, string levelName, string unitySceneName)
        {
            if (_harmless != null || Gear.Firestarter == null)
            {
                return;
            }

            Log.ExpectedInfo($"Getting explosions...");

            var fs = Gear.Firestarter.DirectPrefab.GetComponent<RocketLauncher>();
            var ce = Gear.CoreEject.DirectPrefab.GetComponent<Shotgun>();
            var rocket = fs.rocket.GetComponent<Grenade>();
            var grenade = ce.grenade.GetComponent<Grenade>();

            _harmless = Instantiate(rocket.harmlessExplosion, AssetsRoot.Holder).AddComponent<ExplosionRoot>();
            _normal = Instantiate(grenade.explosion, AssetsRoot.Holder).AddComponent<ExplosionRoot>();
            _super = Instantiate(grenade.superExplosion, AssetsRoot.Holder).AddComponent<ExplosionRoot>();

            _harmless.SetMaxPlayerDamageOverride(-1);
            _normal.SetMaxPlayerDamageOverride(-1);
            _super.SetMaxPlayerDamageOverride(-1);
        }
    }
}