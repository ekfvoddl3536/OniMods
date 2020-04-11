using System;
using Harmony;
using STRINGS;
using TUNING;
using UnityEngine;

namespace EasyFarming
{
	// Token: 0x02000007 RID: 7
	[HarmonyPatch(typeof(PrickleFlowerConfig), "CreatePrefab")]
	public static class PrickleFlowerCreatePB_PATCH_02
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002260 File Offset: 0x00000460
		public static bool Prefix(ref GameObject __result)
		{
			__result = EntityTemplates.CreatePlacedEntity("PrickleFlower", CREATURES.SPECIES.PRICKLEFLOWER.NAME, CREATURES.SPECIES.PRICKLEFLOWER.DESC, 1f, Assets.GetAnim("bristleblossom_kanim"), "idle_empty", 21, 1, 2, DECOR.BONUS.TIER3, default(EffectorValues), 976099455, null, 293f);
			EntityTemplates.ExtendEntityToBasicPlant(__result, 218.15f, 278.15f, 303.15f, 398.15f, CON.air, true, 0f, 0.15f, PrickleFruitConfig.ID, true, true, true, true, 2400f);
			EntityTemplates.ExtendPlantToIrrigated(__result, new PlantElementAbsorber.ConsumeInfo(GameTags.Water, 0.00125f));
			EntityTemplateExtensions.AddOrGet<StandardCropPlant>(__result);
			DiseaseDropper.Def def = EntityTemplateExtensions.AddOrGetDef<DiseaseDropper.Def>(__result);
			def.diseaseIdx = Db.Get().Diseases.GetIndex(Db.Get().Diseases.PollenGerms.id);
			def.singleEmitQuantity = 1000000;
			EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(__result, 2, "PrickleFlowerSeed", CREATURES.SPECIES.SEEDS.PRICKLEFLOWER.NAME, CREATURES.SPECIES.SEEDS.PRICKLEFLOWER.DESC, Assets.GetAnim("seed_bristleblossom_kanim"), "object", 0, CON.CP, 0, default(Tag), 2, CREATURES.SPECIES.PRICKLEFLOWER.DOMESTICATEDDESC, 0, 0.25f, 0.25f, null, "", false), "PrickleFlower_preview", Assets.GetAnim("bristleblossom_kanim"), "place", 1, 2);
			SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_harvest", NOISE_POLLUTION.CREATURES.TIER3);
			SoundEventVolumeCache.instance.AddVolume("bristleblossom_kanim", "PrickleFlower_grow", NOISE_POLLUTION.CREATURES.TIER3);
			return false;
		}

		// Token: 0x04000001 RID: 1
		private const string anim = "bristleblossom_kanim";
	}
}
