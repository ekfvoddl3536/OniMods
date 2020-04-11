using System;
using Harmony;
using STRINGS;
using TUNING;
using UnityEngine;

namespace EasyFarming
{
	// Token: 0x0200000B RID: 11
	[HarmonyPatch(typeof(BeanPlantConfig), "CreatePrefab")]
	public static class BeanPlantConfigPatch
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002A44 File Offset: 0x00000C44
		public static bool Prefix(ref GameObject __result)
		{
			__result = EntityTemplates.CreatePlacedEntity("BeanPlant", CREATURES.SPECIES.BEAN_PLANT.NAME, CREATURES.SPECIES.BEAN_PLANT.DESC, 1f, Assets.GetAnim("beanplant_kanim"), "idle_empty", 21, 1, 2, DECOR.BONUS.TIER1, default(EffectorValues), 976099455, null, 258.15f);
			EntityTemplates.ExtendEntityToBasicPlant(__result, 198.15f, 248.15f, 303.15f, 398.15f, CON.air, true, 0f, 0.15f, "BeanPlantSeed", true, true, true, true, 2400f);
			GameObject gameObject = __result;
			PlantElementAbsorber.ConsumeInfo[] array = new PlantElementAbsorber.ConsumeInfo[1];
			int num = 0;
			PlantElementAbsorber.ConsumeInfo consumeInfo = default(PlantElementAbsorber.ConsumeInfo);
			consumeInfo.tag = GameTags.Dirt;
			consumeInfo.massConsumptionRate = 0.000833333354f;
			array[num] = consumeInfo;
			EntityTemplates.ExtendPlantToFertilizable(gameObject, array);
			__result.GetComponent<UprootedMonitor>().monitorCell = new CellOffset(0, -1);
			EntityTemplateExtensions.AddOrGet<StandardCropPlant>(__result);
			GameObject gameObject2 = EntityTemplates.CreateAndRegisterSeedForPlant(__result, 1, "BeanPlantSeed", CREATURES.SPECIES.SEEDS.BEAN_PLANT.NAME, CREATURES.SPECIES.SEEDS.BEAN_PLANT.DESC, Assets.GetAnim("seed_beanplant_kanim"), "object", 1, CON.CP, 0, default(Tag), 4, CREATURES.SPECIES.BEAN_PLANT.DOMESTICATEDDESC, 1, 0.6f, 0.3f, null, "", false);
			EntityTemplates.ExtendEntityToFood(gameObject2, FOOD.FOOD_TYPES.BEAN);
			EntityTemplates.CreateAndRegisterPreviewForPlant(gameObject2, "BeanPlant_preview", Assets.GetAnim("beanplant_kanim"), "place", 1, 2);
			return false;
		}

		// Token: 0x04000008 RID: 8
		private const string anim = "beanplant_kanim";

		// Token: 0x04000009 RID: 9
		private const float rate = 0.000833333354f;
	}
}
