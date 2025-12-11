// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using TUNING;
using UnityEngine;

namespace AdvancedGenerators;

using static GlobalConsts;
using static GlobalConsts.RefinedCarbonGenerator;
public sealed class RefinedCarbonGeneratorConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        var res =
            BuildingTemplates.CreateBuildingDef(
                ID, 3, 3, AnimSTR, HITPT, CONSTRUCT_TIME,
                MateMassKg, Materials,
                MELT_PT, BuildLocationRule.OnFloor, DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER5);

        res.GeneratorWattageRating = WATT;
        res.GeneratorBaseCapacity = WATT;

        res.ExhaustKilowattsWhenActive = HEAT_EXHAUST;
        res.SelfHeatKilowattsWhenActive = HEAT_SELF;

        res.ViewMode = OverlayModes.Power.ID;

        res.AudioCategory = AU_HOLLOWMETAL;
        res.AudioSize = "large";

        res.RequiresPowerOutput = true;

        res.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(default);

        return res;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

        var tag = GameTagExtensions.Create(SimHashes.RefinedCarbon);

        var eg = go.AddOrGet<EnergyGenerator>();
        eg.formula = new EnergyGenerator.Formula
        {
            inputs =
            [
                new EnergyGenerator.InputItem(tag, CARBONE_BURN_RATE, CARBONE_CAPACITY)
            ],
            outputs =
            [
                new EnergyGenerator.OutputItem(SimHashes.CarbonDioxide, CO2_GEN_RATE, false, new CellOffset(1, 0), OUT_CO2_TEMP)
            ],
        };
        eg.powerDistributionOrder = 8;

        var st = go.AddOrGet<Storage>();
        st.capacityKg = CARBONE_CAPACITY;
        st.showInUI = true;

        go.AddOrGet<LoopingSounds>();
        Prioritizable.AddRef(go);

        var mdkg = go.AddOrGet<ManualDeliveryKG>();
        mdkg.allowPause = false;
        mdkg.SetStorage(st);
        mdkg.RequestedItemTag = tag;
        mdkg.capacity = CARBONE_CAPACITY;
        mdkg.refillMass = REFILL_CAPACITY;
        mdkg.choreTypeIDHash = Db.Get().ChoreTypes.PowerFetch.IdHash;

        Tinkerable.MakePowerTinkerable(go);
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        go.AddOrGet<LogicOperationalController>();
        go.AddOrGetDef<PoweredActiveController.Def>();
    }
}
