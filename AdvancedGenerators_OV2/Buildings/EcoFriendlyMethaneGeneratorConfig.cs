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

using TUNING;
using UnityEngine;

namespace AdvancedGenerators
{
    using static GlobalConsts;
    using static GlobalConsts.EcoFriendlyMethaneGenerator;
    public class EcoFriendlyMethaneGeneratorConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef bd = BuildingTemplates.CreateBuildingDef(ID, 4, 3, ANISTR, 100, 60,
                MateMassKg, Materials, 2400, BuildLocationRule.OnFloor, DECOR.BONUS.TIER1, NOISE_POLLUTION.NOISY.TIER6);

            bd.GeneratorBaseCapacity = WATT;
            bd.GeneratorWattageRating = WATT;

            bd.ExhaustKilowattsWhenActive = HEAT_EXHAUST;
            bd.SelfHeatKilowattsWhenActive = HEAT_SELF;

            bd.ViewMode = OverlayModes.Power.ID;
            bd.AudioCategory = AU_METAL;

            bd.PowerOutputOffset = default; // (0, 0)
            bd.UtilityInputOffset = default; // (0, 0)

            bd.UtilityOutputOffset = new CellOffset(2, 2);

            bd.PermittedRotations = PermittedRotations.FlipH;

            bd.InputConduitType = ConduitType.Gas;
            bd.OutputConduitType = ConduitType.Gas;

            bd.RequiresPowerOutput = true;

            bd.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(default);

            return bd;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

            go.AddOrGet<LoopingSounds>();
            Prioritizable.AddRef(go);

            var ist = go.AddOrGet<Storage>();
            var ost = go.AddComponent<Storage>();

            var cc = go.AddOrGet<ConduitConsumer>();
            cc.storage = ist;
            cc.conduitType = ConduitType.Gas;
            cc.consumptionRate = 0.2f;
            cc.capacityKG = 1;
            cc.capacityTag = SimHashes.Methane.CreateTag();
            cc.forceAlwaysSatisfied = true;
            cc.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

            var cd = go.AddOrGet<ConduitDispenser>();
            cd.conduitType = ConduitType.Gas;
            cd.storage = ost;

            var ec = go.AddOrGet<ElementConsumer>();
            ec.storage = ist;
            ec.configuration = ElementConsumer.Configuration.Element;
            ec.elementToConsume = SimHashes.Oxygen;
            ec.capacityKG = OXYGEN_MAXSTORED;
            ec.consumptionRate = OXYGEN_CONSUM_RATE;
            ec.consumptionRadius = 2;
            ec.isRequired = ec.storeOnConsume = true;

            var adg = go.AddOrGet<ConsumGasPowerGenerator>();
            adg.InStorage = ist;
            adg.OutStorage = ost;
            adg.Consumer = ec;
            adg.InOutItems = new EnergyGenerator.Formula
            {
                inputs = new[]
                {
                    new EnergyGenerator.InputItem(cc.capacityTag, METHANE_CONSUM_RATE, 1),
                    new EnergyGenerator.InputItem(SimHashes.Oxygen.CreateTag(), EXHAUST_CO2_RATE, OXYGEN_MAXSTORED),
                },
                outputs = new[]
                {
                    new EnergyGenerator.OutputItem(SimHashes.Water, EXHAUST_H2O_RATE, false, new CellOffset(1, 1), 348.15f),
                    new EnergyGenerator.OutputItem(SimHashes.CarbonDioxide, EXHAUST_CO2_RATE, true, 383.15f)
                }
            };

            Tinkerable.MakePowerTinkerable(go);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGetDef<PoweredActiveController.Def>();
        }
    }
}
