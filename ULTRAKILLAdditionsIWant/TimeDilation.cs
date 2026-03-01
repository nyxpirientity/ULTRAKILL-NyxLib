using System;
using MelonLoader;
using UnityEngine;
using HarmonyLib;
using MelonLoader.Utils;
using System.Reflection;
using System.Collections.Generic;
using UKAIW.Diagnostics.Debug;

namespace UKAIW
{
    public static class TimeScale
    {
        private static TimeController _controller = null;
        public static TimeController Controller
        {
            get 
            {
                if (_controller == null)
                {
                    if (TimeController.Instance != null)
                    {
                        Log.ExpectedInfo($"Had to get TimeController via TimeController.Instance (then cached the value)");
                        _controller = TimeController.Instance;
                    }
                }

                return _controller;
            }
        }

        [HarmonyPatch(typeof(TimeController), "Awake", new Type[] { })]
        static class TimeControllerAwakePatch
        {
            public static void Prefix(TimeController __instance)
            {
            }

            public static void Postfix(TimeController __instance)
            {
                _controller = __instance;
            }
        }
        public static bool ModDisableHitstop = false;
    }

    [HarmonyPatch(typeof(TimeController), "TrueStop")]
    static class TrueStopPatch
    {
        public static void Prefix(TimeController __instance, float length)
        {
            if (Cheats.IsCheatEnabled(Cheats.DisableStops) || TimeScale.ModDisableHitstop)
            {
                FieldInfo currentStopFI = __instance.GetType().GetField("currentStop", BindingFlags.NonPublic | BindingFlags.Instance);

                currentStopFI.SetValue(__instance, length + 100.0f);
            }
        }

        public static void Postfix(TimeController __instance, float length)
        {
            if (Cheats.IsCheatEnabled(Cheats.DisableStops)  || TimeScale.ModDisableHitstop)
            {
                FieldInfo currentStopFI = __instance.GetType().GetField("currentStop", BindingFlags.NonPublic | BindingFlags.Instance);

                currentStopFI.SetValue(__instance, 0.0f);
            }
        }
    }

    [HarmonyPatch(typeof(TimeController), "HitStop")]
    static class HitStopPatch
    {
        public static void Prefix(TimeController __instance, float length)
        {
            if (Cheats.IsCheatEnabled(Cheats.DisableStops) || TimeScale.ModDisableHitstop)
            {
                FieldInfo currentStopFI = __instance.GetType().GetField("currentStop", BindingFlags.NonPublic | BindingFlags.Instance);

                currentStopFI.SetValue(__instance, length + 100.0f);
            }
        }

        public static void Postfix(TimeController __instance, float length)
        {
            if (Cheats.IsCheatEnabled(Cheats.DisableStops) || TimeScale.ModDisableHitstop)
            {
                FieldInfo currentStopFI = __instance.GetType().GetField("currentStop", BindingFlags.NonPublic | BindingFlags.Instance);

                currentStopFI.SetValue(__instance, 0.0f);
            }
        }
    }
    
    [HarmonyPatch(typeof(TimeController), "SlowDown")]
    static class SlowDownPatch
    {
        public static void Prefix(TimeController __instance, float amount)
        {
        }

        public static void Postfix(TimeController __instance, float amount)
        {
            if (Cheats.IsCheatEnabled(Cheats.DisableSlowdown))
            {
                FieldInfo slowDownFI = __instance.GetType().GetField("slowDown", BindingFlags.NonPublic | BindingFlags.Instance);

                slowDownFI.SetValue(__instance, 1.0f);
            }
        }
    }    
    
    [HarmonyPatch(typeof(TimeController), "ParryFlash")]
    static class ParryFlashPatch
    {
        public static bool ModShortenHitStop = false;

        public static void Prefix(TimeController __instance)
        {
        }

        public static void Postfix(TimeController __instance)
        {
            if (Cheats.IsCheatEnabled(Cheats.ShortHitStop) || ModShortenHitStop)
            {
                UpdatePatch.RemainingStopTime = 0.125f;
            }
        }
    }    

    [HarmonyPatch(typeof(TimeController), "Update")]
    static class UpdatePatch
    {
        public static float RemainingStopTime = -1.0f;

