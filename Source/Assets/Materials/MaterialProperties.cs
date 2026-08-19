using System.IO;
using HarmonyLib;
using TMPro;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Rendering;

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets;

public static class MaterialProperties
{
    public static class Vectors
    {
        public const string Color = "_Color";
        public const string EmissiveColor = "_EmissiveColor";
        public const string CustomColor1 = "_CustomColor1";
        public const string CustomColor2 = "_CustomColor2";
        public const string CustomColor3 = "_CustomColor3";
    }

    public static class Textures
    {
        public const string MainTex = "_MainTex";
        public const string EmissiveTex = "_EmissiveTex";
        public const string IDTex = "_IDTex";
        public const string BlendTex = "_BlendTex";
        public const string CubeTex = "_CubeTex";
        public const string DitherTexture = "_DitherTexture";
        public const string NoiseTex = "_NoiseTex";
        public const string Perlin = "_Perlin";
        public const string VertexNoiseTex = "_VertexNoiseTex";
    }

    public static class Floats
    {
        public const string Opacity = "_Opacity";
    }
}