using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace VacuumTools
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(Db), "Initialize")]
	public class DbPatch_0x
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000021DC File Offset: 0x000003DC
		public static void Prefix()
		{
			DbPatch_0x.AddTech("SmartStorage", "AnotherVoidStorage");
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021F0 File Offset: 0x000003F0
		private static void AddTech(string techgroup, string id)
		{
			List<string> t = new List<string>(Techs.TECH_GROUPING[techgroup])
			{
				id
			};
			Techs.TECH_GROUPING[techgroup] = t.ToArray();
		}
	}
}
