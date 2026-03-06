using System;
using UKAIW.Diagnostics.Debug;
using UnityEngine;

namespace UKAIW
{
    public static class GameObjectUtils
    {
        public static void DebugPrintChildren(this GameObject go, bool forceLog = true, bool includeComponents = true)
        {
            Action<string> logFunc = forceLog ? new Action<string>((string str) => { Log.Message(str); }) : (string str) => { Log.TraceExpectedInfo(str); };

            logFunc($"----- Debug Print for {go.name} start! -----");
            DebugPrintChildren(go, logFunc, includeComponents, 0);
            logFunc($"----- Debug Print for {go.name} end! -----");
        }

        private static void DebugPrintChildren(GameObject go, Action<string> logFunc, bool includeComponents, ulong depth)
        {
            string indent = "";
            
            for (ulong i = 0; i < depth; i++)
            {
                indent += "  ";
            }

            logFunc($"{indent}GO::{go.name}:");
            if (includeComponents)
            {
                var comps = go.GetComponents<Component>();
                logFunc($"{indent}COMPONENTS:");
                foreach (var comp in comps)
                {
                    logFunc($"{indent}COMP::{comp.GetType()}");
                }
            }

            logFunc($"{indent}CHILDREN:");
            for (int i = 0; i < go.transform.childCount; i++)
            {
                DebugPrintChildren(go.transform.GetChild(i).gameObject, logFunc, includeComponents, depth + 1);
            }
            if (go.transform.childCount == 0)
            {
                logFunc($"{indent}  ![NO CHILDREN]!");
            }
        }
    }
}