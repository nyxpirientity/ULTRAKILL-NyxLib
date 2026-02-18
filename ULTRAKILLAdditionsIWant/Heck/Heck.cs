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

        protected void Awake()
        {
            Instance = this;
        }

        protected void Start()
        {
            PainStore = gameObject.AddComponent<PainStore>();
            AggressiveAgony = gameObject.AddComponent<AggressiveAgony>();
        }

        protected void Update()
        {
            
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