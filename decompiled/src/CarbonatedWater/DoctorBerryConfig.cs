using System;
using UnityEngine;

namespace CarbonatedWater
{
	// Token: 0x0200000C RID: 12
	public class DoctorBerryConfig : CarbonatedWaterBaseConfig
	{
		// Token: 0x0600001D RID: 29 RVA: 0x0000285C File Offset: 0x00000A5C
		public override GameObject CreatePrefab()
		{
			return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DoctorBerry", CarbonatedWaterList.DOCTORBERRY.NAME, CarbonatedWaterList.DOCTORBERRY.DESC, 1.8f, false, Assets.GetAnim("pickledmeal_kanim"), "object", 26, 1, 0.6f, 0.7f, true, 0, 976099455, null), CarbonatedWaterTypes.FI_DOCTOR_BERRY);
		}

		// Token: 0x0400001C RID: 28
		public static ComplexRecipe recipe;
	}
}
