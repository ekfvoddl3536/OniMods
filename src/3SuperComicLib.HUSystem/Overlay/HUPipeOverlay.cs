using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
    public class HUPipeOverlay : OverlayModes.Mode
    {
        public static HashSet<Tag> PipeIDs = new HashSet<Tag>();
        public const string ID = "HUPipe";

        private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>(),
            ptargets = new HashSet<SaveLoadRoot>();
        private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();
        private List<LocText> labels = new List<LocText>();
        private UniformGrid<SaveLoadRoot> partition;
        private List<UpdateHUInfo> infos = new List<UpdateHUInfo>();
        private int targetLayer, cameraLayerMask;
        private LocText labelPrefab;
        private Vector3 default_labelOffset, minHU_labelOffset;
        private Canvas canvas;
        private int freeLabels;
        private Color32 consumerColour, generatorColour;

        public HUPipeOverlay(Canvas parent, LocText prefab, Color con, Color gen)
        {
            targetLayer = LayerMask.NameToLayer("MaskedOverlay");
            cameraLayerMask = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");

            canvas = parent;
            default_labelOffset = new Vector3(0, 1.2f, 0);
            minHU_labelOffset = new Vector3(0, 1.48f, 0);
            consumerColour = con;
            generatorColour = gen;

            labelPrefab = Util.KInstantiateUI<LocText>(prefab.gameObject, parent.transform.gameObject);

            LocText unit = labelPrefab.transform.GetChild(0).GetComponent<LocText>();
            unit.rectTransform.sizeDelta = new Vector2(15, unit.rectTransform.rect.height);
            unit.text = "HU/s";
        }

        public override HashedString ViewMode() => ID;
        public override string GetSoundName() => "LiquidVent";

        public override void Enable()
        {
            RegisterSaveLoadListeners();
            partition = PopulatePartition<SaveLoadRoot>(PipeIDs);
            Camera.main.cullingMask |= cameraLayerMask;
            GridCompositor.Instance.ToggleMinor(false);
        }

        protected override void OnSaveLoadRootRegistered(SaveLoadRoot root)
        {
            if (PipeIDs.Contains(root.GetComponent<KPrefabID>().GetSaveLoadTag()))
                partition.Add(root);
        }

        protected override void OnSaveLoadRootUnregistered(SaveLoadRoot root)
        {
            if (root == null || root.gameObject == null)
                return;
            if (layerTargets.Contains(root))
                layerTargets.Remove(root);
            partition.Remove(root);
        }

        public override void Disable()
        {
            ResetDisplayValues(layerTargets);
            Camera.main.cullingMask &= ~cameraLayerMask;
            SelectTool.Instance.ClearLayerMask();
            UnregisterSaveLoadListeners();
            foreach (LocText b in labels)
                b.gameObject.SetActive(false);
            // freeLabels = 0;
            infos.Clear();
            ptargets.Clear();
            partition.Clear();
            layerTargets.Clear();
            GridCompositor.Instance.ToggleMinor(false);
        }

        public override void Update()
        {
            Grid.GetVisibleExtents(out Vector2I min, out Vector2I max);
            RemoveOffscreenTargets(layerTargets, min, max);
            IEnumerable iter = partition.GetAllIntersecting(min, max);
            try
            {
                foreach (object obj in iter)
                    AddTargetIfVisible((SaveLoadRoot)obj, min, max, layerTargets, targetLayer);
            }
            finally
            {
                if (iter is IDisposable d)
                    d.Dispose();
            }
            float num = 1;
            connectedNetworks.Clear();
            UtilityNetworkManager<HUNetwork, HUPipe> mgr = Constants.hupipeSystem;
            if (SelectTool.Instance != null && SelectTool.Instance.hover?.gameObject.GetComponent<IHaveHUCell>() is IHaveHUCell item)
            {
                HashSet<int> visited = new HashSet<int>();
                FindConnectedNetworks(item.HUCell, mgr, connectedNetworks, visited);
                visited.Clear();
                num = OverlayModes.ModeUtil.GetHighlightScale();
            }
            foreach (SaveLoadRoot layerTarget in layerTargets)
                if (layerTarget != null)
                {
                    Color32 c32 = Game.Instance.liquidConduitVisInfo.overlayTint;
                    if (connectedNetworks.Count > 0 && layerTarget.GetComponent<IHaveHUCell>() is IHaveHUCell it2 && connectedNetworks.Contains(mgr.GetNetworkForCell(it2.HUCell)))
                    {
                        c32.r = (byte)(c32.r * num);
                        c32.g = (byte)(c32.g * num);
                        c32.b = (byte)(c32.b * num);
                    }
                    layerTarget.GetComponent<KBatchedAnimController>().TintColour = c32;
                }
            foreach (IHUGenerator g in Constants.manager.generators)
                if (g.IfAddLabel(ptargets, min, max))
                    AddLabel(g, default_labelOffset);
            foreach (IHUConsumer c in Constants.manager.consumers)
                if (c.IfAddLabel(ptargets, min, max))
                {
                    AddLabel(c, default_labelOffset);
                    AddLabel(c, minHU_labelOffset, 1);
                }
            UpdateHULabels();
        }

        private void AddLabel(IHUOverlayUpdate target, Vector3 offset, byte _handle = 0)
        {
            LocText free = GetFreeLabel();
            free.gameObject.SetActive(true);
            free.gameObject.name = target.ObjectName + "hu label";
            LocText com = free.transform.GetChild(0).GetComponent<LocText>();
            com.gameObject.SetActive(true);
            com.enabled = free.enabled = true;
            free.rectTransform.SetPosition(Grid.CellToPos(target.HUCell, 0.5f, 0, 0) + offset);
            if (free.GetComponent<ToolTip>() is ToolTip tip)
                tip.toolTip = target.ToolTipText;
            infos.Add(new UpdateHUInfo(free, com, target, _handle));
        }

        private LocText GetFreeLabel()
        {
            LocText temp;
            if (freeLabels < labels.Count)
                temp = labels[freeLabels];
            else
            {
                temp = Util.KInstantiateUI<LocText>(labelPrefab.gameObject, canvas.transform.gameObject);
                labels.Add(temp);
            }
            freeLabels++;
            return temp;
        }

        private void UpdateHULabels()
        {
            foreach (UpdateHUInfo ix in infos)
                ix.updater.UpdateVisualizer(ix.label, ix.unit, generatorColour, consumerColour, ix.labelHandle);
            // ix.label.color = ix.unit.color = generatorColour;
        }

        private void FindConnectedNetworks(int hUCell, UtilityNetworkManager<HUNetwork, HUPipe> mgr, HashSet<UtilityNetwork> connectedNetworks, HashSet<int> visited)
        {
            if (visited.Contains(hUCell))
                return;
            visited.Add(hUCell);
            UtilityNetwork nfc = mgr.GetNetworkForCell(hUCell);
            if (nfc == null)
                return;
            connectedNetworks.Add(nfc);
            UtilityConnections c = mgr.GetConnections(hUCell, false);
            if (c.HasFlag(UtilityConnections.Right))
                FindConnectedNetworks(Grid.CellRight(hUCell), mgr, connectedNetworks, visited);
            if (c.HasFlag(UtilityConnections.Left))
                FindConnectedNetworks(Grid.CellLeft(hUCell), mgr, connectedNetworks, visited);
            if (c.HasFlag(UtilityConnections.Up))
                FindConnectedNetworks(Grid.CellAbove(hUCell), mgr, connectedNetworks, visited);
            if (c.HasFlag(UtilityConnections.Down))
                FindConnectedNetworks(Grid.CellBelow(hUCell), mgr, connectedNetworks, visited);
        }

        public sealed class UpdateHUInfo
        {
            public LocText label, unit;
            public IHUOverlayUpdate updater;
            public byte labelHandle;

            public UpdateHUInfo(LocText _label, LocText _unit, IHUOverlayUpdate iver, byte handle = 0)
            {
                label = _label;
                unit = _unit;
                updater = iver;
                labelHandle = handle;
            }
        }
    }
}
