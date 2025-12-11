// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using TUNING;
using UnityEngine;

namespace AdvancedGenerators;

using static GlobalConsts;
using static GlobalConsts.ThermoelectricGenerator;
public sealed class ThermoelectricGeneratorConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        BuildingDef bd = BuildingTemplates.CreateBuildingDef(ID, 4, 3, ANISTR, 100, 45,
            MateMassKg, Materials, MELT_PT, BuildLocationRule.OnFloor, DECOR.BONUS.TIER0, NOISE_POLLUTION.NOISY.TIER6);

        bd.GeneratorWattageRating = WATT;
        bd.GeneratorBaseCapacity = WATT;

        bd.ExhaustKilowattsWhenActive = HEAT_EXHAUST;
        bd.SelfHeatKilowattsWhenActive = HEAT_SELF;

        bd.OverheatTemperature = MAXIMUM_TEMP;

        bd.ViewMode = OverlayModes.Temperature.ID;

        bd.AudioCategory = "Metal";

        bd.PowerOutputOffset = new CellOffset(1, 0);
        bd.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(-1, 0));

        bd.RequiresPowerOutput = true;

        return bd;
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        go.AddOrGet<LogicOperationalController>();

        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

        go.AddOrGet<LoopingSounds>();

        go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = MINIMUM_TEMP;

        var gen = go.AddOrGet<ThermoelectricPowerGenerator>();
        gen.powerDistributionOrder = 8;

        Tinkerable.MakePowerTinkerable(go);

        go.AddOrGetDef<PoweredActiveController.Def>();
    }
}
