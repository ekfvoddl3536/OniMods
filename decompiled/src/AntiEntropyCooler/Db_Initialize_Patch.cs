using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace AntiEntropyCooler
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000022F0 File Offset: 0x000004F0
		public static void Prefix()
		{
			List<string> temp = new List<string>(Techs.TECH_GROUPING["HVAC"])
			{
				"AntiEntropyCooler"
			};
			Techs.TECH_GROUPING["HVAC"] = temp.ToArray();
		}
	}
}
