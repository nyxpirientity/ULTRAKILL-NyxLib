using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace UKAIW
{
    
    [HarmonyPatch(typeof(StainVoxelManager), "TryIgniteAt", new Type[] { typeof(Vector3Int), typeof(int) })]
    static class TryIgniteAtPatch
    {
        // exists just to get rid of the seemingly useless tryigniteat log
        public static bool Prefix(ref bool __result, StainVoxelManager __instance, Vector3Int voxelPosition, int checkSize = 3)
        {
            FieldPublisher<StainVoxelManager, Dictionary<Vector3Int, StainVoxel>> stainVoxels = new FieldPublisher<StainVoxelManager, Dictionary<Vector3Int, StainVoxel>>(__instance, "stainVoxels");
            if (stainVoxels.Value.Count == 0)
            {
                __result = false;
                return false;
            }

            if (!__instance.TryGetVoxels(voxelPosition, out var voxels, checkSize))
            {
                __result = false;
                return false;
            }

            bool result = false;
            foreach (StainVoxel item in voxels)
            {
                if (item.TryIgnite())
                {
                    result = true;
                }
            }
            
            __result = result;
            return false;
        }
    }

}