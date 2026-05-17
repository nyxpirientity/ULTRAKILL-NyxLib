using System.Reflection;
using UnityEngine;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using System.Collections.Generic;
using System;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class PlayerComponents : MonoBehaviour
    {
        public delegate void PreStartEventHandler(EventMethodCanceler canceler, PlayerComponents player);
        public static event PreStartEventHandler PreStart = null;
        public delegate void PostStartEventHandler(EventMethodCancelInfo canceler, PlayerComponents player);
        public static event PostStartEventHandler PostStart = null;

        public static MonoRegistrar MonoRegistrar = new MonoRegistrar();
        public NewMovement NewMovement { get; private set; } = null;

        public static PlayerComponents Instance { get; private set; } = null;
        
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
            NewMovement = gameObject.GetComponent<NewMovement>();

            _monoBehaviours = new List<MonoBehaviour>(MonoRegistrar.RegisteredTypes.Count);

            foreach (var type in MonoRegistrar.RegisteredTypes)
            {
                _monoBehaviours.Add((MonoBehaviour)gameObject.GetOrAddComponent(type));
            }

            Instance = this;
        }

        protected void Start()
        {
            EventMethodCancellationTracker cancellationTracker = new EventMethodCancellationTracker();
            PreStart?.Invoke(cancellationTracker.GetCanceler(), this);
            
            if (cancellationTracker.ShouldRunMethod)
            {
                
            }

            PostStart?.Invoke(cancellationTracker.GetCancelInfo(), this);
        }

        private List<MonoBehaviour> _monoBehaviours = null;
    }
}