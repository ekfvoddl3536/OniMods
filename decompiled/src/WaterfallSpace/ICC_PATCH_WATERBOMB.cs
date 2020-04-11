using System;
using Harmony;
using STRINGS;
using UnityEngine;

namespace WaterfallSpace
{
	// Token: 0x02000002 RID: 2
	[HarmonyPatch(typeof(IronCometConfig), "CreatePrefab")]
	public class ICC_PATCH_WATERBOMB
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static bool Prefix(ref GameObject __result)
		{
			__result = EntityTemplates.CreateEntity(IronCometConfig.ID, UI.SPACEDESTINATIONS.COMETS.IRONCOMET.NAME, true);
			EntityTemplateExtensions.AddOrGet<SaveLoadRoot>(__result);
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(__result);
			Comet c = EntityTemplateExtensions.AddOrGet<Comet>(__result);
			c.massRange = new Vector2(15f, 200f);
			c.temperatureRange = new Vector2(275f, 340.15f);
			c.addTiles = Random.Range(1, 4);
			c.addTilesMinHeight = 1;
			c.addTilesMaxHeight = 2;
			c.totalTileDamage = 0f;
			c.splashRadius = (c.flyingSoundID = 1);
			c.impactSound = "Meteor_Medium_Impact";
			c.explosionEffectHash = 58009621;
			PrimaryElement primaryElement = EntityTemplateExtensions.AddOrGet<PrimaryElement>(__result);
			primaryElement.SetElement(1836671383);
			primaryElement.Temperature = Random.Range(c.temperatureRange.x, c.temperatureRange.y);
			KBatchedAnimController kb = EntityTemplateExtensions.AddOrGet<KBatchedAnimController>(__result);
			kb.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("meteor_metal_kanim")
			};
			kb.isMovable = true;
			kb.initialAnim = "fall_loop";
			kb.initialMode = 0;
			kb.visibilityType = 1;
			EntityTemplateExtensions.AddOrGet<KCircleCollider2D>(__result).radius = 0.5f;
			__result.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
			return false;
		}
	}
}
