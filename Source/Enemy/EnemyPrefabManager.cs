using Nyxpiri.Collections.Generic;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public static class EnemyPrefabManager
    {
        public static void Initialize()
        {
            UpdateEvents.OnLateUpdate += LateUpdate;
        }

        public static void LateUpdate()
        {
            if (InstanceStores.Count > 0)
            {
                for (int i = 0, j = 0; i < Options.HydraPrefabPoolGrowPerUpdate * 2 && j < Options.HydraPrefabPoolGrowPerUpdate; i++)
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
            return InstanceStores.Add(instanceStore);
        }

        public static void UnregisterInstanceStore(int idx)
        {
            InstanceStores.RemoveAt(idx);
        }

        private static int InstanceStoreTickIdx = 0;
        private static ReserveList<EnemyPrefabStore.InstanceStore> InstanceStores = new ReserveList<EnemyPrefabStore.InstanceStore>(256);
    }
}