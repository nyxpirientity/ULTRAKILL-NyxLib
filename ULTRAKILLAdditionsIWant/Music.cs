using System;
using UnityEngine;

namespace UKAIW
{
    public static class MusicAdditions
    {
        public static void Initialize()
        {
            ScenesEvents.OnSceneWasUnloaded += OnSceneUnload;
            ScenesEvents.OnSceneWasLoaded += OnSceneLoad;
            UpdateEvents.OnUpdate += Update;
        }

        private static void OnSceneLoad(int arg1, string arg2)
        {
        }

        private static void OnSceneUnload(int arg1, string arg2)
        {
        }

        private static bool HasVotedForBattleMusic = false;
        public static void VoteForBattleMusic()
        {
            if (MusicManager.Instance.battleTheme != null)
            {
                MusicManager.Instance.PlayBattleMusic();
                HasVotedForBattleMusic = true;
            }
        }

        public static void UnvoteForBattleMusic()
        {
            if (HasVotedForBattleMusic)
            {
                MusicManager.Instance.PlayCleanMusic();
                HasVotedForBattleMusic = false;
            }
        }

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

            if (Cheats.IsCheatEnabled(Cheats.PlayCleanMusicWithBattle))
            {
                if (MusicManager.Instance.targetTheme == MusicManager.Instance.bossTheme)
                {
                    MusicManager.Instance.cleanTheme.volume = Mathf.Max(MusicManager.Instance.battleTheme.volume, MusicManager.Instance.cleanTheme.volume);
                }
                else if (MusicManager.Instance.targetTheme == MusicManager.Instance.battleTheme)
                {
                    MusicManager.Instance.cleanTheme.volume = MusicManager.Instance.battleTheme.volume;
                }
                else if (MusicManager.Instance.targetTheme == MusicManager.Instance.cleanTheme)
                {
                    
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
                    
                }
            }
        }
    }
}