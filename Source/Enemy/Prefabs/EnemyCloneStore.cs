using System;
using System.Collections.Generic;
using Nyxpiri;
using Nyxpiri.ULTRAKILL.NyxLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public class EnemyCloneStore : ScriptableObject
{
    public int ReferenceCount => _reservations + Instances.Count;
    public int Reservations => _reservations;
    public bool IsFull => Instances.Count >= EnemyCloneManager.InstanceStoreCapacity;
    public bool IsActive => RegistrationTracker.Registered;

    public GameObject GetNewInstance(Transform parent = null)
    {
        Assert.IsNotNull(Prefab);

        GameObject instGo = null;

        if (Instances.Count > 0)
        {
            instGo = Instances.Pop();
        }

        instGo ??= Instantiate(Prefab);

        instGo.transform.parent = parent;

        if (PrefabEadd.Eid.enemyType == global::EnemyType.Stalker) // TODO: this is necessary to make them not... ragdoll instead of explode. not sure what the best approach is to fixing right now
        {
            var instEnemy = instGo.GetComponent<EnemyComponents>();
            instEnemy.PreDeath += (canceler, instakill) => { instGo.GetComponent<Stalker>().SandExplode(); };
            instEnemy.PostDeath += (cancelInfo, instakill) => { instGo.GetComponent<EnemyComponents>().InstaDestroy(); };
        }

        return instGo;
    }

    public void AddReservation()
    {
        Log.TraceExpectedInfo($"EnemyCloneStore '{_debugName}' had reservation added");
        _reservations += 1;
    }

    public void RemoveReservation()
    {
        Log.TraceExpectedInfo($"EnemyCloneStore '{_debugName}' had reservation removed");
        Assert.IsTrue(_reservations > 0);
        _reservations -= 1;
    }

    public GameObject Prefab = null;
    public Transform PrefabHolder = null;
    public Transform SpawnedInstanceParent = null;
    public EnemyComponents PrefabEadd = null;

    [SerializeField] private string _debugName = "UNNAMED";
    Stack<GameObject> Instances = new Stack<GameObject>();

    public void RegisterEnemy(EnemyCloning store)
    {
        RegistoredEnemies.Add(store);

        if (RegistoredEnemies.Count == 1)
        {
            RegistrationTracker.Register();
        }

        Assert.IsNotNull(Prefab);
    }

    public void UnregisterEnemy(EnemyCloning store)
    {
        RegistoredEnemies.Remove(store);

        if (RegistoredEnemies.Count == 0)
        {
            RegistrationTracker.Unregister();
        }
    }

    protected void OnDestroy()
    {
        Log.TraceExpectedInfo($"{_debugName} clone store being destroyed");
        GameObject.Destroy(PrefabHolder.gameObject);
    }

    internal void Initialize(GameObject prefab, Transform prefabParent, EnemyComponents prefabEadd, string debugName)
    {
        Prefab = prefab;
        SpawnedInstanceParent = prefabParent;
        PrefabEadd = prefabEadd;
        _debugName = debugName;

        Log.TraceExpectedInfo($"New instance store by the name of {debugName} being created with prefab {Prefab}");

        if (Cheats.Enabled)
        {
            Assert.IsNotNull(Prefab);
        }

        RegistrationTracker = new RegistrationTracker(
            registerAction: () =>
            {
                Log.TraceExpectedInfo($"{_debugName}: Registering to prefab manager");

                if (Cheats.Enabled)
                {
                    Assert.IsNotNull(Prefab);
                }

                RegistrationIdx = EnemyCloneManager.RegisterCloneStore(this);
                return true;
            },
            unregisterAction: () =>
            {
                Log.TraceExpectedInfo($"{_debugName}: Unregistering from prefab manager");

                if (Cheats.Enabled)
                {
                    Assert.IsNotNull(Prefab);
                }

                EnemyCloneManager.UnregisterCloneStore(RegistrationIdx);
                RegistrationIdx = -1;
                return true;
            }
        );
    }

    internal void InstantiateAndStore()
    {
        if (Prefab == null)
        {
            RegistrationTracker.Unregister();
            Log.Error($"{_debugName}: InstanceStore had instantiate and store called despite prefab being null, and thus destroyed.");
            return;
        }

        if (IsFull)
        {
            return;
        }

        Assert.IsNotNull(PrefabHolder);

        PrefabHolder.gameObject.SetActive(false);
        var newGo = Instantiate(Prefab, PrefabHolder);

        Log.TraceExpectedInfo($"{_debugName}: Instantiating and storing for prefab {Prefab}");

        Instances.Push(newGo);

        newGo.SetActive(false);
    }

    [SerializeField] private int _reservations = 0;

    HashSet<EnemyCloning> RegistoredEnemies = new HashSet<EnemyCloning>(32);
    RegistrationTracker RegistrationTracker = null;
    private int RegistrationIdx = -1;

}