
using System.Collections.Generic;

namespace Nyxpiri.Unity.Collections
{
    public static class CollectionSorting
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int targetIdx = UnityEngine.Random.Range(0, list.Count - 1);
                var movingValue = list[i];
                list.RemoveAt(i);
                list.Insert(targetIdx, movingValue);
            }
        }
    }
}