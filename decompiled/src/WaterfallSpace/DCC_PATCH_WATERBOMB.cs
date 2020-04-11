using System;
using Harmony;
using STRINGS;
using UnityEngine;

namespace WaterfallSpace
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(DustCometConfig), "CreatePrefab")]
	public class DCC_PATCH_WATERBOMB
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000022FC File Offset: 0x000004FC
		public static bool Prefix(ref GameObject __result)
		{
			__result = EntityTemplates.CreateEntity(DustCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.DUSTCOMET.NAME, true);
			EntityTemplateExtensions.AddOrGet<SaveLoadRoot>(__result);
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(__result);
			Comet c = EntityTemplateExtensions.AddOrGet<Comet>(__result);
			c.massRange = new Vector2(1f, 10f);
			c.temperatureRange = new Vector2(272.15f, 299.15f);
			c.addTiles = Random.Range(1, 1);
			c.addTilesMinHeight = 1;
			c.addTilesMaxHeight = 1;
			c.totalTileDamage = 0f;
			c.splashRadius = (c.flyingSoundID = 0);
			c.impactSound = "Meteor_Small_Impact";
			c.explosionEffectHash = -31719612;
			PrimaryElement primaryElement = EntityTemplateExtensions.AddOrGet<PrimaryElement>(__result);
			primaryElement.SetElement(1836671383);
			primaryElement.Temperature = Random.Range(c.temperatureRange.x, c.temperatureRange.y);
			KBatchedAnimController kb = EntityTemplateExtensions.AddOrGet<KBatchedAnimController>(__result);
			kb.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("meteor_sand_kanim")
			};
			kb.isMovable = true;
			kb.initialAnim = "fall_loop";
			kb.initialMode = 0;
			kb.visibilityType = 1;
			EntityTemplateExtensions.AddOrGet<KCircleCollider2D>(__result).radius = 0.5f;
			__result.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
			return false;
		}
	}
}
