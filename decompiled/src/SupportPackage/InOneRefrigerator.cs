using System;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x02000011 RID: 17
	public sealed class InOneRefrigerator : KMonoBehaviour
	{
		// Token: 0x0600003D RID: 61 RVA: 0x000036E8 File Offset: 0x000018E8
		protected override void OnPrefabInit()
		{
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000036EA File Offset: 0x000018EA
		protected override void OnSpawn()
		{
			this.temperatureAdjuster = new SimulatedTemperatureAdjuster(this.simulatedTemperature, this.simulatedHeatCapacity, this.simulatedThermalConductivity, this.outStorage);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000370F File Offset: 0x0000190F
		protected override void OnCleanUp()
		{
			this.temperatureAdjuster.CleanUp();
		}

		// Token: 0x0400000F RID: 15
		[SerializeField]
		public float simulatedTemperature = 273.15f;

		// Token: 0x04000010 RID: 16
		[SerializeField]
		public float simulatedHeatCapacity = 400f;

		// Token: 0x04000011 RID: 17
		[SerializeField]
		public float simulatedThermalConductivity = 1000f;

		// Token: 0x04000012 RID: 18
		public Storage outStorage;

		// Token: 0x04000013 RID: 19
		private SimulatedTemperatureAdjuster temperatureAdjuster;
	}
}
