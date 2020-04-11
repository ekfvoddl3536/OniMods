using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace NuclearReaction
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002467 File Offset: 0x00000667
		public static void Prefix()
		{
			Db_Initialize_Patch.Add("RenewableEnergy", "FusionPowerPlant");
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002478 File Offset: 0x00000678
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
