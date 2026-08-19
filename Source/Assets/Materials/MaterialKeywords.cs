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

public static class MaterialKeywords
{
    public const string StereoInstancingOn = "STEREO_INSTANCING_ON";
    public const string UnitySinglePassStereo = "UNITY_SINGLE_PASS_STEREO";
    public const string StereoMultiviewOn = "STEREO_MULTIVIEW_ON";
    public const string StereoCubemapRenderOn = "STEREO_CUBEMAP_RENDER_ON";
    public const string VertexLighting = "VERTEX_LIGHTING";
    public const string VertexWarping = "VERTEX_WARPING";
    public const string PortalLights = "PORTAL_LIGHTS";
    public const string PortalClipPlane = "PORTAL_CLIP_PLANE";
    public const string Rain = "RAIN";
    public const string Burning = "BURNING";
    public const string FogOn = "_FOG_ON";
    public const string FogOff = "_FOG_OFF";
    public const string FogTransparent = "_FOG_TRANSPARENT";
    public const string AlphaTest = "ALPHA_TEST";
    public const string AnimatedTexture = "ANIMATED_TEXTURE";
    public const string BloodFIller = "BLOOD_FILLER";
    public const string BloodAbsorber = "BLOOD_ABSORBER";
    public const string Billboard = "BILLBOARD";
    public const string Caustics = "CAUSTICS";
    public const string CyberGrind = "CYBER_GRIND";
    public const string CustomColors = "CUSTOM_COLORS";
    public const string CustomLightmap = "CUSTOM_LIGHTMAP";
    public const string DistanceFade = "DISTANCE_FADE";
    public const string Enemy = "ENEMY";
    public const string Emissive = "EMISSIVE";
    public const string FadeableAmbient = "FADEABLE_AMBIENT";
    public const string Fresnel = "FRESNEL";
    public const string LimboWalls = "LIMBO_WALLS";
    public const string LimboWaterFade = "LIMBO_WATER_FADE";
    public const string NoTextureWarping = "NO_TEXTURE_WARPING";
    public const string Radiance = "RADIANCE";
    public const string Reflection = "REFLECTION";
    public const string Sparkles = "SPARKLES";
    public const string Transparency = "TRANSPARENCY";
    public const string Transmission = "TRANSMISSION";
    public const string TransparentFog = "TRANSPARENT_FOG";
    public const string VertexDisplacement = "VERTEX_DISPLACEMENT";
    public const string VertexBlending = "VERTEX_BLENDING";
    public const string Fooled = "FOOLED";
    public const string AllowMipmaps = "ALLOW_MIPMAPS";
    public const string Scrolling = "SCROLLING";
}