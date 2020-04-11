using System;
using Harmony;

namespace AntiEntropyCooler
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch("LoadGeneratedBuildings")]
	public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002254 File Offset: 0x00000454
		public static void Prefix()
		{
			string temp = "STRINGS.BUILDINGS.PREFABS." + Consts.ID_UPPER;
			Strings.Add(new string[]
			{
				temp + ".NAME",
				Consts.NAME
			});
			Strings.Add(new string[]
			{
				temp + ".DESC",
				"이 장치는 듀플이 유적에서 발견한 항엔트로피열무효화장치의 설계도를 카피하여 개발했습니다.\n튜닝을 통해 이 장치는 기존의 항엔트로피열무효화장치의 성능을 압도합니다!"
			});
			Strings.Add(new string[]
			{
				temp + ".EFFECT",
				"항엔트로피열무효화장치는 오래전, 이 지구를 창조한 누군가가 개발한 것입니다."
			});
			ModUtil.AddBuildingToPlanScreen("Utilities", "AntiEntropyCooler");
		}
	}
}
