using System;
using Harmony;

namespace DupeWhatDiamond
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch("LoadGeneratedBuildings")]
	public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002400 File Offset: 0x00000600
		public static void Prefix()
		{
			string temp = "STRINGS.BUILDINGS.PREFABS." + MConstants.DWD_ID_UPPER;
			Strings.Add(new string[]
			{
				temp + ".NAME",
				MConstants.NAME
			});
			Strings.Add(new string[]
			{
				temp + ".DESC",
				MConstants.DESC
			});
			Strings.Add(new string[]
			{
				temp + ".EFFECT",
				"이 시설은 다이아몬드를 만들기 위해 존재합니다."
			});
			ModUtil.AddBuildingToPlanScreen("Utilities", "DiamondCompressor");
		}
	}
}
