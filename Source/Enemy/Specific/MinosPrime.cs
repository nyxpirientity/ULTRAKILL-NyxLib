using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    [HarmonyPatch(typeof(MinosPrime), nameof(MinosPrime.Death))]
    static class MinosPrimeDeathPatch
    {
        private static MinosPrime _minosPrime = null;
        
        static void SlowDownReplacement(TimeController tc, float amount)
        {
            var enemy = _minosPrime.GetComponent<EnemyComponents>();

            Action originalMethod = () => tc.SlowDown(amount);

            if (enemy == null)
            {
                originalMethod.Invoke();
                return;
            }

            if (enemy.AvoidHealthBasedSlowDown)
            {
                return;
            }

            originalMethod.Invoke();
        }

        static void Prefix(MinosPrime __instance)
        {
            _minosPrime = __instance;
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instr in instructions)
            {
                if (instr.Calls(typeof(TimeController).GetMethod(nameof(TimeController.SlowDown))))
                {
                    instr.operand = typeof(MinosPrime).GetMethod(nameof(SlowDownReplacement), BindingFlags.Static | BindingFlags.NonPublic);
                }

                yield return instr;
            }
        }
    }
}