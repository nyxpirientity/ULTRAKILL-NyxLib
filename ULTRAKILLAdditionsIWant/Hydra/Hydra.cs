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
        }

        public static void Initialize()
        {
            EnemyDeathEvents.PreDeath += PreEnemyDeath;
            EnemyDeathEvents.PostDeath += PostEnemyDeath;
            UpdateEvents.OnLateUpdate += LateUpdate;
            UpdateEvents.OnFixedUpdate += FixedUpdate;
        }
        
        private static int RemainingHydraBloodFxThisTick = 0;
        private static bool BloodDisabledByUs = false;
        public static void DecrementRemainingHydraBloodFxThisTick()
        {
            RemainingHydraBloodFxThisTick -= 1;
            
            bool wasBloodEnabled = MonoSingleton<PrefsManager>.Instance.GetBoolLocal("bloodEnabled");

            if (RemainingHydraBloodFxThisTick <= 0 && wasBloodEnabled)
            {
                MonoSingleton<PrefsManager>.Instance.SetBoolLocal("bloodEnabled", false);
                BloodDisabledByUs = true;
            }
        }

        public static int InstantiatedThisTick = 0;
        public static ulong NumFixedUpdates = 0;
        private static void FixedUpdate()
        {
            if ((NumFixedUpdates % (ulong)Options.HydraBloodCapNumUpdatesPerTick) == 0)
            {
                RemainingHydraBloodFxThisTick = Options.HydraBloodCapNumBloodPerTick;
                
                if (BloodDisabledByUs)
                {
                    MonoSingleton<PrefsManager>.Instance.SetBoolLocal("bloodEnabled", true);
                    BloodDisabledByUs = false;
                }
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
            var ehm = eadd.GetMod<EnemyHydraMod>();
            
            ehm.OnDeath();
        }

        private static void PostEnemyDeath(EnemyIdentifier eid)
        {
            if (Cheats.IsCheatEnabled(Cheats.HydraMode))
            {
                Hydra.DecrementRemainingHydraBloodFxThisTick();
            }
        }

        private static Queue<QueuedDupeInfo> DupeQueue = new Queue<QueuedDupeInfo>(256);
        private static Stack<QueuedDupeInfo> ImmediatelyDupeStack = new Stack<QueuedDupeInfo>(256);

        private static void LateUpdate()
        {
            if (SharedDatas.Count > 0)
            {
                SharedDataForPrefabCacheIdx = (SharedDataForPrefabCacheIdx + 1) % SharedDatas.Count;
                var sharedData = SharedDatas[SharedDataForPrefabCacheIdx];
                if (sharedData.PrefabPool.Count < sharedData.PrefabPool.Capacity)
                {
                    sharedData.InstantiatePrefabToPool();
                }
            }

            while (ImmediatelyDupeStack.Count > 0)
            {
                InstantiateDupe(ImmediatelyDupeStack.Pop());
            }
        }
        private static int SharedDataForPrefabCacheIdx = 0;
        public static List<EnemyHydraMod.SharedData> SharedDatas = new List<EnemyHydraMod.SharedData>(256);

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
                eadd = dupeGo.GetComponent<EnemyAdditions>();
            }

            dupeGo.SetActive(true);
            malFaceDupeGo?.SetActive(true);
            var eid = dupeGo.GetComponent<EnemyIdentifier>();

            eid.spawnIn = false;
            eid.timeSinceSpawned = 0.0f;

            eadd.FindAndCacheMods();
            eadd.HydraMod.ContributesToInstanceCount = true;

            Assert.IsNotNull(eadd);
            Assert.IsNotNull(eadd.HydraMod);
            Assert.IsNotNull(eadd.HydraMod.Shared);

            dupeGo.transform.position = dupeInfo.Position;
            dupeGo.transform.rotation = dupeInfo.Rotation;
            dupeGo.transform.localScale = dupeInfo.LocalScale;

            eadd.HydraMod.Depth = dupeInfo.Depth;
        }
    }
}