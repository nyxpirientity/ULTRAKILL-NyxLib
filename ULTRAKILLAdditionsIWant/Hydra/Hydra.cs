using UnityEngine;
using System.Collections.Generic;

namespace UKAIW
{
    public static class Hydra
    {
        public struct QueuedDupeInfo
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 LocalScale;
            public EnemyHydra.SharedData SharedData;
            public EnemyPrefabStore.InstanceStore InstanceStore;
            public int Depth;
            public EnemyType EnemyType;
            public bool BossBar;
        }

        public static void Initialize()
        {
            EnemyEvents.PreDeath += PreEnemyDeath;
            EnemyEvents.PreDeath += (eid, instakill) =>
            {
                var go = eid.gameObject;
                var eadd = go.GetComponent<EnemyAdditions>();
                var ehm = eadd.Hydra;
                
                ehm.NotifyOfDeath(instakill);
            };

            EnemyEvents.Death += DuringEnemyDeath;
            EnemyEvents.PostDeath += PostEnemyDeath;
            UpdateEvents.OnLateUpdate += LateUpdate;
            UpdateEvents.OnFixedUpdate += FixedUpdate;
            ScenesEvents.OnSceneWasUnloaded += OnSceneUnload;
        }

        private static void OnSceneUnload(int buildIndex, string sceneName)
        {

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

        private static void PreEnemyDeath(EnemyAdditions enemy, bool instakill)
        {
            var go = enemy.gameObject;
            var eadd = go.GetComponent<EnemyAdditions>();
            var ehm = eadd.Hydra;
            
            ehm.NotifyOfDeath(instakill);
        }

        private static void PostEnemyDeath(EnemyAdditions enemy, bool instakill)
        {
            if (Cheats.IsCheatEnabled(Cheats.HydraMode))
            {
                Hydra.DecrementRemainingHydraBloodFxThisTick();
            }
        }

        private static void DuringEnemyDeath(EnemyAdditions enemy)
        {
            var go = enemy.gameObject;
            var eadd = go.GetComponent<EnemyAdditions>();
            var ehm = eadd.Hydra;
            ehm.NotifyOfDeath(false);
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
            
            while (ImmediatelyDupeStack.Count > 0)
            {
                InstantiateDupe(ImmediatelyDupeStack.Pop());
            }
        }

        public static void InstantiateDupe(QueuedDupeInfo dupeInfo)
        {
            InstantiatedThisTick += 1;
            var dupeGo = dupeInfo.InstanceStore.GetNewInstance();
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

            Assert.IsNotNull(eadd);
            Assert.IsNotNull(eadd.Hydra);
            Assert.IsNotNull(eadd.Hydra.Shared);

            eadd.Hydra.Depth = dupeInfo.Depth;

            if (dupeInfo.BossBar)
            {
                eid.BossBar(true);
            }
        }
    }
}