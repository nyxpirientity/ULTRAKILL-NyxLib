using System;
using MelonLoader;
using UnityEngine;
using HarmonyLib;
using MelonLoader.Utils;
using System.Reflection;
using System.Collections.Generic;
using UKAIW.Diagnostics.Debug;
using ULTRAKILL.Cheats;
using UnityEngine.UI;

namespace UKAIW
{
    public static class Hydra
    {
        public struct QueuedDupeInfo
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 LocalScale;
            public EnemyHydraMod.SharedData SharedData;
            public int Depth;
            public EnemyType EnemyType;
            public bool BossBar;
        }

        public static void Initialize()
        {
            EnemyEvents.PreNoIKDeath += PreEnemyDeath;
            EnemyEvents.DuringDeath += DuringEnemyDeath;
            EnemyEvents.PostNoIKDeath += PostEnemyDeath;
            UpdateEvents.OnLateUpdate += LateUpdate;
            UpdateEvents.OnFixedUpdate += FixedUpdate;
            ScenesEvents.OnSceneWasUnloaded += OnSceneUnload;
        }

        private static void OnSceneUnload(int buildIndex, string sceneName)
        {
            for (int i = 0; i < SharedDatas.Count; i++)
            {
                if (!SharedDatas.IsIndexValid(i))
                {
                    continue;
                }

                SharedDatas.RemoveAt(i);
            }
        }

        public static void DecrementRemainingHydraBloodFxThisTick()
        {
            BloodOptimizer.DecrementRemainingBloodFxThisTick();
        }

        public static int InstantiatedThisTick = 0;
        public static ulong NumFixedUpdates = 0;
        private static void FixedUpdate()
        {
            if (!Cheats.IsHydraModeOn)
            {
                return;
            }

            for (int i = InstantiatedThisTick; (i < Options.HydraMaxPerUpdate) && (DupeQueue.Count != 0); i++)
            {
                var dupeInfo = DupeQueue.Dequeue();

                InstantiateDupe(dupeInfo);
            }

            InstantiatedThisTick = 0;
            NumFixedUpdates += 1;
        }

        public static void EnqueueDupe(QueuedDupeInfo dupeInfo)
        {
            if (InstantiatedThisTick >= Options.HydraMaxPerUpdate)
            {
                DupeQueue.Enqueue(dupeInfo);
            }
            else
            {
                ImmediatelyDupeStack.Push(dupeInfo);
            }

            if (DupeQueue.Count > Options.HydraMaxPerUpdate && Cheats.IsCheatEnabled(Cheats.HitstopOnHeavyHydraLoad))
            {
                Hitstop(0.35);
            }
        }

        private static double LastHitstopTimestamp = 0.0;
        public static void Hitstop(double maxRate)
        {
            if (LastHitstopTimestamp > Time.unscaledTimeAsDouble - maxRate)
            {
                return;
            }

            TimeController.Instance.ParryFlash();
            LastHitstopTimestamp = Time.unscaledTimeAsDouble;
        }

        private static void PreEnemyDeath(EnemyIdentifier eid)
        {
            var go = eid.gameObject;
            var eadd = go.GetComponent<EnemyAdditions>();
            var ehm = eadd.HydraMod;
            
            ehm.NotifyOfDeath();
        }

        private static void PostEnemyDeath(EnemyIdentifier eid)
        {
            if (Cheats.IsCheatEnabled(Cheats.HydraMode))
            {
                Hydra.DecrementRemainingHydraBloodFxThisTick();
            }
        }

        private static void DuringEnemyDeath(EnemyIdentifier eid)
        {
            var go = eid.gameObject;
            var eadd = go.GetComponent<EnemyAdditions>();
            var ehm = eadd.HydraMod;
            ehm.DuringDeath();
        }

        private static Queue<QueuedDupeInfo> DupeQueue = new Queue<QueuedDupeInfo>(256);
        private static Stack<QueuedDupeInfo> ImmediatelyDupeStack = new Stack<QueuedDupeInfo>(256);

        private static void LateUpdate()
        {
            if (!Cheats.IsHydraModeOn)
            {
                return;
            }

            if (SharedDatas.Count > 0)
            {
                for (int i = 0, j = 0; i < Options.HydraPrefabPoolGrowPerUpdate * 2 && j < Options.HydraPrefabPoolGrowPerUpdate; i++)
                {
                    SharedDataForPrefabCacheIdx = (SharedDataForPrefabCacheIdx + 1) % SharedDatas.SoftCapacity;

                    if (!SharedDatas.IsIndexValid(SharedDataForPrefabCacheIdx))
                    {
                        continue;
                    }

                    var sharedData = SharedDatas[SharedDataForPrefabCacheIdx];
                    if (!sharedData.PrefabPoolFull)
                    {
                        Assert.IsNotNull(sharedData);
                        sharedData.InstantiatePrefabToPool();
                        j++;
                    }
                }
            }
            
            while (ImmediatelyDupeStack.Count > 0)
            {
                InstantiateDupe(ImmediatelyDupeStack.Pop());
            }
        }
        private static int SharedDataForPrefabCacheIdx = 0;
        public static ReserveList<EnemyHydraMod.SharedData> SharedDatas = new ReserveList<EnemyHydraMod.SharedData>(256);

        public static void InstantiateDupe(QueuedDupeInfo dupeInfo)
        {
            InstantiatedThisTick += 1;
            var dupeGo = dupeInfo.SharedData.GetNewInstance();
            GameObject malFaceDupeGo = null;
            EnemyAdditions eadd;

            if (dupeInfo.EnemyType == EnemyType.MaliciousFace)
            {
                malFaceDupeGo = dupeGo;
                eadd = dupeGo.GetComponentInChildren<EnemyAdditions>();
                Assert.IsNotNull(eadd);
                dupeGo = eadd.gameObject;
            }
            else
            {
                Assert.IsNotNull(dupeGo);
                eadd = dupeGo.GetComponent<EnemyAdditions>();
            }
            
            dupeGo.transform.position = dupeInfo.Position;
            dupeGo.transform.rotation = dupeInfo.Rotation;

            dupeGo.SetActive(true);
            malFaceDupeGo?.SetActive(true);
            var eid = dupeGo.GetComponent<EnemyIdentifier>();
            
            eid.spawnIn = false;
            eid.timeSinceSpawned = 0.0f;

            eadd.FindAndCacheMods();

            Assert.IsNotNull(eadd);
            Assert.IsNotNull(eadd.HydraMod);
            Assert.IsNotNull(eadd.HydraMod.Shared);

            eadd.HydraMod.Depth = dupeInfo.Depth;

            if (dupeInfo.BossBar)
            {
                eid.BossBar(true);
            }
        }
    }
}