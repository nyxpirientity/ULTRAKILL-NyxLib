using System;
using UnityEngine;

namespace UKAIW
{
    /* Represents heck itself. */
    public class Heck : MonoBehaviour
    {
        public static Heck Instance = null;

        public PainStore PainStore { get; private set; } = null;
        public AggressiveAgony AggressiveAgony { get; private set; } = null;
        public GameObject PainMeterGo { get; private set; } = null;
        public PainMeter PainMeter { get; private set; } = null;

        protected void Awake()
        {
            Instance = this;
        }

        protected void Start()
        {
            PainStore = gameObject.AddComponent<PainStore>();
            AggressiveAgony = gameObject.AddComponent<AggressiveAgony>();

            if (Assets.HeatResistancePrefabWithoutHeatResistance != null || CanvasController.Instance == null)
            {
                PainMeterGo = GameObject.Instantiate(Assets.HeatResistancePrefabWithoutHeatResistance.transform.GetChild(0).gameObject, CanvasController.Instance.transform);
                PainMeter = PainMeterGo.AddComponent<PainMeter>();   
                PainMeterGo.SetActive(true);
            }
        }

        protected void Update()
        {
            if (PainMeterGo != null)
            {
                if (AggressiveAgony.Enabled && PainStore.Pain >= 0.1f)
                {
                    PainMeterGo.SetActive(true);
                }
                else
                {
                    PainMeterGo.SetActive(false);
                }
            }
        }

        protected void FixedUpdate()
        {
            
        }

        protected void OnDestroy()
        {
            
        }

        protected void OnEnable()
        {
            
        }
        
        protected void OnDisable()
        {
            
        }

        internal static void Initialize()
        {
            ScenesEvents.OnSceneWasLoaded += (sceneIndex, sceneName) => { CreateHeck(); };
        }

        private static void CreateHeck()
        {
            if (Instance != null)
            {
                return;
            }

            GameObject go = new GameObject();
            go.AddComponent<Heck>();
        }
    }
}