using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000013 RID: 19
	public class HUPipeOverlay : OverlayModes.Mode
	{
		// Token: 0x06000034 RID: 52 RVA: 0x0000391C File Offset: 0x00001B1C
		public HUPipeOverlay(Canvas parent, LocText prefab, Color con, Color gen)
		{
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.canvas = parent;
			this.default_labelOffset = new Vector3(0f, 1.2f, 0f);
			this.minHU_labelOffset = new Vector3(0f, 1.48f, 0f);
			this.consumerColour = con;
			this.generatorColour = gen;
			this.labelPrefab = Util.KInstantiateUI<LocText>(prefab.gameObject, parent.transform.gameObject, false);
			LocText unit = this.labelPrefab.transform.GetChild(0).GetComponent<LocText>();
			unit.rectTransform.sizeDelta = new Vector2(15f, unit.rectTransform.rect.height);
			unit.text = "HU/s";
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003A52 File Offset: 0x00001C52
		public override HashedString ViewMode()
		{
			return "HUPipe";
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003A5E File Offset: 0x00001C5E
		public override string GetSoundName()
		{
			return "LiquidVent";
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003A65 File Offset: 0x00001C65
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(HUPipeOverlay.PipeIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
			GridCompositor.Instance.ToggleMinor(false);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003A9F File Offset: 0x00001C9F
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot root)
		{
			if (HUPipeOverlay.PipeIDs.Contains(root.GetComponent<KPrefabID>().GetSaveLoadTag()))
			{
				this.partition.Add(root);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003AC4 File Offset: 0x00001CC4
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot root)
		{
			if (root == null || root.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(root))
			{
				this.layerTargets.Remove(root);
			}
			this.partition.Remove(root);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003B10 File Offset: 0x00001D10
		public override void Disable()
		{
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			foreach (LocText locText in this.labels)
			{
				locText.gameObject.SetActive(false);
			}
			this.infos.Clear();
			this.ptargets.Clear();
			this.partition.Clear();
			this.layerTargets.Clear();
			GridCompositor.Instance.ToggleMinor(false);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003BD0 File Offset: 0x00001DD0
		public override void Update()
		{
			Vector2I min;
			Vector2I max;
			Grid.GetVisibleExtents(ref min, ref max);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, min, max, null);
			using (IEnumerable iter = this.partition.GetAllIntersecting(min, max))
			{
				foreach (object obj in iter)
				{
					base.AddTargetIfVisible<SaveLoadRoot>((SaveLoadRoot)obj, min, max, this.layerTargets, this.targetLayer, null, null);
				}
			}
			float num = 1f;
			this.connectedNetworks.Clear();
			UtilityNetworkManager<HUNetwork, HUPipe> mgr = Constants.hupipeSystem;
			if (SelectTool.Instance != null)
			{
				KSelectable hover = SelectTool.Instance.hover;
				IHaveHUCell item = (hover != null) ? hover.gameObject.GetComponent<IHaveHUCell>() : null;
				if (item != null)
				{
					HashSet<int> visited = new HashSet<int>();
					this.FindConnectedNetworks(item.HUCell, mgr, this.connectedNetworks, visited);
					visited.Clear();
					num = OverlayModes.ModeUtil.GetHighlightScale();
				}
			}
			foreach (SaveLoadRoot layerTarget in this.layerTargets)
			{
				if (layerTarget != null)
				{
					Color32 c32 = Game.Instance.liquidConduitVisInfo.overlayTint;
					if (this.connectedNetworks.Count > 0)
					{
						IHaveHUCell it2 = layerTarget.GetComponent<IHaveHUCell>();
						if (it2 != null && this.connectedNetworks.Contains(mgr.GetNetworkForCell(it2.HUCell)))
						{
							c32.r = (byte)((float)c32.r * num);
							c32.g = (byte)((float)c32.g * num);
							c32.b = (byte)((float)c32.b * num);
						}
					}
					layerTarget.GetComponent<KBatchedAnimController>().TintColour = c32;
				}
			}
			foreach (IHUGenerator g in Constants.manager.generators)
			{
				if (g.IfAddLabel(this.ptargets, min, max))
				{
					this.AddLabel(g, this.default_labelOffset, 0);
				}
			}
			foreach (IHUConsumer c33 in Constants.manager.consumers)
			{
				if (c33.IfAddLabel(this.ptargets, min, max))
				{
					this.AddLabel(c33, this.default_labelOffset, 0);
					this.AddLabel(c33, this.minHU_labelOffset, 1);
				}
			}
			this.UpdateHULabels();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003EB0 File Offset: 0x000020B0
		private void AddLabel(IHUOverlayUpdate target, Vector3 offset, byte _handle = 0)
		{
			LocText free = this.GetFreeLabel();
			free.gameObject.SetActive(true);
			free.gameObject.name = target.ObjectName + "hu label";
			LocText com = free.transform.GetChild(0).GetComponent<LocText>();
			com.gameObject.SetActive(true);
			com.enabled = (free.enabled = true);
			TransformExtensions.SetPosition(free.rectTransform, Grid.CellToPos(target.HUCell, 0.5f, 0f, 0f) + offset);
			ToolTip tip = free.GetComponent<ToolTip>();
			if (tip != null)
			{
				tip.toolTip = target.ToolTipText;
			}
			this.infos.Add(new HUPipeOverlay.UpdateHUInfo(free, com, target, _handle));
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003F70 File Offset: 0x00002170
		private LocText GetFreeLabel()
		{
			LocText temp;
			if (this.freeLabels < this.labels.Count)
			{
				temp = this.labels[this.freeLabels];
			}
			else
			{
				temp = Util.KInstantiateUI<LocText>(this.labelPrefab.gameObject, this.canvas.transform.gameObject, false);
				this.labels.Add(temp);
			}
			this.freeLabels++;
			return temp;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003FE4 File Offset: 0x000021E4
		private void UpdateHULabels()
		{
			foreach (HUPipeOverlay.UpdateHUInfo ix in this.infos)
			{
				ix.updater.UpdateVisualizer(ix.label, ix.unit, this.generatorColour, this.consumerColour, ix.labelHandle);
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000405C File Offset: 0x0000225C
		private void FindConnectedNetworks(int hUCell, UtilityNetworkManager<HUNetwork, HUPipe> mgr, HashSet<UtilityNetwork> connectedNetworks, HashSet<int> visited)
		{
			if (visited.Contains(hUCell))
			{
				return;
			}
			visited.Add(hUCell);
			UtilityNetwork nfc = mgr.GetNetworkForCell(hUCell);
			if (nfc == null)
			{
				return;
			}
			connectedNetworks.Add(nfc);
			UtilityConnections c = mgr.GetConnections(hUCell, false);
			if (c.HasFlag(2))
			{
				this.FindConnectedNetworks(Grid.CellRight(hUCell), mgr, connectedNetworks, visited);
			}
			if (c.HasFlag(1))
			{
				this.FindConnectedNetworks(Grid.CellLeft(hUCell), mgr, connectedNetworks, visited);
			}
			if (c.HasFlag(4))
			{
				this.FindConnectedNetworks(Grid.CellAbove(hUCell), mgr, connectedNetworks, visited);
			}
			if (c.HasFlag(8))
			{
				this.FindConnectedNetworks(Grid.CellBelow(hUCell), mgr, connectedNetworks, visited);
			}
		}

		// Token: 0x0400002F RID: 47
		public static HashSet<Tag> PipeIDs = new HashSet<Tag>();

		// Token: 0x04000030 RID: 48
		public const string ID = "HUPipe";

		// Token: 0x04000031 RID: 49
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04000032 RID: 50
		private HashSet<SaveLoadRoot> ptargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04000033 RID: 51
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x04000034 RID: 52
		private List<LocText> labels = new List<LocText>();

		// Token: 0x04000035 RID: 53
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x04000036 RID: 54
		private List<HUPipeOverlay.UpdateHUInfo> infos = new List<HUPipeOverlay.UpdateHUInfo>();

		// Token: 0x04000037 RID: 55
		private int targetLayer;

		// Token: 0x04000038 RID: 56
		private int cameraLayerMask;

		// Token: 0x04000039 RID: 57
		private LocText labelPrefab;

		// Token: 0x0400003A RID: 58
		private Vector3 default_labelOffset;

		// Token: 0x0400003B RID: 59
		private Vector3 minHU_labelOffset;

		// Token: 0x0400003C RID: 60
		private Canvas canvas;

		// Token: 0x0400003D RID: 61
		private int freeLabels;

		// Token: 0x0400003E RID: 62
		private Color32 consumerColour;

		// Token: 0x0400003F RID: 63
		private Color32 generatorColour;

		// Token: 0x02000029 RID: 41
		public sealed class UpdateHUInfo
		{
			// Token: 0x0600009E RID: 158 RVA: 0x00004E65 File Offset: 0x00003065
			public UpdateHUInfo(LocText _label, LocText _unit, IHUOverlayUpdate iver, byte handle = 0)
			{
				this.label = _label;
				this.unit = _unit;
				this.updater = iver;
				this.labelHandle = handle;
			}

			// Token: 0x0400005D RID: 93
			public LocText label;

			// Token: 0x0400005E RID: 94
			public LocText unit;

			// Token: 0x0400005F RID: 95
			public IHUOverlayUpdate updater;

			// Token: 0x04000060 RID: 96
			public byte labelHandle;
		}
	}
}
