using System;
using System.Collections.Generic;
using Harmony;

namespace SupportPackage
{
	// Token: 0x02000014 RID: 20
	[HarmonyPatch(typeof(GeneratedBuildings))]
	[HarmonyPatch("LoadGeneratedBuildings")]
	public static class Patchs
	{
		// Token: 0x0600004D RID: 77 RVA: 0x000039C0 File Offset: 0x00001BC0
		public static void Prefix()
		{
			string str = "STRINGS.BUILDINGS.PREFABS.";
			Patchs.SetString(str + IN_Constants.EasyElectrolyzer.ID_UPPER, IN_Constants.EasyElectrolyzer.NAME, IN_Constants.EasyElectrolyzer.DESC, "개조된 전해조입니다. 재료값이 조금 비싸지만, 그만큼 가치가 있습니다.");
			Patchs.SetString(str + IN_Constants.SuperDeodorizer.ID_UPPER, IN_Constants.SuperDeodorizer.NAME, IN_Constants.SuperDeodorizer.DESC, IN_Constants.SuperDeodorizer.EFFC);
			Patchs.SetString(str + IN_Constants.TwoInOneGrill.ID_UPPER, IN_Constants.TwoInOneGrill.NAME, IN_Constants.TwoInOneGrill.DESC, "냉장고 + 전기 그릴");
			Patchs.SetString(str + IN_Constants.WastewaterSterilizer.ID_UPPER, IN_Constants.WastewaterSterilizer.NAME, IN_Constants.WastewaterSterilizer.DESC, "폐수가 세균없는 순수한 물이 될 수 있는 가장 간단한 방법입니다.");
			Patchs.SetString(str + IN_Constants.MagicTile.ID_UPPER, IN_Constants.MagicTile.NAME, IN_Constants.MagicTile.DESC, "액체는 투과되지만, 기체 흐름은 막습니다.");
			Patchs.SetString(str + IN_Constants.SuperMiner.ID_UPPER, IN_Constants.SuperMiner.NAME, IN_Constants.SuperMiner.DESC, "범위내 있는 모든 광물을 자동으로 채굴합니다.\n\n받은 액체는 가열될 수도 있습니다.");
			Patchs.SetString(str + IN_Constants.SolidElementSensor.ID_UPPER, IN_Constants.SolidElementSensor.NAME, IN_Constants.SolidElementSensor.DESC, "고체 수송 자동화");
			Patchs.SetString(str + IN_Constants.LargeStorage.ID_UPPER, IN_Constants.LargeStorage.NAME, "바닥에 남겨진 자원은 치우지 않으면 \"잔해\"가 되고 장식이 낮아집니다.", "선택한 자원을 운반합니다.");
			Patchs.SetString(str + IN_Constants.ExtremeLargeStorage.ID_UPPER, IN_Constants.ExtremeLargeStorage.NAME, "바닥에 남겨진 자원은 치우지 않으면 \"잔해\"가 되고 장식이 낮아집니다.", "선택한 자원을 운반합니다.");
			Patchs.SetString(str + IN_Constants.OrganicDeoxidizer.ID_UPPER, IN_Constants.OrganicDeoxidizer.NAME, "흙과 전기를 소모하여, 약간의 산소를 만듭니다.", "매우 비효율적으로 흙을 산소로 만듭니다.");
			Patchs.SetString(str + IN_Constants.DirtyOrganicFilter.ID_UPPER, IN_Constants.DirtyOrganicFilter.NAME, IN_Constants.DirtyOrganicFilter.DESC, IN_Constants.DirtyOrganicFilter.EFFC);
			Patchs.SetString(str + IN_Constants.BleachedStoneMaker.ID_UPPER, IN_Constants.BleachedStoneMaker.NAME, IN_Constants.BleachedStoneMaker.DESC, IN_Constants.BleachedStoneMaker.EFFC);
			Patchs.SetString(str + IN_Constants.H2OSynthesizer.ID_UPPER, IN_Constants.H2OSynthesizer.NAME, IN_Constants.H2OSynthesizer.DESC, IN_Constants.H2OSynthesizer.EFFC);
			Patchs.SetString(str + IN_Constants.CO2ConverterEZ.ID_UPPER, IN_Constants.CO2ConverterEZ.NAME, IN_Constants.CO2ConverterEZ.DESC, IN_Constants.CO2ConverterEZ.EFFC);
			ModUtil.AddBuildingToPlanScreen("Oxygen", "SuperDeodorizer");
			ModUtil.AddBuildingToPlanScreen("Oxygen", "EasyElectrolyzer");
			ModUtil.AddBuildingToPlanScreen("Food", "TwoInOneGrill");
			ModUtil.AddBuildingToPlanScreen("Refining", "WastewaterSterilizer");
			ModUtil.AddBuildingToPlanScreen("Base", "MagicTile");
			ModUtil.AddBuildingToPlanScreen("Conveyance", "SuperMiner");
			ModUtil.AddBuildingToPlanScreen("Conveyance", "SolidElementSensor");
			ModUtil.AddBuildingToPlanScreen("Base", "LargeStorage");
			ModUtil.AddBuildingToPlanScreen("Base", "ExtremeLargeStorage");
			ModUtil.AddBuildingToPlanScreen("Oxygen", "OrganicDeoxidizer");
			ModUtil.AddBuildingToPlanScreen("Utilities", "DirtyOrganicFilter");
			ModUtil.AddBuildingToPlanScreen("Utilities", "BleachedStoneMaker");
			ModUtil.AddBuildingToPlanScreen("Oxygen", "CO2ConverterEZ");
			ModUtil.AddBuildingToPlanScreen("Utilities", "H2OSynthesizer");
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003D2C File Offset: 0x00001F2C
		private static void SetString(string fullid, string name, string desc, string effect)
		{
			Strings.Add(new string[]
			{
				fullid + ".NAME",
				name
			});
			Strings.Add(new string[]
			{
				fullid + ".DESC",
				desc
			});
			Strings.Add(new string[]
			{
				fullid + ".EFFECT",
				effect
			});
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003D90 File Offset: 0x00001F90
		public static void Postfix()
		{
			List<BuildingDef> list = Assets.BuildingDefs;
			for (int x = 0; x < list.Count; x++)
			{
				BuildingDef def = list[x];
				if (def.GeneratorWattageRating > 0f)
				{
					def.GeneratorWattageRating *= 2f;
				}
				if (def.EnergyConsumptionWhenActive > 0f)
				{
					def.EnergyConsumptionWhenActive /= 2f;
				}
				if (def.ExhaustKilowattsWhenActive > 0f)
				{
					def.ExhaustKilowattsWhenActive /= 2f;
				}
				if (def.SelfHeatKilowattsWhenActive > 0f)
				{
					def.SelfHeatKilowattsWhenActive /= 2f;
				}
			}
		}
	}
}
