using Nyxpiri.ULTRAKILL.NyxLib;
using UnityEngine;

public class EnemyFriendIdentifier : EnemyModifier
{
    public bool IsLeader = false;
    public EnemyFriendIdentifier Leader = null;
    public EnemyFriendIdentifier[] Friends = null;
    public int FriendIdx = -1;
    public EnemyIdentifier Eid = null;
    public EnemyComponents Ead = null;
    public bool ExcludedFromFriends = false;


    protected void FixedUpdate()
    {
        if (Eid.enemyType == EnemyType.Idol || !IsLeader && Cheats.IsCheatEnabled(Cheats.GiveEnemiesFriends))
        {
            IdolFindRightTarget();
        }
    }

    protected void OnDisable()
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
                Destroy(friend.gameObject);
            }
        }
    }

    protected void Awake()
    {
        Ead = GetComponent<EnemyComponents>();
        Eid = Ead.Eid;
        
    }

    protected void Start()
    {
        if (Eid.enemyType == EnemyType.Deathcatcher)
        {
            ExcludedFromFriends = true;
        }
        
        if (Cheats.IsCheatEnabled(Cheats.GiveEnemiesFriends) && !Ead.UniquelySolo && !ExcludedFromFriends)
        {
            if (IsLeader && !Eid.Dead)
            {
                Friends = new EnemyFriendIdentifier[Options.NumFriendsToSpawn];
                
                var totalEnemyNum = Options.NumFriendsToSpawn + 1;
                Bounds bounds = EnemyUtils.SolveEnemyBounds(gameObject);
                Vector3 offset = Vector3.Project(bounds.size, transform.rotation * Vector3.right);
                Vector3 initialOrigin = transform.position;
                
                bool useRotaryPositioning = false;

                if (Eid.enemyType == EnemyType.Idol)
                {
                    useRotaryPositioning = true;
                    offset *= 0.3f;
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
                else if ((Eid.enemyType == EnemyType.FleshPanopticon || Eid.enemyType == EnemyType.FleshPrison) && totalEnemyNum >= 3)
                {
                    useRotaryPositioning = true;
                }

                if (!useRotaryPositioning)
                {
                    transform.position += (offset) * -((float)(totalEnemyNum / 2) + -0.5f);
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
                    //transform.position = initialOrigin + (Quaternion.Euler(new Vector3(0.0f, Mathf.Lerp(0.0f, 360.0f, ((float)(0) + -0.5f) / totalEnemyNum), 0.0f)) * (offset)); ;   
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
        if (CybergrindAdditions.CybergrindActive)
        {
            CybergrindAdditions.LastStartedEndlessGrid.tempEnemyAmount += 1;
        }
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
        
        var leaderTargetEadd = leaderTarget.gameObject.GetComponent<EnemyComponents>();
        var leaderTargetFriends = leaderTargetEadd.FriendID.Friends;

        if (leaderTargetFriends.Length <= FriendIdx)
        {
            return;
        }

        var target = leaderTargetFriends[FriendIdx];
        
        if (leaderTargetFriends.Length <= FriendIdx)
        {
            return;
        }
        
        if (target.Eid == Eid.idol.target)
        {
            return;
        }

        Eid.idol.ChangeOverrideTarget(target.Eid);
    }

    private EnemyFriendIdentifier SpawnFriend(Vector3 offset, int idx)
    {
        EnemyComponents enemy = GetComponent<EnemyComponents>();
        var prefab = enemy.PrefabStore.Prefab;
        var friend = Instantiate(prefab, enemy.RootGameObject.transform.parent);
        EnemyComponents friendEadd = friend.GetComponent<EnemyComponents>() ?? friend.GetComponentInChildren<EnemyComponents>();
        friend.transform.position = transform.position + offset;
        friend.SetActive(true);
        friend = friendEadd.gameObject;
        friend.SetActive(true);
        friendEadd.FriendID.Leader = this;
        friendEadd.PrefabStore.StorePrefab(force: true);
        friendEadd.FriendID.FriendIdx = idx;

        return friendEadd.FriendID;
    }
}