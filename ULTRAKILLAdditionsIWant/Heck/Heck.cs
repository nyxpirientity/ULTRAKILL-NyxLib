using System;
using UnityEngine;

namespace UKAIW
{
    /* Represents heck itself. */
    public class Heck : MonoBehaviour
    {
        public static Heck Instance = null;

        public AgonyTracker AgonyTracker { get; private set; } = null;

        protected void Awake()
        {
            Instance = this;
        }

        protected void Start()
        {
            AgonyTracker = gameObject.AddComponent<AgonyTracker>();
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