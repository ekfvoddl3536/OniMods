// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

//using System.Collections.Generic;
//using UnityEngine;
//using static TUNING.BUILDINGS;

//namespace SupportPackage;

//using FOOD_TYPES = TUNING.FOOD.FOOD_TYPES;
//using RCELEM = ComplexRecipe.RecipeElement;
//using static GlobalConsts.ElectricFoodDehydrator;
//public sealed class ElectricFoodDehydratorConfig : IBuildingConfig
//{
//    public override BuildingDef CreateBuildingDef()
//    {
//        var def = BuildingTemplates.CreateBuildingDef(
//            ID, 3, 3, ANISTR,
//            HITPOINTS.TIER1, CONSTRUCTION_TIME_SECONDS.TIER2,
//            TMass, TMate,
//            MELTING_POINT_KELVIN.TIER1,
//            BuildLocationRule.OnFloor,
//            DECOR.NONE,
//            TUNING.NOISE_POLLUTION.NOISY.TIER3);

//        def.AudioCategory = TUNING.AUDIO.CATEGORY.METAL;
//        def.AudioSize = TUNING.AUDIO.SIZE.LARGE;

//        def.RequiresPowerInput = true;
//        def.PowerInputOffset = new CellOffset(0, 0);
        
//        def.EnergyConsumptionWhenActive = POWER_USE;

//        def.SelfHeatKilowattsWhenActive = 4f;

//        def.UtilityOutputOffset = new CellOffset(-1, 0);
//        def.OutputConduitType = ConduitType.Liquid;

//        def.ViewMode = OverlayModes.LiquidConduits.ID;

//        def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(default);
//        def.ForegroundLayer = Grid.SceneLayer.BuildingFront;
//        def.AddSearchTerms(STRINGS.SEARCH_TERMS.FOOD);

//        return def;
//    }

//    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
//    {
//        go.AddOrGet<DropAllWorkable>();

//        go.AddOrGet<BuildingComplete>().isManuallyOperated = true;

//        var fabricator = go.AddOrGet<ComplexFabricator>();
//        fabricator.heatedTemperature = 368.15f;
//        fabricator.duplicantOperated = false;
//        fabricator.showProgressBar = true;
//        fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
//        fabricator.storeProduced = true;
//        fabricator.keepExcessLiquids = true;

//        go.AddOrGet<FabricatorIngredientStatusManager>();
//        go.AddOrGet<CopyBuildingSettings>();

//        BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
//        fabricator.inStorage.SetDefaultStoredItemModifiers(STORED_ITEM_MODIFIERS);
//        fabricator.buildStorage.SetDefaultStoredItemModifiers(STORED_ITEM_MODIFIERS);
//        fabricator.outStorage.SetDefaultStoredItemModifiers(STORED_ITEM_MODIFIERS);

//        var cd = go.AddOrGet<ConduitDispenser>();
//        cd.alwaysDispense = true;
//        cd.conduitType = ConduitType.Liquid;
//        cd.storage = fabricator.outStorage;
//        cd.elementFilter = null;

//        ConfigureRecipes();
        
//        Prioritizable.AddRef(go);
//        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

//        FoodDehydratorWorkableEmpty dehydratorWorkableEmpty = go.AddOrGet<FoodDehydratorWorkableEmpty>();
//        dehydratorWorkableEmpty.workTime = FOODDEHYDRATORTUNING.DUPE_WORK_TIME;
//        dehydratorWorkableEmpty.overrideAnims = [Assets.GetAnim("anim_interacts_dehydrator_kanim")];
//        dehydratorWorkableEmpty.workLayer = Grid.SceneLayer.Front;
//    }

//    private static void ConfigureRecipes()
//    {
//        var list = new List<(EdiblesManager.FoodInfo foodInfo, Tag material)>(9)
//        {
//            (FOOD_TYPES.SALSA, DehydratedSalsaConfig.ID),
//            (FOOD_TYPES.MUSHROOM_WRAP, DehydratedMushroomWrapConfig.ID),
//            (FOOD_TYPES.SURF_AND_TURF, DehydratedSurfAndTurfConfig.ID),
//            (FOOD_TYPES.SPICEBREAD, DehydratedSpiceBreadConfig.ID),
//            (FOOD_TYPES.QUICHE, DehydratedQuicheConfig.ID),
//            (FOOD_TYPES.CURRY, DehydratedCurryConfig.ID),
//            (FOOD_TYPES.SPICY_TOFU, DehydratedSpicyTofuConfig.ID),
//            (FOOD_TYPES.BURGER, DehydratedFoodPackageConfig.ID)
//        };

//        if (DlcManager.IsExpansion1Active())
//            list.Add((FOOD_TYPES.BERRY_PIE, DehydratedBerryPieConfig.ID));

//        var plasticTag = SimHashes.Polypropylene.CreateTag();
//        var waterTag = SimHashes.Water.CreateTag();
//        var selfTag = TagManager.Create(ID);

//        int num = 100;
//        foreach (var it in list)
//        {
//            var foodInfo = it.foodInfo;
//            var material = it.material;
//            RCELEM[] inputs =
//            [
//                new RCELEM(foodInfo, FOODDEHYDRATORTUNING.FOOD_KCAL / foodInfo.CaloriesPerUnit, true),
//                new RCELEM(plasticTag, FOODDEHYDRATORTUNING.PLASTIC_KG)
//            ];
//            RCELEM[] outputs =
//            [
//                new RCELEM(material, FOODDEHYDRATORTUNING.FOOD_PACKETS, RCELEM.TemperatureOperation.Dehydrated),
//                new RCELEM(waterTag, FOODDEHYDRATORTUNING.WATER_OUTPUT_KG, RCELEM.TemperatureOperation.Heated)
//            ];

//            new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, inputs, outputs), inputs, outputs)
//            {
//                time = FOODDEHYDRATORTUNING.INTERNAL_WORK_TIME,
//                nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
//                customName = string.Format(STRINGS.BUILDINGS.PREFABS.FOODDEHYDRATOR.RECIPE_NAME, foodInfo.Name),
//                description = string.Format(STRINGS.BUILDINGS.PREFABS.FOODDEHYDRATOR.RESULT_DESCRIPTION, foodInfo.Name),
//                fabricators = [selfTag],
//                sortOrder = num
//            };
//            ++num;
//        }
//    }

//    public override void DoPostConfigureComplete(GameObject go)
//    {
//        go.AddOrGet<LogicOperationalController>();
//        go.AddOrGetDef<FoodDehydrator.Def>();
//    }
//}
