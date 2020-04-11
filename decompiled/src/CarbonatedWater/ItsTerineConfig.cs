using System;
using UnityEngine;

namespace CarbonatedWater
{
	// Token: 0x0200000D RID: 13
	public class ItsTerineConfig : CarbonatedWaterBaseConfig
	{
		// Token: 0x0600001F RID: 31 RVA: 0x000028C8 File Offset: 0x00000AC8
		public override GameObject CreatePrefab()
		{
			return Extend.ExtendEntityToSpiecalFood(EntityTemplates.CreateLooseEntity("ItsTerine", CarbonatedWaterList.ITSTERINE.NAME, CarbonatedWaterList.ITSTERINE.DESC, 1f, false, Assets.GetAnim("pill_2_kanim"), "object", 26, 1, 0.8f, 0.4f, true, 0, 976099455, null), CarbonatedWaterTypes.FI_ITSTERINE);
		}

		// Token: 0x0400001D RID: 29
		public static ComplexRecipe recipe;
	}
}
