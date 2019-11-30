using TUNING;
using UnityEngine;

namespace AdvancedGeneratos
{
    using static Constans;
    using static Constans.ThermoelectricGenerator;
    public class ThermoelectricGeneratorConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef bd = BuildingTemplates.CreateBuildingDef(ID, 4, 3, ANISTR, 100, 45,
                MateMassKg, Materials, MeltPT, BuildLocationRule.OnFloor, DECOR.BONUS.TIER0, NOISE_POLLUTION.NOISY.TIER6);

            bd.GeneratorWattageRating = bd.GeneratorBaseCapacity = Watt;

            bd.ExhaustKilowattsWhenActive = Heat_Exhaust;
            bd.SelfHeatKilowattsWhenActive = Heat_Self;

            bd.ViewMode = OverlayModes.Temperature.ID;

            bd.AudioCategory = "Metal";

            bd.PowerOutputOffset = new CellOffset(1, 0);

            return bd;
        }

        public override void DoPostConfigureUnderConstruction(GameObject go) => Register(go);

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go) => Register(go);

        public override void DoPostConfigureComplete(GameObject go)
        {
            Register(go);
            go.AddOrGet<LogicOperationalController>();
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

            go.AddOrGet<LoopingSounds>();

            go.AddOrGet<ThermoelectricPowerGenerator>();
            go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = MinimumTemp;

            Tinkerable.MakePowerTinkerable(go);
            go.AddOrGetDef<PoweredActiveController.Def>();
        }

        protected void Register(GameObject go) =>
            GeneratedBuildings.RegisterLogicPorts(go, INPUT_PORTS);
    }
}
