using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
    [HarmonyPatch(typeof(FilteredDragTool), "OnOverlayChanged")]
    public class PATCH_3
    {
        private static FieldInfo current, filter, isact;
        public static bool Prepare()
        {
            Type t = typeof(FilteredDragTool);
            BindingFlags f = BindingFlags.NonPublic | BindingFlags.Instance;
            current = t.GetField("currentFilterTargets", f);
            filter = t.GetField("filterTargets", f);
            isact = t.GetField("active", f);
            return true;
        }
        public static bool Prefix(FilteredDragTool __instance, HashedString overlay)
        {
            if ((bool)isact.GetValue(__instance) == false)
                return false;
            if (overlay == HUPipeOverlay.ID)
            {
                Dictionary<string, ToolParameterMenu.ToggleState> fi = (Dictionary<string, ToolParameterMenu.ToggleState>)filter.GetValue(__instance);

                List<string> enums = new List<string>(fi.Keys);
                foreach (string k in enums)
                {
                    fi[k] = ToolParameterMenu.ToggleState.Disabled;
                    if (k == ToolParameterMenu.FILTERLAYERS.BACKWALL)
                        fi[k] = ToolParameterMenu.ToggleState.On;
                }
                current.SetValue(__instance, fi);
                // ToolMenu.Instance.toolParameterMenu.PopulateMenu((Dictionary<string, ToolParameterMenu.ToggleState>)current.GetValue(__instance));
                ToolMenu.Instance.toolParameterMenu.PopulateMenu(fi);
                return false;
            }
            else
                return true;
        }
    }

    [HarmonyPatch(typeof(PlanScreen), "OnRecipeElementsFullySelected")]
    public class PATCH_PS_0
    {
        private static FieldInfo pic, current;

        public static bool Prepare()
        {
            Type t = typeof(PlanScreen);
            BindingFlags f = BindingFlags.NonPublic | BindingFlags.Instance;
            pic = t.GetField("productInfoScreen", f);
            current = t.GetField("currentlySelectedToggle", f);
            return true;
        }

        public static bool Prefix(PlanScreen __instance)
        {
            BuildingDef def = null;
            KToggle toggle = (KToggle)current.GetValue(__instance);
            foreach (KeyValuePair<BuildingDef, KToggle> at in __instance.ActiveToggles)
                if (at.Value == toggle)
                {
                    def = at.Key;
                    break;
                }
            if (def != null)
            {
                IList<Tag> eles = ((ProductInfoScreen)pic.GetValue(__instance)).materialSelectionPanel.GetSelectedElementAsList;
                if (def.isKAnimTile && def.isUtility)
                    if (def.BuildingComplete.GetComponent<Wire>() != null || def.BuildingComplete.GetComponent<HUPipe>() != null)
                        WireBuildTool.Instance.Activate(def, eles);
                    else
                        UtilityBuildTool.Instance.Activate(def, eles);
                else
                    BuildTool.Instance.Activate(def, eles);
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(BuildMenu), "OnRecipeElementsFullySelected")]
    public class PATCH_BM_0
    {
        private static FieldInfo sb, pic;

        public static bool Prepare()
        {
            Type t = typeof(BuildMenu);
            BindingFlags f = BindingFlags.NonPublic | BindingFlags.Instance;
            sb = t.GetField("selectedBuilding", f);
            pic = t.GetField("productInfoScreen", f);
            return true;
        }

        public static bool Prefix(BuildMenu __instance)
        {
            if (sb.GetValue(__instance) is BuildingDef selDef)
            {
                IList<Tag> eles = ((ProductInfoScreen)pic.GetValue(__instance)).materialSelectionPanel.GetSelectedElementAsList;
                if (selDef.isKAnimTile && selDef.isUtility)
                    if (selDef.BuildingComplete.GetComponent<Wire>() != null || selDef.BuildingComplete.GetComponent<HUPipe>() != null)
                        WireBuildTool.Instance.Activate(selDef, eles);
                    else
                        UtilityBuildTool.Instance.Activate(selDef, eles);
                else
                    BuildTool.Instance.Activate(selDef, eles);
            }
            else
                Debug.Log("No def!");
            return false;
        }
    }

    [HarmonyPatch(typeof(BaseUtilityBuildTool), "BuildPath")]
    public class BUBT_HARD_PATCH
    {
        private static MethodInfo apply;
        private static FieldInfo path, mgr, def, selElem;

        public static bool Prepare()
        {
            Type t = typeof(BaseUtilityBuildTool);
            BindingFlags f = BindingFlags.NonPublic | BindingFlags.Instance;
            apply = t.GetMethod("ApplyPathToConduitSystem", f);
            path = t.GetField("path", f);
            mgr = t.GetField("conduitMgr", f);
            def = t.GetField("def", f);
            selElem = t.GetField("selectedElements", f);
            return true;
        }
        public static bool Prefix(BaseUtilityBuildTool __instance)
        {
            apply.Invoke(__instance, null);
            int cc = 0;
            IList list = (IList)path.GetValue(__instance);
            IUtilityNetworkMgr imgr = (IUtilityNetworkMgr)mgr.GetValue(__instance);
            BuildingDef bdef = (BuildingDef)def.GetValue(__instance);
            IList<Tag> tags = (IList<Tag>)selElem.GetValue(__instance);
            bool sandboxInstant = Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild;
            bool debugInstant = DebugHandler.InstantBuildMode || sandboxInstant;
            foreach (object o in list)
            {
                int pnode = GetCell(o);
                Vector3 poscbc = Grid.CellToPosCBC(pnode, Grid.SceneLayer.Building);
                GameObject go = Grid.Objects[pnode, (int)bdef.TileLayer];
                UtilityConnections uc2;
                if (go == null)
                {
                    uc2 = imgr.GetConnections(pnode, false);
                    if (debugInstant && bdef.IsValidBuildLocation(__instance.visualizer, poscbc, 0) && bdef.IsValidPlaceLocation(__instance.visualizer, poscbc, 0, out string _))
                        go = bdef.Build(pnode, 0, null, tags, 293.15f, true, GameClock.Instance.GetTime());
                    else
                    {
                        go = bdef.TryPlace(null, poscbc, 0, tags);
                        if (go != null)
                        {
                            if (!bdef.MaterialsAvailable(tags) && !DebugHandler.InstantBuildMode)
                                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, STRINGS.UI.TOOLTIPS.NOMATERIAL, null, poscbc);
                            float delay = 0.1f * cc;
                            Constructable com1 = go.GetComponent<Constructable>();
                            if (com1.IconConnectionAnimation(delay, cc, "Wire", "OutletConnected_release") || com1.IconConnectionAnimation(delay, cc, "Pipe", "OutletConnected_release"))
                                cc++;
                            if (go.GetComponent<Prioritizable>() is Prioritizable com2)
                            {
                                if (BuildMenu.Instance != null)
                                    com2.SetMasterPriority(BuildMenu.Instance.GetBuildingPriority());
                                if (PlanScreen.Instance != null)
                                    com2.SetMasterPriority(PlanScreen.Instance.GetBuildingPriority());
                            }
                        }
                    }
                }
                else
                {
                    uc2 = imgr.GetConnections(pnode, false);
                    if (go.GetComponent<KAnimGraphTileVisualizer>() is KAnimGraphTileVisualizer com)
                    {
                        uc2 |= com.Connections;
                        if (go.GetComponent<BuildingComplete>() != null)
                            com.UpdateConnections(uc2);
                    }
                }
                if (bdef.ReplacementLayer != ObjectLayer.NumLayers && !debugInstant && bdef.IsValidBuildLocation(null, poscbc, 0) &&
                    Grid.Objects[pnode, (int)bdef.ReplacementLayer] == null &&
                    Grid.Objects[pnode, (int)bdef.TileLayer]?.GetComponent<BuildingComplete>() is BuildingComplete complete &&
                    complete.Def != bdef)
                {
                    Constructable com2 = bdef.BuildingUnderConstruction.GetComponent<Constructable>();
                    com2.IsReplacementTile = true;
                    go = bdef.Instantiate(poscbc, 0, tags);
                    com2.IsReplacementTile = false;
                    if (!bdef.MaterialsAvailable(tags) && !DebugHandler.InstantBuildMode)
                        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, STRINGS.UI.TOOLTIPS.NOMATERIAL, null, poscbc);
                    Grid.Objects[pnode, (int)bdef.ReplacementLayer] = go;
                    if (go.GetComponent<KAnimGraphTileVisualizer>() is KAnimGraphTileVisualizer com)
                    {
                        uc2 = com.Connections;
                        if (go.GetComponent<BuildingComplete>() != null)
                            com.UpdateConnections(uc2);
                    }
                    uc2 |= imgr.GetConnections(pnode, false);
                    string vs = imgr.GetVisualizerString(uc2), anim = vs + "_place";
                    KBatchedAnimController controller = go.GetComponent<KBatchedAnimController>();
                    controller.Play(controller.HasAnimation(anim) ? anim : vs);
                }
                if (go != null && go.GetComponent<KAnimGraphTileVisualizer>() is KAnimGraphTileVisualizer last)
                    last.Connections = uc2;
                TileVisualizer.RefreshCell(pnode, bdef.TileLayer, bdef.ReplacementLayer);
            }
            ResourceRemainingDisplayScreen.instance.SetNumberOfPendingConstructions(0);
            return false;
        }

        public static int GetCell(object o) => (int)o.GetType().GetField("cell").GetValue(o);
    }
}
