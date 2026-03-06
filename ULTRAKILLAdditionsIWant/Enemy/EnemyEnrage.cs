using System;
using HarmonyLib;

namespace UKAIW
{
    [HarmonyPatch(typeof(StatueBoss), "Enrage")]
    static class StatueEnragePatch
    {
        public static void Prefix(StatueBoss __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreEnrage?.Invoke();
        }

        public static void Postfix(StatueBoss __instance)
        {
        }
    }

    [HarmonyPatch(typeof(StatueBoss), "UnEnrage")]
    static class StatueBossUnEnragePatch
    {
        public static void Prefix(StatueBoss __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreUnEnrage?.Invoke();
        }

        public static void Postfix(StatueBoss __instance)
        {
        }
    }

    [HarmonyPatch(typeof(SwordsMachine), "Enrage")]
    static class SwordsMachineEnragePatch
    {
        public static void Prefix(SwordsMachine __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreEnrage?.Invoke();
        }

        public static void Postfix(SwordsMachine __instance)
        {
        }
    }

    [HarmonyPatch(typeof(SwordsMachine), "UnEnrage")]
    static class SwordsMachineUnEnragePatch
    {
        public static void Prefix(SwordsMachine __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreUnEnrage?.Invoke();
        }

        public static void Postfix(SwordsMachine __instance)
        {
        }
    }
    
    [HarmonyPatch(typeof(Drone), "Enrage")]
    static class DroneEnragePatch
    {
        public static void Prefix(Drone __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreEnrage?.Invoke();
        }

        public static void Postfix(Drone __instance)
        {
        }
    }

    [HarmonyPatch(typeof(Drone), "UnEnrage")]
    static class DroneUnEnragePatch
    {
        public static void Prefix(Drone __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreUnEnrage?.Invoke();
        }

        public static void Postfix(Drone __instance)
        {
        }
    }

    [HarmonyPatch(typeof(V2), "Enrage", new Type[] {typeof(string)})]
    static class V2EnragePatch
    {
        public static void Prefix(V2 __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreEnrage?.Invoke();
        }

        public static void Postfix(V2 __instance)
        {
        }
    }

    [HarmonyPatch(typeof(V2), "UnEnrage")]
    static class V2UnEnragePatch
    {
        public static void Prefix(V2 __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreUnEnrage?.Invoke();
        }

        public static void Postfix(V2 __instance)
        {
        }
    }

    [HarmonyPatch(typeof(Mindflayer), "Enrage")]
    static class MindflayerEnragePatch
    {
        public static void Prefix(Mindflayer __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreEnrage?.Invoke();
        }

        public static void Postfix(Mindflayer __instance)
        {
        }
    }
    [HarmonyPatch(typeof(Mindflayer), "UnEnrage")]
    static class MindflayerUnEnragePatch
    {
        public static void Prefix(Mindflayer __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreUnEnrage?.Invoke();
        }

        public static void Postfix(Mindflayer __instance)
        {
        }
    }

    [HarmonyPatch(typeof(SpiderBody), "Enrage")]
    static class SpiderBodyEnragePatch
    {
        public static void Prefix(SpiderBody __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreEnrage?.Invoke();
        }

        public static void Postfix(SpiderBody __instance)
        {
        }
    }

    [HarmonyPatch(typeof(SpiderBody), "UnEnrage")]
    static class SpiderBodyUnEnragePatch
    {
        public static void Prefix(SpiderBody __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreUnEnrage?.Invoke();
        }

        public static void Postfix(SpiderBody __instance)
        {
        }
    }

    [HarmonyPatch(typeof(Gutterman), "Enrage")]
    static class GuttermanEnragePatch
    {
        public static void Prefix(Gutterman __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreEnrage?.Invoke();
        }

        public static void Postfix(Gutterman __instance)
        {
        }
    }

    [HarmonyPatch(typeof(Gutterman), "UnEnrage")]
    static class GuttermanUnEnragePatch
    {
        public static void Prefix(Gutterman __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreUnEnrage?.Invoke();
        }

        public static void Postfix(Gutterman __instance)
        {
        }
    }

    [HarmonyPatch(typeof(Mass), "Enrage")]
    static class MassEnragePatch
    {
        public static void Prefix(Mass __instance)
        {
            __instance.GetComponent<EnemyAdditions>().PreEnrage?.Invoke();
        }

        public static void Postfix(Mass __instance)
        {
        }
    }
}