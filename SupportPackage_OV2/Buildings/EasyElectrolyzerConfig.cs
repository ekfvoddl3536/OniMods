// MIT License
//
// Copyright (c) 2022-2023. Super Comic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using TUNING;
using UnityEngine;

namespace SupportPackage
{
    using static GlobalConsts;
    using static GlobalConsts.EasyElectrolyzer;
	using ce_ = ElementConverter.ConsumedElement;
	using oe_ = ElementConverter.OutputElement;
	public class EasyElectrolyzerConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
			BuildingDef res =
				BuildingTemplates.CreateBuildingDef(
					ID, 2, 2, ANISTR,
					DEF_HITPT, DEF_CON_TIME,
					TMass,
					TMates,
					DEF_MELPT, BuildLocationRule.OnFloor, 
					BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER3);

			res.RequiresPowerInput = true;
			res.PowerInputOffset = new CellOffset(1, 0);

			res.EnergyConsumptionWhenActive = USE_POWER;

			res.UtilityOutputOffset = new CellOffset(1, 0);
			res.OutputConduitType = ConduitType.Gas;

			res.UtilityInputOffset = default; // (0, 0)
			res.InputConduitType = ConduitType.Liquid;

			res.ExhaustKilowattsWhenActive = HEAT_EXHAUS;
			res.SelfHeatKilowattsWhenActive = HEAT_SELF;

			res.ViewMode = OverlayModes.Oxygen.ID;

			res.AudioCategory = "HollowMetal";

			res.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));

			return res;
		}

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

			var ec = go.AddOrGet<Electrolyzer>();
			ec.maxMass = 1.8f;
			ec.hasMeter = true;

			var cc = go.AddOrGet<ConduitConsumer>();
			cc.conduitType = ConduitType.Liquid;
			cc.capacityTag = new Tag(nameof(SimHashes.Water));
			cc.consumptionRate = WATER_USE_RATE;
			cc.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

			var cd = go.AddOrGet<ConduitDispenser>();
			cd.alwaysDispense = true;
			cd.conduitType = ConduitType.Gas;

			var ev = go.AddOrGet<ElementConverter>();
			ev.consumedElements = new[]
			{
				new ce_(new Tag(nameof(SimHashes.Water)), WATER_USE_RATE)
			};
			ev.outputElements = new[]
			{
				new oe_(O2_RATE, SimHashes.Oxygen, 0, false, false, 0f, 1f),
				new oe_(H2_RATE, SimHashes.Hydrogen, 0, false, true, 0f, 1f)
			};

			Prioritizable.AddRef(go);
		}

        public override void DoPostConfigureComplete(GameObject go)
        {
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGetDef<PoweredActiveController.Def>();
        }
    }
}
