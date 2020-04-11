using System;

namespace AdvancedGeneratos
{
	// Token: 0x0200000A RID: 10
	public class ThermoelectricPowerGenerator : Generator
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002B09 File Offset: 0x00000D09
		protected static void OnActivateChangedStatic(ThermoelectricPowerGenerator gen, object data)
		{
			gen.OnActivateChanged(data as Operational);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B17 File Offset: 0x00000D17
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.Subscribe<ThermoelectricPowerGenerator>(824508782, ThermoelectricPowerGenerator.OnActivateChangeDelegate);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002B30 File Offset: 0x00000D30
		protected override void OnSpawn()
		{
			base.OnSpawn();
			if (this.HasMeter)
			{
				this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", this.MeterOffset, -2, new string[]
				{
					"meter_target",
					"meter_fill",
					"meter_frame",
					"meter_OL"
				});
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002B94 File Offset: 0x00000D94
		protected virtual void OnActivateChanged(Operational op)
		{
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Power, op.IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline, this);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override void EnergySim200ms(float dt)
		{
			base.EnergySim200ms(dt);
			this.operational.SetFlag(Generator.wireConnectedFlag, base.CircuitID != ushort.MaxValue);
			this.operational.SetActive(this.operational.IsOperational, false);
			if (this.HasMeter)
			{
				this.MeterSet();
			}
			if (!this.operational.IsOperational)
			{
				return;
			}
			base.GenerateJoules(base.WattageRating * dt, false);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, this);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002C89 File Offset: 0x00000E89
		protected virtual void MeterSet()
		{
			this.meter.SetPositionPercent((float)(this.operational.IsActive ? 1 : 0));
		}

		// Token: 0x04000009 RID: 9
		protected const int OnActivateChangeFlag = 824508782;

		// Token: 0x0400000A RID: 10
		protected static readonly EventSystem.IntraObjectHandler<ThermoelectricPowerGenerator> OnActivateChangeDelegate = new EventSystem.IntraObjectHandler<ThermoelectricPowerGenerator>(new Action<ThermoelectricPowerGenerator, object>(ThermoelectricPowerGenerator.OnActivateChangedStatic));

		// Token: 0x0400000B RID: 11
		public bool HasMeter = true;

		// Token: 0x0400000C RID: 12
		public Meter.Offset MeterOffset;

		// Token: 0x0400000D RID: 13
		protected MeterController meter;
	}
}
