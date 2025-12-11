// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using TUNING;
using UnityEngine;

namespace AdvancedGenerators;

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
        cd.elementFilter = [SimHashes.Helium];

        var g = go.AddOrGet<AdvancedEnergyGenerator>();
        g.powerDistributionOrder = 8;
        g.InStorage = ist;
        g.OutStorage = ist;
        g.InOutItems = new EnergyGenerator.Formula
        {
            inputs =
            [
                new EnergyGenerator.InputItem(new Tag(nameof(SimHashes.LiquidHydrogen)), USE_HYDROGEN, 1f)
            ],
            outputs =
            [
                new EnergyGenerator.OutputItem(SimHashes.Helium, EMIT_HELIUM, true, HELIUM_TEMP)
            ],
        };

        Tinkerable.MakePowerTinkerable(go);
        go.AddOrGetDef<PoweredActiveController.Def>();
    }
}
