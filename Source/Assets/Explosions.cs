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
        public static PrefabAsset<GameObject> Harmless { get; private set; } = null;
        public static PrefabAsset<GameObject> Normal { get; private set; } = null;
        public static PrefabAsset<GameObject> Super { get; private set; } = null;

        private GameObject _harmless = null;
        private GameObject _normal = null;
        private GameObject _super = null;

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

            _harmless = Instantiate(rocket.harmlessExplosion, AssetsRoot.Holder);
            _normal = Instantiate(grenade.explosion, AssetsRoot.Holder);
            _super = Instantiate(grenade.superExplosion, AssetsRoot.Holder);

            Harmless = new PrefabAsset<GameObject>(_harmless);
            Normal = new PrefabAsset<GameObject>(_normal);
            Super = new PrefabAsset<GameObject>(_super);
        }
    }
}