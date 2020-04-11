using System;
using System.Collections.Generic;
using Harmony;
using STRINGS;
using TUNING;
using UnityEngine;

namespace EasyFarming
{
	// Token: 0x0200000D RID: 13
	[HarmonyPatch(typeof(SpiceVineConfig), "CreatePrefab")]
	public static class SpiceVineConfigPatch
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002D9C File Offset: 0x00000F9C
		public static bool Prefix(ref GameObject __result)
		{
			__result = EntityTemplates.CreatePlacedEntity("SpiceVine", CREATURES.SPECIES.SPICE_VINE.NAME, CREATURES.SPECIES.SPICE_VINE.DESC, 2f, Assets.GetAnim("vinespicenut_kanim"), "idle_empty", 21, 1, 3, DECOR.BONUS.TIER1, default(EffectorValues), 976099455, new List<Tag>
			{
				GameTags.Hanging
			}, 320f);
			EntityTemplates.MakeHangingOffsets(__result, 1, 3);
			EntityTemplates.ExtendEntityToBasicPlant(__result, 258.15f, 273.15f, 358.15f, 448.15f, CON.air, true, 0f, 0.15f, SpiceNutConfig.ID, true, true, true, true, 2400f);
			GameObject gameObject = __result;
			PlantElementAbsorber.ConsumeInfo[] array = new PlantElementAbsorber.ConsumeInfo[1];
			int num = 0;
			PlantElementAbsorber.ConsumeInfo consumeInfo = default(PlantElementAbsorber.ConsumeInfo);
			consumeInfo.tag = GameTags.DirtyWater;
			consumeInfo.massConsumptionRate = 0.00166666671f;
			array[num] = consumeInfo;
			EntityTemplates.ExtendPlantToIrrigated(gameObject, array);
			__result.GetComponent<UprootedMonitor>().monitorCell = new CellOffset(0, 1);
			EntityTemplateExtensions.AddOrGet<StandardCropPlant>(__result);
			EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(__result, 2, "SpiceVineSeed", CREATURES.SPECIES.SEEDS.SPICE_VINE.NAME, CREATURES.SPECIES.SEEDS.SPICE_VINE.DESC, Assets.GetAnim("seed_spicenut_kanim"), "object", 1, CON.CP, 2, default(Tag), 4, CREATURES.SPECIES.SPICE_VINE.DOMESTICATEDDESC, 0, 0.3f, 0.3f, null, "", false), "SpiceVine_preview", Assets.GetAnim("vinespicenut_kanim"), "place", 1, 3), 1, 3);
			return false;
		}

		// Token: 0x0400000D RID: 13
		private const string anim = "vinespicenut_kanim";

		// Token: 0x0400000E RID: 14
		private const float rate = 0.00166666671f;
	}
}
