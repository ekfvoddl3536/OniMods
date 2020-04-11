using System;
using Harmony;

namespace AdvancedGeneratos
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch("LoadGeneratedBuildings")]
	public sealed class GeneratedBuildings_LoadGeneratedBuildings_Patch
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002624 File Offset: 0x00000824
		public static void Prefix()
		{
			GeneratedBuildings_LoadGeneratedBuildings_Patch.SetString(Constans.RefinedCarbonGenerator.ID_UPPER, Constans.RefinedCarbonGenerator.NAME, Constans.RefinedCarbonGenerator.DESC, "정제 탄소를 연소하고, 많은 전기를 생산합니다.");
			GeneratedBuildings_LoadGeneratedBuildings_Patch.SetString(Constans.ThermoelectricGenerator.ID_UPPER, Constans.ThermoelectricGenerator.NAME, Constans.ThermoelectricGenerator.DESC, "열을 제거하고 전기를 생산합니다.");
			GeneratedBuildings_LoadGeneratedBuildings_Patch.SetString(Constans.NaphthaGenerator.ID_UPPER, Constans.NaphthaGenerator.NAME, Constans.NaphthaGenerator.DESC, "산소를 필요로 하며, 나프타를 연료로 합니다.");
			GeneratedBuildings_LoadGeneratedBuildings_Patch.SetString(Constans.EcoFriendlyMethaneGenerator.ID_UPPER, Constans.EcoFriendlyMethaneGenerator.NAME, Constans.EcoFriendlyMethaneGenerator.DESC, "산소와 여과 매질을 필요로 하며, 오염된 물을 배출하지 않습니다.");
			ModUtil.AddBuildingToPlanScreen("Power", "RefinedCarbonGenerator");
			ModUtil.AddBuildingToPlanScreen("Power", "ThermoelectricGenerator");
			ModUtil.AddBuildingToPlanScreen("Power", "NaphthaGenerator");
			ModUtil.AddBuildingToPlanScreen("Power", "EcoFriendlyMethaneGenerator");
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002710 File Offset: 0x00000910
		private static void SetString(string path, string name, string desc, string eff)
		{
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS." + path + ".NAME",
				name
			});
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS." + path + ".DESC",
				desc
			});
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS." + path + ".EFFECT",
				eff
			});
		}
	}
}
