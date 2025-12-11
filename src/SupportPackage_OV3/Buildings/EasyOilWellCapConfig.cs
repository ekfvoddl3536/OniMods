// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#if PRIVATE_CORE
using UnityEngine;
using static TUNING.BUILDINGS;

namespace SupportPackage;

using static GlobalConsts.EasyOilWellCap;
public sealed class EasyOilWellCapConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        var def = BuildingTemplates.CreateBuildingDef(
            ID, 4, 4, ANISTR,
            HITPOINTS.TIER2, CONSTRUCTION_TIME_SECONDS.TIER4,
            TMass, TMate,
            MELTING_POINT_KELVIN.TIER3,
            BuildLocationRule.Anywhere,
            TUNING.DECOR.NONE,
            TUNING.NOISE_POLLUTION.NOISY.TIER2);

        BuildingTemplates.CreateElectricalBuildingDef(def);

        def.SceneLayer = Grid.SceneLayer.BuildingFront;
        def.ViewMode = OverlayModes.LiquidConduits.ID;

        def.EnergyConsumptionWhenActive = POWER_USE;
        
        def.SelfHeatKilowattsWhenActive = 0;
        def.ExhaustKilowattsWhenActive = 0;

        def.InputConduitType = ConduitType.Liquid;
        def.OutputConduitType = ConduitType.Gas;

        def.UtilityInputOffset = new CellOffset(0, 1);
        def.UtilityOutputOffset = new CellOffset(1, 1);

        def.PowerInputOffset = new CellOffset(1, 1);

        def.OverheatTemperature = OVERHEAT_TEMP;
        
        def.Floodable = false;
        def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(default);

        def.AttachmentSlotTag = GameTags.OilWell;
        def.BuildLocationRule = BuildLocationRule.BuildingAttachPoint;
        def.ObjectLayer = ObjectLayer.AttachableBuilding;

        return def;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        var waterTag = SimHashes.Water.CreateTag();

        go.AddOrGet<LoopingSounds>();

        BuildingTemplates.CreateDefaultStorage(go).showInUI = true;

        var cc = go.AddOrGet<ConduitConsumer>();
        cc.conduitType = ConduitType.Liquid;
        cc.consumptionRate = 2f;
        cc.capacityKG = 10f;
        cc.capacityTag = waterTag;
        cc.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

        var ec = go.AddOrGet<ElementConverter>();
        ec.consumedElements = 
        [
            new ElementConverter.ConsumedElement(waterTag, 0.01f)
        ];
        ec.outputElements =
        [
            new ElementConverter.OutputElement(OIL_RATIO, SimHashes.CrudeOil, OIL_TEMP, outputElementOffsetx: 2f, outputElementOffsety: 1.5f, diseaseWeight: 0f)
        ];

        var owc = go.AddOrGet<OilWellCap>();
        owc.gasElement = SimHashes.Methane;
        owc.gasTemperature = GAS_TEMP;
        owc.addGasRate = 0.0333333351f;
        owc.maxGasPressure = 125f;
        owc.releaseGasRate = 0.5f;

        var cd = go.AddOrGet<ConduitDispenser>();
        cd.conduitType = ConduitType.Gas;
        cd.alwaysDispense = true;

        go.AddOrGet<RequireInputs>().requireConduitHasMass = false;
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        go.AddOrGet<LogicOperationalController>();
    }
}
#endif