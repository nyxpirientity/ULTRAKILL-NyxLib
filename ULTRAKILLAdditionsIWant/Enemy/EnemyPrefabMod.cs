using System;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

[Serializable]
public class EnemyPrefabMod : MonoBehaviour
{
    public GameObject Prefab = null;
    private GameObject _PrefabParent = null;
    public GameObject PrefabParent { get => _PrefabParent ?? null; }
    private EnemyIdentifier Eid = null;

    private static bool IsStoringPrefab = false;
    protected void Awake()
    {
        Eid = GetComponent<EnemyIdentifier>();

        StorePrefab();
    }

    protected void Start()
    {
        if (Prefab == null)
        {
            StorePrefab();
        }
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
            templateGo = transform.parent.gameObject;
        }
        else
        {
            templateGo = gameObject;
            Assert.IsNotNull(templateGo.GetComponent<EnemyPrefabMod>());
        }
        
        IsStoringPrefab = true;
        bool wasActive = false;
        if (templateGo.activeSelf)
        {
            wasActive = true;
            //templateGo.SetActive(false);
        }
        Prefab = UnityEngine.Object.Instantiate(templateGo);
        _PrefabParent = templateGo?.transform?.parent?.gameObject;
        Prefab.SetActive(false);
        
        var prefabEid = Prefab.GetComponent<EnemyIdentifier>() ?? Prefab.GetComponentInChildren<EnemyIdentifier>();
        
        prefabEid.destroyOnDeath = new System.Collections.Generic.List<GameObject>();
        prefabEid.activateOnDeath = new GameObject[0];
        prefabEid.drillers = new System.Collections.Generic.List<Harpoon>();
        prefabEid.stuckMagnets = new System.Collections.Generic.List<Magnet>();
        //prefabEid.onDeath = null;

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

        if (prefabEid.drone != null)
        {
            prefabEid.drone.musicRequested = false;
        }

        if (eid.enemyType != EnemyType.MaliciousFace)
        {
            Assert.IsNotNull(Prefab.GetComponent<EnemyHydraMod>());
            Assert.IsNotNull(Prefab.GetComponent<EnemyPrefabMod>());
            Prefab.GetComponent<EnemyPrefabMod>().Prefab = Prefab;            
        }
        else
        {
            Prefab.GetComponentInChildren<EnemyPrefabMod>().Prefab = Prefab;            
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
        if (wasActive)
        {
            //templateGo.SetActive(true);
        }
    }
}