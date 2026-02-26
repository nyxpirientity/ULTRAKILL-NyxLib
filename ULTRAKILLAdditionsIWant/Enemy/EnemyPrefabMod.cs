using System;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

public class EnemyPrefabStore : EnemyModifier
{
    public GameObject Prefab = null;
    private GameObject _PrefabParent = null;
    public GameObject PrefabParent { get => _PrefabParent ?? null; }
    private EnemyIdentifier Eid = null;
    public EnemyAdditions Eadd { get; private set; } = null;

    private static bool IsStoringPrefab = false;
    protected void Awake()
    {
        Eid = GetComponent<EnemyIdentifier>();
        Eadd = GetComponent<EnemyAdditions>();
    }

    protected void Start()
    {
    }

    public void StorePrefab(bool force = false)
    {
        try
        {
            StorePrefabUnsafe(force);
        }
        catch (System.Exception)
        {
            IsStoringPrefab = false;
            throw;
        }
    }

    private void StorePrefabUnsafe(bool force = false)
    {
        if (IsStoringPrefab)
        {
            return;
        }

        if (Prefab != null && !force)
        {
            Log.TraceExpectedInfo($"EnemyPrefabMod found that {name} already had a prefab, and force is false, no need to make a new one");
            return;
        }
        else if (Prefab != null && force)
        {
            Log.TraceExpectedInfo($"EnemyPrefabMod found that {name} already had a prefab, but force is true, need to make a new one");
        }
        else if (Prefab == null)
        {
            Log.TraceExpectedInfo($"EnemyPrefabMod found that {name} did not have a prefab, need to make a new one");            
        }

        GameObject templateGo;
        
        templateGo = Eadd.RootGameObject;
        
        IsStoringPrefab = true;

        Prefab = UnityEngine.Object.Instantiate(templateGo);
        _PrefabParent = templateGo.transform.parent.gameObject;
        Prefab.SetActive(false);
        
        var prefabEid = Prefab.GetComponent<EnemyIdentifier>() ?? Prefab.GetComponentInChildren<EnemyIdentifier>();
        
        prefabEid.destroyOnDeath = new System.Collections.Generic.List<GameObject>();
        prefabEid.activateOnDeath = new GameObject[0];
        prefabEid.drillers = new System.Collections.Generic.List<Harpoon>();
        prefabEid.stuckMagnets = new System.Collections.Generic.List<Magnet>();
        
        prefabEid.onDeath.RemoveAllListeners();

        if (prefabEid.machine != null)
        {
            prefabEid.machine.musicRequested = false;
        }
        
        if (prefabEid.zombie != null)
        {
            prefabEid.zombie.musicRequested = false;
        }

        if (prefabEid.statue != null)
        {
            prefabEid.statue.musicRequested = false;
        }

        if (Eid.enemyType != EnemyType.MaliciousFace)
        {
            Assert.IsNotNull(Prefab.GetComponent<EnemyHydra>());
            Assert.IsNotNull(Prefab.GetComponent<EnemyPrefabStore>());
            Prefab.GetComponent<EnemyPrefabStore>().Prefab = Prefab;            
        }
        else
        {
            Prefab.GetComponentInChildren<EnemyPrefabStore>().Prefab = Prefab;            
        }

        if (prefabEid.enemyType == EnemyType.Swordsmachine)
        {
            var swordsMachine = prefabEid.GetComponent<SwordsMachine>();
            swordsMachine.secondPhasePosTarget = null;
            swordsMachine.firstPhase = false;
            swordsMachine.GetComponent<EnemyIdentifier>().spawnIn = true;
            swordsMachine.inAction = false;
            swordsMachine.inSemiAction = false;
            swordsMachine.moveAtTarget = false;
        }

        IsStoringPrefab = false;
    }
}