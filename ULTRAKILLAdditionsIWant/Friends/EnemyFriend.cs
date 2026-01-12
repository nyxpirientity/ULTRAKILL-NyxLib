using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MelonLoader;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

public class EnemyFriendIdentifier : ModoBehaviour
{
    public bool IsLeader = false;
    public EnemyFriendIdentifier Leader = null;
    public EnemyFriendIdentifier[] Friends = null;
    public int FriendIdx = -1;
    public EnemyIdentifier Eid = null;

    public override void ModoFixedUpdate()
    {
        if (Eid.enemyType == EnemyType.Idol || !IsLeader && Cheats.IsCheatEnabled(Cheats.GiveEnemiesFriends))
        {
            IdolFindRightTarget();
        }
    }

    public override void ModoLateUpdate()
    {
    }

    public override void ModoOnDestroy()
    {

    }

    public override void ModoOnDisable()
    {
        if (IsLeader && !Eid.Dead && Friends != null)
        {
            foreach (var friend in Friends)
            {
                if (friend == null)
                {
                    continue;
                }

                // evil...
                Destroy(friend.GameObject);
            }
        }
    }

    public override void ModoOnEnable()
    {
    }

    public override void ModoUpdate()
    {
    }

    public override void OnClonedFrom(ModoBehaviour ClonedFrom)
    {
    }

    public override void OnModRemoved()
    {
    }

    protected override void ModoAwake()
    {
        Eid = ((EnemyAdditions)Mono).Eid;
    }

    protected override void ModoStart()
    {
        if (Cheats.IsCheatEnabled(Cheats.GiveEnemiesFriends))
        {
            if (IsLeader && !Eid.Dead)
            {
                Friends = new EnemyFriendIdentifier[Options.NumFriendsToSpawn];
                
                var totalEnemyNum = Options.NumFriendsToSpawn + 1;
                Bounds bounds = EnemyUtils.SolveEnemyBounds(GameObject);
                Vector3 offset = Vector3.Project((bounds.size), (Transform.rotation * Vector3.right));
                Vector3 initialOrigin = Transform.position;
                
                bool useRotaryPositioning = false;

                if (Eid.enemyType == EnemyType.Idol)
                {
                    useRotaryPositioning = true;
                    offset *= 0.5f;
                }
                else if (Eid.enemyType == EnemyType.HideousMass && totalEnemyNum >= 3)
                {
                    useRotaryPositioning = true;
                    offset *= 0.2f;
                }
                else if (Eid.enemyType == EnemyType.Minos)
                {
                    offset *= 0.0f;
                }
                else if (Eid.enemyType == EnemyType.FleshPanopticon || Eid.enemyType == EnemyType.FleshPrison)
                {
                    useRotaryPositioning = true;
                }

                if (!useRotaryPositioning)
                {
                    Transform.position += (offset) * -((float)(totalEnemyNum / 2) + -0.5f);
                }

                for (int i = 0; i < Options.NumFriendsToSpawn; i++)
                {
                    Vector3 currentOffset = offset * (i + 1);
                    
                    if (useRotaryPositioning)
                    {
                        currentOffset = (Quaternion.Euler(new Vector3(0.0f, Mathf.Lerp(0.0f, 360.0f, ((float)(i + 1) + -0.5f) / totalEnemyNum), 0.0f)) * (offset));   
                    }

                    Friends[i] = SpawnFriend(currentOffset, i);
                }
                
                if (useRotaryPositioning)
                {
                    Transform.position = initialOrigin + (Quaternion.Euler(new Vector3(0.0f, Mathf.Lerp(0.0f, 360.0f, 0), 0.0f)) * (offset));   
                }
            }
            else
            {
                NonLeaderStart();
            }
        }
    }

    private void NonLeaderStart()
    {

    }

    private void IdolFindRightTarget()
    {
        //Assert.IsNotNull(Leader);
        
        if (Leader == null)
        {
            return;
        }

        if (Leader.Eid == null)
        {
            return;
        }

        if (Leader.Eid.idol == null)
        {
            return;
        }

        //Assert.IsNotNull(Leader.Eid);
        //Assert.IsNotNull(Leader.Eid.idol);

        var leaderTarget = Leader.Eid.idol.target;
        
        if (leaderTarget == null)
        {
            return;
        }

        var leaderTargetEadd = leaderTarget.gameObject.GetComponent<EnemyAdditions>();
        var leaderTargetFriends = leaderTargetEadd.EnemyFriend.Friends;
        var target = leaderTargetFriends[FriendIdx];
        
        if (leaderTargetFriends.Length <= FriendIdx)
        {
            return;
        }

        Eid.idol.ChangeOverrideTarget(target.Eid);
    }

    private EnemyFriendIdentifier SpawnFriend(Vector3 offset, int idx)
    {
        EnemyAdditions eadd = (EnemyAdditions)Mono;
        var prefab = eadd.PrefabMod.Prefab;
        var friend = Instantiate(prefab);
        EnemyAdditions friendEadd = friend.GetComponent<EnemyAdditions>() ?? friend.GetComponentInChildren<EnemyAdditions>();
        friend.SetActive(true);
        friend = friendEadd.gameObject;
        friend.SetActive(true);
        friend.transform.position = Transform.position + offset;
        friendEadd.FindAndCacheMods();
        friendEadd.HydraMod.InitializeAsNew();
        friendEadd.EnemyFriend.Leader = this;
        friendEadd.PrefabMod.StorePrefab(force: true);
        friendEadd.HydraMod.PassPrefabToShared();
        friendEadd.EnemyFriend.FriendIdx = idx;

        return friendEadd.EnemyFriend;
    }
}