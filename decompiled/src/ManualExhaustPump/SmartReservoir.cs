using System;
using UnityEngine;

namespace ManualExhaustPump
{
	// Token: 0x02000009 RID: 9
	public class SmartReservoir : StorageLocker, ISim1000ms
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002674 File Offset: 0x00000874
		protected override void OnPrefabInit()
		{
			base.Initialize(true);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002680 File Offset: 0x00000880
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.ports = base.gameObject.GetComponent<LogicPorts>();
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", 0, -2, new string[]
			{
				"meter_fill",
				"meter_OL"
			});
			base.Subscribe<SmartReservoir>(-1697596308, SmartReservoir.UpdateLogicCBDeleagte);
			base.Subscribe<SmartReservoir>(-592767678, SmartReservoir.UpdateLogicCBDeleagte);
			this.UpdateLogicAndActivate();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002700 File Offset: 0x00000900
		protected static void UpdateLogicCBStatic(SmartReservoir smart, object data)
		{
			smart.UpdateLogicAndActivate();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002708 File Offset: 0x00000908
		protected virtual void UpdateLogicAndActivate()
		{
			this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
			this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, this.IsOn() ? 1 : 0);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002758 File Offset: 0x00000958
		protected virtual bool IsOn()
		{
			return this.storage.IsFull();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002765 File Offset: 0x00000965
		public virtual void Sim1000ms(float dt)
		{
			this.operational.SetActive(this.operational.IsOperational && !this.storage.IsEmpty(), false);
		}

		// Token: 0x0400000B RID: 11
		protected const int OnStorageChangeFlag = -1697596308;

		// Token: 0x0400000C RID: 12
		protected const int UpdateLogicCBFlag2 = -592767678;

		// Token: 0x0400000D RID: 13
		protected static readonly EventSystem.IntraObjectHandler<SmartReservoir> UpdateLogicCBDeleagte = new EventSystem.IntraObjectHandler<SmartReservoir>(new Action<SmartReservoir, object>(SmartReservoir.UpdateLogicCBStatic));

		// Token: 0x0400000E RID: 14
		public bool allowManualDelivery;

		// Token: 0x0400000F RID: 15
		[MyCmpGet]
		protected LogicPorts ports;

		// Token: 0x04000010 RID: 16
		[MyCmpReq]
		protected Storage storage;

		// Token: 0x04000011 RID: 17
		[MyCmpReq]
		protected Operational operational;

		// Token: 0x04000012 RID: 18
		protected MeterController meter;
	}
}
