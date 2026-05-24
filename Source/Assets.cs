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

        public static GameObject RocketPrefab { get; private set; } = null;
        public static GameObject MortarPrefab { get; private set; } = null;
        public static GameObject HomingProjectilePrefab { get; private set; } = null;

        public static class HookPoints
        {
            public static GameObject HealingHookPoint { get; internal set; } = null;
            public static GameObject SlingshotHookPoint { get; internal set; } = null;
            public static GameObject NormalHookPoint { get; internal set; } = null;
        }

        public static void AddAssetPicker<ObjectType>(Func<ObjectType, bool> pickerFunc) where ObjectType : UnityEngine.Object
        {
            Func<bool> picker = () =>
            {
                var assetHolders = UnityEngine.Object.FindObjectsByType<ObjectType>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                
                if (assetHolders == null)
                {
                    return false;
                }

                foreach (var assetHolder in assetHolders)
                {
                    if (pickerFunc(assetHolder))
                    {
                        return true;
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

        private static List<Func<bool>> _assetPickers = new List<Func<bool>>(64);

        private static void OnSceneWasLoaded(Scene scene, string levelName, string unitySceneName)
        {
            for (int i = 0; i < _assetPickers.Count; i++)
            {
                Func<bool> picker = _assetPickers[i];
                
                try
                {
                    if (picker())
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
    }
}