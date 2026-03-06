using System;

namespace UKAIW
{
    public static class ScenesEvents
    {
        public static Action<UnityEngine.SceneManagement.Scene, string> OnSceneWasLoaded = null;
        public static Action<UnityEngine.SceneManagement.Scene, string> OnSceneWasUnloaded = null;
    }
}