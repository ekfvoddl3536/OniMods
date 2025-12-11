// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine;
using static TUNING.BUILDINGS;

namespace AdvancedGenerators;

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
        def.Floodable = false;

        def.ThermalConductivity = CONDUCTIVITY;

        def.InputConduitType = ConduitType.Liquid;
        def.OutputConduitType = ConduitType.Liquid;

        def.UtilityInputOffset = InputOffset;
        def.UtilityOutputOffset = OutputOffset;

        def.PermittedRotations = PermittedRotations.FlipH;

        def.ViewMode = OverlayModes.LiquidConduits.ID;

        def.AudioCategory = TUNING.AUDIO.CATEGORY.METAL;

        def.RequiresPowerOutput = true;
        def.PowerOutputOffset = default;

        def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(InputOffset);
        // def.LogicInputPorts = [
        //     LogicPorts.Port.RibbonInputPort(LogicConduitModeController.PORT_ID, InputOffset, 
        //         Strings.Get(LOGIC_MODE_INPUT), 
        //         Strings.Get(LOGIC_MODE_INPUT_ACTIVE), 
        //         Strings.Get(LOGIC_MODE_INPUT_INACTIVE))
        // ];

        return def;
    }

    // private static void AttachPort(GameObject go)
    // {
    //     go.AddComponent<ConduitSecondaryInput>().portInfo = PORT_GAS_IN;
    //     go.AddComponent<ConduitSecondaryInput>().portInfo = PORT_LIQUID_IN;
    //     go.AddComponent<ConduitSecondaryInput>().portInfo = PORT_SOLID_IN;
    // 
    //     go.AddComponent<ConduitSecondaryOutput>().portInfo = PORT_GAS_OUT;
    //     go.AddComponent<ConduitSecondaryOutput>().portInfo = PORT_LIQUID_OUT;
    //     go.AddComponent<ConduitSecondaryOutput>().portInfo = PORT_SOLID_OUT;
    // }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        // const float SOLID_FLOW = LogicConduitModeController.SolidFlow;
        // const float LIQUID_FLOW = LogicConduitModeController.LiquidFlow;
        // const float GAS_FLOW = LogicConduitMode.GasFlow;
        const float CAPACITY = ConduitFlow.MAX_LIQUID_MASS;

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

        var cd = go.AddOrGet<ConduitDispenser>();
        cd.storage = otSt;
        cd.conduitType = ConduitType.Liquid;
        cd.alwaysDispense = true;
        cd.elementFilter = null;

        go.AddOrGet<CopyBuildingSettings>().copyGroupTag = TagManager.Create(ID);

        var tpg = go.AddComponent<ThermoconvertPowerGenerator>();
        tpg.HasMeter = false;
        tpg.inStorage = inSt;
        tpg.outStorage = otSt;
        tpg.target_temp = 293.15f;
        tpg.powerDistributionOrder = 8;
        tpg.UseCopySettings = true;

        // var m = go.AddComponent<LogicConduitModeController>();
        // m.InputOffset = InputOffset;
        // m.OutputOffset = OutputOffset;
        // m.InputStorage = inSt;
        // m.OutputStorage = otSt;
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        go.AddOrGet<LogicOperationalController>();

        Tinkerable.MakePowerTinkerable(go);

        go.AddOrGetDef<PoweredActiveController.Def>();
    }

    // public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
    // {
    //     AttachPort(go);
    // }
    // 
    // public override void DoPostConfigureUnderConstruction(GameObject go)
    // {
    //     AttachPort(go);
    // }
}
