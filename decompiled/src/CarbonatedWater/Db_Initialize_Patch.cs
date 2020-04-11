using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace CarbonatedWater
{
	// Token: 0x02000009 RID: 9
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000027A0 File Offset: 0x000009A0
		private static void Prefix()
		{
			List<string> temp = new List<string>(Techs.TECH_GROUPING["FineDining"])
			{
				"CO2WaterGen"
			};
			Techs.TECH_GROUPING["FineDining"] = temp.ToArray();
		}
	}
}
