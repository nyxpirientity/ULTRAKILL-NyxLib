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
        internal PrefabAsset(Func<T> getter)
        {
            _getter = getter;
        }

        public T Instantiate(bool active, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            SetPrefabActive(active);

            return UnityEngine.Object.Instantiate(DirectPrefab, position, rotation, parent);
        }

        public T Instantiate(bool active, Transform parent)
        {
            SetPrefabActive(active);

            return UnityEngine.Object.Instantiate(DirectPrefab, parent);
        }

        public T DirectPrefab => _getter?.Invoke();

        private void SetPrefabActive(bool active)
        {
            if (DirectPrefab is GameObject prefabGo)
            {
                prefabGo.SetActive(active);
            }
            else if (DirectPrefab is Component prefabComp)
            {
                prefabComp.gameObject.SetActive(active);
            }
        }

        private Func<T> _getter = null;
    }
}