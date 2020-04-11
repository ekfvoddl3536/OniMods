using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Harmony;
using STRINGS;
using TUNING;
using UnityEngine;

namespace EasyFarming
{
	// Token: 0x0200000A RID: 10
	[HarmonyPatch(typeof(SeaLettuceConfig), "CreatePrefab")]
	public static class SeaLettuceConfigPatch
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002808 File Offset: 0x00000A08
		public static bool Prefix(ref GameObject __result)
		{
			__result = EntityTemplates.CreatePlacedEntity(SeaLettuceConfig.ID, CREATURES.SPECIES.SEALETTUCE.NAME, CREATURES.SPECIES.SEALETTUCE.DESC, 1f, Assets.GetAnim("sea_lettuce_kanim"), "idle_empty", 18, 1, 2, DECOR.BONUS.TIER0, default(EffectorValues), 976099455, null, 308.15f);
			GameObject gameObject = __result;
			float num = 248.15f;
			float num2 = 283.15f;
			float num3 = 338.15f;
			float num4 = 398.15f;
			SimHashes[] array = new SimHashes[3];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.5C6C88BCCD9217AFDCC69A8144F872B8E263854258DD3DFAFC725B216B60E1C3).FieldHandle);
			EntityTemplates.ExtendEntityToBasicPlant(gameObject, num, num2, num3, num4, array, false, 0f, 100f, "Lettuce", true, true, true, true, 2400f);
			GameObject gameObject2 = __result;
			PlantElementAbsorber.ConsumeInfo[] array2 = new PlantElementAbsorber.ConsumeInfo[1];
			int num5 = 0;
			PlantElementAbsorber.ConsumeInfo consumeInfo = default(PlantElementAbsorber.ConsumeInfo);
			consumeInfo.tag = GameTagExtensions.CreateTag(1911997537);
			consumeInfo.massConsumptionRate = 0.000166666665f;
			array2[num5] = consumeInfo;
			EntityTemplates.ExtendPlantToIrrigated(gameObject2, array2);
			GameObject gameObject3 = __result;
			PlantElementAbsorber.ConsumeInfo[] array3 = new PlantElementAbsorber.ConsumeInfo[1];
			int num6 = 0;
			consumeInfo = default(PlantElementAbsorber.ConsumeInfo);
			consumeInfo.tag = GameTagExtensions.CreateTag(-839728230);
			consumeInfo.massConsumptionRate = 1.66666669E-05f;
			array3[num6] = consumeInfo;
			EntityTemplates.ExtendPlantToFertilizable(gameObject3, array3);
			DrowningMonitor component = __result.GetComponent<DrowningMonitor>();
			component.canDrownToDeath = false;
			component.livesUnderWater = true;
			EntityTemplateExtensions.AddOrGet<StandardCropPlant>(__result);
			EntityTemplateExtensions.AddOrGet<KAnimControllerBase>(__result).randomiseLoopedOffset = true;
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(__result);
			EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(__result, 2, SeaLettuceConfig.ID + "Seed", CREATURES.SPECIES.SEEDS.SEALETTUCE.NAME, CREATURES.SPECIES.SEEDS.SEALETTUCE.DESC, Assets.GetAnim("seed_sealettuce_kanim"), "object", 0, new List<Tag>
			{
				GameTags.WaterSeed
			}, 0, default(Tag), 1, CREATURES.SPECIES.SEALETTUCE.DOMESTICATEDDESC, 0, 0.2f, 0.2f, null, string.Empty, true), SeaLettuceConfig.ID + "_preview", Assets.GetAnim("sea_lettuce_kanim"), "place", 1, 2);
			SoundEventVolumeCache.instance.AddVolume("sea_lettuce_kanim", SeaLettuceConfig.ID + "_harvest", NOISE_POLLUTION.CREATURES.TIER3);
			SoundEventVolumeCache.instance.AddVolume("sea_lettuce_kanim", SeaLettuceConfig.ID + "_grow", NOISE_POLLUTION.CREATURES.TIER3);
			return false;
		}

		// Token: 0x04000005 RID: 5
		private const string anim = "sea_lettuce_kanim";

		// Token: 0x04000006 RID: 6
		private const float frate = 1.66666669E-05f;

		// Token: 0x04000007 RID: 7
		private const float lqrate = 0.000166666665f;
	}
}
