using System;
using Harmony;

namespace VacuumTools
{
	// Token: 0x02000003 RID: 3
	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	public class GenPatch_0x
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002132 File Offset: 0x00000332
		public static void Prefix()
		{
			GenPatch_0x.SetString("STRINGS.BUILDINGS.PREFABS." + VTCONST.AnotherVoidStorage.ID_UPPER, VTCONST.AnotherVoidStorage.NAME, "이 저장소에 저장되는 물질은 영원히 사라집니다!", "심각하게 위험한 저장소");
			ModUtil.AddBuildingToPlanScreen("Base", "AnotherVoidStorage");
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002170 File Offset: 0x00000370
		private static void SetString(string fullid, string name, string desc, string effect)
		{
			Strings.Add(new string[]
			{
				fullid + ".NAME",
				name
			});
			Strings.Add(new string[]
			{
				fullid + ".DESC",
				desc
			});
			Strings.Add(new string[]
			{
				fullid + ".EFFECT",
				effect
			});
		}
	}
}
