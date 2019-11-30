using TUNING;
using UnityEngine;

namespace AdvancedGeneratos
{
    using static Constans;
    using static Constans.NaphthaGenerator;
    public class NaphthaGeneratorConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef bd = BuildingTemplates.CreateBuildingDef(ID, 3, 4, ANISTR, 100, 100,
                MateMassKg, Materials, 2400, BuildLocationRule.OnFloor, DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER4);

            bd.GeneratorWattageRating = bd.GeneratorBaseCapacity = Watt;

            bd.ExhaustKilowattsWhenActive = Heat_Exhaust;
            bd.SelfHeatKilowattsWhenActive = Heat_Self;

            bd.ViewMode = OverlayModes.Power.ID;

            bd.AudioCategory = AU_METAL;

            bd.UtilityInputOffset = new CellOffset(-1, 0);
            bd.PowerOutputOffset = new CellOffset(0, 0);

            bd.RequiresPowerOutput = true;

            bd.InputConduitType = ConduitType.Liquid;

            return bd;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go) => RPort(go);

        public override void DoPostConfigureUnderConstruction(GameObject go) => RPort(go);

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
            go.AddOrGet<LoopingSounds>();

            Storage st = go.AddOrGet<Storage>();
            st.showInUI = true;

            ConduitConsumer cc = go.AddOrGet<ConduitConsumer>();
            cc.conduitType = ConduitType.Liquid;
            cc.consumptionRate = 10;
            cc.capacityKG = 100;
            cc.forceAlwaysSatisfied = true;
            cc.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

            ElementConsumer ec = go.AddOrGet<ElementConsumer>();
            ec.storage = st;
            ec.configuration = ElementConsumer.Configuration.Element;
            ec.elementToConsume = SimHashes.Oxygen;
            ec.capacityKG = Oxygen_MaxStored;
            ec.consumptionRate = OxygenCosumRate;
            ec.consumptionRadius = 2;
            ec.isRequired = ec.storeOnConsume = true;

            ConsumGasPowerGenerator aeg = go.AddOrGet<ConsumGasPowerGenerator>();
            aeg.InStorage = st;
            aeg.OutStorage = go.AddComponent<Storage>();
            aeg.Consumer = ec;
            aeg.InOutItems = new EnergyGenerator.Formula
            {
                inputs = new EnergyGenerator.InputItem[]
                {
                    new EnergyGenerator.InputItem(SimHashes.Naphtha.CreateTag(), UseNaphtha, Naphtha_MaxStored),
                    new EnergyGenerator.InputItem(SimHashes.Oxygen.CreateTag(), ExhaustCO2, Oxygen_MaxStored)
                },
                outputs = new EnergyGenerator.OutputItem[]
                {
                    new EnergyGenerator.OutputItem(SimHashes.CarbonDioxide, ExhaustCO2, false, new CellOffset(0, 0), 320.15f)
                }
            };

            Tinkerable.MakePowerTinkerable(go);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            RPort(go);
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGetDef<PoweredActiveController.Def>();
        }

        protected void RPort(GameObject go) =>
            GeneratedBuildings.RegisterLogicPorts(go, INPUT_PORT_00);
    }
}
