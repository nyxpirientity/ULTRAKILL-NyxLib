using HarmonyLib;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    [HarmonyPatch(typeof(ActivateNextWave), "AddDeadEnemy")]
    static class CybergrindStartPatch
    {
        public static void Prefix()
        {
        }

        public static void Postfix()
        {
        }
    }
}