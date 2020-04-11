using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace LiquidMethaneEngine
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		// Token: 0x06000009 RID: 9 RVA: 0x0000234B File Offset: 0x0000054B
		public static void Prefix()
		{
			Db_Initialize_Patch.AddRange("BasicRocketry", "LiquidMethaneEngine");
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000235C File Offset: 0x0000055C
		private static void AddRange(string group, string ids)
		{
			List<string> tech = new List<string>(Techs.TECH_GROUPING[group])
			{
				ids
			};
			Techs.TECH_GROUPING[group] = tech.ToArray();
		}
	}
}
