using System.Collections.Generic;
using Nyxpiri;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class QuickMsg : MonoBehaviour
    {
        public Vector3 Velocity = Vector3.zero;
        public float Duration = 0.0f;
        public GlobalTimeStamp EnableTimestamp;

        public RectTransform RectTransform { get; private set; }
        public bool FlashEnabled { get; internal set; }
        public float FlashDelay { get; internal set; }
        public float FlashFadeTime { get; internal set; }
        public Color FlashColor { get; internal set; }

        public TextMeshProUGUI TextMesh = null;

        protected void Update()
        {
            Velocity = NyxMath.EaseInterpTo(Velocity, Vector3.zero, 3.0f, Time.deltaTime);
            RectTransform.anchoredPosition3D += Velocity * Time.deltaTime;
        }

        protected void OnEnable()
        {
            EnableTimestamp.UpdateToNow();
            RectTransform = GetComponent<RectTransform>();
            TextMesh = GetComponent<TextMeshProUGUI>();
        }
        
        protected void OnDisable()
        {
            
        }
    }

    public static class QuickMsgPool
    {
        public static void Initialize()
        {
            ScenesEvents.OnSceneWasUnloaded += OnSceneUnloaded;
            ScenesEvents.OnSceneWasLoaded += OnSceneLoaded;
            UpdateEvents.OnUpdate += OnUpdate;

            Pool = new ObjPool<GameObject>(() => 
            { 
                var go = GameObject.Instantiate(Assets.LabelPrefab, CanvasController.Instance.gameObject.transform);
                go.AddComponent<QuickMsg>();
                var textMesh = go.GetComponent<TextMeshProUGUI>();
                var rectTransform = go.GetComponent<RectTransform>();
                textMesh.text = "UKAIW QuickMsg default";
                textMesh.color = Color.white;
                textMesh.fontSize = 24.0f;
                textMesh.horizontalAlignment = HorizontalAlignmentOptions.Center;
                
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
                rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1000.0f);

                return go;
            }, 
            (go) => 
            { 
                GameObject.Destroy(go); 
            });
            
            Pool.PrepareObject = (go) =>
            {
            };

            Pool.UnprepareObject += (go) =>
            {
                go.SetActive(false);
            };
        }

        private static ObjPool<GameObject> Pool = null;
        private static List<(PoolObject<GameObject>, QuickMsg)> ActiveQuickMsgs = new List<(PoolObject<GameObject>, QuickMsg)>(32);

        private static void OnUpdate()
        {
            for (int i = 0; i < ActiveQuickMsgs.Count; i++)
            {
                (PoolObject<GameObject>, QuickMsg) pair = ActiveQuickMsgs[i];
                var poolObj = pair.Item1;
                var quickMsg = pair.Item2;

                if (quickMsg.EnableTimestamp.TimeSince > quickMsg.Duration)
                {
                    ActiveQuickMsgs.RemoveAt(i);
                    i -= 1;
                    poolObj.Dispose();
                    continue;
                }

                quickMsg.TextMesh.alpha = NyxMath.InverseNormalizeToRange((float)(quickMsg.EnableTimestamp.TimeSince), 0.0f, quickMsg.Duration);
            }
        }

        private static void OnSceneUnloaded(Scene scene, string levelName, string unitySceneName)
        {
            Pool?.Clear();
            ActiveQuickMsgs?.Clear();
        }

        private static void OnSceneLoaded(Scene scene, string levelName, string unitySceneName)
        {
            if (Assets.LabelPrefab != null && CanvasController.Instance.NullInvalid()?.gameObject != null)
            {
                Pool.EnsureSize(32);
            }
        }
        
        public static void DisplayQuickMsg(string text, Color color, float duration, Vector3 velocity, float fontSize, bool flashing = false)
        {
            var poolObj = Pool.Take();
            var go = poolObj.Value;
            go.SetActive(true);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
            var textMesh = go.GetComponent<TextMeshProUGUI>();
            textMesh.text = text;
            textMesh.color = color;
            textMesh.fontSize = fontSize;
            textMesh.outlineWidth = 0.1f;
            textMesh.outlineColor = new Color((1.0f - color.grayscale) * 5.0f, (1.0f - color.grayscale) * 5.0f, (1.0f - color.grayscale) * 5.0f);
            var quickMsg = go.GetComponent<QuickMsg>();
            quickMsg.Duration = duration;
            quickMsg.Velocity = velocity;
            quickMsg.FlashEnabled = flashing;
            quickMsg.FlashDelay = 0.25f;
            quickMsg.FlashFadeTime = 0.25f;
            quickMsg.FlashColor = Color.white;
            ActiveQuickMsgs.Add((poolObj, quickMsg));
        }
    }
}