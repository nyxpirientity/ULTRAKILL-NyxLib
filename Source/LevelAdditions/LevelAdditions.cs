using System;
using System.Collections.Generic;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine.SceneManagement;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public abstract class LevelAdditions
    {
        internal abstract void OnSceneLoad();
        internal abstract void OnSceneUnload();
    }

    public static class LevelAdditionsManager
    {
        public static void Initialize()
        {
            ScenesEvents.OnSceneWasLoaded += OnSceneLoad;
            ScenesEvents.OnSceneWasUnloaded += OnSceneUnload;
        }

        private static Dictionary<string, Func<LevelAdditions>> LevelAdditionsCtorDict = new Dictionary<string, Func<LevelAdditions>>
        {
            {"Level P-2", () => { return new P2Additions(); }}
        };

        private static LevelAdditions CurrentAdditions = null;

        private static void OnSceneLoad(Scene scene, string sceneName)
        {
            sceneName = SceneHelper.CurrentScene;
            CurrentAdditions = null;
            Func<LevelAdditions> ctor = null;
            LevelAdditionsCtorDict.TryGetValue(sceneName, out ctor);
            Log.TraceExpectedInfo($"Level Additions OnSceneLoad called with sceneName {sceneName}, trying to find valid constructor...");
            if (ctor != null)
            {
                CurrentAdditions = ctor.Invoke();
                Log.ExpectedInfo($"Loading New LevelAdditions of type {CurrentAdditions.GetType()}!");
                CurrentAdditions.OnSceneLoad();
            }
            else
            {
                Log.ExpectedInfo($"No constructor for Level Additions found!");
            }
        }

        private static void OnSceneUnload(Scene scene, string sceneName)
        {
            CurrentAdditions?.OnSceneUnload();
            CurrentAdditions = null;
        }
    }
}