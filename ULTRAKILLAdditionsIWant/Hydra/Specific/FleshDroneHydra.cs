using System;
using System.Collections.Generic;
using System.Diagnostics;
using MelonLoader;
using UKAIW;
using UKAIW.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.Assertions;

namespace UKAIW
{
    [Serializable]
    public class FleshDroneHydra : MonoBehaviour
    {
        public class SharedData : ScriptableObject
        {
            public FleshPrison Prison = null;
        }

        EnemyHydraMod Hydra = null;
        public SharedData Shared = null;
        private bool RegisteredWithPrison = false;
        protected void Start()
        {
            Hydra = GetComponent<EnemyHydraMod>();
            Shared = (SharedData)(Hydra.Shared.EnemySpecificShared);

            if (Hydra.Depth > 0)
            {
                RegisterWithPrison();
            }
        }

        protected void FixedUpdate()
        {
            if (Hydra.Depth > 0)
            {
                RegisterWithPrison();

                if (Shared.Prison == null)
                {
                    Hydra.Eid.InstaKill();
                    return;
                }

                if (Shared.Prison.GetComponent<EnemyIdentifier>().dead)
                {
                    Hydra.Eid.InstaKill();
                    return;
                }
            }
        }

        private void RegisterWithPrison()
        {
            if (RegisteredWithPrison)
            {
                return;
            }
            
            if (Hydra.Eid.dead)
            {
                return;
            }

            Shared.Prison?.currentDrones.Add(GetComponent<DroneFlesh>());
            RegisteredWithPrison = true;
        }

        internal void NotifyPrisonDied()
        {
            if (Hydra.Eid.dead)
            {
                return;
            }

            Hydra.Eid.InstaKill();
        }
    }
}