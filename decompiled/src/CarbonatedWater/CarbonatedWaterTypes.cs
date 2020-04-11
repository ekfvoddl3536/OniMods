using System;
using System.Collections.Generic;

namespace CarbonatedWater
{
	// Token: 0x02000004 RID: 4
	internal sealed class CarbonatedWaterTypes
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000230A File Offset: 0x0000050A
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002311 File Offset: 0x00000511
		public static EdiblesManager.FoodInfo FI_COLA { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002319 File Offset: 0x00000519
		// (set) Token: 0x0600000B RID: 11 RVA: 0x00002320 File Offset: 0x00000520
		public static FoodMagicInfo FI_ITSTERINE { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002328 File Offset: 0x00000528
		// (set) Token: 0x0600000D RID: 13 RVA: 0x0000232F File Offset: 0x0000052F
		public static EdiblesManager.FoodInfo FI_DOCTOR_BERRY { get; private set; }

		// Token: 0x0600000E RID: 14 RVA: 0x00002338 File Offset: 0x00000538
		public static void FirstSetup()
		{
			CarbonatedWaterTypes.FI_COLA = new EdiblesManager.FoodInfo("Cola", 320000f, 4, 255.15f, 263.15f, 9600f, false);
			CarbonatedWaterTypes.FI_DOCTOR_BERRY = new EdiblesManager.FoodInfo("DoctorBerry", 2200000f, 5, 255.15f, 275.15f, 9600f, false);
			CarbonatedWaterTypes.FI_DOCTOR_BERRY.AddEffects(new List<string>
			{
				"GoodEats"
			});
			CarbonatedWaterTypes.FI_ITSTERINE = new FoodMagicInfo("ItsTerine", 90000f, 0, 255.15f, 275.15f, 3200f, true, "Medicine_BasicBooster", 0, null);
		}
	}
}
