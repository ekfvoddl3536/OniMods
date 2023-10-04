// MIT License
//
// Copyright (c) 2022-2023. Super Comic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
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

            res.ViewMode = OverlayModes.Power.ID;

            return res;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;

            var cf = go.AddOrGet<InOneCockingStationEX>();
            cf.heatedTemperature = 368.15f;
            cf.storeProduced = false;
            
            go.AddOrGet<FabricatorIngredientStatusManager>();

            go.AddOrGet<CopyBuildingSettings>();

            go.AddOrGet<ComplexFabricatorWorkable>().overrideAnims = new KAnimFile[1] 
            { 
                Assets.GetAnim("anim_interacts_cookstation_kanim") 
            };

            cf.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;

            Prioritizable.AddRef(go);

            ConfigureRecipes();

            StorageSetup(go, cf);

            var def = go.AddOrGetDef<InOneRefrigeratorController.Def>();
            def.simulatedInternalTemperature = 255.15f; // -18℃
            def.simulatedThermalConductivity = 8192f;
            def.powerSaverEnergyUsage = 20f;
            def.coolingHeatKW = 0.125f;
            def.steadyHeatKW = 0;

            go.AddOrGet<UserNameable>();
            go.AddOrGet<DropAllWorkable>();

            go.AddOrGetDef<PoweredController.Def>();

            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.CookTop);
        }

        protected static void StorageSetup(GameObject go, ComplexFabricator cf)
        {
            var st = go.AddOrGet<Storage>();
            st.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
            st.capacityKg = STORED_MAX;
            st.fetchCategory = Storage.FetchCategory.GeneralStorage;
            st.storageFilters = STORAGEFILTERS.FOOD;
            st.showInUI = st.showDescriptor = st.allowItemRemoval = st.showCapacityStatusItem = true;

            var inSt = go.AddComponent<Storage>();
            inSt.showInUI = true;
            inSt.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);

            cf.inStorage = inSt;

            var buildSt = go.AddComponent<Storage>();
            buildSt.showInUI = true;
            buildSt.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);

            cf.buildStorage = buildSt;

            cf.outStorage = st;

            go.AddOrGet<TreeFilterable>();
            go.AddOrGet<FoodStorage>().storage = st;
            go.AddOrGet<InOneRefrigerator>().storage = st;
        }

        private static void ConfigureRecipes()
        {
            AddRecipe(PICKLEDMEAL.RECIPEDESC, 21, PickledMealConfig.ID, new[]
            {
                new re_(BasicPlantFoodConfig.ID, 3f)
            },
            30f);

            AddRecipe(FRIEDMUSHBAR.RECIPEDESC, 1, FriedMushBarConfig.ID, new[]
            {
                new re_(MushBarConfig.ID, 1f)
            });

            AddRecipe(FRIEDMUSHROOM.RECIPEDESC, 20, FriedMushroomConfig.ID, new[]
            {
                new re_(MushroomConfig.ID, 1f)
            });

            AddRecipe(COOKEDMEAT.RECIPEDESC, 21, CookedMeatConfig.ID, new[]
            {
                new re_(MeatConfig.ID, 2f)
            });

            AddRecipe(COOKEDMEAT.RECIPEDESC, 22, CookedFishConfig.ID, new[]
            {
                new re_(FishMeatConfig.ID, 1f),
            });

            AddRecipe(COOKEDMEAT.RECIPEDESC, 22, CookedFishConfig.ID, new[]
            {
                new re_(ShellfishMeatConfig.ID, 1f)
            });

            AddRecipe(GRILLEDPRICKLEFRUIT.RECIPEDESC, 20, GrilledPrickleFruitConfig.ID, new[]
            {
                new re_(PrickleFruitConfig.ID, 1f)
            });

            AddRecipe(COLDWHEATBREAD.RECIPEDESC, 50, ColdWheatBreadConfig.ID, new[]
            {
                new re_(ColdWheatConfig.SEED_ID, 3f)
            });

            AddRecipe(COOKEDEGG.RECIPEDESC, 1, CookedEggConfig.ID, new[]
            {
                new re_(RawEggConfig.ID, 1f)
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
