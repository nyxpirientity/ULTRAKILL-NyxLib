using HarmonyLib;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    [HarmonyPatch(typeof(ShotgunHammer), "HitNade")]
    static class PistonDeviceHitNadePatch
    {
        public static void Prefix(ShotgunHammer __instance)
        {
        }

        public static void Postfix(ShotgunHammer __instance)
        {
        }
    }
}