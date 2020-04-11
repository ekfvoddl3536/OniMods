using System;
using UnityEngine;

namespace CarbonatedWater
{
	// Token: 0x0200000B RID: 11
	public class ColaConfig : CarbonatedWaterBaseConfig
	{
		// Token: 0x0600001B RID: 27 RVA: 0x000027F0 File Offset: 0x000009F0
		public override GameObject CreatePrefab()
		{
			return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Cola", CarbonatedWaterList.COLA.NAME, CarbonatedWaterList.COLA.DESC, 1.2f, false, Assets.GetAnim("pill_1_kanim"), "object", 26, 1, 0.8f, 0.4f, true, 0, 976099455, null), CarbonatedWaterTypes.FI_COLA);
		}

		// Token: 0x0400001B RID: 27
		public static ComplexRecipe recipe;
	}
}
