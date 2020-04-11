using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

namespace AdvancedExcavatorAndPump
{
	// Token: 0x02000002 RID: 2
	[SerializationConfig(1)]
	public class AdvancedExcavator : KMonoBehaviour, ISim1000ms
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static void OnOperChangeStatic(AdvancedExcavator arg1, object arg2)
		{
			arg1.OnOperChanged((bool)arg2);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x0000205E File Offset: 0x0000025E
		public bool HasDigCell
		{
			get
			{
				return this.reservedCells.Count > 0;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000206E File Offset: 0x0000026E
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.simRenderLoadBalance = true;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002080 File Offset: 0x00000280
		protected override void OnSpawn()
		{
			base.OnSpawn();
			base.Subscribe<AdvancedExcavator>(-592767678, AdvancedExcavator.OnOperChangeDelegate);
			this.trcell = Grid.PosToCell(base.transform.localPosition);
			this.consumer.sampleCellOffset = new Vector3(0f, (float)(-1 * Math.Max(1, this.currentLength)));
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020DE File Offset: 0x000002DE
		protected virtual int GetSampleCell()
		{
			return this.GetSampleCell(this.currentLength);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020EC File Offset: 0x000002EC
		protected int GetSampleCell(int len)
		{
			return Grid.OffsetCell(this.trcell, new CellOffset(0, -1 * len));
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002102 File Offset: 0x00000302
		protected virtual void OnOperChanged(bool data)
		{
			if (data ^ this.operational.IsActive)
			{
				this.operational.SetActive(data, false);
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002120 File Offset: 0x00000320
		protected virtual void RefreshDiggableCell(int cell)
		{
			if (this.IsDiggableCell(cell) && Grid.NavValidatorMasks[cell] == null)
			{
				this.reservedCells.Enqueue(cell);
				int x;
				int y;
				Grid.CellToXY(cell, ref x, ref y);
				this.RefreshDiggableCell(x, y, (int)(-(int)this.sidemax), -1);
				this.RefreshDiggableCell(x, y, 1, (int)this.sidemax);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002174 File Offset: 0x00000374
		protected void RefreshDiggableCell(int x, int y, int start, int end)
		{
			while (start <= end)
			{
				int ncell = Grid.XYToCell(x + start, y);
				if (Grid.IsValidCell(ncell) && this.IsDiggableCell(ncell))
				{
					this.reservedCells.Enqueue(ncell);
				}
				start++;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021B4 File Offset: 0x000003B4
		protected bool IsDiggableCell(int cell)
		{
			return Grid.Solid[cell] && !Grid.Foundation[cell] && Grid.Element[cell].hardness <= this.hardnessLv;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021EC File Offset: 0x000003EC
		public void UpdateDig(float dt)
		{
			int digcell = this.reservedCells.Peek();
			Diggable.DoDigTick(digcell, dt);
			this.miningsd.SetPercentComplete(Grid.Damage[digcell]);
			if (!Grid.Solid[digcell])
			{
				this.reservedCells.Dequeue();
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002238 File Offset: 0x00000438
		public virtual void Sim1000ms(float dt)
		{
			if (this.operational.IsActive)
			{
				if (this.HasDigCell)
				{
					this.UpdateDig(dt);
					return;
				}
				if (this.currentLength <= this.maxLength)
				{
					int ncell = this.GetSampleCell(this.currentLength + 1);
					if (Grid.IsValidCell(ncell))
					{
						this.RefreshDiggableCell(ncell);
						this.OnUpdateNextCell(ncell);
						return;
					}
				}
				else
				{
					this.operational.SetActive(false, false);
					this.EnableConsum(false);
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022AC File Offset: 0x000004AC
		protected virtual void OnUpdateNextCell(int ncell)
		{
			Element.State temp = Grid.Element[ncell].state & 3;
			if (temp == 2)
			{
				this.EnableConsum(true);
				return;
			}
			this.EnableConsum(false);
			if (this.HasDigCell ^ temp <= 1)
			{
				this.currentLength++;
				this.consumer.sampleCellOffset += Vector3.down;
				this.controller.Play("working_loop", 0, 1f, 0f);
				return;
			}
			this.controller.Play("off", 1, 1f, 0f);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002355 File Offset: 0x00000555
		public void EnableConsum(bool enable)
		{
			this.consumer.EnableConsumption(enable);
			this.consumer.showDescriptor = enable;
		}

		// Token: 0x04000001 RID: 1
		protected const int tilemain = 30;

		// Token: 0x04000002 RID: 2
		protected const int OnOperChangedHash = -592767678;

		// Token: 0x04000003 RID: 3
		protected const Grid.BuildFlags flag = 10;

		// Token: 0x04000004 RID: 4
		protected static readonly EventSystem.IntraObjectHandler<AdvancedExcavator> OnOperChangeDelegate = new EventSystem.IntraObjectHandler<AdvancedExcavator>(new Action<AdvancedExcavator, object>(AdvancedExcavator.OnOperChangeStatic));

		// Token: 0x04000005 RID: 5
		public float consumptionRate = 5f;

		// Token: 0x04000006 RID: 6
		[MyCmpGet]
		protected Operational operational;

		// Token: 0x04000007 RID: 7
		[MyCmpGet]
		protected ElementConsumer consumer;

		// Token: 0x04000008 RID: 8
		[MyCmpReq]
		protected MiningSounds miningsd;

		// Token: 0x04000009 RID: 9
		[MyCmpGet]
		protected KBatchedAnimController controller;

		// Token: 0x0400000A RID: 10
		public byte hardnessLv = 150;

		// Token: 0x0400000B RID: 11
		public int maxLength = 50;

		// Token: 0x0400000C RID: 12
		public sbyte sidemax = 1;

		// Token: 0x0400000D RID: 13
		[Serialize]
		protected int currentLength;

		// Token: 0x0400000E RID: 14
		protected int trcell;

		// Token: 0x0400000F RID: 15
		[Serialize]
		protected Queue<int> reservedCells = new Queue<int>();
	}
}
