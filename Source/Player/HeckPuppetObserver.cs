using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    public class HeckPuppetObserver : MonoBehaviour
    {
        public static HeckPuppetObserver Instance { get; private set; } = null;

        SceneTimeStamp HeckPuppetComboTimeStamp;
        StyleHUD Shud = null;

        public int NumNormalHeckPuppetsCombo { get; private set; } = 0;
        public int NumMiniBossHeckPuppetsCombo { get; private set; } = 0;
        public int NumBossHeckPuppetsCombo { get; private set; } = 0;
        public int NumUltraBossHeckPuppetsCombo { get; private set; } = 0;

        public void IncrementNormalPuppetsCombo()
        {
            NumNormalHeckPuppetsCombo += 1;
        }

        public void IncrementMiniBossPuppetsCombo()
        {
            NumMiniBossHeckPuppetsCombo += 1;
        }

        public void IncrementBossPuppetsCombo()
        {
            NumBossHeckPuppetsCombo += 1;
        }

        public void IncrementUltraBossPuppetsCombo()
        {
            NumUltraBossHeckPuppetsCombo += 1;
        }

        protected void Start()
        {
            HeckPuppetComboTimeStamp.UpdateToNow();
            Shud = StyleHUD.Instance;
            Instance = this;
        }

        protected void OnDestroy()
        {
        }

        protected void FixedUpdate()
        {
            if (HeckPuppetComboTimeStamp.TimeSince >= 2.0)
            {
                ApplyHeckPuppetCombo();
                HeckPuppetComboTimeStamp.UpdateToNow();
            }
        }

        private void ApplyHeckPuppetCombo()
        {
            if (NumNormalHeckPuppetsCombo > 0)
            {
                Shud.AddPoints(NumNormalHeckPuppetsCombo, "<color=#37ff00>MORE JUICE</color>", null, null, NumNormalHeckPuppetsCombo);
            }

            if (NumMiniBossHeckPuppetsCombo > 0)
            {
                Shud.AddPoints(NumMiniBossHeckPuppetsCombo * 5, "<color=#00fff2>MINI MITOSIS</color>", null, null, NumMiniBossHeckPuppetsCombo);
            }

            if (NumBossHeckPuppetsCombo > 0)
            {
                Shud.AddPoints(NumBossHeckPuppetsCombo * 25, "<color=#ffff00>INANIMATED</color>", null, null, NumBossHeckPuppetsCombo);
            }

            if (NumUltraBossHeckPuppetsCombo > 0)
            {
                Shud.AddPoints(NumUltraBossHeckPuppetsCombo * 150, "<color=#ff0000>CATACLYSMIC PUPPETRY</color>", null, null, NumUltraBossHeckPuppetsCombo);
            }

            NumNormalHeckPuppetsCombo = 0;
            NumMiniBossHeckPuppetsCombo = 0;
            NumBossHeckPuppetsCombo = 0;
            NumUltraBossHeckPuppetsCombo = 0;
        }

        protected void LateUpdate()
        {

        }
    }
}