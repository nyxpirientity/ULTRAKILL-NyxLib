using System;
using UnityEngine;

namespace UKAIW
{
    public static class MusicAdditions
    {
        public static int PlayBattleWithCleanVotes { get; set; } = 0;

        internal static void Initialize()
        {
            ScenesEvents.OnSceneWasUnloaded += OnSceneUnload;
            ScenesEvents.OnSceneWasLoaded += OnSceneLoad;
            UpdateEvents.OnUpdate += Update;
        }

        private static void OnSceneLoad(int arg1, string arg2)
        {
            PlayBattleWithCleanVotes = 0;
            HasVotedForBattleMusic = false;
        }

        private static void OnSceneUnload(int arg1, string arg2)
        {
        }

        private static bool HasVotedForBattleMusic = false;

        private static void VoteForBattleMusic()
        {
            if (MusicManager.Instance.battleTheme != null)
            {
                MusicManager.Instance.PlayBattleMusic();
                HasVotedForBattleMusic = true;
            }
        }

        private static void UnvoteForBattleMusic()
        {
            if (HasVotedForBattleMusic)
            {
                MusicManager.Instance.PlayCleanMusic();
                HasVotedForBattleMusic = false;
            }
        }

        private static bool WasPlayCleanWithBattle = false;
        private static void Update()
        {
            if (CheatsManager.Instance == null)
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

            bool playCleanWithBattle = Cheats.IsCheatEnabled(Cheats.PlayCleanMusicWithBattle) || PlayBattleWithCleanVotes > 0;

            if (playCleanWithBattle)
            {
                if (MusicManager.Instance.targetTheme == MusicManager.Instance.bossTheme)
                {
                    MusicManager.Instance.cleanTheme.volume = Mathf.Max(MusicManager.Instance.battleTheme.volume, MusicManager.Instance.cleanTheme.volume);
                }
                else if (MusicManager.Instance.targetTheme == MusicManager.Instance.battleTheme)
                {
                    MusicManager.Instance.cleanTheme.volume = Mathf.Max(MusicManager.Instance.battleTheme.volume, MusicManager.Instance.cleanTheme.volume);
                    if (Mathf.Abs(MusicManager.Instance.cleanTheme.time - MusicManager.Instance.battleTheme.time) > 0.1f)
                    {
                        MusicManager.Instance.cleanTheme.time = MusicManager.Instance.battleTheme.time;
                        
                        if (!MusicManager.Instance.cleanTheme.isPlaying)
                        {
                            MusicManager.Instance.cleanTheme.Play();
                        }
                    }
                }
                else if (MusicManager.Instance.targetTheme == MusicManager.Instance.cleanTheme)
                {
                    MusicManager.Instance.battleTheme.volume = Mathf.MoveTowards(MusicManager.Instance.battleTheme.volume, 0f, MusicManager.Instance.fadeSpeed * Time.deltaTime);
                }
            }
            else if (MusicManager.Instance.off || MusicManager.Instance.targetTheme.volume == MusicManager.Instance.volume) 
            {
                if (MusicManager.Instance.targetTheme == MusicManager.Instance.bossTheme)
                {
                    MusicManager.Instance.cleanTheme.volume = Mathf.MoveTowards(MusicManager.Instance.cleanTheme.volume, 0f, MusicManager.Instance.fadeSpeed * Time.deltaTime);
                }
                else if (MusicManager.Instance.targetTheme == MusicManager.Instance.battleTheme)
                {
                    MusicManager.Instance.cleanTheme.volume = Mathf.MoveTowards(MusicManager.Instance.cleanTheme.volume, 0f, MusicManager.Instance.fadeSpeed * Time.deltaTime);
                }
                else if (MusicManager.Instance.targetTheme == MusicManager.Instance.cleanTheme)
                {
                    MusicManager.Instance.battleTheme.volume = Mathf.MoveTowards(MusicManager.Instance.battleTheme.volume, 0f, MusicManager.Instance.fadeSpeed * Time.deltaTime);
                }
            }

            WasPlayCleanWithBattle = playCleanWithBattle;
        }
    }
}