using System;
using Harmony;

namespace ManualExhaustPump
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	public class GEN_PATCH_0A
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002450 File Offset: 0x00000650
		public static void Prefix()
		{
			GEN_PATCH_0A.SetString(Constants.ManualGasPump.ID_UPPER, Constants.ManualGasPump.NAME, "기체를 임시 저장소에 저장했다가, 기체관을 통해 내보냅니다.받은 기체는 항상 내보내려고 시도합니다, 만약 배관이 막혀, 저장소에 기체가 가득찬 경우 자동화 신호를 활성화로 변경합니다.", "병에 든 기체를 기체관으로 내보냅니다.");
			GEN_PATCH_0A.SetString(Constants.ManualLiquidPump.ID_UPPER, Constants.ManualLiquidPump.NAME, "액체를 임시 저장소에 저장했다가, 액체관을 통해 내보냅니다.\n받은 액체는 항상 내보내려고 시도합니다, 만약 배관이 막혀, 저장소에 액체가 가득찬 경우 자동화 신호를 활성화로 변경합니다.", "병에 든 액체를 액체관으로 내보냅니다.");
			GEN_PATCH_0A.SetString(Constants.LiquidBottleCharger.ID_UPPER, Constants.LiquidBottleCharger.NAME, "저장된 액체는 듀플이 가져갈 수 있습니다.", "배관을 통해 받은 액체를 저장하고, 병에 담습니다.");
			ModUtil.AddBuildingToPlanScreen("Plumbing", "ManualLiquidPump");
			ModUtil.AddBuildingToPlanScreen("Plumbing", "LiquidBottleCharger");
			ModUtil.AddBuildingToPlanScreen("HVAC", "ManualGasPump");
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000024F4 File Offset: 0x000006F4
		private static void SetString(string UPPER, string name, string desc, string eff)
		{
			UPPER = "STRINGS.BUILDINGS.PREFABS." + UPPER;
			Strings.Add(new string[]
			{
				UPPER + ".NAME",
				name
			});
			Strings.Add(new string[]
			{
				UPPER + ".DESC",
				desc
			});
			Strings.Add(new string[]
			{
				UPPER + ".EFFECT",
				eff
			});
		}
	}
}
