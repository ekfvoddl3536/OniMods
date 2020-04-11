using System;

namespace CarbonatedWater
{
	// Token: 0x02000006 RID: 6
	internal struct FoodMagicInfo
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002424 File Offset: 0x00000624
		public FoodMagicInfo(EdiblesManager.FoodInfo _foodinfo, MedicineInfo _mdinfo)
		{
			this.FoodInfo = _foodinfo;
			this.MedicineInfo = _mdinfo;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002434 File Offset: 0x00000634
		public FoodMagicInfo(string id, float caloriesPerUnit, int quality, float savedTemperatue, float rotTemperature, float spoilTime, bool can_rot, string effect, MedicineInfo.MedicineType medicineType, string[] curedDiseases = null)
		{
			this.FoodInfo = new EdiblesManager.FoodInfo(id, caloriesPerUnit, quality, savedTemperatue, rotTemperature, spoilTime, can_rot);
			this.MedicineInfo = new MedicineInfo(id, effect, medicineType, curedDiseases);
		}

		// Token: 0x04000019 RID: 25
		public EdiblesManager.FoodInfo FoodInfo;

		// Token: 0x0400001A RID: 26
		public MedicineInfo MedicineInfo;
	}
}
