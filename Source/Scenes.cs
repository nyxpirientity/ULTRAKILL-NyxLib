using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class SceneEvents
    {
        public delegate void OnSceneWasLoadedEventHandler(UnityEngine.SceneManagement.Scene scene, string levelName, string unitySceneName);
        public static event OnSceneWasLoadedEventHandler OnSceneLoad = null;

        public delegate void OnSceneStartEventHandler(UnityEngine.SceneManagement.Scene scene, string levelName, string unitySceneName);
        public static event OnSceneStartEventHandler OnSceneStart = null;

        public delegate void OnSceneWasUnloadedEventHandler(UnityEngine.SceneManagement.Scene scene, string levelName, string unitySceneName);
        public static event OnSceneWasUnloadedEventHandler OnSceneUnload = null;

        internal static void NotifySceneLoad(UnityEngine.SceneManagement.Scene scene, string levelName, string unitySceneName)
        {
            OnSceneLoad?.Invoke(scene, levelName, unitySceneName);

            GameObject go = new GameObject();
            SceneManager.MoveGameObjectToScene(go, scene);
            var startNotif = go.AddComponent<NotifyAndDestroyOnStart>();
            startNotif.OnStart += () => { OnSceneStart?.Invoke(scene, levelName, unitySceneName); };
        }

        internal static void NotifySceneUnload(UnityEngine.SceneManagement.Scene scene, string levelName, string unitySceneName)
        {
            OnSceneUnload?.Invoke(scene, levelName, unitySceneName);
        }

        private class NotifyAndDestroyOnStart : MonoBehaviour
        {
            public event Action OnStart;

            protected void Start()
            {
                OnStart?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}