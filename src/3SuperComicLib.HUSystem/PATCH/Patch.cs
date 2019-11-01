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
        private static FieldInfo oi, ii;
        private static MethodInfo draw;
        private delegate void Handler(int cell, Sprite icon_img, ref GameObject visualizerObj, Color tint);
        public static bool Prepare()
        {
            Type t = typeof(BuildingCellVisualizer);
            BindingFlags f = BindingFlags.NonPublic | BindingFlags.Instance;
            oi = t.GetField("outputVisualizer", f);
            ii = t.GetField("inputVisualizer", f);
            draw = t.GetMethod("DrawUtilityIcon", f, null, new[] { typeof(int), typeof(Sprite), typeof(GameObject).MakeByRefType(), typeof(Color) }, null);
            return true;
        }

        public static bool Prefix(HashedString mode, BuildingCellVisualizer __instance)
        {
            if (mode == HUPipeOverlay.ID)
            {
                Handler func = (Handler)Delegate.CreateDelegate(typeof(Handler), __instance, draw);
                BuildingCellVisualizerResources resource = BuildingCellVisualizerResources.Instance();
                if (__instance.GetComponent<IHUConsumer>() is IHUConsumer consumer)
                {
                    GameObject op = null;
                    func.Invoke(consumer.HUCell, resource.liquidInputIcon, ref op, resource.liquidIOColours.input.connected);
                    ii.SetValue(__instance, op);
                }
                if (__instance.GetComponent<IHUGenerator>() is IHUGenerator generator)
                {
                    GameObject op = null;
                    func.Invoke(generator.HUCell, resource.liquidOutputIcon, ref op, resource.liquidIOColours.output.connected);
                    oi.SetValue(__instance, op);
                }
                return false;
            }
            return true;
        }
    }
}