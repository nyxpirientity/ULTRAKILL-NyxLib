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
        public static PrefabAsset<ExplosionRoot> Harmless { get; private set; } = null;
        public static PrefabAsset<ExplosionRoot> Normal { get; private set; } = null;
        public static PrefabAsset<ExplosionRoot> Super { get; private set; } = null;

        private ExplosionRoot _harmless = null;
        private ExplosionRoot _normal = null;
        private ExplosionRoot _super = null;

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

            var fs = Gear.Firestarter.ToAsset().GetComponent<RocketLauncher>();
            var ce = Gear.CoreEject.ToAsset().GetComponent<Shotgun>();
            var rocket = fs.rocket.GetComponent<Grenade>();
            var grenade = ce.grenade.GetComponent<Grenade>();

            _harmless = Instantiate(rocket.harmlessExplosion, AssetsRoot.Holder).AddComponent<ExplosionRoot>();
            _normal = Instantiate(grenade.explosion, AssetsRoot.Holder).AddComponent<ExplosionRoot>();
            _super = Instantiate(grenade.superExplosion, AssetsRoot.Holder).AddComponent<ExplosionRoot>();

            Harmless = new PrefabAsset<ExplosionRoot>(_harmless);
            Normal = new PrefabAsset<ExplosionRoot>(_normal);
            Super = new PrefabAsset<ExplosionRoot>(_super);
        }
    }
}