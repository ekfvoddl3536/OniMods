using System;
using Harmony;
using STRINGS;
using UnityEngine;

namespace WaterfallSpace
{
	// Token: 0x02000003 RID: 3
	[HarmonyPatch(typeof(RockCometConfig), "CreatePrefab")]
	public class RCC_PATCH_WATERBOMB
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000021B4 File Offset: 0x000003B4
		public static bool Prefix(ref GameObject __result)
		{
			__result = EntityTemplates.CreateEntity(RockCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.ROCKCOMET.NAME, true);
			EntityTemplateExtensions.AddOrGet<SaveLoadRoot>(__result);
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(__result);
			Comet c = EntityTemplateExtensions.AddOrGet<Comet>(__result);
			c.massRange = new Vector2(900f, 30000f);
			c.temperatureRange = new Vector2(273.15f, 353.15f);
			c.addTiles = Random.Range(2, 8);
			c.addTilesMinHeight = 4;
			c.addTilesMaxHeight = 9;
			c.totalTileDamage = 0f;
			c.splashRadius = (c.flyingSoundID = 1);
			c.impactSound = "Meteor_Large_Impact";
			c.explosionEffectHash = -130923271;
			PrimaryElement primaryElement = EntityTemplateExtensions.AddOrGet<PrimaryElement>(__result);
			primaryElement.SetElement(1836671383);
			primaryElement.Temperature = Random.Range(c.temperatureRange.x, c.temperatureRange.y);
			KBatchedAnimController kb = EntityTemplateExtensions.AddOrGet<KBatchedAnimController>(__result);
			kb.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("meteor_rock_kanim")
			};
			kb.isMovable = true;
			kb.initialAnim = "fall_loop";
			kb.initialMode = 0;
			kb.visibilityType = 1;
			EntityTemplateExtensions.AddOrGet<KCircleCollider2D>(__result).radius = 0.5f;
			return false;
		}
	}
}
