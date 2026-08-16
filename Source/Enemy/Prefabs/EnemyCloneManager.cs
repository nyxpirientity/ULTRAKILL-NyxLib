using System.Collections.Generic;
using Nyxpiri.Collections.Generic;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public static class EnemyCloneManager
{
    public static void RequestInstanceStoreCapacity(int requestedCapacity)
    {
        InstanceStoreCapacityModsAdditional = Mathf.Max(InstanceStoreCapacity, requestedCapacity);
    }

    public static int InstanceStoreCapacity => Mathf.Min(InstanceStoreCapacityModsAdditional, Options.EnemyPrefabInstanceStoreCapacityMax.Value);
    public static int InstanceStoreCapacityModsAdditional { get; private set; } = 0;

    public static void Initialize()
    {
        UpdateEvents.OnLateUpdate += LateUpdate;

        SceneEvents.OnSceneLoad += (scene, levelName, unitySceneName) =>
        {
            FindEIDs();
            _findEidsFrameCountdown = 5;
        };
    }

    private static void FindEIDs()
    {
        var eids = UnityEngine.Object.FindObjectsOfType<EnemyIdentifier>(true);

        foreach (var eid in eids)
        {
            var enemyComps = eid.GetOrAddComponent<EnemyComponents>();

            if (!enemyComps.HasDoneSetup)
            {
                Log.TraceExpectedInfo($"FindEIDs search is setting up an enemycomps on {eid.gameObject}!");
            }

            enemyComps.Setup();
        }
    }

    private static void LateUpdate()
    {
        if (CloneStores.Count > 0)
        {
            List<EnemyCloneStore> removingStores = new();
            foreach (var store in CloneStores)
            {
                if (store.IsActive)
                {
                    continue;
                }

                if (store.ReferenceCount > 0)
                {
                    CloneDestructionWaits[store] = 1;
                    continue;
                }

                if (CloneDestructionWaits[store] <= 0)
                {
                    EnemyCloneStore.Destroy(store);
                    removingStores.Add(store);
                    CloneDestructionWaits.Remove(store);
                    continue;
                }

                CloneDestructionWaits[store] -= 1;
            }

            foreach (var store in removingStores)
            {
                CloneStores.Remove(store);
            }
        }

        if (Options.SkipPrefabManagerTicks.Value)
        {
            return;
        }

        if (!Cheats.Enabled)
        {
            return;
        }

        if (_findEidsFrameCountdown >= 0)
        {
            if (_findEidsFrameCountdown == 0)
            {
                FindEIDs();
            }

            _findEidsFrameCountdown -= 1;
        }

        if (TickSkipInstantiation)
        {
            TickSkipInstantiation = false;
            return;
        }

        if (ActiveCloneStores.Count > 0)
        {
            for (int i = 0, j = 0; i < 50 && j < 1; i++) // TODO: Make options for this
            {
                InstanceStoreTickIdx = (InstanceStoreTickIdx + 1) % ActiveCloneStores.SoftCapacity;

                if (!ActiveCloneStores.IsIndexValid(InstanceStoreTickIdx))
                {
                    continue;
                }

                var instanceStore = ActiveCloneStores[InstanceStoreTickIdx];
                if (!instanceStore.IsFull)
                {
                    Assert.IsNotNull(instanceStore);
                    instanceStore.InstantiateAndStore();
                    j++;
                }
            }
        }
    }

    internal static int RegisterCloneStore(EnemyCloneStore cloneStore)
    {
        TickSkipInstantiation = true;
        CloneStores.Add(cloneStore);
        CloneDestructionWaits.TryAdd(cloneStore, 1);
        return ActiveCloneStores.Add(cloneStore);
    }

    internal static void UnregisterCloneStore(int idx)
    {
        ActiveCloneStores.RemoveAt(idx);
    }

    private static int InstanceStoreTickIdx = 0;
    private static ReserveList<EnemyCloneStore> ActiveCloneStores = new(256);
    private static HashSet<EnemyCloneStore> CloneStores = new(256);
    private static Dictionary<EnemyCloneStore, int> CloneDestructionWaits = new(256);
    private static int _findEidsFrameCountdown;

    public static bool TickSkipInstantiation { get; private set; } = false;
}