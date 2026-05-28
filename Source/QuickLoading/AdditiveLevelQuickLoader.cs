using System;
using System.Collections.Generic;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class AdditiveLevelQuickLoader : ILevelQuickLoader
    {
        public class LevelLoader
        {
            public event Action<LevelLoader> OnFinished;
            public event Action<LevelLoader> OnWaitForActivation;
            public event Action<LevelLoader, Scene> OnSceneActivated;
            public bool AutoActivate = false;
            public bool WaitingForActivation = false;
            public bool Activating = false;
            public bool Success = false;
            public bool Finished = false;
            private string _levelName;
            private AsyncOperationHandle<SceneInstance> _handle;
            private SceneInstance _currentSceneInst;
            private AsyncOperation _activationHandle;

            public void QuickLoadLevel(string levelName)
            {
                Log.Message($"LevelQuickLoader loading level {levelName}");
                _levelName = levelName;
                _handle = Addressables.LoadSceneAsync(levelName, LoadSceneMode.Additive, false, 10000);
                _handle.Completed += OnSceneLoaded;
            }

            public void AllowWaitForCompletion()
            {
                try
                {
                    if (!_handle.IsDone)
                    {
                        _handle.WaitForCompletion();
                    }
                }
                catch (System.Exception e)
                {
                    Log.Warning($"Exception whilst waiting for LevelQuickLoad completion {e}");
                    throw;
                }
            }

            private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
            {
                if (handle.Status == AsyncOperationStatus.Failed)
                {
                    Log.Error($"Error trying to quick load level {_levelName}, {handle.OperationException}");
                    Finish(false);
                    return;
                }

                var scene = handle.Result.Scene;
                Log.Message($"LevelQuickLoader level loaded {_levelName}");

                _currentSceneInst = handle.Result;

                if (AutoActivate)
                {
                    Activate();
                }
                else
                {
                    WaitingForActivation = true;
                    OnWaitForActivation?.Invoke(this);
                }
            }

            public void Activate()
            {
                WaitingForActivation = false;
                Activating = true;
                _activationHandle = _currentSceneInst.ActivateAsync();
                _activationHandle.completed += SceneActivated;
            }

            private void Finish(bool success)
            {
                Success = success;
                OnFinished?.Invoke(this);
            }

            private void SceneActivated(AsyncOperation operation)
            {
                if (!operation.isDone)
                {
                    Finish(false);
                    return;
                }

                var scene = _currentSceneInst.Scene;

                var gos = scene.GetRootGameObjects();
                OnSceneActivated?.Invoke(this, scene);

                foreach (var go in gos)
                {
                    if (go.scene != scene)
                    {
                        continue;
                    }

                    go.SetActive(false);
                }

                Activating = false;
                Log.Message($"LevelQuickLoader level activated {_levelName}");
                Unload();
                Finish(true);
            }

            private void Unload()
            {
                var unloadOp = Addressables.UnloadSceneAsync(_currentSceneInst, true);
                unloadOp.Completed += OnSceneUnloaded;
            }

            private void OnSceneUnloaded(AsyncOperationHandle<SceneInstance> handle)
            {
                Log.Message($"LevelQuickLoader level unloaded {_levelName}");
            }

            internal void Retry()
            {
                QuickLoadLevel(_levelName);
            }
        }

        public void QuickLoadLevel(string levelName)
        {
            if (Options.DisableQuickLoad.Value)
            {
                Log.Warning($"AddQuickLoadLevel called for {levelName} when DisableQuickLoad == true");
                return;
            }

            _queue.Enqueue(levelName);
        }

        private LevelLoader SetupNewLoader(string levelName)
        {
            Assert.IsFalse(Options.DisableQuickLoad.Value);

            var loader = new LevelLoader();

            _levelLoaders.Add(loader);
            loader.OnFinished += (finishedLoader) =>
            {
                if (finishedLoader.Success)
                {
                    _levelLoaders.Remove(finishedLoader);
                }
            };

            loader.OnWaitForActivation += (waitingLoader) =>
            {
                if (QueueActivation)
                {
                    if (ActivatingLoader != null)
                    {
                        _activationQueue.Enqueue(waitingLoader);
                    }
                    else
                    {
                        ActivateLoader(waitingLoader);
                    }
                }
                else
                {
                    waitingLoader.Activate();
                }
            };

            loader.OnSceneActivated += (loader, scene) =>
            {
                OnQuickLoad?.Invoke(scene);
            };

            loader.AutoActivate = !QueueActivation;
            loader.QuickLoadLevel(levelName);
            return loader;
        }

        private void ActivateLoader(LevelLoader loader)
        {
            ActivatingLoader = loader;
            loader.Activate();
        }

        private List<LevelLoader> _levelLoaders = new List<LevelLoader>();

        private Queue<LevelLoader> _activationQueue = new Queue<LevelLoader>();
        private Queue<string> _queue = new Queue<string>();

        private LevelLoader _activatingLoader = null;
        public LevelLoader ActivatingLoader
        {
            get
            {
                if (_activatingLoader == null)
                {
                    return null;
                }

                if (!_activatingLoader.Activating)
                {
                    return null;
                }

                return _activatingLoader;
            }

            set => _activatingLoader = value;
        }
        public bool QueueActivation = false;

        public AdditiveLevelQuickLoader()
        {
            Log.Warning($"Using experimental AdditiveLevelQuickLoader");
        }

        event ILevelQuickLoader.OnQuickLoadEventHandler ILevelQuickLoader.OnQuickLoad
        {
            add
            {
                OnQuickLoad += value;
            }

            remove
            {
                OnQuickLoad -= value;
            }
        }

        public event ILevelQuickLoader.OnQuickLoadEventHandler OnQuickLoad;

        public void Flush()
        {
            UpdateEvents.OnLateUpdate += LateUpdate;

            if (Options.DisableQuickLoad.Value)
            {
                return;
            }

            List<LevelLoader> loaders = new List<LevelLoader>();

            foreach (var levelName in _queue)
            {
                try
                {
                    var loader = SetupNewLoader(levelName);
                    loaders.Add(loader);
                }
                catch (System.Exception e)
                {
                    Log.Warning($"LevelQuickLoader error = {e}");
                }
            }

            _queue.Clear();

            foreach (var loader in loaders)
            {
                loader.AllowWaitForCompletion();
            }
        }

        private void LateUpdate()
        {
            Flush();

            foreach (var loader in _levelLoaders)
            {
                if (!loader.Success && loader.Finished)
                {
                    loader.Retry();
                }
            }

            if (ActivatingLoader == null && _activationQueue.Count > 0)
            {
                var loader = _activationQueue.Dequeue();
                ActivateLoader(loader);
            }
        }

        void ILevelQuickLoader.PreInitQueueAdded()
        {
            Flush();
        }
    }
}