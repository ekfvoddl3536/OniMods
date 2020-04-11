using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;
using UnityEngine.UI;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000014 RID: 20
	[SerializationConfig(1)]
	public class HUGenerator : KMonoBehaviour, ISaveLoadable, IHUGenerator, IHUSim200ms, IHUOverlayUpdate, IHaveHUCell
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00004132 File Offset: 0x00002332
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00004144 File Offset: 0x00002344
		protected virtual bool IsConnected
		{
			get
			{
				return this.operational.GetFlag(Constants.connectedFlag);
			}
			set
			{
				this.operational.SetFlag(Constants.connectedFlag, value);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00004157 File Offset: 0x00002357
		// (set) Token: 0x06000044 RID: 68 RVA: 0x0000415F File Offset: 0x0000235F
		public virtual int HUCell { get; protected set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00004168 File Offset: 0x00002368
		public virtual string ObjectName
		{
			get
			{
				return base.gameObject.name;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00004175 File Offset: 0x00002375
		public virtual string ToolTipText
		{
			get
			{
				return "HU Generated";
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000417C File Offset: 0x0000237C
		protected override void OnSpawn()
		{
			Constants.manager.Connect(this);
			this.HUCell = this.building.GetRotatedOffsetCell(this.offset);
			this.IsConnected = false;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000041A7 File Offset: 0x000023A7
		protected override void OnCleanUp()
		{
			this.IsConnected = false;
			Constants.manager.Disconnect(this);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000041BB File Offset: 0x000023BB
		public virtual int GenerateHeat(float dt)
		{
			return this.generateHU;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000041C3 File Offset: 0x000023C3
		public virtual void HUSim200ms(float dt)
		{
			this.IsConnected = (Constants.manager.GetID(this.HUCell) != ushort.MaxValue);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000041E8 File Offset: 0x000023E8
		public bool IfAddLabel(ICollection<SaveLoadRoot> roots, Vector2I min, Vector2I max)
		{
			Vector2I xy = Grid.PosToXY(TransformExtensions.GetPosition(base.transform));
			if (min <= xy && xy <= max)
			{
				SaveLoadRoot root = base.GetComponent<SaveLoadRoot>();
				if (!roots.Contains(root))
				{
					roots.Add(root);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004234 File Offset: 0x00002434
		public virtual void UpdateVisualizer(LocText label, LocText units, Color32 generator_color, Color32 consumer_color, byte labelHandle)
		{
			label.text = string.Format("+{0}", this.generateHU);
			label.color = (units.color = generator_color);
			Image img = base.GetComponent<BuildingCellVisualizer>().GetOutputIcon();
			if (img != null)
			{
				img.color = generator_color;
			}
		}

		// Token: 0x04000040 RID: 64
		[MyCmpReq]
		protected Building building;

		// Token: 0x04000041 RID: 65
		[MyCmpReq]
		protected Operational operational;

		// Token: 0x04000042 RID: 66
		public int generateHU;

		// Token: 0x04000043 RID: 67
		public CellOffset offset;
	}
}
