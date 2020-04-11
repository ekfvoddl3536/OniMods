using System;
using Harmony;

namespace CarbonatedWater
{
	// Token: 0x02000008 RID: 8
	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch("LoadGeneratedBuildings")]
	public static class MYPATCH
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002570 File Offset: 0x00000770
		public static void Prefix()
		{
			string temp = "STRINGS.BUILDINGS.PREFABS." + "CO2WaterGen".ToUpper();
			Strings.Add(new string[]
			{
				temp + ".NAME",
				Consts.CWG_NAME
			});
			Strings.Add(new string[]
			{
				temp + ".DESC",
				Consts.CWG_DESC
			});
			Strings.Add(new string[]
			{
				temp + ".EFFECT",
				"듀플리칸트는 먹어야합니다. 좋은 품질의 음식을 요구한다면, 탄산음료는 거의 필수 입니다.\n또한, 특별한 레시피를 통해 만들 수 있는 특별한 음료가 있습니다."
			});
			temp = "STRINGS.ITEMS.FOOD." + "Cola".ToUpper();
			Strings.Add(new string[]
			{
				temp + ".NAME",
				CarbonatedWaterList.COLA.NAME
			});
			Strings.Add(new string[]
			{
				temp + ".DESC",
				CarbonatedWaterList.COLA.DESC
			});
			Strings.Add(new string[]
			{
				temp + ".RECIPEDESC",
				"탄산수 제조기를 통해 얻을 수 있습니다."
			});
			temp = "STRINGS.ITEMS.FOOD." + "DoctorBerry".ToUpper();
			Strings.Add(new string[]
			{
				temp + ".NAME",
				CarbonatedWaterList.DOCTORBERRY.NAME
			});
			Strings.Add(new string[]
			{
				temp + ".DESC",
				CarbonatedWaterList.DOCTORBERRY.DESC
			});
			Strings.Add(new string[]
			{
				temp + ".RECIPEDESC",
				"탄산수 제조기를 통해 얻을 수 있습니다."
			});
			temp = "STRINGS.ITEMS.FOOD." + "ItsTerine".ToUpper();
			Strings.Add(new string[]
			{
				temp + ".NAME",
				CarbonatedWaterList.ITSTERINE.NAME
			});
			Strings.Add(new string[]
			{
				temp + ".DESC",
				CarbonatedWaterList.ITSTERINE.DESC
			});
			Strings.Add(new string[]
			{
				temp + ".RECIPEDESC",
				"탄산수 제조기를 통해 얻을 수 있습니다."
			});
			CarbonatedWaterTypes.FirstSetup();
			ModUtil.AddBuildingToPlanScreen("Food", "CO2WaterGen");
		}
	}
}
