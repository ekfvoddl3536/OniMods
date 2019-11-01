using Harmony;
using System;
using System.Reflection;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
    using static Constants;
    [HarmonyPatch(typeof(Game), "OnSpawn")]
    public class P2
    {
        public static void Postfix() => HUSimUpdater.Clear();
    }

    [HarmonyPatch(typeof(Game), "OnPrefabInit")]
    public class P3
    {
        public static void Postfix()
        {
            hupipeSystem = new UtilityNetworkManager<HUNetwork, HUPipe>(Grid.WidthInCells, Grid.HeightInCells, layer);
            manager = new HUPipeManager();
        }
    }

    [HarmonyPatch(typeof(BuildingCellVisualizer), nameof(BuildingCellVisualizer.DrawIcons))]
    public class P4
    {
        private static FieldInfo oi, ii, building;
        private static MethodInfo draw;
        private delegate void Handler(int cell, Sprite icon_img, ref GameObject visualizerObj, Color tint);
        public static bool Prepare()
        {
            Type t = typeof(BuildingCellVisualizer);
            BindingFlags f = BindingFlags.NonPublic | BindingFlags.Instance;
            oi = t.GetField("outputVisualizer", f);
            ii = t.GetField("inputVisualizer", f);
            building = t.GetField("building", f);
            draw = t.GetMethod("DrawUtilityIcon", f, null, new[] { typeof(int), typeof(Sprite), typeof(GameObject).MakeByRefType(), typeof(Color) }, null);
            return true;
        }

        public static bool Prefix(HashedString mode, BuildingCellVisualizer __instance)
        {
            if (mode == HUPipeOverlay.ID)
            {
                Building bd = (Building)building.GetValue(__instance);
                if (bd.Def is HUBuildingDef def)
                {
                    Handler func = (Handler)Delegate.CreateDelegate(typeof(Handler), __instance, draw);
                    BuildingCellVisualizerResources resource = BuildingCellVisualizerResources.Instance();
                    if ((def.hutypes & HUPipeTypes.InputOnly) != 0)
                    {
                        GameObject op = null;
                        func.Invoke(bd.GetRotatedOffsetCell(def.huInputOffset), resource.liquidInputIcon, ref op, resource.liquidIOColours.input.connected);
                        ii.SetValue(__instance, op);
                    }
                    if ((def.hutypes & HUPipeTypes.OutputOnly) != 0)
                    {
                        GameObject op = null;
                        func.Invoke(bd.GetRotatedOffsetCell(def.huOutputOffset), resource.liquidOutputIcon, ref op, resource.liquidIOColours.output.connected);
                        oi.SetValue(__instance, op);
                    }
                }
                return false;
            }
            return true;
        }
    }
}