using System;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

[Serializable]
public class EnemyAdditions : MonoBehaviour
{
    public EnemyHydraMod HydraMod { get; private set; }
    public EnemyPrefabMod PrefabMod { get; private set; }
    public EnemyFriendIdentifier EnemyFriend { get; private set; } = null;
    public EnemyBloodFuel EnemyBloodFuel { get; private set; } = null;
    public SaltyEnemy SaltyEnemy { get; private set; } = null;
    
    [NonSerialized] public EnemyIdentifier Eid = null;

    private new void Awake()
    {
        Eid = GetComponent<EnemyIdentifier>() ?? GetComponentInChildren<EnemyIdentifier>();
        FindAndCacheMods(true);
    }

    private new void Start()
    {
        FindAndCacheMods(true);
    }

    /* to allow patching lol */
    private new void OnDestroy()
    {
    }

    /* if called we were instantiated by the game, most likely */
    public void SetupMods()
    {
        //Log.ExpectedInfo($"{name}.EnemyAdditions is setting up new modules...");
        HydraMod = gameObject.AddComponent<EnemyHydraMod>();
        HydraMod.InitializeAsNew();
        EnemyFriend = gameObject.AddComponent<EnemyFriendIdentifier>();
        EnemyBloodFuel = gameObject.AddComponent<EnemyBloodFuel>();
        SaltyEnemy = gameObject.AddComponent<SaltyEnemy>();
        EnemyFriend.IsLeader = false;
        PrefabMod = gameObject.AddComponent<EnemyPrefabMod>();
        HydraMod.PassPrefabToShared();
        EnemyFriend.IsLeader = true;
    }

    /* If called then we were instantiated by the mod most likely. */
    public void FindAndCacheMods(bool nullAcceptable = false)
    {
        //Log.ExpectedInfo($"{name}.EnemyAdditions is finding and caching modules...");
        HydraMod = GetComponent<EnemyHydraMod>();   
        PrefabMod = GetComponent<EnemyPrefabMod>();   
        EnemyFriend = GetComponent<EnemyFriendIdentifier>();
        SaltyEnemy = GetComponent<SaltyEnemy>();
        EnemyBloodFuel = GetComponent<EnemyBloodFuel>();
        
        if (nullAcceptable)
        {
            return;
        }

        Assert.IsNotNull(HydraMod);
        Assert.IsNotNull(PrefabMod);
    }
}