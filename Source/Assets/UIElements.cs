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
    public class UIElements : MonoSingleton<UIElements>
    {
        public static PrefabAsset<TextMeshProUGUI> Label { get; private set; } = null;
        private TextMeshProUGUI _labelPrefab = null;

        private void Awake()
        {
            SceneEvents.OnSceneStart += OnNewSceneStart;
        }

        private void OnNewSceneStart(Scene scene, string levelName, string unitySceneName)
        {
            if (_labelPrefab == null)
            {
                var hc = HudController.Instance;

                if (hc != null)
                {
                    var text = hc.speedometer.textMesh;
                    var labelGo = GameObject.Instantiate(text.gameObject, AssetsRoot.Holder);
                    labelGo.SetActive(true);
                    _labelPrefab.GetComponent<TextMeshProUGUI>().text = "UKAIW-Label!";
                    Label = new PrefabAsset<TextMeshProUGUI>(_labelPrefab);
                }
            }
        }
    }
}