using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace EasyFarming
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(Db), "Initialize")]
	public static class DbTechPatcher
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002200 File Offset: 0x00000400
		public static void Prefix()
		{
			DbTechPatcher.Add("Agriculture", "WildFarmTile");
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002214 File Offset: 0x00000414
		private static void Add(string group, string ids)
		{
			List<string> tech = new List<string>(Techs.TECH_GROUPING[group])
			{
				ids
			};
			Techs.TECH_GROUPING[group] = tech.ToArray();
		}
	}
}
