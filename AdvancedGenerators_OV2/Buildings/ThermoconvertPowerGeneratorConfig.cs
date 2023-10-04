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
using static TUNING.BUILDINGS;

namespace AdvancedGenerators
{
    using static GlobalConsts.ThermoconvertGenerator;
    public sealed class ThermoconvertPowerGeneratorConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID, 2, 2, ANISTR, 
                HITPOINTS.TIER2, CONSTRUCTION_TIME_SECONDS.TIER4,
                MassKG, Materials, 
                MELTING_POINT_KELVIN.TIER1, 
                BuildLocationRule.OnFloor, 
                DECOR.NONE, TUNING.NOISE_POLLUTION.NONE);

            def.GeneratorWattageRating = WATT;
            def.GeneratorBaseCapacity = WATT;

            def.ExhaustKilowattsWhenActive = HEAT_EXHAUST;
            def.SelfHeatKilowattsWhenActive = HEAT_SELF;

            def.Overheatable = false;

            def.ThermalConductivity = CONDUCTIVITY;

            def.InputConduitType = ConduitType.Liquid;
            def.OutputConduitType = ConduitType.Liquid;

            def.UtilityInputOffset = new CellOffset(0, 1);
            def.UtilityOutputOffset = new CellOffset(1, 0);

            def.PermittedRotations = PermittedRotations.FlipH;

            def.ViewMode = OverlayModes.LiquidConduits.ID;

            def.AudioCategory = TUNING.AUDIO.CATEGORY.METAL;

            def.RequiresPowerOutput = true;
            def.PowerOutputOffset = default;

            def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            const float CAPACITY = 10f;

            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

            go.AddOrGet<LoopingSounds>();
            Prioritizable.AddRef(go);

            var inSt = go.AddOrGet<Storage>();
            inSt.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
            inSt.capacityKg = CAPACITY;
            inSt.showInUI = true;

            var otSt = go.AddComponent<Storage>();
            otSt.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
            otSt.capacityKg = CAPACITY;
            otSt.showInUI = true;

            var cc = go.AddOrGet<ConduitConsumer>();
            cc.conduitType = ConduitType.Liquid;
            cc.storage = inSt;
            cc.consumptionRate = CAPACITY;
            cc.capacityKG = CAPACITY;
            cc.capacityTag = GameTags.Liquid;
            cc.alwaysConsume = true;
            cc.forceAlwaysSatisfied = true;
            cc.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

            var cd = go.AddOrGet<ConduitDispenser>();
            cd.storage = otSt;
            cd.conduitType = ConduitType.Liquid;
            cd.elementFilter = null;
            cd.alwaysDispense = true;

            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = TagManager.Create(ID);

            var tpg = go.AddComponent<ThermoconvertPowerGenerator>();
            tpg.HasMeter = false;
            tpg.inStorage = inSt;
            tpg.outStorage = otSt;
            tpg.convert_kg = CAPACITY;
            tpg.target_temp = 293.15f;
            tpg.powerDistributionOrder = 8;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<LogicOperationalController>();

            Tinkerable.MakePowerTinkerable(go);

            go.AddOrGetDef<PoweredActiveController.Def>();
        }
    }
}
