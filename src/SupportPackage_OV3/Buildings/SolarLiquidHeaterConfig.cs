// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#if PRIVATE_CORE
using UnityEngine;
using static TUNING.BUILDINGS;

namespace SupportPackage;

using static GlobalConsts.SolarLiquidHeater;
public sealed class SolarLiquidHeaterConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        var def = BuildingTemplates.CreateBuildingDef(
            ID, 7, 3, ANISTR,
            HITPOINTS.TIER2, CONSTRUCTION_TIME_SECONDS.TIER4,
            TMass, TMate,
            MELTING_POINT_KELVIN.TIER2,
            BuildLocationRule.Anywhere,
            DECOR.BONUS.TIER2,
            TUNING.NOISE_POLLUTION.NONE);

        def.ExhaustKilowattsWhenActive = 0.0f;
        def.SelfHeatKilowattsWhenActive = 0.0f;

        def.ViewMode = OverlayModes.LiquidConduits.ID;

        def.InputConduitType = ConduitType.Liquid;
        def.OutputConduitType = ConduitType.Liquid;

        def.UtilityInputOffset = InputOffset;
        def.UtilityOutputOffset = OutputOffset;

        def.AudioCategory = TUNING.AUDIO.CATEGORY.HOLLOW_METAL;
        def.AudioSize = TUNING.AUDIO.SIZE.LARGE;

        def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(default);
        return def;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        const float CAPACITY = ConduitFlow.MAX_LIQUID_MASS;

        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

        go.AddOrGet<LoopingSounds>();
        Prioritizable.AddRef(go);

        go.AddOrGet<CopyBuildingSettings>().copyGroupTag = TagManager.Create(ID);

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
        cd.useSecondaryOutput = false;
        cd.elementFilter = null;

        var cd2 = go.AddComponent<ConduitDispenser>();
        cd2.storage = otSt;
        cd2.conduitType = ConduitType.Gas;
        cd2.alwaysDispense = true;
        cd2.useSecondaryOutput = true;
        cd2.elementFilter = null;

        var slh = go.AddComponent<SolarLiquidBoiler>();
        slh.inStorage = inSt;
        slh.outStorage = otSt;
        slh.target_temp = MIN_TEMP;
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        go.AddOrGet<LogicOperationalController>();
        go.AddOrGetDef<ActiveController.Def>();

        var def = go.AddOrGetDef<MakeBaseSolid.Def>();
        def.occupyFoundationLayer = false;

        var solids = new CellOffset[7];
        def.solidOffsets = solids;

        for (int i = 0; i < solids.Length; ++i)
            solids[i] = new CellOffset(i - 3, 0);
    }

    public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
    {
        DoPostConfigureUnderConstruction(go);
    }

    public override void DoPostConfigureUnderConstruction(GameObject go)
    {
        go.AddComponent<ConduitSecondaryOutput>().portInfo = new ConduitPortInfo(ConduitType.Gas, GasOutputOffset);
    }
}
#endif
