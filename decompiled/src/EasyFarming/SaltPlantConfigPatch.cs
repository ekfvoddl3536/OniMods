using System;
using System.Collections.Generic;
using Harmony;
using STRINGS;
using TUNING;
using UnityEngine;

namespace EasyFarming
{
	// Token: 0x0200000C RID: 12
	[HarmonyPatch(typeof(SaltPlantConfig), "CreatePrefab")]
	public static class SaltPlantConfigPatch
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002BC4 File Offset: 0x00000DC4
		public static bool Prefix(ref GameObject __result)
		{
			__result = EntityTemplates.CreatePlacedEntity("SaltPlant", CREATURES.SPECIES.SALTPLANT.NAME, CREATURES.SPECIES.SALTPLANT.DESC, 2f, Assets.GetAnim("saltplant_kanim"), "idle_empty", 21, 1, 2, DECOR.BONUS.TIER1, default(EffectorValues), 976099455, new List<Tag>
			{
				GameTags.Hanging
			}, 258.15f);
			EntityTemplates.MakeHangingOffsets(__result, 1, 2);
			EntityTemplates.ExtendEntityToBasicPlant(__result, 198.15f, 248.15f, 323.15f, 393.15f, CON.air, true, 0f, 0.15f, 381665462.ToString(), true, true, true, true, 2400f);
			GameObject gameObject = __result;
			PlantElementAbsorber.ConsumeInfo[] array = new PlantElementAbsorber.ConsumeInfo[1];
			int num = 0;
			PlantElementAbsorber.ConsumeInfo consumeInfo = default(PlantElementAbsorber.ConsumeInfo);
			consumeInfo.tag = GameTags.DirtyWater;
			consumeInfo.massConsumptionRate = 1.66666669E-05f;
			array[num] = consumeInfo;
			EntityTemplates.ExtendPlantToIrrigated(gameObject, array);
			GameObject gameObject2 = __result;
			PlantElementAbsorber.ConsumeInfo[] array2 = new PlantElementAbsorber.ConsumeInfo[1];
			int num2 = 0;
			consumeInfo = default(PlantElementAbsorber.ConsumeInfo);
			consumeInfo.tag = GameTags.Dirt;
			consumeInfo.massConsumptionRate = 0.0116666667f;
			array2[num2] = consumeInfo;
			EntityTemplates.ExtendPlantToFertilizable(gameObject2, array2);
			EntityTemplateExtensions.AddOrGet<StandardCropPlant>(__result);
			__result.GetComponent<UprootedMonitor>().monitorCell = new CellOffset(0, 1);
			EntityTemplates.MakeHangingOffsets(EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(__result, 2, "SaltPlantSeed", CREATURES.SPECIES.SEEDS.SALTPLANT.NAME, CREATURES.SPECIES.SEEDS.SALTPLANT.DESC, Assets.GetAnim("seed_saltplant_kanim"), "object", 1, CON.CP, 2, default(Tag), 4, CREATURES.SPECIES.SALTPLANT.DOMESTICATEDDESC, 0, 0.35f, 0.35f, null, "", false), "SaltPlant_preview", Assets.GetAnim("saltplant_kanim"), "place", 1, 2), 1, 2);
			return false;
		}

		// Token: 0x0400000A RID: 10
		private const string anim = "saltplant_kanim";

		// Token: 0x0400000B RID: 11
		private const float rate = 0.0116666667f;

		// Token: 0x0400000C RID: 12
		private const float lqrate = 1.66666669E-05f;
	}
}
