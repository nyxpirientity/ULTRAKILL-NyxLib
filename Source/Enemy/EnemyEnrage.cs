using System;
using HarmonyLib;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    [HarmonyPatch(typeof(StatueBoss), "Enrage")]
    static class StatueEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(StatueBoss __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreEnrage(_cancellationTracker);
        }

        public static void Postfix(StatueBoss __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostEnrage(_cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(StatueBoss), "UnEnrage")]
    static class StatueBossUnEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(StatueBoss __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreUnEnrage(_cancellationTracker);
        }

        public static void Postfix(StatueBoss __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostUnEnrage(_cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(SwordsMachine), "Enrage")]
    static class SwordsMachineEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(SwordsMachine __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreEnrage(_cancellationTracker);
        }

        public static void Postfix(SwordsMachine __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostEnrage(_cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(SwordsMachine), "UnEnrage")]
    static class SwordsMachineUnEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(SwordsMachine __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreUnEnrage(_cancellationTracker);
        }

        public static void Postfix(SwordsMachine __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostUnEnrage(_cancellationTracker);
        }
    }
    
    [HarmonyPatch(typeof(Drone), "Enrage")]
    static class DroneEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(Drone __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreEnrage(_cancellationTracker);
        }

        public static void Postfix(Drone __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostEnrage(_cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(Drone), "UnEnrage")]
    static class DroneUnEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(Drone __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreUnEnrage(_cancellationTracker);
        }

        public static void Postfix(Drone __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostUnEnrage(_cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(V2), "Enrage", new Type[] {typeof(string)})]
    static class V2EnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(V2 __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreEnrage(_cancellationTracker);
        }

        public static void Postfix(V2 __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostEnrage(_cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(V2), "UnEnrage")]
    static class V2UnEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(V2 __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreUnEnrage(_cancellationTracker);
        }

        public static void Postfix(V2 __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostUnEnrage(_cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(Mindflayer), "Enrage")]
    static class MindflayerEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(Mindflayer __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreEnrage(_cancellationTracker);
        }

        public static void Postfix(Mindflayer __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostEnrage(_cancellationTracker);
        }
    }
    [HarmonyPatch(typeof(Mindflayer), "UnEnrage")]
    static class MindflayerUnEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(Mindflayer __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreUnEnrage(_cancellationTracker);
        }

        public static void Postfix(Mindflayer __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostUnEnrage(_cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(SpiderBody), "Enrage")]
    static class SpiderBodyEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(SpiderBody __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreEnrage(_cancellationTracker);
        }

        public static void Postfix(SpiderBody __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostEnrage(_cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(SpiderBody), "UnEnrage")]
    static class SpiderBodyUnEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(SpiderBody __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreUnEnrage(_cancellationTracker);
        }

        public static void Postfix(SpiderBody __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostUnEnrage(_cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(Gutterman), "Enrage")]
    static class GuttermanEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(Gutterman __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreEnrage(_cancellationTracker);
        }

        public static void Postfix(Gutterman __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostEnrage(_cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(Gutterman), "UnEnrage")]
    static class GuttermanUnEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(Gutterman __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreUnEnrage(_cancellationTracker);
        }

        public static void Postfix(Gutterman __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostUnEnrage(_cancellationTracker);
        }
    }

    [HarmonyPatch(typeof(Mass), "Enrage")]
    static class MassEnragePatch
    {
        private static EventMethodCancellationTracker _cancellationTracker = new EventMethodCancellationTracker();
        public static void Prefix(Mass __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPreEnrage(_cancellationTracker);
        }

        public static void Postfix(Mass __instance)
        {
            __instance.GetComponent<EnemyComponents>().CallPostEnrage(_cancellationTracker);
        }
    }
}