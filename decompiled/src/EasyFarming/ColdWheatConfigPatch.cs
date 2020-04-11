using System;
using Harmony;
using STRINGS;
using TUNING;
using UnityEngine;

namespace EasyFarming
{
	// Token: 0x02000009 RID: 9
	[HarmonyPatch(typeof(ColdWheatConfig), "CreatePrefab")]
	public static class ColdWheatConfigPatch
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002634 File Offset: 0x00000834
		public static bool Prefix(ref GameObject __result)
		{
			__result = EntityTemplates.CreatePlacedEntity("ColdWheat", CREATURES.SPECIES.COLDWHEAT.NAME, CREATURES.SPECIES.COLDWHEAT.DESC, 1f, Assets.GetAnim("coldwheat_kanim"), "idle_empty", 21, 1, 1, DECOR.BONUS.TIER1, default(EffectorValues), 976099455, null, 293f);
			EntityTemplates.ExtendEntityToBasicPlant(__result, 118.15f, 218.15f, 278.15f, 358.15f, CON.air, true, 0f, 0.15f, "ColdWheatSeed", true, true, true, true, 2400f);
			GameObject gameObject = __result;
			PlantElementAbsorber.ConsumeInfo[] array = new PlantElementAbsorber.ConsumeInfo[1];
			int num = 0;
			PlantElementAbsorber.ConsumeInfo consumeInfo = default(PlantElementAbsorber.ConsumeInfo);
			consumeInfo.tag = GameTags.Dirt;
			consumeInfo.massConsumptionRate = 0.000833333354f;
			array[num] = consumeInfo;
			EntityTemplates.ExtendPlantToFertilizable(gameObject, array);
			GameObject gameObject2 = __result;
			PlantElementAbsorber.ConsumeInfo[] array2 = new PlantElementAbsorber.ConsumeInfo[1];
			int num2 = 0;
			consumeInfo = default(PlantElementAbsorber.ConsumeInfo);
			consumeInfo.tag = GameTags.Water;
			consumeInfo.massConsumptionRate = 0.000833333354f;
			array2[num2] = consumeInfo;
			EntityTemplates.ExtendPlantToIrrigated(gameObject2, array2);
			EntityTemplateExtensions.AddOrGet<StandardCropPlant>(__result);
			GameObject gameObject3 = EntityTemplates.CreateAndRegisterSeedForPlant(__result, 1, "ColdWheatSeed", CREATURES.SPECIES.SEEDS.COLDWHEAT.NAME, CREATURES.SPECIES.SEEDS.COLDWHEAT.DESC, Assets.GetAnim("seed_coldwheat_kanim"), "object", 1, CON.CP, 0, default(Tag), 2, CREATURES.SPECIES.COLDWHEAT.DOMESTICATEDDESC, 0, 0.2f, 0.2f, null, string.Empty, true);
			EntityTemplates.ExtendEntityToFood(gameObject3, FOOD.FOOD_TYPES.COLD_WHEAT_SEED);
			EntityTemplates.CreateAndRegisterPreviewForPlant(gameObject3, "ColdWheat_preview", Assets.GetAnim("coldwheat_kanim"), "place", 1, 1);
			SoundEventVolumeCache.instance.AddVolume("coldwheat_kanim", "ColdWheat_harvest", NOISE_POLLUTION.CREATURES.TIER3);
			SoundEventVolumeCache.instance.AddVolume("coldwheat_kanim", "ColdWheat_grow", NOISE_POLLUTION.CREATURES.TIER3);
			return false;
		}

		// Token: 0x04000003 RID: 3
		private const string anim = "coldwheat_kanim";

		// Token: 0x04000004 RID: 4
		private const float rate = 0.000833333354f;
	}
}
