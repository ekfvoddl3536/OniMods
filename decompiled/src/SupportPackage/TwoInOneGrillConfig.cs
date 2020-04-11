using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x02000018 RID: 24
	public class TwoInOneGrillConfig : IBuildingConfig
	{
		// Token: 0x0600005C RID: 92 RVA: 0x000042DC File Offset: 0x000024DC
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("TwoInOneGrill", 3, 2, "cookstation_kanim", 30, 30f, IN_Constants.TwoInOneGrill.TMass, IN_Constants.TwoInOneGrill.TMates, 1600f, 1, DECOR.NONE, NOISE_POLLUTION.NOISY.TIER4, 0.2f);
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "large";
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 100f;
			buildingDef.ExhaustKilowattsWhenActive = 1f;
			buildingDef.SelfHeatKilowattsWhenActive = 4f;
			buildingDef.PowerInputOffset = new CellOffset(0, 0);
			buildingDef.PermittedRotations = 3;
			return buildingDef;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004374 File Offset: 0x00002574
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			EntityTemplateExtensions.AddOrGet<BuildingComplete>(go).isManuallyOperated = true;
			EntityTemplateExtensions.AddOrGet<DropAllWorkable>(go);
			InOneCookingStation cs = EntityTemplateExtensions.AddOrGet<InOneCookingStation>(go);
			EntityTemplateExtensions.AddOrGet<FabricatorIngredientStatusManager>(go);
			EntityTemplateExtensions.AddOrGet<CopyBuildingSettings>(go);
			EntityTemplateExtensions.AddOrGet<ComplexFabricatorWorkable>(go).overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_cookstation_kanim")
			};
			cs.sideScreenStyle = 7;
			Prioritizable.AddRef(go);
			this.StorageSetup(go, cs);
			cs.storeProduced = true;
			InOneRefrigerator inOneRefrigerator = EntityTemplateExtensions.AddOrGet<InOneRefrigerator>(go);
			inOneRefrigerator.simulatedTemperature = 263.15f;
			inOneRefrigerator.outStorage = cs.outStorage;
			this.ComAdds();
			EntityTemplateExtensions.AddOrGetDef<PoweredController.Def>(go);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004414 File Offset: 0x00002614
		protected virtual void ComAdds()
		{
			Tag ab;
			ab..ctor("TwoInOneGrill");
			this.AddRecipe(50f, ITEMS.FOOD.FRIEDMUSHBAR.RECIPEDESC, ab, 1, new ComplexRecipe.RecipeElement("FriedMushBar", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("MushBar", 1f)
			});
			this.AddRecipe(50f, ITEMS.FOOD.COLDWHEATBREAD.RECIPEDESC, ab, 50, new ComplexRecipe.RecipeElement("ColdWheatBread", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("ColdWheatSeed", 3f)
			});
			this.AddRecipe(50f, ITEMS.FOOD.COOKEDEGG.RECIPEDESC, ab, 1, new ComplexRecipe.RecipeElement("CookedEgg", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("RawEgg", 1f)
			});
			this.AddRecipe(50f, ITEMS.FOOD.GRILLEDPRICKLEFRUIT.RECIPEDESC, ab, 20, new ComplexRecipe.RecipeElement("GrilledPrickleFruit", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(PrickleFruitConfig.ID, 1f)
			});
			this.AddRecipe(50f, ITEMS.FOOD.SALSA.RECIPEDESC, ab, 101, new ComplexRecipe.RecipeElement("Salsa", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(PrickleFruitConfig.ID, 2f),
				new ComplexRecipe.RecipeElement(SpiceNutConfig.ID, 2f)
			});
			this.AddRecipe(30f, ITEMS.FOOD.PICKLEDMEAL.RECIPEDESC, ab, 21, new ComplexRecipe.RecipeElement("PickledMeal", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("BasicPlantFood", 3f)
			});
			this.AddRecipe(50f, ITEMS.FOOD.FRIEDMUSHROOM.RECIPEDESC, ab, 20, new ComplexRecipe.RecipeElement("FriedMushroom", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(MushroomConfig.ID, 1f)
			});
			this.AddRecipe(50f, ITEMS.FOOD.COOKEDMEAT.RECIPEDESC, ab, 21, new ComplexRecipe.RecipeElement("CookedMeat", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(SpiceNutConfig.ID, 1f),
				new ComplexRecipe.RecipeElement("Meat", 2f)
			});
			this.AddRecipe(50f, ITEMS.FOOD.SPICEBREAD.RECIPEDESC, ab, 100, new ComplexRecipe.RecipeElement("SpiceBread", 1f), new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(SpiceNutConfig.ID, 1f),
				new ComplexRecipe.RecipeElement("ColdWheatSeed", 10f)
			});
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004704 File Offset: 0x00002904
		protected virtual void AddRecipe(float cooktime, string desc, Tag me, int sortod, ComplexRecipe.RecipeElement output, params ComplexRecipe.RecipeElement[] inputs)
		{
			ComplexRecipe.RecipeElement[] temp = new ComplexRecipe.RecipeElement[]
			{
				output
			};
			ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("TwoInOneGrill", inputs, temp), inputs, temp);
			complexRecipe.time = cooktime;
			complexRecipe.description = desc;
			complexRecipe.nameDisplay = 1;
			complexRecipe.fabricators = new List<Tag>
			{
				me
			};
			complexRecipe.sortOrder = sortod;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004760 File Offset: 0x00002960
		protected virtual void StorageSetup(GameObject go, ComplexFabricator st)
		{
			st.inStorage = go.AddComponent<Storage>();
			st.inStorage.capacityKg = 1000f;
			st.inStorage.showInUI = true;
			st.inStorage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			st.buildStorage = go.AddComponent<Storage>();
			st.buildStorage.capacityKg = 1000f;
			st.buildStorage.showInUI = true;
			st.buildStorage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			st.outStorage = go.AddComponent<Storage>();
			st.outStorage.capacityKg = 50f;
			st.outStorage.showInUI = true;
			st.outStorage.allowItemRemoval = true;
			st.outStorage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
			st.outStorage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004831 File Offset: 0x00002A31
		public override void DoPostConfigureComplete(GameObject go)
		{
		}
	}
}
