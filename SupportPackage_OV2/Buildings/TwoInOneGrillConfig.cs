using System.Collections.Generic;
using TUNING;
using UnityEngine;
using re_ = ComplexRecipe.RecipeElement;

namespace SupportPackage
{
    using static GlobalConsts;
    using static GlobalConsts.TwoInOneGrill;
    using static ComplexRecipe;
    using static ComplexRecipeManager;
    using static STRINGS.ITEMS.FOOD;
    public class TwoInOneGrillConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef res = 
                BuildingTemplates.CreateBuildingDef(
                    ID, 3, 2, ANISTR,
                    DEF_HITPT, DEF_CON_TIME, 
                    TMass, TMates, 
                    DEF_MELPT * 2f, BuildLocationRule.OnFloor, 
                    DECOR.NONE, NOISE_POLLUTION.NOISY.TIER4);
            
            res.AudioCategory = "Metal";
            res.AudioSize = "large";

            res.RequiresPowerInput = true;
            res.EnergyConsumptionWhenActive = USE_POWER;

            res.ExhaustKilowattsWhenActive = HEAT_EXHAUS;
            res.SelfHeatKilowattsWhenActive = HEAT_SELF;

            res.PowerInputOffset = default; // (0, 0)
            res.PermittedRotations = PermittedRotations.FlipH;

            res.AddLogicPowerPort = false;

            return res;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;

            go.AddOrGet<DropAllWorkable>();
            
            var cf = go.AddOrGet<InOneCockingStationEX>();
            cf.heatedTemperature = 253.15f;
            cf.storeProduced = true;
            
            go.AddOrGet<FabricatorIngredientStatusManager>();

            go.AddOrGet<CopyBuildingSettings>();

            go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1] 
            { 
                Assets.GetAnim("anim_interacts_cookstation_kanim") 
            };

            cf.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;

            Prioritizable.AddRef(go);

            StorageSetup(go, cf);

            ConfigureRecipes();

            go.AddOrGet<InOneRefrigerator>();

            go.AddOrGet<UserNameable>();

            go.AddOrGetDef<RocketUsageRestriction.Def>().restrictOperational = false;
            go.AddOrGetDef<PoweredController.Def>();
        }

        protected static void StorageSetup(GameObject go, ComplexFabricator cf)
        {
            var st = go.AddOrGet<Storage>();
            st.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
            st.capacityKg = 20_000f;
            st.showInUI = true;
            cf.inStorage = st;

            st = go.AddComponent<Storage>();
            st.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
            st.capacityKg = 20_000f;
            st.showInUI = true;
            cf.buildStorage = st;

            st = go.AddComponent<Storage>();
            st.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
            st.capacityKg = STORED_MAX;
            st.fetchCategory = Storage.FetchCategory.GeneralStorage;
            st.storageFilters = STORAGEFILTERS.FOOD;
            st.showInUI = st.allowItemRemoval = st.showCapacityStatusItem = true;
            cf.outStorage = st;
        }

        private static void ConfigureRecipes()
        {
            AddRecipe(FRIEDMUSHBAR.RECIPEDESC, 1, FriedMushBarConfig.ID, new[]
            {
                new re_(MushBarConfig.ID, 1f)
            });

            AddRecipe(COLDWHEATBREAD.RECIPEDESC, 50, ColdWheatBreadConfig.ID, new[]
            {
                new re_(ColdWheatConfig.SEED_ID, 3f)
            });

            AddRecipe(COOKEDEGG.RECIPEDESC, 1, CookedEggConfig.ID, new[]
            {
                new re_(RawEggConfig.ID, 1f)
            });

            AddRecipe(GRILLEDPRICKLEFRUIT.RECIPEDESC, 20, GrilledPrickleFruitConfig.ID, new[]
            {
                new re_(PrickleFruitConfig.ID, 1f)
            });

            AddRecipe(SALSA.RECIPEDESC, 101, SalsaConfig.ID, new[]
            {
                new re_(PrickleFruitConfig.ID, 2f),
                new re_(SpiceNutConfig.ID, 2f)
            });

            AddRecipe(PICKLEDMEAL.RECIPEDESC, 21, PickledMealConfig.ID, new[]
            {
                new re_(BasicPlantFoodConfig.ID, 3f)
            }, 
            30f);

            AddRecipe(FRIEDMUSHROOM.RECIPEDESC, 20, FriedMushroomConfig.ID, new[]
            {
                new re_(MushroomConfig.ID, 1f)
            });

            AddRecipe(COOKEDMEAT.RECIPEDESC, 21, CookedMeatConfig.ID, new[]
            {
                new re_(SpiceNutConfig.ID, 1f),
                new re_(MeatConfig.ID, 2f)
            });

            AddRecipe(SPICEBREAD.RECIPEDESC, 100, SpiceBreadConfig.ID, new[]
            {
                new re_(SpiceNutConfig.ID, 1f),
                new re_(ColdWheatConfig.SEED_ID, 10f)
            });

            AddRecipe(COOKEDFISH.RECIPEDESC, 21, CookedFishConfig.ID, new[]
            {
                new re_(FishMeatConfig.ID, 1f),
            });

            if (DlcManager.IsExpansion1Active())
                ADD_DLC_Recipes();
        }

        private static void ADD_DLC_Recipes()
        {
            AddRecipe(SWAMPDELIGHTS.RECIPEDESC, 20, SwampDelightsConfig.ID, new[]
            {
                new re_(SwampFruitConfig.ID, 1f),
            });

            AddRecipe(WORMBASICFOOD.RECIPEDESC, 20, WormBasicFoodConfig.ID, new[]
            {
                new re_(WormBasicFruitConfig.ID, 1f),
            });

            AddRecipe(WORMSUPERFOOD.RECIPEDESC, 20, WormSuperFoodConfig.ID, new[]
            {
                new re_(WormSuperFruitConfig.ID, 8f),
                new re_(new Tag(nameof(SimHashes.Sucrose)), 4f)
            });
        }

        private static void AddRecipe(
            string desc,
            int sortod,
            string output_id,
            re_[] inputs,
            float cooktime = 50f)
        {
            var output = new re_[1]
            {
                new re_(new Tag(output_id), 1, re_.TemperatureOperation.Heated)
            };

            new ComplexRecipe(MakeRecipeID(ID, inputs, output), inputs, output)
            {
                time = cooktime,
                description = desc,
                nameDisplay = RecipeNameDisplay.Result,
                fabricators = new List<Tag>(1) { new Tag(ID) },
                sortOrder = sortod
            };
        }

        
        public override void DoPostConfigureComplete(GameObject go)
        {
        }
    }
}
