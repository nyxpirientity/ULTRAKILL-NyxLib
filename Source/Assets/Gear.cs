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
    public class Gear : MonoSingleton<Gear>
    {
        public static bool AssetsLoaded => (Instance?._assetsLoaded).GetValueOrDefault(false);

        public static PrefabAsset<GameObject> Piercer => new PrefabAsset<GameObject>(() => Instance?._piercer);
        public static PrefabAsset<GameObject> Marksman => new PrefabAsset<GameObject>(() => Instance?._marksman);
        public static PrefabAsset<GameObject> Sharpshooter => new PrefabAsset<GameObject>(() => Instance?._sharpshooter);

        public static PrefabAsset<GameObject> AltPiercer => new PrefabAsset<GameObject>(() => Instance?._altPiercer);
        public static PrefabAsset<GameObject> AltMarksman => new PrefabAsset<GameObject>(() => Instance?._altMarksman);
        public static PrefabAsset<GameObject> AltSharpshooter => new PrefabAsset<GameObject>(() => Instance?._altSharpshooter);


        public static PrefabAsset<GameObject> CoreEject => new PrefabAsset<GameObject>(() => Instance?._coreEject);
        public static PrefabAsset<GameObject> PumpCharge => new PrefabAsset<GameObject>(() => Instance?._pumpCharge);
        public static PrefabAsset<GameObject> SawedOn => new PrefabAsset<GameObject>(() => Instance?._sawedOn);

        public static PrefabAsset<GameObject> AltCoreEject => new PrefabAsset<GameObject>(() => Instance?._altCoreEject);
        public static PrefabAsset<GameObject> AltPumpCharge => new PrefabAsset<GameObject>(() => Instance?._altPumpCharge);
        public static PrefabAsset<GameObject> AltSawedOn => new PrefabAsset<GameObject>(() => Instance?._altSawedOn);


        public static PrefabAsset<GameObject> Attractor => new PrefabAsset<GameObject>(() => Instance?._attractor);
        public static PrefabAsset<GameObject> Overheat => new PrefabAsset<GameObject>(() => Instance?._overheat);
        public static PrefabAsset<GameObject> Jumpstart => new PrefabAsset<GameObject>(() => Instance?._jumpstart);

        public static PrefabAsset<GameObject> AltAttractor => new PrefabAsset<GameObject>(() => Instance?._altAttractor);
        public static PrefabAsset<GameObject> AltOverheat => new PrefabAsset<GameObject>(() => Instance?._altOverheat);
        public static PrefabAsset<GameObject> AltJumpstart => new PrefabAsset<GameObject>(() => Instance?._altJumpstart);


        public static PrefabAsset<GameObject> ElectricRailcannon => new PrefabAsset<GameObject>(() => Instance?._electricRailcannon);
        public static PrefabAsset<GameObject> Screwdriver => new PrefabAsset<GameObject>(() => Instance?._screwdriver);
        public static PrefabAsset<GameObject> MaliciousRailcannon => new PrefabAsset<GameObject>(() => Instance?._maliciousRailcannon);


        public static PrefabAsset<GameObject> FreezeFrame => new PrefabAsset<GameObject>(() => Instance?._freezeframe);
        public static PrefabAsset<GameObject> SRSCannon => new PrefabAsset<GameObject>(() => Instance?._srsCannon);
        public static PrefabAsset<GameObject> Firestarter => new PrefabAsset<GameObject>(() => Instance?._firestarter);

        public static class AssetRefs
        {
            /* revolvers :3 */
            public static AssetReference Piercer => GunSetter.Instance.NullInvalid()?.revolverPierce[0];
            public static AssetReference Marksman => GunSetter.Instance.NullInvalid()?.revolverRicochet[0];
            public static AssetReference Sharpshooter => GunSetter.Instance.NullInvalid()?.revolverTwirl[0];

            public static AssetReference AltPiercer => GunSetter.Instance.NullInvalid()?.revolverPierce[1];
            public static AssetReference AltMarksman => GunSetter.Instance.NullInvalid()?.revolverRicochet[1];
            public static AssetReference AltSharpshooter => GunSetter.Instance.NullInvalid()?.revolverTwirl[1];

            /* shotguns :3 */
            public static AssetReference CoreEject => GunSetter.Instance.NullInvalid()?.shotgunGrenade[0];
            public static AssetReference PumpCharge => GunSetter.Instance.NullInvalid()?.shotgunPump[0];
            public static AssetReference SawedOn => GunSetter.Instance.NullInvalid()?.shotgunRed[0];

            public static AssetReference AltCoreEject => GunSetter.Instance.NullInvalid()?.shotgunGrenade[1];
            public static AssetReference AltPumpCharge => GunSetter.Instance.NullInvalid()?.shotgunPump[1];
            public static AssetReference AltSawedOn => GunSetter.Instance.NullInvalid()?.shotgunRed[1];

            /* nailguns :3 */
            public static AssetReference Attractor => GunSetter.Instance.NullInvalid()?.nailMagnet[0];
            public static AssetReference Overheat => GunSetter.Instance.NullInvalid()?.nailOverheat[0];
            public static AssetReference Jumpstart => GunSetter.Instance.NullInvalid()?.nailRed[0];

            public static AssetReference AltAttractor => GunSetter.Instance.NullInvalid()?.nailMagnet[1];
            public static AssetReference AltOverheat => GunSetter.Instance.NullInvalid()?.nailOverheat[1];
            public static AssetReference AltJumpstart => GunSetter.Instance.NullInvalid()?.nailRed[1];

            /* railcannons :3 */
            public static AssetReference ElectricRailcannon => GunSetter.Instance.NullInvalid()?.railCannon[0];
            public static AssetReference Screwdriver => GunSetter.Instance.NullInvalid()?.railHarpoon[0];
            public static AssetReference MaliciousRailcannon => GunSetter.Instance.NullInvalid()?.railMalicious[0];

            /* rocket launchers :3 */
            public static AssetReference Freezeframe => GunSetter.Instance.NullInvalid()?.rocketBlue[0];
            public static AssetReference SRSCannon => GunSetter.Instance.NullInvalid()?.rocketGreen[0];
            public static AssetReference Firestarter => GunSetter.Instance.NullInvalid()?.rocketRed[0];
        }

        [SerializeField] private bool _assetsLoaded = false;

        [SerializeField] private GameObject _piercer = null;
        [SerializeField] private GameObject _marksman = null;
        [SerializeField] private GameObject _sharpshooter = null;

        [SerializeField] private GameObject _altPiercer = null;
        [SerializeField] private GameObject _altMarksman = null;
        [SerializeField] private GameObject _altSharpshooter = null;


        [SerializeField] private GameObject _coreEject = null;
        [SerializeField] private GameObject _pumpCharge = null;
        [SerializeField] private GameObject _sawedOn = null;

        [SerializeField] private GameObject _altCoreEject = null;
        [SerializeField] private GameObject _altPumpCharge = null;
        [SerializeField] private GameObject _altSawedOn = null;


        [SerializeField] private GameObject _attractor = null;
        [SerializeField] private GameObject _overheat = null;
        [SerializeField] private GameObject _jumpstart = null;

        [SerializeField] private GameObject _altAttractor = null;
        [SerializeField] private GameObject _altOverheat = null;
        [SerializeField] private GameObject _altJumpstart = null;


        [SerializeField] private GameObject _electricRailcannon = null;
        [SerializeField] private GameObject _maliciousRailcannon = null;
        [SerializeField] private GameObject _screwdriver = null;


        [SerializeField] private GameObject _freezeframe = null;
        [SerializeField] private GameObject _srsCannon = null;
        [SerializeField] private GameObject _firestarter = null;

        private void Awake()
        {
            SceneEvents.OnSceneStart += OnNewSceneStart;
        }

        private void OnNewSceneStart(Scene scene, string levelName, string unitySceneName)
        {
            if (AssetRefs.Piercer == null || _assetsLoaded)
            {
                return;
            }

            var holder = AssetsRoot.Holder;

            _piercer = GameObject.Instantiate(AssetRefs.Piercer.ToAsset(), holder);
            _marksman = GameObject.Instantiate(AssetRefs.Marksman.ToAsset(), holder);
            _sharpshooter = GameObject.Instantiate(AssetRefs.Sharpshooter.ToAsset(), holder);

            _altPiercer = GameObject.Instantiate(AssetRefs.AltPiercer.ToAsset(), holder);
            _altMarksman = GameObject.Instantiate(AssetRefs.AltMarksman.ToAsset(), holder);
            _altSharpshooter = GameObject.Instantiate(AssetRefs.AltSharpshooter.ToAsset(), holder);


            _coreEject = GameObject.Instantiate(AssetRefs.CoreEject.ToAsset(), holder);
            _pumpCharge = GameObject.Instantiate(AssetRefs.PumpCharge.ToAsset(), holder);
            _sawedOn = GameObject.Instantiate(AssetRefs.SawedOn.ToAsset(), holder);

            _altCoreEject = GameObject.Instantiate(AssetRefs.AltCoreEject.ToAsset(), holder);
            _altPumpCharge = GameObject.Instantiate(AssetRefs.AltPumpCharge.ToAsset(), holder);
            _altSawedOn = GameObject.Instantiate(AssetRefs.AltSawedOn.ToAsset(), holder);


            _attractor = GameObject.Instantiate(AssetRefs.Attractor.ToAsset(), holder);
            _overheat = GameObject.Instantiate(AssetRefs.Overheat.ToAsset(), holder);
            _jumpstart = GameObject.Instantiate(AssetRefs.Jumpstart.ToAsset(), holder);

            _altAttractor = GameObject.Instantiate(AssetRefs.AltAttractor.ToAsset(), holder);
            _altOverheat = GameObject.Instantiate(AssetRefs.AltOverheat.ToAsset(), holder);
            _altJumpstart = GameObject.Instantiate(AssetRefs.AltJumpstart.ToAsset(), holder);


            _electricRailcannon = GameObject.Instantiate(AssetRefs.ElectricRailcannon.ToAsset(), holder);
            _screwdriver = GameObject.Instantiate(AssetRefs.Screwdriver.ToAsset(), holder);
            _maliciousRailcannon = GameObject.Instantiate(AssetRefs.MaliciousRailcannon.ToAsset(), holder);


            _freezeframe = GameObject.Instantiate(AssetRefs.Freezeframe.ToAsset(), holder);
            _srsCannon = GameObject.Instantiate(AssetRefs.SRSCannon.ToAsset(), holder);
            _firestarter = GameObject.Instantiate(AssetRefs.Firestarter.ToAsset(), holder);

            _assetsLoaded = true;
        }
    }
}