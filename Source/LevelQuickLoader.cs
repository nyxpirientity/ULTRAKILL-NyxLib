using System.Collections.Generic;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;

public static class LevelQuickLoader
{
    public static void AddQuickLoadLevel(string levelName)
    {
        _quickLoadStates.TryAdd(levelName, LevelQuickLoadState.Needed);
    }

    internal static void Initialize()
    {
        if (Options.DisableQuickLoad.Value)
        {
            _quickLoadStates.Clear();
            Log.Message($"Clearing _quickLoadStates due to DisableQuickLoad being true...");
        }

        UpdateEvents.OnUpdate += Update;
    }

    static Dictionary<string, LevelQuickLoadState> _quickLoadStates = new Dictionary<string, LevelQuickLoadState>
    {
    };

    private static bool _quickLoading = false;
    private static bool _currentLevelIsFromQuickLoad = false;
    private static string _quickLoadLevel = null;
    private static string _preQuickLoadLevel = null;

    private static bool TryFindQuickLoadLevel()
    {
        if ((SceneHelper.PendingScene == null) && !_quickLoading)
        {
            _quickLoadLevel = null;
            foreach (var pair in _quickLoadStates)
            {
                if (pair.Value is LevelQuickLoadState.Needed)
                {
                    _quickLoadLevel = pair.Key;
                    break;
                }
            }
            
            if (_quickLoadLevel != null)
            {
                if (!_currentLevelIsFromQuickLoad)
                {
                    _preQuickLoadLevel = SceneHelper.CurrentScene;
                }
                Log.TraceExpectedInfo($"Quickloading {_quickLoadLevel}");
                SceneHelper.LoadScene(_quickLoadLevel);
                _currentLevelIsFromQuickLoad = true;
                _quickLoadStates[_quickLoadLevel] = LevelQuickLoadState.AwaitingLoad;
                _quickLoading = true;
                return true;
            }
        }
        
        return false;
    }

    private static void Update()
    {
        TryFindQuickLoadLevel();

        if (SceneHelper.CurrentScene == _quickLoadLevel && (SceneHelper.PendingScene == null))
        {
            if (_quickLoadStates[_quickLoadLevel] is LevelQuickLoadState.AwaitingLoad)
            {
                _quickLoadStates[_quickLoadLevel] = LevelQuickLoadState.WaitingToReturn;
            }
            else if (_quickLoadStates[_quickLoadLevel] is LevelQuickLoadState.WaitingToReturn)
            {
                Log.TraceExpectedInfo($"{_quickLoadLevel} quick load done!");
                _quickLoadStates[_quickLoadLevel] = LevelQuickLoadState.Done;
                _quickLoadLevel = null;
                _quickLoading = false;

                if (!TryFindQuickLoadLevel())
                {
                    SceneHelper.LoadScene(_preQuickLoadLevel);
                    _preQuickLoadLevel = null;
                    _currentLevelIsFromQuickLoad = false;
                }
            }
        }
    }

    enum LevelQuickLoadState
    {
        Needed, AwaitingLoad, WaitingToReturn, Returning, Done
    }
}
