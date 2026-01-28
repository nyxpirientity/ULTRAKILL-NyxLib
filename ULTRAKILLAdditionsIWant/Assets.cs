using System;
using System.IO;
using MelonLoader;
using MelonLoader.Utils;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

namespace UKAIW
{
    public static class Assets
    {
        public static void Load()
        {
            Log.TraceExpectedInfo($"Assets.Load called!");
            var modsDir = MelonEnvironment.ModsDirectory;
            var assetsDir = $"{modsDir}/ukaiw_assets";
            Log.TraceExpectedInfo($"Loading assets in {assetsDir}!");
            
            MundaneMurderIcon = LoadTexture($"{assetsDir}/mundane_murder.png");

            EnemyEvents.PostStart += EnemyStart;
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

        public static Texture2D MundaneMurderIcon { get; private set; } = null;

        public static GameObject HuskEnrageSound_0 { get; private set; } = null;
        public static GameObject MachineEnrageSound_0 { get; private set; } = null;
        public static AudioClip MachineEnrageSound_1 { get; private set; } = null;
    }
}