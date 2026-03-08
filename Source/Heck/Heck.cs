using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    /* Represents heck itself. */
    public class Heck : MonoBehaviour
    {
        public static Heck Itself = null;
        public static MonoRegistrar MonoRegistrar = new MonoRegistrar();

        public GameObject PainMeterGo { get; private set; } = null;

        public T GetMonoByIndex<T>(int idx) where T : MonoBehaviour
        {
            if (idx < 0)
            {
                throw new IndexOutOfRangeException($"Index {idx} was less than 0");
            }

            if (idx > _monoBehaviours.Count)
            {
                return null;
            }

            return _monoBehaviours[idx] as T;
        }

        protected void Awake()
        {
            Itself = this;
        }

        protected void Start()
        {
            _monoBehaviours = new List<MonoBehaviour>(MonoRegistrar.RegisteredTypes.Count);

            foreach (var type in MonoRegistrar.RegisteredTypes)
            {
                _monoBehaviours.Add((MonoBehaviour)(gameObject.AddComponent(type)));
            }
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
            if (Itself != null)
            {
                return;
            }

            GameObject go = new GameObject();
            go.AddComponent<Heck>();
        }

        private List<MonoBehaviour> _monoBehaviours = null;
    }
}