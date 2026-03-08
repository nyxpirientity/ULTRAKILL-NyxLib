using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class MonoRegistrar
    {
        public IReadOnlyList<Type> RegisteredTypes { get => _registeredTypes; }

        public int Register<T>() where T : MonoBehaviour
        {
            if (_registeredTypes.Contains(typeof(T)))
            {
                throw new ArgumentException($"Type '{typeof(T).FullName}' was already registered in this MonoRegistrar");
            }
            
            int idx = _registeredTypes.Count;

            _registeredTypes.Add(typeof(T));

            return idx;
        }

        private List<Type> _registeredTypes = new List<Type>(32);
    }
}