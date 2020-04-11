using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace SupportPackages
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(Db), "Initialize")]
	public class DB_PATCH
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002228 File Offset: 0x00000428
		public static void Prefix()
		{
			DB_PATCH.AddTech("SmartStorage", "AnotherSpaceStorage");
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000223C File Offset: 0x0000043C
		private static void AddTech(string techgroup, string id)
		{
			List<string> t = new List<string>(Techs.TECH_GROUPING[techgroup])
			{
				id
			};
			Techs.TECH_GROUPING[techgroup] = t.ToArray();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002274 File Offset: 0x00000474
		private static void AddTechs(string techgroup, params string[] id)
		{
			List<string> t = new List<string>(Techs.TECH_GROUPING[techgroup]);
			t.AddRange(id);
			Techs.TECH_GROUPING[techgroup] = t.ToArray();
		}
	}
}
