using System;
using System.Collections;
using System.Diagnostics;
using HarmonyLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class Music
    {
        private static MusicManager _manager = null;
        public static MusicManager Manager
        {
            get 
            {
                if (_manager == null)
                {
                    if (MusicManager.Instance != null)
                    {
                        Log.ExpectedInfo($"Had to get MusicManager via MusicManager.Instance (then cached the value)");
                        _manager = MusicManager.Instance;
                    }
                }

                return _manager;
            }
        }

        public static void AddPlayCleanWithBattleVote()
        {
            _playBattleWithCleanVotes += 1;
        }
        
        public static void SubtractPlayCleanWithBattleVote()
        {
            _playBattleWithCleanVotes -= 1;

            if (_playBattleWithCleanVotes < 0)
            {
                Log.Error($"_playBattleWithCleanVotes is {_playBattleWithCleanVotes}, which is considered such it should be impossible! :c\nI don't know what caused this. Here's the current stack which may not be relevant. {StackDebug.GetStackString()}");
            }
        }

        public static int PlayCleanWithBattleVotes => _playBattleWithCleanVotes + (Options.ForcePlayCleanMusicWithBattleMusic.Value ? 1 : 0);

        internal static void Initialize()
        {
            ScenesEvents.OnSceneWasLoaded += OnSceneLoad;
            UpdateEvents.OnUpdate += Update;
        }

        private static int _playBattleWithCleanVotes = 0;

        private static void OnSceneLoad(Scene scene, string levelName, string unitySceneName)
        {
            _playBattleWithCleanVotes = 0;
            HasVotedForBattleMusic = false;
        }

        private static bool HasVotedForBattleMusic = false;

        private static void VoteForBattleMusic()
        {
            if (Music.Manager.battleTheme != null)
            {
                Music.Manager.PlayBattleMusic();
                HasVotedForBattleMusic = true;
            }
        }

        private static void UnvoteForBattleMusic()
        {
            if (HasVotedForBattleMusic)
            {
                Music.Manager.PlayCleanMusic();
                HasVotedForBattleMusic = false;
            }
        }

        private static bool WasPlayCleanWithBattle = false;
        private static void Update()
        {
            if (Cheats.Manager == null)
            {
                return;
            }

            if (Cheats.IsCheatEnabled(Cheats.AlwaysBattleMusic))
            {
                VoteForBattleMusic();
            }
            else
            {
                UnvoteForBattleMusic();
            }

            bool playCleanWithBattle = Cheats.IsCheatEnabled(Cheats.PlayCleanMusicWithBattle) || PlayCleanWithBattleVotes > 0;
            
            if (Music.Manager.battleTheme == null || Music.Manager.cleanTheme == null)
            {
                return;
            }

            if (playCleanWithBattle)
            {
                if (Music.Manager.targetTheme == Music.Manager.bossTheme)
                {
                    Music.Manager.cleanTheme.volume = Mathf.Max(Music.Manager.battleTheme.volume, Music.Manager.cleanTheme.volume);
                }
                else if (Music.Manager.targetTheme == Music.Manager.battleTheme)
                {
                    Music.Manager.cleanTheme.volume = Mathf.Max(Music.Manager.battleTheme.volume, Music.Manager.cleanTheme.volume);
                    if (Mathf.Abs(Music.Manager.cleanTheme.time - Music.Manager.battleTheme.time) > 0.1f)
                    {
                        Music.Manager.cleanTheme.time = Music.Manager.battleTheme.time;
                        
                        if (!Music.Manager.cleanTheme.isPlaying)
                        {
                            Music.Manager.cleanTheme.Play();
                        }
                    }
                }
                else if (Music.Manager.targetTheme == Music.Manager.cleanTheme)
                {
                    Music.Manager.battleTheme.volume = Mathf.MoveTowards(Music.Manager.battleTheme.volume, 0f, Music.Manager.fadeSpeed * Time.deltaTime);
                }
            }
            else if (Music.Manager.off || Music.Manager.targetTheme.volume == Music.Manager.volume) 
            {
                if (Music.Manager.targetTheme == Music.Manager.bossTheme)
                {
                    Music.Manager.cleanTheme.volume = Mathf.MoveTowards(Music.Manager.cleanTheme.volume, 0f, Music.Manager.fadeSpeed * Time.deltaTime);
                }
                else if (Music.Manager.targetTheme == Music.Manager.battleTheme)
                {
                    Music.Manager.cleanTheme.volume = Mathf.MoveTowards(Music.Manager.cleanTheme.volume, 0f, Music.Manager.fadeSpeed * Time.deltaTime);
                }
                else if (Music.Manager.targetTheme == Music.Manager.cleanTheme)
                {
                    Music.Manager.battleTheme.volume = Mathf.MoveTowards(Music.Manager.battleTheme.volume, 0f, Music.Manager.fadeSpeed * Time.deltaTime);
                }
            }

            WasPlayCleanWithBattle = playCleanWithBattle;
        }

        [HarmonyPatch(typeof(MusicManager), "OnEnable", new Type[] { })]
        static class MusicManagerAwakePatch
        {
            public static void Prefix(MusicManager __instance)
            {
            }

            public static void Postfix(MusicManager __instance)
            {
                _manager = __instance;
            }
        }
    }
}