using System.Collections.Generic;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine.SceneManagement;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class SimpleLevelQuickLoader : ILevelQuickLoader
    {
        void ILevelQuickLoader.QuickLoadLevel(string levelName)
        {
            _quickLoadStates.TryAdd(levelName, LevelQuickLoadState.Needed);
        }

        public event ILevelQuickLoader.OnQuickLoadEventHandler OnQuickLoad;

        internal SimpleLevelQuickLoader()
        {
            if (Options.DisableQuickLoad.Value)
            {
                _quickLoadStates.Clear();
                Log.Message($"Clearing _quickLoadStates due to DisableQuickLoad being true...");
            }

            UpdateEvents.OnUpdate += Update;
        }

        Dictionary<string, LevelQuickLoadState> _quickLoadStates = new Dictionary<string, LevelQuickLoadState>
        {
        };

        private bool _quickLoading = false;
        private bool _currentLevelIsFromQuickLoad = false;
        private string _quickLoadLevel = null;
        private string _preQuickLoadLevel = null;

        private bool TryFindQuickLoadLevel()
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

        private void Update()
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
                    OnQuickLoad?.Invoke(SceneManager.GetActiveScene());
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

        void ILevelQuickLoader.PreInitQueueAdded()
        {

        }

        enum LevelQuickLoadState
        {
            Needed, AwaitingLoad, WaitingToReturn, Returning, Done
        }
    }
}