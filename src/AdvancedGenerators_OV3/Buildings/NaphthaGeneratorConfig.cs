// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using TUNING;
using UnityEngine;

namespace AdvancedGenerators;

using static GlobalConsts;
using static GlobalConsts.NaphthaGenerator;
public class NaphthaGeneratorConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        BuildingDef bd = BuildingTemplates.CreateBuildingDef(ID, 3, 4, ANISTR, 100, 100,
            MateMassKg, Materials, 2400, BuildLocationRule.OnFloor, DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER4);

        bd.GeneratorWattageRating = WATT;
        bd.GeneratorBaseCapacity = WATT;

        bd.ExhaustKilowattsWhenActive = HEAT_EXHAUST;
        bd.SelfHeatKilowattsWhenActive = HEAT_SELF;

        bd.ViewMode = OverlayModes.Power.ID;

        bd.AudioCategory = AU_METAL;

        bd.UtilityInputOffset = new CellOffset(-1, 0);
        bd.PowerOutputOffset = default; // (0, 0)

        bd.RequiresPowerOutput = true;

        bd.InputConduitType = ConduitType.Liquid;
        bd.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(default);

        return bd;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.AddOrGet<LoopingSounds>();

        var st = go.AddOrGet<Storage>();
        st.showInUI = true;

        var cc = go.AddOrGet<ConduitConsumer>();
        cc.conduitType = ConduitType.Liquid;
        cc.consumptionRate = 10;
        cc.capacityKG = 100;
        cc.capacityTag = SimHashes.Naphtha.CreateTag();
        cc.forceAlwaysSatisfied = true;
        cc.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

        // var ec = go.AddOrGet<ElementConsumer>();
        // ec.storage = st;
        // ec.configuration = ElementConsumer.Configuration.Element;
        // ec.elementToConsume = SimHashes.Oxygen;
        // ec.capacityKG = OXYGEN_MAX_STORED;
        // ec.consumptionRate = OXYGEN_CONSUM_RATE;
        // ec.consumptionRadius = 2;
        // ec.isRequired = ec.storeOnConsume = true;

        var aeg = go.AddOrGet<AdvancedEnergyGenerator>();
        aeg.InStorage = st;
        aeg.OutStorage = go.AddComponent<Storage>();
        // aeg.Consumer = ec;
        aeg.InOutItems = new EnergyGenerator.Formula
        {
            inputs =
            [
                new EnergyGenerator.InputItem(cc.capacityTag, CONSUM_NAPHTHA_RATE, NAPHTHA_MAXSTORED),
                // new EnergyGenerator.InputItem(SimHashes.Oxygen.CreateTag(), EXHAUST_CO2_RATE, OXYGEN_MAX_STORED)
            ],
            outputs =
            [
                new EnergyGenerator.OutputItem(SimHashes.CarbonDioxide, EXHAUST_CO2_RATE, false, new CellOffset(0, 0), 320.15f)
            ]
        };

        Tinkerable.MakePowerTinkerable(go);
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        go.AddOrGet<LogicOperationalController>();
        go.AddOrGetDef<PoweredActiveController.Def>();
    }
}
