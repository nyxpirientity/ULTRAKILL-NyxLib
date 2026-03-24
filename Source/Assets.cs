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

        private static GameObject FleshPrisonPrefab { get; set; } = null;
        private static GameObject FleshPanopticonPrefab { get; set; } = null;
        
        public static void EnableExplosionsPicking()
        {
            LevelQuickLoader.AddQuickLoadLevel("uk_construct");
        }

        public static void EnableProjectilePicking()
        {
            LevelQuickLoader.AddQuickLoadLevel("Endless");
        }

        public static void AddAssetPicker<ObjectType>(Func<ObjectType, bool> pickerFunc) where ObjectType : UnityEngine.Object
        {
            Func<bool> picker = () =>
            {
                var assetHolder = UnityEngine.Object.FindAnyObjectByType<ObjectType>(FindObjectsInactive.Include);
                
                if (assetHolder == null)
                {
                    return false;
                }

                return pickerFunc(assetHolder);
            };

            _assetPickers.Add(picker);
        }

        private static List<Func<bool>> _assetPickers = new List<Func<bool>>(64);

        private static void OnSceneWasLoaded(Scene scene, string sceneName)
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

            if (LabelPrefab == null)
            {
                var possibleHeatResistance = UnityEngine.Object.FindAnyObjectByType<HeatResistance>(FindObjectsInactive.Include);
                if (possibleHeatResistance != null)
                {
                    TextMeshProUGUI textMesh = null;
                    var textMeshProGuis = possibleHeatResistance.gameObject.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
                    foreach (var elem in textMeshProGuis)
                    {
                        if (elem.text.Contains("COMPROMISED"))
                        {
                            textMesh = elem;
                            break;
                        }
                    }
                    
                    Assert.IsNotNull(textMesh);

                    LabelPrefab = UnityEngine.Object.Instantiate(textMesh.gameObject);
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

            if (FleshPrisonPrefab == null || FleshPanopticonPrefab == null)
            {
                var possibleFleshPrison = UnityEngine.Object.FindAnyObjectByType<FleshPrison>(FindObjectsInactive.Include);
                
                if (possibleFleshPrison != null)
                {
                    if (possibleFleshPrison.altVersion)
                    {
                        FleshPanopticonPrefab = GameObject.Instantiate(possibleFleshPrison.gameObject);
                        GameObject.DontDestroyOnLoad(FleshPanopticonPrefab);
                    }
                    else
                    {
                        FleshPrisonPrefab = GameObject.Instantiate(possibleFleshPrison.gameObject);
                        GameObject.DontDestroyOnLoad(FleshPrisonPrefab);
                    }
                }
                else
                {
                    Log.ExpectedInfo($"We'd like a flesh prison in order to yoink it as a prefab, but this scene \"{SceneHelper.CurrentScene}\" didn't have it yet!");
                }
            }
        }

        internal static void Initialize()
        {
            ScenesEvents.OnSceneWasLoaded += OnSceneWasLoaded;
            LevelQuickLoader.AddQuickLoadLevel("Level 0-E");
        }
    }
}