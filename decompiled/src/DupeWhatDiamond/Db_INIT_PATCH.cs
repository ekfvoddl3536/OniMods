using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace DupeWhatDiamond
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_INIT_PATCH
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000024A0 File Offset: 0x000006A0
		public static void Prefix()
		{
			List<string> t = new List<string>(Techs.TECH_GROUPING["Smelting"])
			{
				"DiamondCompressor"
			};
			Techs.TECH_GROUPING["Smelting"] = t.ToArray();
		}
	}
}
