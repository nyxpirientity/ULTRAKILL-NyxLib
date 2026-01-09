using System;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

namespace UKAIW
{
    public class P2Additions : LevelAdditions
    {
        internal override void OnSceneLoad()
        {
            Log.ExpectedInfo($"Level Additions for P-2 OnSceneLoad called!");
            EnemyEvents.PostStart += OnEnemySpawned;
            EnemyEvents.DuringDeath += OnEnemyDie;
            EnemyEvents.PreDestroy += OnEnemyDestroy;
        }

        internal override void OnSceneUnload()
        {
            EnemyEvents.PreStart -= OnEnemySpawned;
            EnemyEvents.DuringDeath -= OnEnemyDie;
            EnemyEvents.PreDestroy -= OnEnemyDestroy;

            DisableBattleWithClean();
        }

        private int NumVirtues = 0;
        private int NumMindflayers = 0;
        private int NumHideousMasses = 0;
        private bool IsBattleWithClean = false;
        private void OnEnemySpawned(EnemyIdentifier eid, GameObject go)
        {
            Assert.IsTrue(go.activeInHierarchy);
            
            if (eid.Dead)
            {
                return;
            }

            if (!eid.enabled || !eid.isActiveAndEnabled)
            {
                return;
            }

            switch (eid.enemyType)
            {
                case EnemyType.Mindflayer:
                    Log.TraceExpectedInfo($"P2Additions Detected a Mindflayer spawn!");
                    NumMindflayers += 1;
                    break;
                case EnemyType.Virtue:
                    Log.TraceExpectedInfo($"P2Additions Detected a Virtue spawn!");
                    NumVirtues += 1;
                    break;
                case EnemyType.HideousMass:
                    Log.TraceExpectedInfo($"P2Additions Detected a HideousMass spawn!");
                    NumHideousMasses += 1;
                    break;
                default:
                    break;
            }

            if (Cheats.IsCheatEnabled(Cheats.HydraMode))
            {
                if (NumMindflayers >= 2 && NumVirtues >= 2)
                {
                    EnableBattleWithClean();
                }
                else if (NumVirtues >= 3 && NumHideousMasses >= 1)
                {
                    EnableBattleWithClean();
                }
            }
            else
            {
                DisableBattleWithClean();
            }
        }

        private void EnableBattleWithClean()
        {
            if (!IsBattleWithClean)
            {
                Log.ExpectedInfo($"Level Additions for P-2 trying to play battle music with clean!\nNumVirtues: {NumVirtues}\nNumMindflayers: {NumMindflayers}");
                MusicAdditions.PlayBattleWithCleanVotes += 1;
                IsBattleWithClean = true;
            }
        }

        private void DisableBattleWithClean()
        {
            if (IsBattleWithClean)
            {
                Log.ExpectedInfo($"Level Additions for P-2 trying to NOT play battle music with clean!");
                MusicAdditions.PlayBattleWithCleanVotes -= 1;
                IsBattleWithClean = false;
            }
        }

        private void OnEnemyDie(EnemyIdentifier eid)
        {
            switch (eid.enemyType)
            {
                case EnemyType.Mindflayer:
                    Log.TraceExpectedInfo($"P2Additions Detected a Mindflayer death!");
                    NumMindflayers -= 1;
                    break;
                case EnemyType.Virtue:
                    Log.TraceExpectedInfo($"P2Additions Detected a Virtue death!");
                    NumVirtues -= 1;
                    break;
                case EnemyType.HideousMass:
                    Log.TraceExpectedInfo($"P2Additions Detected a HideousMass death!");
                    NumHideousMasses -= 1;
                    break;
                default:
                    break;
            }
        }

        private void OnEnemyDestroy(EnemyIdentifier eid, GameObject go)
        {
            if (eid.dead)
            {
                return;
            }

            switch (eid.enemyType)
            {
                case EnemyType.Mindflayer:
                    Log.TraceExpectedInfo($"P2Additions Detected a Mindflayer destruction!");
                    NumMindflayers -= 1;
                    break;
                case EnemyType.Virtue:
                    Log.TraceExpectedInfo($"P2Additions Detected a Virtue destruction!");
                    NumVirtues -= 1;
                    break;
                case EnemyType.HideousMass:
                    Log.TraceExpectedInfo($"P2Additions Detected a HideousMass destruction!");
                    NumHideousMasses -= 1;
                    break;
                default:
                    break;
            }
        }
    }
}