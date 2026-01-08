using System;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

[Serializable]
public class EnemyAdditions : ModoBehaviourManager
{
    public EnemyHydraMod HydraMod { get; private set; }
    public EnemyPrefabMod PrefabMod { get; private set; }

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

    public void SetupMods()
    {
        //Log.ExpectedInfo($"{name}.EnemyAdditions is setting up new modules...");
        HydraMod = AddMod<EnemyHydraMod>();
        HydraMod.InitializeAsNew();
        PrefabMod = AddMod<EnemyPrefabMod>();
        HydraMod.PassPrefabToShared();
    }

    public void FindAndCacheMods(bool nullAcceptable = false)
    {
        //Log.ExpectedInfo($"{name}.EnemyAdditions is finding and caching modules...");
        HydraMod = GetMod<EnemyHydraMod>();   
        PrefabMod = GetMod<EnemyPrefabMod>();   
        
        if (nullAcceptable)
        {
            return;
        }

        Assert.IsNotNull(HydraMod);
        Assert.IsNotNull(PrefabMod);
    }
}