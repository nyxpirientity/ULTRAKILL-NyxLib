using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MelonLoader;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

public class EnemyFriend : ModoBehaviour
{
    public bool Leader = false;

    public override void ModoFixedUpdate()
    {
    }

    public override void ModoLateUpdate()
    {
    }

    public override void ModoOnDestroy()
    {
    }

    public override void ModoOnDisable()
    {
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
    }

    protected override void ModoStart()
    {
        if (Cheats.IsCheatEnabled(Cheats.GiveEnemiesFriends))
        {
            if (Leader)
            {
                EnemyAdditions eadd = (EnemyAdditions)Mono;
                var prefab = eadd.PrefabMod.Prefab;
                var friend = Instantiate(prefab);
                friend.transform.position = Transform.position;
                EnemyAdditions friendEadd = friend.GetComponent<EnemyAdditions>();
                friend.SetActive(true);
                friendEadd.FindAndCacheMods();
                friendEadd.HydraMod.InitializeAsNew();
                friendEadd.PrefabMod.StorePrefab(force: true);
                friendEadd.HydraMod.PassPrefabToShared();
            }
        }
    }
}