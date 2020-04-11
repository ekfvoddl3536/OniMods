using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace SuperSpeedRunner
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(Db), "Initialize")]
	public static class Db_Initialize_Patch
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002149 File Offset: 0x00000349
		public static void Prefix()
		{
			Db_Initialize_Patch.Adds("Plastics", new string[]
			{
				"RunnerTile",
				"RunnerLadder"
			});
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000216C File Offset: 0x0000036C
		private static void Adds(string group, params string[] ids)
		{
			List<string> tech = new List<string>(Techs.TECH_GROUPING[group]);
			tech.AddRange(ids);
			Techs.TECH_GROUPING[group] = tech.ToArray();
		}
	}
}
