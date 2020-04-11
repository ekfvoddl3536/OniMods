using System;
using Harmony;

namespace SulfurDeoxydizer
{
	// Token: 0x02000003 RID: 3
	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch("LoadGeneratedBuildings")]
	public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002174 File Offset: 0x00000374
		public static void Prefix()
		{
			string temp = "STRINGS.BUILDINGS.PREFABS." + Constans.SUFDE_ID_UPPER;
			Strings.Add(new string[]
			{
				temp + ".NAME",
				Constans.SUFDE_Displyname
			});
			Strings.Add(new string[]
			{
				temp + ".DESC",
				Constans.SUFDE_Descript
			});
			Strings.Add(new string[]
			{
				temp + ".EFFECT",
				Constans.SUFDE_Effect
			});
			ModUtil.AddBuildingToPlanScreen("Utilities", "SulfurDeoxidizer");
		}
	}
}
