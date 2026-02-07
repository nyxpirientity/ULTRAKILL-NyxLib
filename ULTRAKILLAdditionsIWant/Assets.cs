using System;
using System.IO;
using HarmonyLib;
using MelonLoader;
using MelonLoader.Utils;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

namespace UKAIW
{
    public static class Assets
    {
        public static Texture2D MundaneMurderIcon { get; private set; } = null;

        public static GameObject HuskEnrageSound_0 { get; private set; } = null;
        public static GameObject MachineEnrageSound_0 { get; private set; } = null;
        public static AudioClip MachineEnrageSound_1 { get; private set; } = null;

        public static GameObject HeatResistancePrefab { get; private set; } = null;
        public static GameObject ExplosionPrefab { get; private set; } = null;

        public static void Load()
        {
            Log.TraceExpectedInfo($"Assets.Load called!");
            var modsDir = MelonEnvironment.ModsDirectory;
            var assetsDir = $"{modsDir}/ukaiw_assets";
            Log.TraceExpectedInfo($"Loading assets in {assetsDir}!");
            
            MundaneMurderIcon = LoadTexture($"{assetsDir}/mundane_murder.png");

            EnemyEvents.PostStart += EnemyStart;
            ScenesEvents.OnSceneWasLoaded += OnSceneWasLoaded;
        }

        private static void OnSceneWasLoaded(int sceneIdx, string sceneName)
        {
            if (HeatResistancePrefab == null)
            {
                var possibleHeatResistance = UnityEngine.Object.FindAnyObjectByType<HeatResistance>(FindObjectsInactive.Include);
                if (possibleHeatResistance != null)
                {
                    Log.ExpectedInfo($"Heat Resistance object found in scene \"{SceneHelper.CurrentScene}\" yoinking it (instantiating/cloning it) as a prefab!");
                    HeatResistancePrefab = UnityEngine.Object.Instantiate(possibleHeatResistance.gameObject.transform.parent.gameObject, null, false);
                    HeatResistancePrefab.SetActive(false);
                    UnityEngine.Object.DontDestroyOnLoad(HeatResistancePrefab);
                }
                else
                {
                    Log.ExpectedInfo($"We'd like a heat resistence prefab, but this scene \"{SceneHelper.CurrentScene}\" didn't have it yet!");
                }
            }
        }

        private static void EnemyStart(EnemyIdentifier eid, GameObject go)
        {
            switch (eid.enemyType)
            {
                case EnemyType.Swordsmachine:
                if (MachineEnrageSound_0 == null)
                {
                    MachineEnrageSound_0 = UnityEngine.Object.Instantiate(eid.GetComponent<SwordsMachine>().bigPainSound, null, false);
                    MachineEnrageSound_0.SetActive(false);
                    UnityEngine.Object.DontDestroyOnLoad(MachineEnrageSound_0);
                    if (MachineEnrageSound_0 != null)
                    {
                        Log.TraceExpectedInfo($"Yoink, thanks {go.name}, your enrage sound is mine and has been copied :) (you'll still keep yours though probably!)");
                    }
                }
                break;
                case EnemyType.Cerberus:
                if (HuskEnrageSound_0 == null && eid.GetComponent<StatueBoss>() != null)
                {
                    if (eid.GetComponent<StatueBoss>().statueChargeSound2 != null)
                    {
                        HuskEnrageSound_0 = UnityEngine.Object.Instantiate(eid.GetComponent<StatueBoss>().statueChargeSound2, null, false);
                        HuskEnrageSound_0.SetActive(false);
                        UnityEngine.Object.DontDestroyOnLoad(HuskEnrageSound_0);
                        if (HuskEnrageSound_0 != null)
                        {
                            Log.TraceExpectedInfo($"Yoink, thanks {go.name}, your enrage sound is mine and has been copied :) (you'll still keep yours though probably!)");
                        }
                    }
                }
                break;

                case EnemyType.Streetcleaner:
                if (MachineEnrageSound_1 == null)
                {
                    MachineEnrageSound_1 = UnityEngine.Object.Instantiate(eid.machine.deathSound);
                }
                break;
            }
        }

        private static Texture2D LoadTexture(string path)
        {
            Log.ExpectedInfo($"Attempting to load texture from '{path}'");
            var fileBytes = File.ReadAllBytes(path);
            var texture = new Texture2D(1, 1);
            
            if (fileBytes != null)
            {
                try
                {
                    if (!ImageConversion.LoadImage(texture, fileBytes))
                    {
                        MelonLogger.Error($"Failed to load asset '{path}'");
                    }   
                }
                catch (System.Exception e)
                {
                    MelonLogger.Error($"Failed to load asset '{path}' error/exception: {e}");
                    texture = new Texture2D(1, 1);
                }
            }

            Log.ExpectedInfo($"Seemingly successfully loaded asset at '{path}'");
            return texture;
        }

        [HarmonyPatch(typeof(ExplosionController), "Start")]
        static class ExplosionAwakePatch
        {
            public static void Prefix(ExplosionController __instance)
            {
     
            }

            public static void Postfix(ExplosionController __instance)
            {
                if (ExplosionPrefab == null)
                {
                    Log.TraceExpectedInfo($"Yoinking (instantiating/cloning) explosion prefab {__instance.gameObject.name} thats about to start, as we needed an ExplosionPrefab!");
                    ExplosionPrefab = UnityEngine.GameObject.Instantiate(__instance.gameObject, null, false);
                    ExplosionPrefab.SetActive(false);
                    Explosion explosion = ExplosionPrefab.GetComponentInChildren<Explosion>();
                    ExplosionPrefab.GetComponentInChildren<ExplosionController>().enabled = true;
                    explosion.damage = 0;
                    explosion.enemy = false;
                    explosion.harmless = true;
                    explosion.lowQuality = false;
                    explosion.speed = 1.0f;
                    explosion.maxSize = 1.0f;
                    explosion.enemyDamageMultiplier = 1.0f;
                    explosion.playerDamageOverride = -1;
                    explosion.ignite = true;
                    explosion.friendlyFire = false;
                    explosion.isFup = false;
                    explosion.hitterWeapon = "";
                    explosion.halved = false;
                    explosion.canHit = AffectedSubjects.All;
                    explosion.originEnemy = null;
                    explosion.rocketExplosion = false;
                    explosion.toIgnore = new System.Collections.Generic.List<EnemyType>();
                    explosion.ultrabooster = false;
                    explosion.unblockable = false;
                    explosion.electric = false;
                    explosion.enabled = true;
                    UnityEngine.Object.DontDestroyOnLoad(ExplosionPrefab);
                }
            }
        }
    }
}