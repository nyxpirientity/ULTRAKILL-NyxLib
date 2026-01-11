using System;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

[Serializable]
public class EnemyAdditions : ModoBehaviourManager
{
    public EnemyHydraMod HydraMod { get; private set; }
    public EnemyPrefabMod PrefabMod { get; private set; }
    public EnemyFriend EnemyFriend { get; private set; } = null;

    private new void Awake()
    {
        base.Awake();
        FindAndCacheMods(true);      
    }

    private new void Start()
    {
        base.Start();
        FindAndCacheMods(true);      
    }

    /* to allow patching lol */
    private new void OnDestroy()
    {
        base.OnDestroy();
    }

    /* if called we were instantiated by the game, most likely */
    public void SetupMods()
    {
        //Log.ExpectedInfo($"{name}.EnemyAdditions is setting up new modules...");
        HydraMod = AddMod<EnemyHydraMod>();
        HydraMod.InitializeAsNew();
        EnemyFriend = AddMod<EnemyFriend>();
        EnemyFriend.Leader = false;
        PrefabMod = AddMod<EnemyPrefabMod>();
        HydraMod.PassPrefabToShared();
        EnemyFriend.Leader = true;
    }

    /* If called then we were instantiated by the mod most likely. */
    public void FindAndCacheMods(bool nullAcceptable = false)
    {
        //Log.ExpectedInfo($"{name}.EnemyAdditions is finding and caching modules...");
        HydraMod = GetMod<EnemyHydraMod>();   
        PrefabMod = GetMod<EnemyPrefabMod>();   
        EnemyFriend = GetMod<EnemyFriend>();
        
        if (nullAcceptable)
        {
            return;
        }

        Assert.IsNotNull(HydraMod);
        Assert.IsNotNull(PrefabMod);
    }
}