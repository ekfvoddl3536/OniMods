using TUNING;
using UnityEngine;

namespace AdvancedGeneratos
{
    using static Constans;
    using static Constans.EcoFriendlyMethaneGenerator;
    public class EcoFriendlyMethaneGeneratorConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef bd = BuildingTemplates.CreateBuildingDef(ID, 4, 3, ANISTR, 100, 60,
                TMass, TMate, 2400, BuildLocationRule.OnFloor, DECOR.BONUS.TIER1, NOISE_POLLUTION.NOISY.TIER6);

            bd.GeneratorBaseCapacity = bd.GeneratorWattageRating = Watt;
            bd.ExhaustKilowattsWhenActive = Heat_Exhaust;
            bd.SelfHeatKilowattsWhenActive = Heat_Self;

            bd.ViewMode = OverlayModes.Power.ID;
            bd.AudioCategory = AU_METAL;

            bd.PowerOutputOffset = bd.UtilityInputOffset = new CellOffset(0, 0);
            bd.UtilityOutputOffset = new CellOffset(2, 2);

            bd.PermittedRotations = PermittedRotations.FlipH;

            bd.InputConduitType = bd.OutputConduitType = ConduitType.Gas;

            return bd;
        }

        public override void DoPostConfigureUnderConstruction(GameObject go) => R(go);

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go) => R(go);

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
            go.AddOrGet<LoopingSounds>();

            Storage ist = go.AddOrGet<Storage>(), ost = go.AddComponent<Storage>();

            ConduitConsumer cc = go.AddOrGet<ConduitConsumer>();
            cc.storage = ist;
            cc.conduitType = ConduitType.Gas;
            cc.consumptionRate = 0.2f;
            cc.capacityKG = 1;
            cc.capacityTag = SimHashes.Methane.CreateTag();
            cc.forceAlwaysSatisfied = true;
            cc.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

            ConduitDispenser cd = go.AddOrGet<ConduitDispenser>();
            cd.conduitType = ConduitType.Gas;
            cd.storage = ost;

            ElementConsumer ec = go.AddOrGet<ElementConsumer>();
            ec.storage = ist;
            ec.configuration = ElementConsumer.Configuration.Element;
            ec.elementToConsume = SimHashes.Oxygen;
            ec.capacityKG = Oxygen_MaxStored;
            ec.consumptionRate = OxygenCosumRate;
            ec.consumptionRadius = 2;
            ec.isRequired = ec.storeOnConsume = true;

            ManualDeliveryKG mdkg = go.AddOrGet<ManualDeliveryKG>();
            mdkg.SetStorage(ist);
            mdkg.allowPause = false;
            mdkg.capacity = FilterMaxStored;
            mdkg.refillMass = 10;
            mdkg.requestedItemTag = Filter;
            mdkg.choreTypeIDHash = Db.Get().ChoreTypes.GeneratePower.Id;

            ConsumGasPowerGenerator adg = go.AddOrGet<ConsumGasPowerGenerator>();
            adg.Consumer = ec;
            adg.InOutItems = new EnergyGenerator.Formula
            {
                inputs = new[]
                {
                    new EnergyGenerator.InputItem(cc.capacityTag, UseMethane, 1),
                    new EnergyGenerator.InputItem(SimHashes.Oxygen.CreateTag(), ExhaustCO2, Oxygen_MaxStored),
                    new EnergyGenerator.InputItem(Filter, ExhaustH2O, 50)
                },
                outputs = new[]
                {
                    new EnergyGenerator.OutputItem(SimHashes.Water, ExhaustH2O, false, new CellOffset(1, 1), 348.15f),
                    new EnergyGenerator.OutputItem(SimHashes.CarbonDioxide, ExhaustCO2, true, 383.15f)
                }
            };
            adg.InStorage = ist;
            adg.OutStorage = ost;

            Tinkerable.MakeFarmTinkerable(go);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            R(go);
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGetDef<PoweredActiveController.Def>();
        }

        private void R(GameObject go) =>
            GeneratedBuildings.RegisterLogicPorts(go, INPUT);
    }
}
