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
    [ConfigureSingleton(SingletonFlags.NoAutoInstance)]
    public class HookPoints : MonoSingleton<HookPoints>
    {
        public static PrefabAsset<GameObject> Normal { get; private set; } = null;
        public static PrefabAsset<GameObject> Slingshot { get; private set; } = null;
        public static PrefabAsset<GameObject> Healing { get; private set; } = null;

        private GameObject _normalHookPoint = null;
        private GameObject _slingshotHookPoint = null;
        private GameObject _healingHookPoint = null;

        private void Awake()
        {
            SpawnDbPicker.OnGettingPrefabs += GetPrefabs;
        }

        private void GetPrefabs(SpawnableObjectsDatabase db)
        {
            Log.ExpectedInfo($"Getting hookpoints...");
            foreach (var obj in db.sandboxObjects)
            {
                var hookPoint = obj.gameObject.GetComponentInChildren<HookPoint>();

                if (hookPoint != null)
                {
                    switch (hookPoint.type)
                    {
                        case hookPointType.Normal:
                            _normalHookPoint = GameObject.Instantiate(obj.gameObject, AssetsRoot.Holder);
                            break;
                        case hookPointType.Slingshot:
                            if (hookPoint.healPlayer)
                            {
                                _healingHookPoint = GameObject.Instantiate(obj.gameObject, AssetsRoot.Holder);
                            }
                            else
                            {
                                _slingshotHookPoint = GameObject.Instantiate(obj.gameObject, AssetsRoot.Holder);
                            }
                            break;
                        case hookPointType.Switch:
                            break;
                    }
                }
            }
        }
    }
}