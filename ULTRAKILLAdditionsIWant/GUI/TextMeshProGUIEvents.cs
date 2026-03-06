using System;
using HarmonyLib;
using TMPro;

namespace UKAIW
{
    public static class TextMeshProUGUIEvents
    {
        public static Action<TextMeshProUGUI> PreEnable = null;
        public static Action<TextMeshProUGUI> PostEnable = null;

        public static Action<TextMeshProUGUI> PreDisable = null;
        public static Action<TextMeshProUGUI> PostDisable = null;

        [HarmonyPatch(typeof(TextMeshProUGUI), "OnEnable")]
        static class TextMeshProUGUIEnablePatch
        {
            public static void Prefix(TextMeshProUGUI __instance)
            {
                PreEnable?.Invoke(__instance);
            }

            public static void Postfix(TextMeshProUGUI __instance)
            {
                PostEnable?.Invoke(__instance);
            }
        }   

        [HarmonyPatch(typeof(TextMeshProUGUI), "OnDisable")]
        static class TextMeshProUGUIDisablePatch
        {
            public static void Prefix(TextMeshProUGUI __instance)
            {
                PreDisable?.Invoke(__instance);
            }

            public static void Postfix(TextMeshProUGUI __instance)
            {
                PostDisable?.Invoke(__instance);
            }
        }   
    }
}