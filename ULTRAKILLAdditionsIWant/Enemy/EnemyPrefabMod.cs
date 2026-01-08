using System;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

[Serializable]
public class EnemyPrefabMod : ModoBehaviour
{
    public GameObject Prefab = null;
    private EnemyIdentifier Eid = null;

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

    public override void OnModRemoved()
    {
    }

    public override void ModoUpdate()
    {
    }

    private static bool IsStoringPrefab = false;
    protected override void ModoAwake()
    {
        Eid = GetComponent<EnemyIdentifier>();

        StorePrefab();
    }

    protected override void ModoStart()
    {
        if (Prefab == null)
        {
            StorePrefab();
        }
    }

    public void StorePrefab()
    {
        if (IsStoringPrefab)
        {
            return;
        }

        if (Prefab != null)
        {
            //Log.ExpectedInfo($"EnemyPrefabMod found that {Mono.name} already had a prefab, no need to make a new one");
            return;
        }

        //Log.ExpectedInfo($"EnemyPrefabMod found that {Mono.name} did not have a prefab, need to make a new one");

        var eid = GetComponent<EnemyIdentifier>();

        if (eid == null)
        {
            return;
        }

        GameObject templateGo;
        
        if (eid.enemyType == EnemyType.MaliciousFace)
        {
            templateGo = GameObject.transform.parent.gameObject;
        }
        else
        {
            templateGo = GameObject;
            Assert.IsNotNull(templateGo.GetComponent<EnemyAdditions>().GetMod<EnemyHydraMod>());
            Assert.IsNotNull(templateGo.GetComponent<EnemyAdditions>().GetMod<EnemyPrefabMod>());
        }
        
        IsStoringPrefab = true;
        bool wasActive = false;
        if (templateGo.activeSelf)
        {
            wasActive = true;
            //templateGo.SetActive(false);
        }
        Prefab = UnityEngine.Object.Instantiate(templateGo);
        Prefab.SetActive(false);

        if (eid.enemyType != EnemyType.MaliciousFace)
        {
            Assert.IsNotNull(Prefab.GetComponent<EnemyAdditions>().GetMod<EnemyHydraMod>());
            Assert.IsNotNull(Prefab.GetComponent<EnemyAdditions>().GetMod<EnemyPrefabMod>());
        }
        else
        {
            Prefab.GetComponentInChildren<EnemyAdditions>().GetMod<EnemyPrefabMod>().Prefab = Prefab;            
        }

        IsStoringPrefab = false;
        if (wasActive)
        {
            //templateGo.SetActive(true);
        }
    }

    public override void OnClonedFrom(ModoBehaviour ClonedFrom)
    {
        var other = (EnemyPrefabMod)ClonedFrom;

        //Prefab = other.Prefab;
    }
}