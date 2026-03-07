
using System;
using HarmonyLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;

public static class GamePrefs
{
    private static PrefsManager _manager = null;
    public static PrefsManager Manager
    {
        get 
        {
            if (_manager == null)
            {
                if (PrefsManager.Instance != null)
                {
                    Log.ExpectedInfo($"Had to get PrefsManager via PrefsManager.Instance (then cached the value)");
                    _manager = PrefsManager.Instance;
                }
            }

            return _manager;
        }
    }

    [HarmonyPatch(typeof(PrefsManager), "Awake", new Type[] { })]
    static class PrefsManagerAwakePatch
    {
        public static void Prefix(PrefsManager __instance)
        {
        }

        public static void Postfix(PrefsManager __instance)
        {
            _manager = __instance;
        }
    }
}