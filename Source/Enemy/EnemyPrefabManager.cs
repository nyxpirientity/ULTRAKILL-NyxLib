using Nyxpiri.Collections.Generic;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class EnemyPrefabManager
    {
        public static void Initialize()
        {
            UpdateEvents.OnLateUpdate += LateUpdate;

            ScenesEvents.OnSceneWasLoaded += (scene, levelName, unitySceneName) =>
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

        public static void LateUpdate()
        {
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

            if (InstanceStores.Count > 0)
            {
                for (int i = 0, j = 0; i < 50 && j < 1; i++) // TODO: Make options for this
                {
                    InstanceStoreTickIdx = (InstanceStoreTickIdx + 1) % InstanceStores.SoftCapacity;

                    if (!InstanceStores.IsIndexValid(InstanceStoreTickIdx))
                    {
                        continue;
                    }

                    var instanceStore = InstanceStores[InstanceStoreTickIdx];
                    if (!instanceStore.IsFull)
                    {
                        Assert.IsNotNull(instanceStore);
                        instanceStore.InstantiateAndStore();
                        j++;
                    }
                }
            }
        }

        public static int RegisterInstanceStore(EnemyPrefabStore.InstanceStore instanceStore)
        {
            TickSkipInstantiation = true;
            return InstanceStores.Add(instanceStore);
        }

        public static void UnregisterInstanceStore(int idx)
        {
            InstanceStores.RemoveAt(idx);
        }

        private static int InstanceStoreTickIdx = 0;
        private static ReserveList<EnemyPrefabStore.InstanceStore> InstanceStores = new ReserveList<EnemyPrefabStore.InstanceStore>(256);
        private static int _findEidsFrameCountdown;

        public static bool TickSkipInstantiation { get; private set; } = false;
    }
}