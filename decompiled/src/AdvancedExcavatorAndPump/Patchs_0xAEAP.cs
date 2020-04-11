using System;
using Harmony;

namespace AdvancedExcavatorAndPump
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch("LoadGeneratedBuildings")]
	public class Patchs_0xAEAP
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000025B5 File Offset: 0x000007B5
		public static void Prefix()
		{
			Patchs_0xAEAP.SetString(Constants.ID_UPPER, Constants.NAME, "최대 3x100 칸을 채굴할 수 있으며, 채굴 중 진로를 방해하는 액체를 퍼 올립니다.", "시험적.. 버그 많음");
			ModUtil.AddBuildingToPlanScreen("Base", "AdvancedExcavatorAndPump");
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000025EC File Offset: 0x000007EC
		private static void SetString(string idup, string name, string desc, string effect)
		{
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS." + idup + ".NAME",
				name
			});
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS." + idup + ".DESC",
				desc
			});
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS." + idup + ".EFFECT",
				effect
			});
		}
	}
}
