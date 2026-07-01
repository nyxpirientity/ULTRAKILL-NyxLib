using System.IO;
using HarmonyLib;
using TMPro;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System;

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets
{
    public class PrefabAsset<T> where T : UnityEngine.Object
    {
        internal PrefabAsset(T prefab)
        {
            _prefab = prefab;
        }

        public T Instantiate(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return UnityEngine.Object.Instantiate(_prefab, position, rotation, parent);
        }

        public T Instantiate(Transform parent)
        {
            return UnityEngine.Object.Instantiate(_prefab, parent);
        }

        private T _prefab = null;
    }
}