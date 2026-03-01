using System;
using System.Collections.Generic;
using UKAIW;

namespace Nyxpiri
{
    // TODO: this ObjPool implementation is worse than the one I currently use for my game, maybe should update it to the new one?
    /* 
    Represents a pool of reusable objects, originally written for my personal godot game, NyxpiriOS 
    */
    public class ObjPool<T> where T : class, new()
    {
        public Func<T> ConstructObject = null;
        public Action<T> PrepareObject = (obj) => { };
        public Action<T> UnprepareObject = (obj) => { };
        public Action<T> DestructObject = (obj) => { };
        private Stack<int> _FreeList = new Stack<int>();
        private T[] Array = new T[0];

        public int Size { get => Array.Length; }
        public int NumFree { get => _FreeList.Count; }

        public int NumTaken { get => Array.Length - _FreeList.Count; }

        public ObjPool(Func<T> constructObject, Action<T> destructObject)
        {
            Assert.IsNotNull(constructObject);
            DestructObject = destructObject;
            ConstructObject = constructObject;
        }

        public void Clear()
        {
            foreach (var obj in Array)
            {
                DestructObject(obj);
            }

            Array = new T[0];
            _FreeList.Clear();
        }

        public void EnsureSize(int size)
        {
            if (Array.Length >= size)
            {
                return;
            }

            var prevPool = Array;
            Array = new T[size];
            //_FreeList.EnsureCapacity(size); existed in my original but ig this version of the C# standard lib didn't have this??
            
            for (int i = 0; i < prevPool.Length; i++)
            {
                Array[i] = prevPool[i];
            }

            for (int i = prevPool.Length; i < Array.Length; i++)
            {
                Array[i] = ConstructObject();
                _FreeList.Push(i);
                Assert.IsNotNull(Array[i]);
            }
        }

        public ValueTuple<T, int> TakeUnsafe()
        {
            if (_FreeList.Count == 0)
            {
                EnsureSize(Size * 2);
            }

            T obj;

            int idx = _FreeList.Pop();
            obj = Array[idx];

            PrepareObject?.Invoke(obj);

            return (obj, idx);
        }
        
        public PoolObject<T> Take()
        {
            return new PoolObject<T>(this, TakeUnsafe());
        } 

        public void Return(ValueTuple<T, int> element)
        {
            UnprepareObject?.Invoke(element.Item1);
            _FreeList.Push(element.Item2);
        }

        public void Return(PoolObject<T> obj)
        {
            obj.Dispose();
        }

        ~ObjPool()
        {
            Clear();
        }
    }

    /* Safe way to handle an object from ObjPool, on Dispose or it's destructor, it should handle returning the object to the pool for you */
    public class PoolObject<T> : IDisposable where T : class, new()
    {
        public T Obj { get => Element.Item1; }
        public T Value { get => Obj; }
        public bool IsValid { get => Pool != null; }

        private ObjPool<T> Pool;
        private ValueTuple<T, int> Element;

        internal PoolObject(ObjPool<T> pool, ValueTuple<T, int> element)
        {
            Pool = pool;
            Element = element;
        }

        public void Dispose()
        {
            if (!IsValid)
            {
                return;
            }

            Pool.Return(Element);
            Element = (null, -1);
            Pool = null;
        }

        ~PoolObject()
        {
            Dispose();
        }
    }
}