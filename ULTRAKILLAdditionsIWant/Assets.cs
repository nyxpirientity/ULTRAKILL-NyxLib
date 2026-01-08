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

            return texture;
        }

        public static Texture2D MundaneMurderIcon { get; private set; } = null;
    }
}