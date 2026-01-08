using System.Reflection;
using MelonLoader;
using UnityEngine;

[RegisterTypeInIl2Cpp]
public class EnemyFriend : MonoBehaviour
{
    EnemyIdentifier Enemy = null;
    public LeaderFriend Leader = null;
    public int Idx = -1;

    private void Start()
    {
        try
        {
            UnsafeStart();
        }
        catch (System.Exception e)
        {
            MelonLogger.Error(e.ToString());
        }
    }

    private void UnsafeStart()
    {
        Enemy = gameObject.GetComponent<EnemyIdentifier>();
        
        if (Enemy.enemyType == EnemyType.Idol)
        {
            Enemy.idol.target = null;
        }

        FieldInfo blessingsFI = typeof(EnemyIdentifier).GetField("blessings", BindingFlags.NonPublic | BindingFlags.Instance);
        blessingsFI.SetValue(Enemy, 1);
        Enemy.Unbless();            

        Leader.OnEnabled += () =>
        {
            gameObject.SetActive(true);
        };
        
        Leader.OnDisabled += () =>
        {            
            if (Leader.Enemy.Dead)
            {
                return;
            }

            gameObject.SetActive(false);

            Destroy(gameObject);
        };

        Leader.OnDestroyed += () =>
        {
            if (Leader.Enemy.Dead)
            {
                return;
            }
            
            Destroy(gameObject);
        };

        Leader.OnIdolTargetChanged += (idolTarget) =>
        {
            UpdateIdolTarget();
        };
    }

    private void Update()
    {
        try
        {
            UnsafeUpdate();
        }
        catch (System.Exception e)
        {
            MelonLogger.Error(e.ToString());
        }
    }

    int TicksAlive = 0;
    private void UnsafeUpdate()
    {
        if (Enemy == null)
        {
            Enemy = GetComponent<EnemyIdentifier>();
        }

        if (Enemy == null)
        {
            return;
        }

        UpdateIdolTarget();
    }

    private void UpdateIdolTarget()
    {
        if (Enemy.enemyType == EnemyType.Idol)
        {
            LeaderFriend leaderTargetLeader = Leader.IdolTargetLeaderFriend;

            if (leaderTargetLeader != null)
            {
                if (leaderTargetLeader.Friends.Count > Idx)
                {
                    Enemy.idol.ChangeOverrideTarget(leaderTargetLeader.Friends[Idx].Enemy);
                }
                else
                {
                    Enemy.idol.PickNewTarget();
                }
            }
            else
            {
                Enemy.idol.PickNewTarget();
            }
        }
    }
}