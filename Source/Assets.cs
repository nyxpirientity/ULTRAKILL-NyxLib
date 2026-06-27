using System.IO;
using HarmonyLib;
using TMPro;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class Assets
    {
        public static GameObject LabelPrefab { get; private set; } = null;

        public static GameObject HarmlessExplosionPrefab { get; private set; } = null;
        public static GameObject ExplosionPrefab { get; private set; } = null;
        public static GameObject SuperExplosionPrefab { get; private set; } = null;

        [Obsolete]
        public static GameObject RocketPrefab { get; private set; } = null;
        public static GameObject MortarPrefab { get; private set; } = null;
        public static GameObject HomingProjectilePrefab { get; private set; } = null;

        public static class Projectiles
        {
            public static GameObject PlayerRocket { get; internal set; } = null;
            public static GameObject Core { get; internal set; } = null;
        }

        public static class HookPoints
        {
            public static GameObject HealingHookPoint { get; internal set; } = null;
            public static GameObject SlingshotHookPoint { get; internal set; } = null;
            public static GameObject NormalHookPoint { get; internal set; } = null;
        }

        public static class Gear
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
            public static AssetReference SawedOff => GunSetter.Instance.NullInvalid()?.shotgunRed[0];

            public static AssetReference AltCoreEject => GunSetter.Instance.NullInvalid()?.shotgunGrenade[1];
            public static AssetReference AltPumpCharge => GunSetter.Instance.NullInvalid()?.shotgunPump[1];
            public static AssetReference AltSawedOff => GunSetter.Instance.NullInvalid()?.shotgunRed[1];

            /* nailguns :3 */
            public static AssetReference Attractor => GunSetter.Instance.NullInvalid()?.nailMagnet[0];
            public static AssetReference Overheat => GunSetter.Instance.NullInvalid()?.nailOverheat[0];
            public static AssetReference Jumpstart => GunSetter.Instance.NullInvalid()?.nailRed[0];

            public static AssetReference AltAttractor => GunSetter.Instance.NullInvalid()?.nailMagnet[1];
            public static AssetReference AltOverheat => GunSetter.Instance.NullInvalid()?.nailOverheat[1];
            public static AssetReference AltJumpstart => GunSetter.Instance.NullInvalid()?.nailRed[1];

            /* railcannons :3 */
            public static AssetReference ElectricRailCannon => GunSetter.Instance.NullInvalid()?.railCannon[0];
            public static AssetReference Screwdriver => GunSetter.Instance.NullInvalid()?.railHarpoon[0];
            public static AssetReference MaliciousRailCannon => GunSetter.Instance.NullInvalid()?.railMalicious[0];

            /* rocket launchers :3 */
            public static AssetReference FreezeFrame => GunSetter.Instance.NullInvalid()?.rocketBlue[0];
            public static AssetReference SRSCannon => GunSetter.Instance.NullInvalid()?.rocketGreen[0];
            public static AssetReference Firestarter => GunSetter.Instance.NullInvalid()?.rocketRed[0];
        }

        public static void AddAssetPicker<ObjectType>(Func<ObjectType, bool> pickerFunc) where ObjectType : UnityEngine.Object
        {
            Func<Scene, bool> picker = (scene) =>
            {
                var assetHolders = UnityEngine.Object.FindObjectsByType<ObjectType>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                List<ObjectType> assetHoldersComps = new List<ObjectType>();

                if (typeof(ObjectType).IsSubclassOf(typeof(Component)) || typeof(ObjectType) == typeof(Component))
                {
                    var gos = scene.GetRootGameObjects();

                    foreach (var go in gos)
                    {
                        assetHoldersComps.AddRange(go.GetComponentsInChildren<ObjectType>());
                    }
                }

                if (assetHoldersComps != null)
                {
                    foreach (var assetHolder in assetHoldersComps)
                    {
                        if (pickerFunc(assetHolder))
                        {
                            return true;
                        }
                    }
                }

                if (assetHolders != null)
                {
                    foreach (var assetHolder in assetHolders)
                    {
                        if (pickerFunc(assetHolder))
                        {
                            return true;
                        }
                    }
                }

                return false;
            };

            _assetPickers.Add(picker);
        }

        public static SpawnMenu TryGetSpawnMenu()
        {
            if (CanvasController.Instance == null)
            {
                return null;
            }

            return CanvasController.Instance.GetComponentInChildren<SpawnMenu>(includeInactive: true);
        }

        public static SpawnableObjectsDatabase TryGetSpawnableObjectsDb()
        {
            SpawnMenu spawnMenu = TryGetSpawnMenu();

            if (spawnMenu == null)
            {
                return null;
            }

            return spawnableObjectsDbFA.GetValue(spawnMenu);
        }

        private static List<Func<Scene, bool>> _assetPickers = new List<Func<Scene, bool>>(64);

        private static void OnNewScene(Scene scene)
        {
            if (LabelPrefab == null)
            {
                var hc = HudController.Instance;

                if (hc != null)
                {
                    var text = hc.speedometer.textMesh;
                    LabelPrefab = GameObject.Instantiate(text.gameObject, PrefabHolder.transform);
                    LabelPrefab.SetActive(false);
                    UnityEngine.Object.DontDestroyOnLoad(LabelPrefab);
                    LabelPrefab.GetComponent<TextMeshProUGUI>().text = "UKAIW-Label!";
                }
            }

            if (RocketPrefab == null)
            {
                var possibleGrenades = UnityEngine.Object.FindObjectsByType<Grenade>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var grenade in possibleGrenades)
                {
                    if (grenade.rocket)
                    {
                        RocketPrefab = GameObject.Instantiate(grenade.gameObject);
                        GameObject.DontDestroyOnLoad(RocketPrefab);
                        RocketPrefab.SetActive(false);
                    }

                    if (HarmlessExplosionPrefab == null && grenade.harmlessExplosion != null)
                    {
                        HarmlessExplosionPrefab = GameObject.Instantiate(grenade.harmlessExplosion);
                        GameObject.DontDestroyOnLoad(HarmlessExplosionPrefab);
                        HarmlessExplosionPrefab.SetActive(false);
                        HarmlessExplosionPrefab.GetOrAddComponent<ExplosionAdditions>();
                    }

                    if (ExplosionPrefab == null && grenade.explosion != null)
                    {
                        ExplosionPrefab = GameObject.Instantiate(grenade.explosion);
                        GameObject.DontDestroyOnLoad(ExplosionPrefab);
                        ExplosionPrefab.SetActive(false);
                        ExplosionPrefab.GetOrAddComponent<ExplosionAdditions>();
                    }

                    if (SuperExplosionPrefab == null && grenade.superExplosion != null)
                    {
                        SuperExplosionPrefab = GameObject.Instantiate(grenade.superExplosion);
                        GameObject.DontDestroyOnLoad(SuperExplosionPrefab);
                        SuperExplosionPrefab.SetActive(false);
                        SuperExplosionPrefab.GetOrAddComponent<ExplosionAdditions>();
                    }

                    if (SuperExplosionPrefab != null && ExplosionPrefab != null && HarmlessExplosionPrefab != null && RocketPrefab != null)
                    {
                        break;
                    }
                }
            }

            GameObject startNotifier = new GameObject();
            startNotifier.AddComponent<NotifyAndDestroyOnStart>().OnStart += () => { TryLoadAssetsLate(scene); };

            if (MortarPrefab == null)
            {
                var possibleHideousMass = UnityEngine.Object.FindAnyObjectByType<Mass>(FindObjectsInactive.Include);

                if (possibleHideousMass != null)
                {
                    MortarPrefab = GameObject.Instantiate(possibleHideousMass.explosiveProjectile);
                    GameObject.DontDestroyOnLoad(MortarPrefab);
                    MortarPrefab.SetActive(false);

                    ExplosionPrefab = GameObject.Instantiate(MortarPrefab.GetComponent<Projectile>().explosionEffect);
                    GameObject.DontDestroyOnLoad(ExplosionPrefab);
                    ExplosionPrefab.SetActive(false);
                    ExplosionPrefab.GetOrAddComponent<ExplosionAdditions>();

                    HomingProjectilePrefab = GameObject.Instantiate(possibleHideousMass.homingProjectile);
                    GameObject.DontDestroyOnLoad(HomingProjectilePrefab);
                    HomingProjectilePrefab.SetActive(false);
                }
                else
                {
                    Log.ExpectedInfo($"We'd like a a hideous mass in order to yoink it's projectile prefabs, but this scene \"{SceneHelper.CurrentScene}\" didn't have it yet!");
                }
            }

            TryGetSpawnMenuPrefabs();
        }

        private static void TryRunAssetPickers(Scene scene)
        {
            if (_assetPickers.Count == 0)
            {
                return;
            }

            for (int i = 0; i < _assetPickers.Count; i++)
            {
                Func<Scene, bool> picker = _assetPickers[i];

                try
                {
                    if (picker(scene))
                    {
                        _assetPickers.RemoveAt(i);
                        i -= 1;
                    }
                }
                catch (System.Exception e)
                {
                    Log.Error($"Caught {e.GetType()} whilst trying to execute an asset picker!\n{e}\n");
                }
            }

            if (_assetPickers.Count == 0)
            {
                Log.TraceExpectedInfo($"[Assets] Success! It seems! All asset pickers seem to be satisfied.");
            }
        }

        private static void TryLoadAssetsLate(Scene scene)
        {
            TryRunAssetPickers(scene);

            if (Projectiles.PlayerRocket == null && Gear.Firestarter != null)
            {
                var fs = Gear.Firestarter.ToAsset().GetComponent<RocketLauncher>();
                var ce = Gear.CoreEject.ToAsset().GetComponent<Shotgun>();
                Projectiles.PlayerRocket = GameObject.Instantiate(fs.rocket, PrefabHolder.transform);
                Projectiles.Core = GameObject.Instantiate(ce.grenade, PrefabHolder.transform);
            }
        }

        private static FieldAccess<SpawnMenu, SpawnableObjectsDatabase> spawnableObjectsDbFA = new FieldAccess<SpawnMenu, SpawnableObjectsDatabase>("objects");
        private static bool _spawnMenuPrefabsGotten = false;
        private static void TryGetSpawnMenuPrefabs()
        {
            var db = TryGetSpawnableObjectsDb();

            if (db == null)
            {
                return;
            }

            _spawnMenuPrefabsGotten = true;

            foreach (var obj in db.sandboxObjects)
            {
                var hookPoint = obj.gameObject.GetComponentInChildren<HookPoint>();

                if (hookPoint != null)
                {
                    switch (hookPoint.type)
                    {
                        case hookPointType.Normal:
                            HookPoints.NormalHookPoint = GameObject.Instantiate(obj.gameObject, PrefabHolder.transform);
                            HookPoints.NormalHookPoint.SetActive(false);
                            break;
                        case hookPointType.Slingshot:
                            if (hookPoint.healPlayer)
                            {
                                HookPoints.HealingHookPoint = GameObject.Instantiate(obj.gameObject, PrefabHolder.transform);
                                HookPoints.HealingHookPoint.SetActive(false);
                            }
                            else
                            {
                                HookPoints.SlingshotHookPoint = GameObject.Instantiate(obj.gameObject, PrefabHolder.transform);
                                HookPoints.SlingshotHookPoint.SetActive(false);
                            }
                            break;
                        case hookPointType.Switch:
                            break;
                    }
                }
            }
        }

        internal static void Initialize()
        {
            ScenesEvents.OnSceneWasLoaded += OnSceneWasLoaded;
            LevelQuickLoader.OnQuickLoad += OnNewScene;
        }

        private static void OnSceneWasLoaded(Scene scene, string levelName, string unitySceneName)
        {
            OnNewScene(scene);
        }

        private static GameObject _prefabHolder = null;
        public static GameObject PrefabHolder
        {
            get
            {
                if (_prefabHolder == null)
                {
                    _prefabHolder = new GameObject();
                    GameObject.DontDestroyOnLoad(_prefabHolder);
                    _prefabHolder.SetActive(false);
                }

                return _prefabHolder;
            }
        }

        private class NotifyAndDestroyOnStart : MonoBehaviour
        {
            public event Action OnStart;

            protected void Start()
            {
                OnStart?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}