using System;
using HarmonyLib;
using UnityEngine;

namespace UKAIW
{
    /* Steet cleaner FixedUpdate has a bad implementation for dodging which can break the game (even in vanilla!) if their radiance speed is less than 1.0, this is meant to fix it (if cheats are on) */
    [HarmonyPatch(typeof(Streetcleaner), "FixedUpdate")]
    static class StreetcleanerFixedUpdatePatch
    {
        public static bool Prefix(Streetcleaner __instance)
        {
            if (!Cheats.Enabled && Options.EnableStreetCleanerDodgeFix.Value)
            {
                return true;
            }
            
            FieldPublisher<Streetcleaner, EnemyTarget> target = new FieldPublisher<Streetcleaner, EnemyTarget>(__instance, "target");
            FieldPublisher<Streetcleaner, Rigidbody> rb = new FieldPublisher<Streetcleaner, Rigidbody>(__instance, "rb");
            FieldPublisher<Streetcleaner, float> dodgeSpeed = new FieldPublisher<Streetcleaner, float>(__instance, "dodgeSpeed");

            if (target != null && !__instance.dead && __instance.dodging)
            {
                if (__instance.eid.totalSpeedModifier >= 1.0f && Options.EnableStreetCleanerDodgeFixOnlyWhenNeeded.Value)
                {
                    return true;
                }

                rb.Value.velocity = __instance.transform.forward * -1f * dodgeSpeed.Value * __instance.eid.totalSpeedModifier;
                //dodgeSpeed.Value = dodgeSpeed.Value * 0.95f / __instance.eid.totalSpeedModifier; <--- original, division means <1f totalSpeedModifier causes infinite gaining of speed
                dodgeSpeed.Value = NyxMath.EaseInterpTo(dodgeSpeed.Value, 0.0f, __instance.eid.totalSpeedModifier * Options.StreetCleanerDodgeFixInterpRate.Value, Time.fixedDeltaTime);
            }

            return false;
        }
        
        public static void Postfix(Streetcleaner __instance)
        {
        }
    }
}