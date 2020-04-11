using System;
using Harmony;

namespace NuclearReaction
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch("LoadGeneratedBuildings")]
	public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000023C4 File Offset: 0x000005C4
		public static void Prefix()
		{
			string temp = "STRINGS.BUILDINGS.PREFABS." + Constans.FSPP_ID_UPPER;
			Strings.Add(new string[]
			{
				temp + ".NAME",
				Constans.FSPP_NAME
			});
			Strings.Add(new string[]
			{
				temp + ".DESC",
				Constans.FSPP_DESC
			});
			Strings.Add(new string[]
			{
				temp + ".EFFECT",
				Constans.FSPP_EFFC
			});
			ModUtil.AddBuildingToPlanScreen("Power", "FusionPowerPlant");
		}
	}
}
