using HarmonyLib;

namespace UKAIW
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