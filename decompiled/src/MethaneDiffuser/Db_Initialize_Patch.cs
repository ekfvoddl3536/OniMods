using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace SulfurDeoxydizer
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002217 File Offset: 0x00000417
		public static void Prefix()
		{
			Db_Initialize_Patch.Add("ImprovedOxygen", "SulfurDeoxidizer");
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002228 File Offset: 0x00000428
		private static void Add(string group, string id)
		{
			List<string> tech = new List<string>(Techs.TECH_GROUPING[group])
			{
				id
			};
			Techs.TECH_GROUPING[group] = tech.ToArray();
		}
	}
}
