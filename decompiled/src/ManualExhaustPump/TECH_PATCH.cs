using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace ManualExhaustPump
{
	// Token: 0x02000007 RID: 7
	[HarmonyPatch(typeof(Db), "Initialize")]
	public class TECH_PATCH
	{
		// Token: 0x06000013 RID: 19 RVA: 0x0000256D File Offset: 0x0000076D
		public static void Prefix()
		{
			TECH_PATCH.Add("GasPiping", "ManualGasPump");
			TECH_PATCH.Adds("LiquidPiping", new string[]
			{
				"ManualLiquidPump",
				"LiquidBottleCharger"
			});
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000025A0 File Offset: 0x000007A0
		private static void Add(string tech, string id)
		{
			List<string> vs = new List<string>(Techs.TECH_GROUPING[tech])
			{
				id
			};
			Techs.TECH_GROUPING[tech] = vs.ToArray();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000025D8 File Offset: 0x000007D8
		private static void Adds(string tech, params string[] ids)
		{
			List<string> vs = new List<string>(Techs.TECH_GROUPING[tech]);
			vs.AddRange(ids);
			Techs.TECH_GROUPING[tech] = vs.ToArray();
		}
	}
}
