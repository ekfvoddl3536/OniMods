using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace CarbonatedWater
{
	// Token: 0x02000002 RID: 2
	public class CarbonatedWaterGeneratorConfig : IBuildingConfig
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CO2WaterGen", 2, 3, "microbemusher_kanim", 30, 30f, new float[]
			{
				1000f
			}, MATERIALS.RAW_METALS, 1600f, 1, BUILDINGS.DECOR.PENALTY.TIER3, NOISE_POLLUTION.NOISY.TIER1, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 15f;
			buildingDef.ExhaustKilowattsWhenActive = 0.5f;
			buildingDef.SelfHeatKilowattsWhenActive = 3f;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.AudioCategory = "Glass";
			buildingDef.AudioSize = "large";
			return buildingDef;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020E8 File Offset: 0x000002E8
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			EntityTemplateExtensions.AddOrGet<DropAllWorkable>(go);
			Prioritizable.AddRef(go);
			EntityTemplateExtensions.AddOrGet<BuildingComplete>(go).isManuallyOperated = true;
			ComplexFabricator musher = go.AddComponent<ComplexFabricator>();
			musher.duplicantOperated = true;
			musher.sideScreenStyle = 7;
			EntityTemplateExtensions.AddOrGet<FabricatorIngredientStatusManager>(go);
			EntityTemplateExtensions.AddOrGet<CopyBuildingSettings>(go);
			BuildingTemplates.CreateComplexFabricatorStorage(go, musher);
			EntityTemplateExtensions.AddOrGet<ComplexFabricatorWorkable>(go).overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_musher_kanim")
			};
			this.AddRecipes();
			EntityTemplateExtensions.AddOrGetDef<PoweredController.Def>(go);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002168 File Offset: 0x00000368
		protected virtual void AddRecipes()
		{
			Tag tag;
			tag..ctor("CO2WaterGen");
			Tag watag = GameTagExtensions.CreateTag(1836671383);
			Tag cotag = GameTagExtensions.CreateTag(1960575215);
			this.AddCarbonatedWater(ColaConfig.recipe, tag, "탄산수 제조기를 통해 얻을 수 있습니다.", 1, 30f, new ComplexRecipe.RecipeElement(new Tag("Cola"), 1.2f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(watag, 1f),
				new ComplexRecipe.RecipeElement(cotag, 0.7f)
			});
			this.AddCarbonatedWater(ItsTerineConfig.recipe, tag, "탄산수 제조기를 통해 얻을 수 있습니다.", 2, 50f, new ComplexRecipe.RecipeElement(new Tag("ItsTerine"), 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(watag, 0.5f),
				new ComplexRecipe.RecipeElement(cotag, 0.5f)
			});
			this.AddCarbonatedWater(DoctorBerryConfig.recipe, tag, "탄산수 제조기를 통해 얻을 수 있습니다.", 3, 50f, new ComplexRecipe.RecipeElement(new Tag("DoctorBerry"), 1.8f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(watag, 1.5f),
				new ComplexRecipe.RecipeElement(cotag, 0.9f),
				new ComplexRecipe.RecipeElement(PrickleFruitConfig.ID, 0.5f)
			});
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002298 File Offset: 0x00000498
		protected virtual void AddCarbonatedWater(ComplexRecipe target, Tag _t, string descript, int st, float cookingtime, ComplexRecipe.RecipeElement resultfood, params ComplexRecipe.RecipeElement[] ingredients)
		{
			ComplexRecipe.RecipeElement[] tempres = new ComplexRecipe.RecipeElement[]
			{
				resultfood
			};
			ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("CO2WaterGen", ingredients, tempres), ingredients, tempres);
			complexRecipe.time = cookingtime;
			complexRecipe.description = descript;
			complexRecipe.nameDisplay = 1;
			complexRecipe.fabricators = new List<Tag>
			{
				_t
			};
			complexRecipe.sortOrder = st;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000022F8 File Offset: 0x000004F8
		public override void DoPostConfigureComplete(GameObject go)
		{
		}
	}
}
