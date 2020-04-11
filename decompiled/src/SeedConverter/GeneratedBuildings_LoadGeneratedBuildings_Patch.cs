using System;
using Harmony;

namespace SeedConverter
{
	// Token: 0x02000003 RID: 3
	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch("LoadGeneratedBuildings")]
	public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
	{
		// Token: 0x06000004 RID: 4 RVA: 0x0000210C File Offset: 0x0000030C
		public static void Prefix()
		{
			GeneratedBuildings_LoadGeneratedBuildings_Patch.SetString("STRINGS.BUILDINGS.PREFABS." + Consts.ID_UPPER, Consts.NAME, Consts.DESC, Consts.EFFC);
			ModUtil.AddBuildingToPlanScreen("Food", "SeedConverter");
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002160 File Offset: 0x00000360
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
