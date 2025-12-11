// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#if PRIVATE_CORE
using UnityEngine;
using static TUNING.BUILDINGS;

namespace SupportPackage;

using static GlobalConsts.EasyGlassForge;
public sealed class EasyGlassForgeConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        var def = BuildingTemplates.CreateBuildingDef(
            ID, 5, 4, ANISTR,
            HITPOINTS.TIER1, CONSTRUCTION_TIME_SECONDS.TIER3,
            TMass, TMate,
            MELTING_POINT_KELVIN.TIER2,
            BuildLocationRule.OnFloor,
            DECOR.BONUS.TIER2,
            TUNING.NOISE_POLLUTION.NOISY.TIER4);

        def.RequiresPowerInput = true;
        def.EnergyConsumptionWhenActive = USE_POWER;

        def.SelfHeatKilowattsWhenActive = HEAT_SELF;

        def.ViewMode = OverlayModes.Power.ID;

        def.AudioCategory = TUNING.AUDIO.CATEGORY.HOLLOW_METAL;
        def.AudioSize = TUNING.AUDIO.SIZE.LARGE;
        return def;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        go.AddOrGet<DropAllWorkable>();
        go.AddOrGet<BuildingComplete>().isManuallyOperated = true;

        var forge = go.AddOrGet<ComplexFabricator>();
        forge.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
        forge.duplicantOperated = false;
        forge.storeProduced = false;

        go.AddOrGet<FabricatorIngredientStatusManager>();
        go.AddOrGet<CopyBuildingSettings>();

        go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = [Assets.GetAnim("anim_interacts_metalrefinery_kanim")];

        BuildingTemplates.CreateComplexFabricatorStorage(go, forge);

        var rein = new[] {
            new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Sand).tag, 100f)
        };
        var reout = new[] {
            new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Glass).tag, PRODUCE)
        };

        var id = ComplexRecipeManager.MakeObsoleteRecipeID(ID, rein[0].material);
        var str = ComplexRecipeManager.MakeRecipeID(ID, rein, reout);
        new ComplexRecipe(str, rein, reout)
        {
            time = TIME,
            nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
            description = string.Format(STRINGS.BUILDINGS.PREFABS.GLASSFORGE.RECIPE_DESCRIPTION, ElementLoader.GetElement(reout[0].material).name, ElementLoader.GetElement(rein[0].material).name),
            fabricators = [TagManager.Create(ID)]
        };
        ComplexRecipeManager.Get().AddObsoleteIDMapping(id, str);
        Prioritizable.AddRef(go);

        ConfigureRecipes();
    }

    private void ConfigureRecipes()
    {
        AddReceipe(TIME, SimHashes.Steel, PRODUCE,
            (SimHashes.Iron, 50),
            (SimHashes.Glass, 5));

        AddReceipe(TIME, SimHashes.Diamond, PRODUCE,
            (SimHashes.Carbon, 25),
            (SimHashes.Water, 1));

        AddReceipe(TIME, SimHashes.Carbon, PRODUCE,
            (SimHashes.Diamond, 25),
            (SimHashes.Water, 1));

        AddReceipe(TIME, SimHashes.RefinedCarbon, PRODUCE,
            (SimHashes.Diamond, 50),
            (SimHashes.Water, 5));

        AddReceipe(TIME, SimHashes.Gold, PRODUCE,
            (SimHashes.Iron, 50),
            (SimHashes.Diamond, 5));

        AddReceipe(TIME, SimHashes.Iron, PRODUCE,
            (SimHashes.Gold, 50),
            (SimHashes.Diamond, 5));

        AddReceipe(TIME, SimHashes.Ceramic, PRODUCE,
            (SimHashes.Carbon, 25),
            (SimHashes.Water, 1));

        AddReceipe(TIME, SimHashes.Copper, PRODUCE,
            (SimHashes.Diamond, 25),
            (SimHashes.Sand, 1));

        AddReceipe(TIME, SimHashes.Aluminum, PRODUCE,
            (SimHashes.Copper, 50),
            (SimHashes.Water, 5));

        AddReceipe(TIME, SimHashes.Fertilizer, PRODUCE,
            (SimHashes.Dirt, 10),
            (SimHashes.Water, 10));

        AddReceipe(TIME, SimHashes.Dirt, PRODUCE,
            (SimHashes.Fertilizer, 10),
            (SimHashes.Water, 10));

        AddReceipe(TIME, SimHashes.OxyRock, PRODUCE,
            (SimHashes.Gold, 0.05f),
            (SimHashes.Water, 25f));

        AddReceipe(TIME, SimHashes.Isoresin, PRODUCE,
            (SimHashes.Diamond, 24.5f),
            (SimHashes.Gold, 24.5f),
            (SimHashes.Iron, 24.5f),
            (SimHashes.Water, 0.5f));

        AddReceipe(TIME, SimHashes.SuperInsulator, PRODUCE,
            (SimHashes.Isoresin, 50f),
            (SimHashes.Carbon, 50f));

        AddReceipe(TIME, SimHashes.TempConductorSolid, PRODUCE,
            (SimHashes.Niobium, 5f),
            (SimHashes.Gold, 2.25f),
            (SimHashes.Iron, 2.25f),
            (SimHashes.Water, 0.5f));

        AddReceipe(TIME, SimHashes.Polypropylene, PRODUCE,
            (SimHashes.Petroleum, 0.5f),
            (SimHashes.Water, 0.5f));

        AddReceipe(TIME, SimHashes.SugarWater, PRODUCE,
            (SimHashes.Fertilizer, 1.0f),
            (SimHashes.Water, 0.5f));

        AddReceipe(TIME, SimHashes.Brine, PRODUCE,
            (SimHashes.SugarWater, 1.0f),
            (SimHashes.Water, 0.5f));

        AddReceipe(TIME, SimHashes.BleachStone, PRODUCE,
            (SimHashes.Fertilizer, 5.0f),
            (SimHashes.Sand, 5.0f));

        AddReceipe(TIME, SimHashes.Sand, PRODUCE,
            (SimHashes.Sand, 1.0f),
            (SimHashes.Diamond, 1.0f));

        AddReceipe(TIME, SimHashes.Milk, PRODUCE,
            (SimHashes.Water, 0.5f),
            (SimHashes.Sand, 0.5f));

        AddReceipe(TIME, SimHashes.MilkFat, PRODUCE,
            (SimHashes.Milk, 0.5f),
            (SimHashes.Dirt, 0.25f),
            (SimHashes.Sand, 0.25f));

        AddReceipe(TIME, SimHashes.Lead, PRODUCE,
            (SimHashes.Copper, 25f),
            (SimHashes.Sand, 1f));

        AddReceipe(TIME, SimHashes.Phosphorite, PRODUCE,
            (SimHashes.Dirt, 10f));

        AddReceipe(TIME, SimHashes.Graphite, PRODUCE,
            (SimHashes.Diamond, 5f),
            (SimHashes.Carbon, 5f));

        AddReceipe(TIME, SimHashes.Fullerene, PRODUCE,
            (SimHashes.Graphite, 5f),
            (SimHashes.Aluminum, 5f));

        AddReceipe(TIME, SimHashes.Niobium, PRODUCE,
            (SimHashes.Ceramic, 100f));

        AddReceipe(TIME, SimHashes.HardPolypropylene, PRODUCE,
            (SimHashes.Polypropylene, 100f));

        AddReceipe(TIME, SimHashes.Obsidian, PRODUCE,
            (SimHashes.Obsidian, 5f));

        AddReceipe(TIME, SimHashes.Granite, PRODUCE,
            (SimHashes.Granite, 5f));
    }

    private static void AddReceipe(float time, SimHashes outputElement, float outputAmount, params (SimHashes e, float a)[] input)
    {
        var output = Make(outputElement, outputAmount);

        var source = input.Select(static k => Make(k.e, k.a)).ToArray();
        var result = new[] { output };
        new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, source, result), source, result)
        {
            time = time,
            description = ElementLoader.GetElement(output.material).description,
            nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
            fabricators = [TagManager.Create(ID)]
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ComplexRecipe.RecipeElement Make(SimHashes e, float a) => new(ElementLoader.FindElementByHash(e).tag, a);

    public override void DoPostConfigureComplete(GameObject go)
    {
        SymbolOverrideControllerUtil.AddToPrefab(go);
        go.AddOrGetDef<PoweredActiveStoppableController.Def>();

        go.GetComponent<KPrefabID>().prefabSpawnFn += static go2 =>
        {
            var comp = go2.GetComponent<ComplexFabricatorWorkable>();
            var db = Db.Get();

            comp.WorkerStatusItem = db.DuplicantStatusItems.Processing;
            comp.AttributeConverter = db.AttributeConverters.MachinerySpeed;
            comp.SkillExperienceSkillGroup = db.SkillGroups.Technicals.Id;
            comp.AttributeExperienceMultiplier = TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
            comp.SkillExperienceMultiplier = TUNING.SKILLS.PART_DAY_EXPERIENCE;
        };
    }
}
#endif