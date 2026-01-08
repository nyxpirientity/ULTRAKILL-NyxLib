using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MelonLoader;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

[RegisterTypeInIl2Cpp]
public class LeaderFriend : MonoBehaviour
{
    public Action OnEnabled = null;
    public Action OnDisabled = null;
    public Action OnDestroyed = null;
    public Action<EnemyIdentifier> OnIdolTargetChanged = null;

    public EnemyIdentifier Enemy { get; set; } = null;
    public LeaderFriend IdolTargetLeaderFriend { get; private set; }

    public List<EnemyFriend> Friends = new List<EnemyFriend>();

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
        if (Cheats.FriendCount <= 0)
        {
            return;
        }
        
        if (Friends.Count >= Cheats.FriendCount)
        {
            return;
        }

        Enemy = gameObject.GetComponent<EnemyIdentifier>();

        if (Enemy.enemyType == EnemyType.Idol)
        {
            IdolTarget = Enemy.idol.target;
        }

        Vector3 sideBoundsOffset = Vector3.zero;
        var mainCollider = gameObject.GetComponent<Collider>();
        var colliders = gameObject.GetComponents<Collider>().AddRangeToArray(gameObject.GetComponentsInChildren<Collider>());

        Vector3 position = gameObject.transform.position;
        Bounds bounds = new Bounds();
        bounds.center = position;        
        Vector3 mainColliderBounds = Vector3.zero;

        if (mainCollider != null)
        {
            mainColliderBounds = mainCollider.bounds.extents;
        }

        if (mainColliderBounds.magnitude > 2.0f)
        {
            sideBoundsOffset = (Vector3.right * mainCollider.bounds.extents.magnitude);
        }
        else if (Enemy.enemyType != EnemyType.Leviathan && Enemy.enemyType != EnemyType.Minos)
        {
            foreach (var collider in colliders)
            {
                Vector3 boundsMin = bounds.min;
                Vector3 boundsMax = bounds.max;
                
                if (boundsMin.x > collider.bounds.min.x)
                {
                    boundsMin.x = collider.bounds.min.x;
                }
                
                if (boundsMin.y > collider.bounds.min.y)
                {
                    boundsMin.y = collider.bounds.min.y;
                }

                if (boundsMin.z > collider.bounds.min.z)
                {
                    boundsMin.z = collider.bounds.min.z;
                }                
            
                if (boundsMax.x < collider.bounds.max.x)
                {
                    boundsMax.x = collider.bounds.max.x;
                }
                
                if (boundsMax.y < collider.bounds.max.y)
                {
                    boundsMax.y = collider.bounds.max.y;
                }

                if (boundsMax.z < collider.bounds.max.z)
                {
                    boundsMax.z = collider.bounds.max.z;
                }

                bounds.SetMinMax(boundsMin, boundsMax);
            }
 
            sideBoundsOffset = (Vector3.right * bounds.extents.magnitude);
        }

        sideBoundsOffset = (gameObject.transform.rotation * sideBoundsOffset) * 0.75f;
        
        for (int i = 0; i < Cheats.FriendCount; i++)
        {
            var friendEnemyGo = GameObject.Instantiate(gameObject, transform.parent, true);
            Destroy(friendEnemyGo.GetComponent<LeaderFriend>());
            var enemyFriend = friendEnemyGo.AddComponent<EnemyFriend>();
            enemyFriend.Leader = this;

            friendEnemyGo.name = $"{gameObject.name}'s {i + 1}{(i == 0 ? "st" : (i == 1 ? "nd" : (i == 2 ? "rd" : "th")))} friend";
                        
            //Log.Info(Log.Level.Expected, $"friend spawned by the name of {friendEnemyGo.name}!");

            friendEnemyGo.transform.position += sideBoundsOffset * ((i - ((Cheats.FriendCount + 1) / 2)) + 0.5f);
            
            enemyFriend.Idx = Friends.Count;
            Friends.Add(enemyFriend);
        }

        gameObject.transform.position += sideBoundsOffset * (((Cheats.FriendCount) - ((Cheats.FriendCount + 1) / 2)) + 0.5f);
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

    public EnemyIdentifier IdolTarget { get; private set; } = null;

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

        if (Enemy.enemyType == EnemyType.Idol)
        {
            if (Enemy.idol.target != IdolTarget)
            {
                IdolTarget = Enemy.idol.target;
                OnIdolTargetChanged?.Invoke(IdolTarget);
                IdolTargetLeaderFriend = IdolTarget.gameObject.GetComponent<LeaderFriend>();
            }
        }
    }

    private void OnEnable()
    {
        //Log.Info(Log.Level.Expected, $"leader friend {gameObject.name} enabled!");
        OnEnabled?.Invoke();
    }

    private void OnDisable()
    {
        //Log.Info(Log.Level.Expected, $"leader friend {gameObject.name} disabled!");
        OnDisabled?.Invoke();
    }

    private void OnDestroy()
    {
        //Log.Info(Log.Level.Expected, $"leader friend {gameObject.name} destroyed!");
        OnDestroyed?.Invoke();
    }
}