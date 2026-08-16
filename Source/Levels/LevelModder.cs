using System;
using System.Collections.Generic;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine.SceneManagement;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public abstract class LevelMod
{
    public abstract void OnSceneLoad();
    public abstract void OnSceneUnload();
}

public static class LevelModder
{
    public static void RegisterLevelMod<T>(string levelName) where T : LevelMod, new()
    {
        LevelAdditionsCtorDict.TryAdd(levelName, []);
        LevelAdditionsCtorDict[levelName].Add(() => new T());
    }

    public static void Initialize()
    {
        SceneEvents.OnSceneLoad += OnSceneLoad;
        SceneEvents.OnSceneUnload += OnSceneUnload;
    }

    private static Dictionary<string, List<Func<LevelMod>>> LevelAdditionsCtorDict = new Dictionary<string, List<Func<LevelMod>>> { };

    private static LevelMod CurrentAdditions = null;

    private static void OnSceneLoad(Scene scene, string levelName, string unitySceneName)
    {
        levelName = SceneHelper.CurrentScene;
        CurrentAdditions = null;
        Log.TraceExpectedInfo($"LevelModder OnSceneLoad called with sceneName {levelName}, trying to find valid constructor...");

        if (LevelAdditionsCtorDict.TryGetValue(levelName, out var ctors))
        {
            foreach (var ctor in ctors)
            {
                if (ctor != null)
                {
                    CurrentAdditions = ctor.Invoke();
                    Log.ExpectedInfo($"Loading New LevelMod of type {CurrentAdditions.GetType()}!");
                    CurrentAdditions.OnSceneLoad();
                }
            }
        }
        else
        {
            Log.ExpectedInfo($"No constructors for LevelModder found!");
        }
    }

    private static void OnSceneUnload(Scene scene, string levelName, string unitySceneName)
    {
        CurrentAdditions?.OnSceneUnload();
        CurrentAdditions = null;
    }
}