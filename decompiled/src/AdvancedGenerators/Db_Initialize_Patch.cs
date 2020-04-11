using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace AdvancedGeneratos
{
	// Token: 0x02000007 RID: 7
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_Initialize_Patch
	{
		// Token: 0x06000013 RID: 19 RVA: 0x0000278B File Offset: 0x0000098B
		public static void Prefix()
		{
			Db_Initialize_Patch.Add("AdvancedPowerRegulation", "RefinedCarbonGenerator");
			Db_Initialize_Patch.Add("Plastics", "NaphthaGenerator");
			Db_Initialize_Patch.AddRange("RenewableEnergy", new string[]
			{
				"ThermoelectricGenerator",
				"EcoFriendlyMethaneGenerator"
			});
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000027CC File Offset: 0x000009CC
		private static void Add(string group, string id)
		{
			List<string> tech = new List<string>(Techs.TECH_GROUPING[group])
			{
				id
			};
			Techs.TECH_GROUPING[group] = tech.ToArray();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002804 File Offset: 0x00000A04
		private static void AddRange(string g, params string[] ids)
		{
			List<string> tech = new List<string>(Techs.TECH_GROUPING[g]);
			tech.AddRange(ids);
			Techs.TECH_GROUPING[g] = tech.ToArray();
		}
	}
}
