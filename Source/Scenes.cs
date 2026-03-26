using System;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class ScenesEvents
    {
        public delegate void OnSceneWasLoadedEventHandler(UnityEngine.SceneManagement.Scene scene, string levelName, string unitySceneName);
        public static event OnSceneWasLoadedEventHandler OnSceneWasLoaded = null;

        public delegate void OnSceneWasUnloadedEventHandler(UnityEngine.SceneManagement.Scene scene, string levelName, string unitySceneName);
        public static event OnSceneWasUnloadedEventHandler OnSceneWasUnloaded = null;

        internal static void NotifySceneWasLoaded(UnityEngine.SceneManagement.Scene scene, string levelName, string unitySceneName)
        {
            OnSceneWasLoaded?.Invoke(scene, levelName, unitySceneName);
        }

        internal static void NotifySceneWasUnloaded(UnityEngine.SceneManagement.Scene scene, string levelName, string unitySceneName)
        {
            OnSceneWasLoaded?.Invoke(scene, levelName, unitySceneName);
        }
    }
}