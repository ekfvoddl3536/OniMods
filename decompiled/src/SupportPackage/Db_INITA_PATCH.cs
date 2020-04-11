using System;
using System.Collections.Generic;
using Database;
using Harmony;

namespace SupportPackage
{
	// Token: 0x02000015 RID: 21
	[HarmonyPatch(typeof(Db))]
	[HarmonyPatch("Initialize")]
	public static class Db_INITA_PATCH
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00003E40 File Offset: 0x00002040
		public static void Prefix()
		{
			Db_INITA_PATCH.AddTech("Agriculture", "TwoInOneGrill");
			Db_INITA_PATCH.AddTech("DirectedAirStreams", "SuperDeodorizer");
			Db_INITA_PATCH.AddTechs("ImprovedGasPiping", new string[]
			{
				"MagicTile",
				"BleachedStoneMaker",
				"H2OSynthesizer"
			});
			Db_INITA_PATCH.AddTechs("ImprovedOxygen", new string[]
			{
				"EasyElectrolyzer",
				"DirtyOrganicFilter",
				"CO2ConverterEZ"
			});
			Db_INITA_PATCH.AddTechs("SolidTransport", new string[]
			{
				"SuperMiner",
				"SolidElementSensor"
			});
			Db_INITA_PATCH.AddTechs("SmartStorage", new string[]
			{
				"LargeStorage",
				"ExtremeLargeStorage"
			});
			Db_INITA_PATCH.AddTechs("AdvancedFiltration", new string[]
			{
				"WastewaterSterilizer",
				"OrganicDeoxidizer"
			});
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003F1C File Offset: 0x0000211C
		private static void AddTech(string techgroup, string id)
		{
			List<string> t = new List<string>(Techs.TECH_GROUPING[techgroup])
			{
				id
			};
			Techs.TECH_GROUPING[techgroup] = t.ToArray();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003F54 File Offset: 0x00002154
		private static void AddTechs(string techgroup, params string[] id)
		{
			List<string> t = new List<string>(Techs.TECH_GROUPING[techgroup]);
			t.AddRange(id);
			Techs.TECH_GROUPING[techgroup] = t.ToArray();
		}
	}
}
