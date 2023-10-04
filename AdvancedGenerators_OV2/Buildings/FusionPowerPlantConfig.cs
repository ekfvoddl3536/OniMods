// MIT License
//
// Copyright (c) 2022-2023. SuperComic (ekfvoddl3535@naver.com)
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

using UnityEngine;
using TUNING;

namespace AdvancedGenerators
{
    using static GlobalConsts.FusionPowerPlant;
    public sealed class FusionPowerPlantConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef bd = BuildingTemplates.CreateBuildingDef(ID, 5, 4, ANISTR, MY_HITPT, MY_CONTIME,
                MateMassKg, Materials, MY_MELTPT, BuildLocationRule.Anywhere, DECOR.BONUS.TIER0, NOISE_POLLUTION.NONE);

            bd.GeneratorWattageRating = WATT;
            bd.GeneratorBaseCapacity = WATT;

            bd.Overheatable = false;
            bd.Floodable = false;
            bd.AddLogicPowerPort = false;
            bd.UseStructureTemperature = false;

            bd.ModifiesTemperature = true;
            bd.RequiresPowerOutput = true;

            bd.ViewMode = OverlayModes.Power.ID;

            bd.AudioCategory = "Metal";

            bd.PowerOutputOffset = new CellOffset(2, 1);

            bd.UtilityInputOffset = default; // (0, 0)
            bd.UtilityOutputOffset = new CellOffset(1, 1);

            bd.InputConduitType = ConduitType.Liquid;
            bd.OutputConduitType = ConduitType.Gas;

            return bd;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
            go.AddOrGet<LoopingSounds>();

            var ist = go.AddOrGet<Storage>();
            ist.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
            ist.capacityKg = 10f;
            ist.showInUI = true;

            var cc = go.AddOrGet<ConduitConsumer>();
            cc.storage = ist;
            cc.conduitType = ConduitType.Liquid;
            cc.consumptionRate = cc.capacityKG = 1f;
            cc.capacityTag = SimHashes.LiquidHydrogen.CreateTag();
            cc.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
            cc.forceAlwaysSatisfied = true;

            var cd = go.AddOrGet<ConduitDispenser>();
            cd.storage = ist;
            cd.conduitType = ConduitType.Gas;
            cd.elementFilter = new[] { SimHashes.Helium };

            var g = go.AddOrGet<AdvancedEnergyGenerator>();
            g.powerDistributionOrder = 8;
            g.InStorage = ist;
            g.OutStorage = ist;
            g.InOutItems = new EnergyGenerator.Formula
            {
                inputs = new[]
                {
                    new EnergyGenerator.InputItem(new Tag(nameof(SimHashes.LiquidHydrogen)), USE_HYDROGEN, 1f)
                },
                outputs = new[]
                {
                    new EnergyGenerator.OutputItem(SimHashes.Helium, EMIT_HELIUM, true, HELIUM_TEMP)
                },
            };

            Tinkerable.MakePowerTinkerable(go);
            go.AddOrGetDef<PoweredActiveController.Def>();
        }
    }
}
