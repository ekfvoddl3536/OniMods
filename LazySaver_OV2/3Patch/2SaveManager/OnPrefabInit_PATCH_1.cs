using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LazySaver
{
    [HarmonyPatch(typeof(SaveManager), "OnPrefabInit")]
    public static class OnPrefabInit_PATCH_1
    {
        public static void Postfix(SaveManager __instance)
        {
            const BindingFlags INST = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var t0 = typeof(SaveManager);

            SaveBw_PATCH_2.m_prefabMap = (Dictionary<Tag, GameObject>)t0.GetField("prefabMap", INST).GetValue(__instance);

            t0.GetField("orderedKeys", INST).SetValue(__instance, null);

            GC.Collect(0, GCCollectionMode.Default, false, false);
        }
    }
}
