using System;
using KSerialization;
using SuperComicLib;
using UnityEngine;

namespace SINT_SINT_Is_Not_Toilet
{
	// Token: 0x02000007 RID: 7
	[SerializationConfig(1)]
	public class AutoPurify : NotRequirePoweredEnergyConsumer, IEnergyConsumer, ISim1000ms
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002D8E File Offset: 0x00000F8E
		float IEnergyConsumer.WattsNeededWhenActive
		{
			get
			{
				return 60f;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002D95 File Offset: 0x00000F95
		protected override void OnPrefabInit()
		{
			base.BaseWattageRating = 60f;
			base.OnPrefabInit();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002DA8 File Offset: 0x00000FA8
		public override void EnergySim200ms(float dt)
		{
			this.circuitID.SetValue(this, Game.Instance.circuitManager.GetCircuitID(base.PowerCell));
			if (!base.IsConnected || !this.IsPowered)
			{
				this.Overpwered(false);
			}
			this.circuitOverloadTime = Mathf.Max(0f, this.circuitOverloadTime - dt);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002E0C File Offset: 0x0000100C
		protected override void Overpwered(bool op)
		{
			if (op)
			{
				this.converter.consumedElements[0].massConsumptionRate = (this.converter.outputElements[0].massGenerationRate = this.poweredWaterRate);
				this.converter.consumedElements[1].massConsumptionRate = (this.converter.outputElements[1].massGenerationRate = this.poweredFilterRate);
				base.BaseWattageRating = 60f;
				return;
			}
			this.converter.consumedElements[0].massConsumptionRate = (this.converter.outputElements[0].massGenerationRate = this.unpoweredWaterRate);
			this.converter.consumedElements[1].massConsumptionRate = (this.converter.outputElements[1].massGenerationRate = this.unpoweredFilterRate);
			base.BaseWattageRating = 0f;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002F07 File Offset: 0x00001107
		public void Sim1000ms(float dt)
		{
			if (this.operational.IsActive ^ this.converter.HasEnoughMassToStartConverting())
			{
				this.operational.SetActive(!this.operational.IsActive, false);
			}
		}

		// Token: 0x04000011 RID: 17
		public float poweredWaterRate = 1.624475f;

		// Token: 0x04000012 RID: 18
		public float unpoweredWaterRate = 0.001575f;

		// Token: 0x04000013 RID: 19
		public float poweredFilterRate = 1.9124999f;

		// Token: 0x04000014 RID: 20
		public float unpoweredFilterRate = 0.0025f;

		// Token: 0x04000015 RID: 21
		[MyCmpGet]
		public ElementConverter converter;
	}
}
