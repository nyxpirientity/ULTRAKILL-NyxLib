using System;
using System.Collections.Generic;

namespace Nyxpiri.Collections.Generic
{
    /* list that allows indices to be reserved for individual elements, even if you remove elements at indices before that element */
    public class ReserveList<T>
    {
        private struct ReserveListElem
        {
            public ReserveListElem(T obj, bool free)
            {
                Obj = obj;
                Free = free;
            }

            public T Obj;
            public bool Free;
        }

        ~ReserveList()
        {
            #if GC_CLASS_DISPOSAL_LOGGING
            NyxGCLogging.Log(GetType().FullName);
            #endif
        }

        public ReserveList()
        {
            _List = new List<ReserveListElem>();
            _FreeList = new Stack<int>();
        }

        public ReserveList(int capacity)
        {
            _List = new List<ReserveListElem>(capacity);
            _FreeList = new Stack<int>(capacity);
        }

        private List<ReserveListElem> _List;
        private Stack<int> _FreeList;
        public int Count { get; private set; } = 0;
        public int SoftCapacity { get => _List.Count; }
        public int Capacity { get => _List.Capacity; }

        public T this[int index]
        {
            get
            {
                if (_List[index].Free)
                {
                    throw new IndexOutOfRangeException("Attempt to access an index that was in the _FreeList");
                }

                return _List[index].Obj;
            }
            set
            {
                if (_List[index].Free)
                {
                    throw new IndexOutOfRangeException("Attempt to access an index that was in the _FreeList");
                }
                
                var elem = _List[index];
                elem.Obj = value;
                _List[index] = elem;
            }
        }

        public int Add(T elem)
        {
            int idx;

            Count += 1;

            if (_FreeList.Count == 0)
            {
                idx = _List.Count;
                _List.Add(new ReserveListElem(elem, false));

                return idx;
            }

            idx = _FreeList.Pop();
            _List[idx] = new ReserveListElem(elem, false);
            

            return idx;
        }

        public void RemoveAt(int idx)
        {
            if (_List[idx].Free)
            {
                throw new IndexOutOfRangeException("Attempt to free already freed index");
            }

            Count -= 1;
            _List[idx] = new ReserveListElem(default, true);
            _FreeList.Push(idx);
        }

        public bool IsIndexValid(int idx)
        {
            if (idx < 0)
            {
                return false;
            }
            
            if (_List.Count <= idx)
            {
                return false;
            }

            if (_List[idx].Free)
            {
                return false;
            }

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var elem in _List)
            {
                if (elem.Free)
                {
                    continue;
                }

                yield return elem.Obj;
            }

            yield break;
        }
    }
}