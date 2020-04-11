using System;
using Harmony;
using STRINGS;
using TUNING;
using UnityEngine;

namespace EasyFarming
{
	// Token: 0x02000008 RID: 8
	[HarmonyPatch(typeof(ColdBreatherConfig), "CreatePrefab")]
	public static class ColdBreatherConfig_PATCH_01
	{
		// Token: 0x0600000A RID: 10 RVA: 0x0000240C File Offset: 0x0000060C
		public static bool Prefix(ref GameObject __result)
		{
			__result = EntityTemplates.CreatePlacedEntity("ColdBreather", CREATURES.SPECIES.COLDBREATHER.NAME, CREATURES.SPECIES.COLDBREATHER.DESC, 400f, Assets.GetAnim("coldbreather_kanim"), "grow_seed", 21, 1, 2, DECOR.BONUS.TIER1, NOISE_POLLUTION.NOISY.TIER2, 976099455, null, 293f);
			EntityTemplateExtensions.AddOrGet<ReceptacleMonitor>(__result);
			EntityTemplateExtensions.AddOrGet<EntombVulnerable>(__result);
			EntityTemplateExtensions.AddOrGet<WiltCondition>(__result);
			EntityTemplateExtensions.AddOrGet<Prioritizable>(__result);
			EntityTemplateExtensions.AddOrGet<Uprootable>(__result);
			EntityTemplateExtensions.AddOrGet<UprootedMonitor>(__result);
			EntityTemplateExtensions.AddOrGet<DrowningMonitor>(__result);
			EntityTemplateExtensions.AddOrGet<TemperatureVulnerable>(__result).Configure(213.15f, 183.15f, 368.15f, 463.15f);
			EntityTemplateExtensions.AddOrGet<OccupyArea>(__result).objectLayers = new ObjectLayer[]
			{
				1
			};
			ColdBreather coldBreather = EntityTemplateExtensions.AddOrGet<ColdBreather>(__result);
			coldBreather.deltaEmitTemperature = -5f;
			coldBreather.emitOffsetCell = new Vector3(0f, 1f);
			coldBreather.consumptionRate = 1f;
			EntityTemplateExtensions.AddOrGet<KBatchedAnimController>(__result).randomiseLoopedOffset = true;
			BuildingTemplates.CreateDefaultStorage(__result, false).showInUI = false;
			ElementConsumer elementConsumer = EntityTemplateExtensions.AddOrGet<ElementConsumer>(__result);
			elementConsumer.storeOnConsume = true;
			elementConsumer.configuration = 2;
			elementConsumer.capacityKG = 2f;
			elementConsumer.consumptionRate = 0.25f;
			elementConsumer.consumptionRadius = 1;
			elementConsumer.sampleCellOffset = Vector3.zero;
			SimTemperatureTransfer component = __result.GetComponent<SimTemperatureTransfer>();
			component.SurfaceArea = 10f;
			component.Thickness = 0.001f;
			EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(__result, 0, "ColdBreatherSeed", CREATURES.SPECIES.SEEDS.COLDBREATHER.NAME, CREATURES.SPECIES.SEEDS.COLDBREATHER.DESC, Assets.GetAnim("seed_coldbreather_kanim"), "object", 1, CON.CP, 0, default(Tag), 2, CREATURES.SPECIES.COLDBREATHER.DOMESTICATEDDESC, 0, 0.3f, 0.3f, null, "", false), "ColdBreather_preview", Assets.GetAnim("coldbreather_kanim"), "place", 1, 2);
			SoundEventVolumeCache.instance.AddVolume("coldbreather_kanim", "ColdBreather_grow", NOISE_POLLUTION.CREATURES.TIER3);
			SoundEventVolumeCache.instance.AddVolume("coldbreather_kanim", "ColdBreather_intake", NOISE_POLLUTION.CREATURES.TIER3);
			return false;
		}

		// Token: 0x04000002 RID: 2
		private const string anim = "coldbreather_kanim";
	}
}
