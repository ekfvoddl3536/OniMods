using Harmony;
using System;
using System.Reflection;
using TemplateClasses;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
    using static Constants;
    [HarmonyPatch(typeof(KAnimGraphTileVisualizer), nameof(KAnimGraphTileVisualizer.GetNeighbour))]
    public static class PATCH_003
    {
        public static bool Prefix(ref KAnimGraphTileVisualizer __result, Direction d, KAnimGraphTileVisualizer __instance)
        {
            __result = null;
            Grid.PosToXY(__instance.transform.GetPosition(), out Vector2I v2);
            int idx = -1;
            switch (d)
            {
                case Direction.Up:
                    if (v2.y < Grid.HeightInCells - 1)
                        idx = Grid.XYToCell(v2.x, v2.y + 1);
                    break;
                case Direction.Right:
                    if (v2.x < Grid.WidthInCells - 1)
                        idx = Grid.XYToCell(v2.x + 1, v2.y);
                    break;
                case Direction.Down:
                    if (v2.y > 0)
                        idx = Grid.XYToCell(v2.x, v2.y - 1);
                    break;
                case Direction.Left:
                    if (v2.x > 0)
                        idx = Grid.XYToCell(v2.x - 1, v2.y);
                    break;
                default:
                    break;
            }
            if (idx != -1)
            {
                ObjectLayer ol = ObjectLayer.Backwall;
                switch (__instance.connectionSource)
                {
                    case KAnimGraphTileVisualizer.ConnectionSource.Gas:
                        ol = ObjectLayer.GasConduitTile;
                        break;
                    case KAnimGraphTileVisualizer.ConnectionSource.Liquid:
                        ol = ObjectLayer.LiquidConduitTile;
                        break;
                    case KAnimGraphTileVisualizer.ConnectionSource.Electrical:
                        ol = ObjectLayer.WireTile;
                        break;
                    case KAnimGraphTileVisualizer.ConnectionSource.Logic:
                        ol = ObjectLayer.LogicWiresTiling;
                        break;
                    case KAnimGraphTileVisualizer.ConnectionSource.Tube:
                        ol = ObjectLayer.TravelTubeTile;
                        break;
                    case KAnimGraphTileVisualizer.ConnectionSource.Solid:
                        ol = ObjectLayer.SolidConduitTile;
                        break;
                    default:
                        break;
                }
                if (Grid.Objects[idx, (int)ol] is GameObject go)
                    __result = go.GetComponent<KAnimGraphTileVisualizer>();
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(TemplateLoader), nameof(TemplateLoader.PlaceUtilityConnection))]
    public class PATCH4
    {
        public static bool Prefix(GameObject spawned, Prefab bc, int root_cell)
        {
            string id = bc.id;
            if (id == null)
                return false;
            int cell = Grid.OffsetCell(root_cell, bc.location_x, bc.location_y);
            UtilityConnections cons = (UtilityConnections)bc.connections;
            switch (id)
            {
                case "Wire":
                case "InsulatedWire":
                case "HighWattageWire":
                    spawned.GetComponent<Wire>().SetFirstFrameCallback(() =>
                    {
                        Game.Instance.electricalConduitSystem.SetConnections(cons, cell, true);
                        if (spawned.GetComponent<KAnimGraphTileVisualizer>() is KAnimGraphTileVisualizer compo)
                            compo.Refresh();
                    });
                    break;
                case "GasConduit":
                case "InsulatedGasConduit":
                    spawned.GetComponent<Conduit>().SetFirstFrameCallback(() =>
                    {
                        Game.Instance.gasConduitSystem.SetConnections(cons, cell, true);
                        if (spawned.GetComponent<KAnimGraphTileVisualizer>() is KAnimGraphTileVisualizer compo)
                            compo.Refresh();
                    });
                    break;
                case "LiquidConduit":
                case "InsulatedLiquidConduit":
                    spawned.GetComponent<Conduit>().SetFirstFrameCallback(() =>
                    {
                        Game.Instance.liquidConduitSystem.SetConnections(cons, cell, true);
                        if (spawned.GetComponent<KAnimGraphTileVisualizer>() is KAnimGraphTileVisualizer compo)
                            compo.Refresh();
                    });
                    break;
                case "SolidConduit":
                    spawned.GetComponent<SolidConduit>().SetFirstFrameCallback(() =>
                    {
                        Game.Instance.solidConduitSystem.SetConnections(cons, cell, true);
                        if (spawned.GetComponent<KAnimGraphTileVisualizer>() is KAnimGraphTileVisualizer compo)
                            compo.Refresh();
                    });
                    break;
                case "LogicWire":
                    spawned.GetComponent<LogicWire>().SetFirstFrameCallback(() =>
                    {
                        Game.Instance.logicCircuitSystem.SetConnections(cons, cell, true);
                        if (spawned.GetComponent<KAnimGraphTileVisualizer>() is KAnimGraphTileVisualizer compo)
                            compo.Refresh();
                    });
                    break;
                case "TravelTube":
                    spawned.GetComponent<TravelTube>().SetFirstFrameCallback(() =>
                    {
                        Game.Instance.travelTubeSystem.SetConnections(cons, cell, true);
                        if (spawned.GetComponent<KAnimGraphTileVisualizer>() is KAnimGraphTileVisualizer compo)
                            compo.Refresh();
                    });
                    break;
                default:
                    spawned.GetComponent<HUPipe>().SetFirstFrameCallback(() =>
                    {
                        hupipeSystem.SetConnections(cons, cell, true);
                        if (spawned.GetComponent<KAnimGraphTileVisualizer>() is KAnimGraphTileVisualizer compo)
                            compo.Refresh();
                    });
                    break;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(OverlayScreen), "OnSpawn")]
    public class OVERLAYSCREEN_PATCH_HSG
    {
        private static MethodInfo func;
        private static FieldInfo parent, prefab, conColour, genColour;

        public static bool Prepare()
        {
            Type t = typeof(OverlayScreen);
            BindingFlags x = BindingFlags.NonPublic | BindingFlags.Instance;
            func = t.GetMethod("RegisterMode", x, null, new Type[1] { typeof(OverlayModes.Mode) }, null);
            parent = t.GetField("powerLabelParent", x);
            prefab = t.GetField("powerLabelPrefab", x);
            conColour = t.GetField("consumerColour", x);
            genColour = t.GetField("generatorColour", x);
            return true;
        }

        public static void Postfix(OverlayScreen __instance) =>
            func.Invoke(__instance, new object[]
            {
                new HUPipeOverlay(
                    (Canvas)parent.GetValue(__instance),
                    (LocText)prefab.GetValue(__instance),
                    (Color)conColour.GetValue(__instance),
                    (Color)genColour.GetValue(__instance))
            });
    }

    [HarmonyPatch(typeof(KAnimGraphTileVisualizer), "get_ConnectionManager")]
    public class PATCH5
    {
        public static void Postfix(ref IUtilityNetworkMgr __result)
        {
            if (__result == null)
                __result = hupipeSystem;
        }
    }
}