        public static void Prefix(TimeController __instance)
        {
            if (RemainingStopTime >= 0.0f)
            {
                RemainingStopTime -= Time.unscaledDeltaTime;

                if (RemainingStopTime <= 0.0f)
                {
                    RemainingStopTime = -1.0f;
                   // MethodInfo continueTimeFI = __instance.GetType().GetMethod("ContinueTime", BindingFlags.NonPublic | BindingFlags.Instance);
                    //continueTimeFI.Invoke(__instance, new object[2]{1.0f, true});
                    __instance.RestoreTime();
                }
            }
        }

        public static void Postfix(TimeController __instance)
        {
        }
    }    

    [HarmonyPatch(typeof(AudioSource), "Play", new Type[0])]
    static class AudioPlayPatch
    {
        public static bool Disabled = false;
        public static void Prefix(AudioSource __instance)
        {
        }

        public static void Postfix(AudioSource __instance)
        {
            if (Cheats.IsCheatEnabled(Cheats.UltraStop) && !AudioPlayPatch.Disabled)
            {
                Disabled = true;
                TimeScale.Controller.ParryFlash();
                Disabled = false;
            }
        }
    }

    [HarmonyPatch(typeof(AudioSource), "Play", new Type[1] {typeof(float)})]
    static class AudioPlayDFPatch
    {
        public static void Prefix(AudioSource __instance, float delay)
        {
        }

        public static void Postfix(AudioSource __instance, float delay)
        {
            if (Cheats.IsCheatEnabled(Cheats.UltraStop) && !AudioPlayPatch.Disabled)
            {
                AudioPlayPatch.Disabled = true;
                TimeScale.Controller.ParryFlash();
                AudioPlayPatch.Disabled = false;
            }
        }
    }

    [HarmonyPatch(typeof(AudioSource), "Play", new Type[1] {typeof(ulong)})]
    static class AudioPlayDUPatch
    {
        public static void Prefix(AudioSource __instance, ulong delay)
        {
        }

        public static void Postfix(AudioSource __instance, ulong delay)
        {
            if (Cheats.IsCheatEnabled(Cheats.UltraStop) && !AudioPlayPatch.Disabled)
            {
                AudioPlayPatch.Disabled = true;
                TimeScale.Controller.ParryFlash();
                AudioPlayPatch.Disabled = false;
            }
        }
    }

    [HarmonyPatch(typeof(AudioSource), "PlayDelayed", new Type[1] {typeof(float)})]
    static class AudioPlayDDFPatch
    {
        public static void Prefix(AudioSource __instance, float delay)
        {
        }

        public static void Postfix(AudioSource __instance, float delay)
        {
            if (Cheats.IsCheatEnabled(Cheats.UltraStop) && !AudioPlayPatch.Disabled)
            {
                AudioPlayPatch.Disabled = true;
                TimeScale.Controller.ParryFlash();
                AudioPlayPatch.Disabled = false;
            }
        }
    }
    [HarmonyPatch(typeof(AudioSource), "PlayScheduled", new Type[1] {typeof(double)})]
    static class AudioPlaySDPatch
    {
        public static void Prefix(AudioSource __instance, double time)
        {
        }

        public static void Postfix(AudioSource __instance, double time)
        {
            if (Cheats.IsCheatEnabled(Cheats.UltraStop) && !AudioPlayPatch.Disabled)
            {
                AudioPlayPatch.Disabled = true;
                TimeScale.Controller.ParryFlash();
                AudioPlayPatch.Disabled = false;
            }
        }
    }

    [HarmonyPatch(typeof(AudioSource), "PlayOneShot", new Type[1] {typeof(AudioClip)})]
    static class AudioPlayDOSPatch
    {
        public static void Prefix(AudioSource __instance, AudioClip clip)
        {
        }

        public static void Postfix(AudioSource __instance, AudioClip clip)
        {
            if (Cheats.IsCheatEnabled(Cheats.UltraStop) && !AudioPlayPatch.Disabled)
            {
                AudioPlayPatch.Disabled = true;
                TimeScale.Controller.ParryFlash();
                AudioPlayPatch.Disabled = false;
            }
        }
    }
}