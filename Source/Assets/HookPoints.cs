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
        public static GameObject HealingHookPoint { get; internal set; } = null;
        public static GameObject SlingshotHookPoint { get; internal set; } = null;
        public static GameObject NormalHookPoint { get; internal set; } = null;

        private void Awake()
        {
            SpawnDbPicker.OnGettingPrefabs += GetPrefabs;
        }

        private static void GetPrefabs(SpawnableObjectsDatabase db)
        {
            foreach (var obj in db.sandboxObjects)
            {
                var hookPoint = obj.gameObject.GetComponentInChildren<HookPoint>();

                if (hookPoint != null)
                {
                    switch (hookPoint.type)
                    {
                        case hookPointType.Normal:
                            HookPoints.NormalHookPoint = GameObject.Instantiate(obj.gameObject, AssetsRoot.Holder);
                            HookPoints.NormalHookPoint.SetActive(false);
                            break;
                        case hookPointType.Slingshot:
                            if (hookPoint.healPlayer)
                            {
                                HookPoints.HealingHookPoint = GameObject.Instantiate(obj.gameObject, AssetsRoot.Holder);
                                HookPoints.HealingHookPoint.SetActive(false);
                            }
                            else
                            {
                                HookPoints.SlingshotHookPoint = GameObject.Instantiate(obj.gameObject, AssetsRoot.Holder);
                                HookPoints.SlingshotHookPoint.SetActive(false);
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