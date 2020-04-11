using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperComicLib
{
	// Token: 0x02000003 RID: 3
	public class LiquidCoolingSystem : StateMachineComponent<LiquidCoolingSystem.StatesInstance>
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002536 File Offset: 0x00000736
		protected override void OnSpawn()
		{
			base.smi.StartSM();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002543 File Offset: 0x00000743
		public bool HasEnoughCoolant()
		{
			return this.InStorage.GetAmountAvailable(this.CoolantTag) >= this.MinCoolantMass;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002564 File Offset: 0x00000764
		protected virtual void Cooling()
		{
			if (!this.HasEnoughCoolant() || this.primary.Temperature <= this.LowTemperature)
			{
				return;
			}
			float forchange = GameUtil.CalculateEnergyDeltaForElementChange(this.primary.Element.specificHeatCapacity, this.primary.Mass, this.primary.Temperature, this.primary.Temperature - this.CoolingPerSecond);
			List<GameObject> plist = new List<GameObject>();
			this.InStorage.Find(this.CoolantTag, plist);
			PrimaryElement compo = plist[0].GetComponent<PrimaryElement>();
			float fmass = compo.Mass * compo.Element.specificHeatCapacity;
			compo.Temperature += GameUtil.CalculateTemperatureChange(compo.Element.specificHeatCapacity, compo.Mass, -forchange * fmass * this.ThermalFudge);
			this.primary.Temperature -= this.CoolingPerSecond;
			this.InStorage.Transfer(plist[0], this.OutStorage, false, true);
		}

		// Token: 0x04000009 RID: 9
		public float MinCoolantMass = 5f;

		// Token: 0x0400000A RID: 10
		public float ThermalFudge = 0.8f;

		// Token: 0x0400000B RID: 11
		public float CoolingPerSecond = 0.5f;

		// Token: 0x0400000C RID: 12
		public float LowTemperature = 274f;

		// Token: 0x0400000D RID: 13
		public Tag CoolantTag;

		// Token: 0x0400000E RID: 14
		[MyCmpReq]
		protected Operational operational;

		// Token: 0x0400000F RID: 15
		[MyCmpReq]
		protected PrimaryElement primary;

		// Token: 0x04000010 RID: 16
		public Storage InStorage;

		// Token: 0x04000011 RID: 17
		public Storage OutStorage;

		// Token: 0x02000006 RID: 6
		public class StatesInstance : GameStateMachine<LiquidCoolingSystem.States, LiquidCoolingSystem.StatesInstance, LiquidCoolingSystem, object>.GameInstance
		{
			// Token: 0x06000024 RID: 36 RVA: 0x00002AFF File Offset: 0x00000CFF
			public StatesInstance(LiquidCoolingSystem master) : base(master)
			{
			}
		}

		// Token: 0x02000007 RID: 7
		public class States : GameStateMachine<LiquidCoolingSystem.States, LiquidCoolingSystem.StatesInstance, LiquidCoolingSystem>
		{
			// Token: 0x06000025 RID: 37 RVA: 0x00002B08 File Offset: 0x00000D08
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.disabled;
				this.disabled.EventTransition(824508782, this.cooling, (LiquidCoolingSystem.StatesInstance smi) => !(smi.master.operational != null) || smi.master.operational.IsActive);
				this.cooling.EventTransition(824508782, this.disabled, (LiquidCoolingSystem.StatesInstance smi) => smi.master.operational != null && !smi.master.operational.IsActive).Update("Cooling", delegate(LiquidCoolingSystem.StatesInstance smi, float dt)
				{
					smi.master.Cooling();
				}, 6, true);
			}

			// Token: 0x0400001D RID: 29
			public GameStateMachine<LiquidCoolingSystem.States, LiquidCoolingSystem.StatesInstance, LiquidCoolingSystem, object>.State disabled;

			// Token: 0x0400001E RID: 30
			public GameStateMachine<LiquidCoolingSystem.States, LiquidCoolingSystem.StatesInstance, LiquidCoolingSystem, object>.State cooling;
		}
	}
}
