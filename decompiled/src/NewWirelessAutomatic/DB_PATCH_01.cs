using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace NewWirelessAutomatic
{
	// Token: 0x02000007 RID: 7
	[HarmonyPatch(typeof(Db), "Initialize")]
	public class DB_PATCH_01
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002320 File Offset: 0x00000520
		public static void Prefix()
		{
			List<string> tech = new List<string>(Techs.TECH_GROUPING["DupeTrafficControl"])
			{
				"WirelessSignalEmitter",
				"WirelessSignalReceiver"
			};
			Techs.TECH_GROUPING["DupeTrafficControl"] = tech.ToArray();
		}
	}
}
