using System;
using System.Collections.Generic;
using System.Reflection;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

namespace UKAIW
{
    public static class MundaneMurder
    {
        public static void Initialize()
        {
            
        }

        public static void EnableMundaneMurderIcon(StyleHUD shud)
        {
            shud.rankImage.useSpriteMesh = false;
            shud.rankImage.canvasRenderer.SetTexture(Assets.MundaneMurderIcon);
            shud.rankImage.canvasRenderer.DisableRectClipping();
        }

        public static void DisableMundaneMurderIcon(StyleHUD shud)
        {
            shud.rankImage.useSpriteMesh = true;
        }

        public static bool IsMundaneMurderIconEnabled(StyleHUD shud)
        {
            return shud.rankImage.mainTexture == Assets.MundaneMurderIcon;
        }

        public static void OnStyleUpdate(StyleHUD shud)
        {
            MethodInfo updateMeterMethodInfo = typeof(StyleHUD).GetMethod("UpdateMeter", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo updateFreshnessMethodInfo = typeof(StyleHUD).GetMethod("UpdateFreshness", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo updateHudMethodInfo = typeof(StyleHUD).GetMethod("UpdateHUD", BindingFlags.NonPublic | BindingFlags.Instance);

            //updateMeterMethodInfo.Invoke(shud, null);
            EvilUpdateMeter(shud);
            updateFreshnessMethodInfo.Invoke(shud, null);
            updateHudMethodInfo.Invoke(shud, null);

            if (shud.rankIndex == 7)
            {
                EnableMundaneMurderIcon(shud);
            }
            else
            {
                DisableMundaneMurderIcon(shud);
            }
        }

        private static void EvilUpdateMeter(StyleHUD shud)
        {
            FieldPublisher<StyleHUD, float> currentMeter = new FieldPublisher<StyleHUD, float>(shud, "currentMeter");
            FieldPublisher<StyleHUD, bool> comboActive = new FieldPublisher<StyleHUD, bool>(shud, "comboActive");
            FieldPublisher<StyleHUD, GameObject> styleHud = new FieldPublisher<StyleHUD, GameObject>(shud, "styleHud");

            if (currentMeter.Value > 0f && !comboActive.Value)
            {
                shud.ComboStart();
            }

            float drainSpeedMin = 1.0f;
            float drainSpeedMax = 8.0f;
            float normalizedDrainSpeed = NyxMath.InverseNormalizeToRange(shud.currentRank.drainSpeed, drainSpeedMin, drainSpeedMax);

            if (currentMeter.Value >= (float)shud.currentRank.maxMeter && shud.rankIndex < 7)
            {
                AscendRank(shud);
            }
            else
            {
                currentMeter.Value += Time.deltaTime * ((Mathf.Lerp(drainSpeedMin, drainSpeedMax, normalizedDrainSpeed)) * 15f);
            }
            
            bool flag = comboActive.Value || shud.forceMeterOn;
            if (styleHud.Value.activeSelf != flag)
            {
                styleHud.Value.SetActive(flag);
            }
        }

        private static void AscendRank(StyleHUD shud)
        {
            MethodInfo ascendRank = typeof(StyleHUD).GetMethod("AscendRank", BindingFlags.Instance | BindingFlags.NonPublic);
            ascendRank.Invoke(shud, null);
        }

        internal static void AddPointsPrefix(StyleHUD shud, int points, string pointID, GameObject sourceWeapon = null, EnemyIdentifier eid = null, int count = -1, string prefix = "", string postfix = "")
        {
            PropertyPublisher<StyleHUD, bool> freshnessEnabled = new PropertyPublisher<StyleHUD, bool>(shud, "freshnessEnabled");
            FieldPublisher<StyleHUD, Dictionary<StyleFreshnessState, StyleFreshnessData>> freshnessStateDict = new FieldPublisher<StyleHUD, Dictionary<StyleFreshnessState, StyleFreshnessData>>(shud, "freshnessStateDict");
            FieldPublisher<StyleHUD, StatsManager> sman = new FieldPublisher<StyleHUD, StatsManager>(shud, "sman");
            FieldPublisher<StyleHUD, float> currentMeter = new FieldPublisher<StyleHUD, float>(shud, "currentMeter");
            FieldPublisher<StyleHUD, float> rankScale = new FieldPublisher<StyleHUD, float>(shud, "rankScale");
            FieldPublisher<StyleHUD, Queue<string>> hudItemsQueue = new FieldPublisher<StyleHUD, Queue<string>>(shud, "hudItemsQueue");

            GameObject gameObject = ((pointID == "ultrakill.arsenal") ? GunControl.Instance.currentWeapon : sourceWeapon);
            if ((bool)eid && eid.puppet)
            {
                return;
            }

            bool flag = false;
            if ((bool)eid)
            {
                flag = eid.isBoss;
            }

            if (points > 0)
            {
                float num = points;
                if (freshnessEnabled.Value && gameObject != null)
                {
                    StyleFreshnessState freshnessState = shud.GetFreshnessState(gameObject);
                    num *= freshnessStateDict.Value[freshnessState].scoreMultiplier;
                    shud.DecayFreshness(gameObject, pointID, flag);
                }

                if (flag)
                {
                    num *= shud.bossStyleGainMultiplier;
                }

                sman.Value.stylePoints += Mathf.RoundToInt(num);
                currentMeter.Value = Mathf.Max(currentMeter.Value - num, -0.01f);
                rankScale.Value = 0.2f;
            }

            string localizedName = shud.GetLocalizedName(pointID);
            if (localizedName != "")
            {
                if (count >= 0)
                {
                    hudItemsQueue.Value.Enqueue("+ " + prefix + localizedName + postfix + " x" + count);
                }
                else
                {
                    hudItemsQueue.Value.Enqueue("+ " + prefix + localizedName + postfix);
                }
            }

            //if (currentMeter.Value >= (float)shud.currentRank.maxMeter && shud.rankIndex < 7)
            //{
                //shud.AscendRank();
            //}
            if (currentMeter.Value < 0f)
            {
                shud.DescendRank();
            }
            else if (currentMeter.Value > (float)shud.currentRank.maxMeter)
            {
                currentMeter.Value = shud.currentRank.maxMeter;
            }
        }
    }
}