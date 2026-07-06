using System.IO;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets
{
    public static class ImageLoader
    {
        public static bool TryLoadImage(Texture2D texture, string path)
        {
            try
            {
                var fileBytes = File.ReadAllBytes(path);

                if (!ImageConversion.LoadImage(texture, fileBytes))
                {
                    Log.Error($"Failed to parse image texture at '{path}'");
                    return false;
                }

                Log.TraceExpectedInfo($"Seemingly successfully loaded texture at '{path}'");
                return true;
            }
            catch (System.Exception e)
            {
                Log.Error($"Failed to load texture at '{path}' error/exception: {e}");
                return false;
            }
        }

        public static Texture2D TryLoadImage(string path)
        {
            Log.TraceExpectedInfo($"Attempting to load texture from '{path}'");
            var texture = new Texture2D(1, 1);
            texture.filterMode = FilterMode.Point;

            if (!TryLoadImage(texture, path))
            {
                UnityEngine.Object.Destroy(texture);
                return null;
            }

            return texture;
        }

        public static void TryLoadImageOrDefault(string path, out Texture2D texture, out bool successfullyLoaded)
        {
            Log.TraceExpectedInfo($"Attempting to load texture from '{path}'");
            texture = new Texture2D(1, 1);
            texture.filterMode = FilterMode.Point;

            if (!TryLoadImage(texture, path))
            {
                texture = new Texture2D(4, 4);
                texture.SetPixels(new Color[4 * 4]
                {
                    Color.black, Color.magenta, Color.black, Color.magenta,
                    Color.magenta, Color.black, Color.magenta, Color.black,
                    Color.black, Color.magenta, Color.black, Color.magenta,
                    Color.magenta, Color.black, Color.magenta, Color.black,
                });

                successfullyLoaded = false;
                return;
            }
            else
            {
                successfullyLoaded = true;
                return;
            }
        }
    }
}