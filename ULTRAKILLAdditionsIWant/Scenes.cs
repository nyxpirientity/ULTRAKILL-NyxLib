using System;
using UnityEngine;

namespace UKAIW
{
    public static class ScenesEvents
    {
        public static Action<int, string> OnSceneWasLoaded = null;
        public static Action<int, string> OnSceneWasInitialized = null;
        public static Action<int, string> OnSceneWasUnloaded = null;
    }
}