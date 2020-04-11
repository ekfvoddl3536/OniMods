using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000015 RID: 21
	public class HUConsumer : KMonoBehaviour, ISaveLoadable, IHUConsumer, IHUSim200ms, IHUOverlayUpdate, IHaveHUCell, IEffectDescriptor
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00004294 File Offset: 0x00002494
		// (set) Token: 0x0600004F RID: 79 RVA: 0x000042A6 File Offset: 0x000024A6
		public virtual bool IsConnected
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

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000042B9 File Offset: 0x000024B9
		public virtual int HUMax
		{
			get
			{
				return this.huMaximum;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000042C1 File Offset: 0x000024C1
		public virtual int HUMin
		{
			get
			{
				return this.huMinimum;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000042C9 File Offset: 0x000024C9
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000042D1 File Offset: 0x000024D1
		public virtual int HUCell { get; protected set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000042DA File Offset: 0x000024DA
		public virtual string ObjectName
		{
			get
			{
				return base.gameObject.name;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000055 RID: 85 RVA: 0x000042E7 File Offset: 0x000024E7
		public virtual string ToolTipText
		{
			get
			{
				return "HU Consumed";
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000042EE File Offset: 0x000024EE
		protected override void OnSpawn()
		{
			this.HUCell = this.building.GetRotatedOffsetCell(this.offset);
			Constants.manager.Connect(this);
			this.IsConnected = false;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004319 File Offset: 0x00002519
		protected override void OnCleanUp()
		{
			this.IsConnected = false;
			Constants.manager.Disconnect(this);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000432D File Offset: 0x0000252D
		public virtual List<Descriptor> GetDescriptors(BuildingDef def)
		{
			return null;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004330 File Offset: 0x00002530
		public virtual int ConsumedHU(int HUavailable, float dt)
		{
			return Math.Min(this.HUMax, HUavailable);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000433E File Offset: 0x0000253E
		public virtual void HUSim200ms(float dt)
		{
			this.IsConnected = (Constants.manager.GetID(this.HUCell) != ushort.MaxValue);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004360 File Offset: 0x00002560
		public virtual bool IfAddLabel(ICollection<SaveLoadRoot> roots, Vector2I min, Vector2I max)
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

		// Token: 0x0600005C RID: 92 RVA: 0x000043AC File Offset: 0x000025AC
		public virtual void UpdateVisualizer(LocText label, LocText units, Color32 generator_color, Color32 consumer_color, byte labelHandle)
		{
			if (labelHandle == 1)
			{
				consumer_color..ctor(250, 80, 130, byte.MaxValue);
				label.text = string.Format("-{0}", this.huMinimum);
			}
			else
			{
				label.text = string.Format("-{0}", this.huMaximum);
			}
			label.color = (units.color = consumer_color);
			Image img = base.GetComponent<BuildingCellVisualizer>().GetOutputIcon();
			if (img != null)
			{
				img.color = consumer_color;
			}
		}

		// Token: 0x04000045 RID: 69
		public CellOffset offset;

		// Token: 0x04000046 RID: 70
		[MyCmpReq]
		protected Building building;

		// Token: 0x04000047 RID: 71
		[MyCmpGet]
		protected Operational operational;

		// Token: 0x04000048 RID: 72
		public int huMinimum;

		// Token: 0x04000049 RID: 73
		public int huMaximum;
	}
}
