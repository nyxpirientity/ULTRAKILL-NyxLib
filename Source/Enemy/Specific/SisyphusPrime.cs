using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib
{
    [HarmonyPatch(typeof(SisyphusPrime), nameof(SisyphusPrime.Death))]
    static class SisyphusPrimeDeathPatch
    {
        private static SisyphusPrime _sisyphusPrime = null;
        
        static void SlowDownReplacement(TimeController tc, float amount)
        {
            var enemy = _sisyphusPrime.GetComponent<EnemyComponents>();

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

        static void Prefix(SisyphusPrime __instance)
        {
            _sisyphusPrime = __instance;
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instr in instructions)
            {
                if (instr.Calls(typeof(TimeController).GetMethod(nameof(TimeController.SlowDown))))
                {
                    instr.operand = typeof(SisyphusPrimeDeathPatch).GetMethod(nameof(SlowDownReplacement), BindingFlags.Static | BindingFlags.NonPublic);
                }

                yield return instr;
            }
        }
    }
}