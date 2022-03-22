#region LICENSE
/*
MIT License

Copyright (c) 2022. Super Comic (ekfvoddl3535@naver.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion
using UnityEngine;
using TUNING;
using SuperComicLib.ModONI;

namespace AdvancedGenerators
{
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

            return bd;
        }

        public override void DoPostConfigureUnderConstruction(GameObject go) =>
            BuildingLogicPorts.RegisterSingleInput(go, new CellOffset(1, 0));

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go) =>
            BuildingLogicPorts.RegisterSingleInput(go, new CellOffset(1, 0));

        public override void DoPostConfigureComplete(GameObject go)
        {
            BuildingLogicPorts.RegisterSingleInput(go, new CellOffset(1, 0));

            go.AddOrGet<LogicOperationalController>();
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

            go.AddOrGet<LoopingSounds>();

            go.AddOrGet<ThermoelectricPowerGenerator>();
            go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = MINIMUM_TEMP;

            Tinkerable.MakePowerTinkerable(go);
            go.AddOrGetDef<PoweredActiveController.Def>();
        }
    }
}
