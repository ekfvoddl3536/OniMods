using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace SeedConverter
{
	// Token: 0x02000005 RID: 5
	public class SeedConverterConfig : IBuildingConfig
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000021EC File Offset: 0x000003EC
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SeedConverter", 4, 3, "fertilizer_maker_kanim", 60, 60f, Consts.TMass, Consts.TMate, 1600f, 1, DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER2, 0.2f);
			buildingDef.UseStructureTemperature = (buildingDef.RequiresPowerInput = true);
			buildingDef.EnergyConsumptionWhenActive = 160f;
			buildingDef.SelfHeatKilowattsWhenActive = 6f;
			buildingDef.ExhaustKilowattsWhenActive = 1f;
			buildingDef.PowerInputOffset = new CellOffset(2, 0);
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.ViewMode = OverlayModes.Power.ID;
			return buildingDef;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002284 File Offset: 0x00000484
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<DropAllWorkable>(go);
			Prioritizable.AddRef(go);
			EntityTemplateExtensions.AddOrGet<BuildingComplete>(go).isManuallyOperated = false;
			ComplexFabricator fb = EntityTemplateExtensions.AddOrGet<ComplexFabricator>(go);
			fb.sideScreenStyle = 7;
			fb.duplicantOperated = false;
			EntityTemplateExtensions.AddOrGet<FabricatorIngredientStatusManager>(go);
			EntityTemplateExtensions.AddOrGet<CopyBuildingSettings>(go);
			EntityTemplateExtensions.AddOrGet<ComplexFabricatorWorkable>(go).overrideAnims = new KAnimFile[0];
			BuildingTemplates.CreateComplexFabricatorStorage(go, fb);
			this.CreateRecipes();
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022F4 File Offset: 0x000004F4
		protected virtual void CreateRecipes()
		{
			Tag me;
			me..ctor("SeedConverter");
			Tag wtag = GameTagExtensions.CreateTag(1836671383);
			Tag ftag = GameTagExtensions.CreateTag(-1396791454);
			Tag ptag = GameTagExtensions.CreateTag(-1412059381);
			Tag ctag = GameTagExtensions.CreateTag(947100397);
			this.AddRecipes(CREATURES.SPECIES.SEEDS.BASICSINGLEHARVESTPLANT.DESC, 10, me, new ComplexRecipe.RecipeElement("BasicSingleHarvestPlantSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(wtag, 0.25f),
				new ComplexRecipe.RecipeElement(ftag, 0.6f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.BULBPLANT.DESC, 10, me, new ComplexRecipe.RecipeElement("BulbPlantSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ptag, 0.6f),
				new ComplexRecipe.RecipeElement(ftag, 0.3f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.CACTUSPLANT.DESC, 10, me, new ComplexRecipe.RecipeElement("CactusPlantSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(wtag, 0.5f),
				new ComplexRecipe.RecipeElement(ftag, 0.45f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.COLDBREATHER.DESC, 16, me, new ComplexRecipe.RecipeElement(ColdBreatherConfig.SEED_TAG, 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(wtag, 0.75f),
				new ComplexRecipe.RecipeElement(ftag, 0.05f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.COLDWHEAT.DESC, 16, me, new ComplexRecipe.RecipeElement("ColdWheatSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(wtag, 0.45f),
				new ComplexRecipe.RecipeElement(ftag, 0.45f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.LEAFYPLANT.DESC, 10, me, new ComplexRecipe.RecipeElement("LeafyPlantSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ptag, 0.35f),
				new ComplexRecipe.RecipeElement(ftag, 0.45f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.MUSHROOMPLANT.DESC, 16, me, new ComplexRecipe.RecipeElement("MushroomSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ftag, 0.75f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.PRICKLEGRASS.DESC, 10, me, new ComplexRecipe.RecipeElement("PrickleGrassSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ptag, 0.1f),
				new ComplexRecipe.RecipeElement(ftag, 0.7f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.PRICKLEFLOWER.DESC, 12, me, new ComplexRecipe.RecipeElement("PrickleFlowerSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ptag, 0.25f),
				new ComplexRecipe.RecipeElement(wtag, 0.5f),
				new ComplexRecipe.RecipeElement(ftag, 0.2f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.SPICE_VINE.DESC, 10, me, new ComplexRecipe.RecipeElement("SpiceVineSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ptag, 0.45f),
				new ComplexRecipe.RecipeElement(ftag, 0.3f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.SWAMPLILY.DESC, 16, me, new ComplexRecipe.RecipeElement("SwampLilySeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(1624244999), 0.88f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.BASICFABRICMATERIALPLANT.DESC, 25, me, new ComplexRecipe.RecipeElement("BasicFabricMaterialPlantSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ptag, 0.3f),
				new ComplexRecipe.RecipeElement(wtag, 0.65f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.GASGRASS.DESC, 25, me, new ComplexRecipe.RecipeElement("GasGrassSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ftag, 0.9f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.OILEATER.DESC, 10, me, new ComplexRecipe.RecipeElement("EvilFlowerSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(-279785280), 1f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.BEAN_PLANT.DESC, 16, me, new ComplexRecipe.RecipeElement("BeanPlantSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ptag, 0.9f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.WOOD_TREE.DESC, 20, me, new ComplexRecipe.RecipeElement("ForestTreeSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(wtag, 0.4f),
				new ComplexRecipe.RecipeElement(ftag, 0.3f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.OXYFERN.DESC, 10, me, new ComplexRecipe.RecipeElement("OxyfernSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(wtag, 0.3f),
				new ComplexRecipe.RecipeElement(ctag, 0.5f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.SALTPLANT.DESC, 8, me, new ComplexRecipe.RecipeElement("SaltPlantSeed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(wtag, 0.4f),
				new ComplexRecipe.RecipeElement(ctag, 0.1f),
				new ComplexRecipe.RecipeElement(GameTagExtensions.CreateTag(381665462), 0.1f)
			});
			this.AddRecipes(CREATURES.SPECIES.SEEDS.SEALETTUCE.DESC, 10, me, new ComplexRecipe.RecipeElement(SeaLettuceConfig.ID + "Seed", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ctag, 0.5f)
			});
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002890 File Offset: 0x00000A90
		protected virtual void AddRecipes(string desc, int _time, Tag _tag, ComplexRecipe.RecipeElement input, params ComplexRecipe.RecipeElement[] output)
		{
			ComplexRecipe.RecipeElement[] temp = new ComplexRecipe.RecipeElement[]
			{
				input
			};
			ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SeedConverter", temp, output), temp, output);
			complexRecipe.time = (float)_time;
			complexRecipe.description = desc;
			complexRecipe.nameDisplay = 0;
			complexRecipe.fabricators = new List<Tag>
			{
				_tag
			};
		}
	}
}
