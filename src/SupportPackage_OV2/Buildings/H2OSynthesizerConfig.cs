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
	using static GlobalConsts.H2OSynthesizer;
	using ce_ = ElementConverter.ConsumedElement;
	using oe_ = ElementConverter.OutputElement;
	public class H2OSynthesizerConfig : IBuildingConfig
	{
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef res = BuildingTemplates.CreateBuildingDef(
				ID, 2, 3, ANISTR, 
				100, 120f, 
				TMass, TMate, 
				DEF_MELPT * 2f, BuildLocationRule.OnFloor, 
				DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0);

			res.RequiresPowerInput = true;
			res.EnergyConsumptionWhenActive = USE_POWER;

			res.UtilityInputOffset = new CellOffset(1, 0);
			res.InputConduitType = ConduitType.Gas;

			res.UtilityOutputOffset = new CellOffset(0, 2);
			res.OutputConduitType = ConduitType.Liquid;

			res.ExhaustKilowattsWhenActive = HEAT_EXHAUST;
			res.SelfHeatKilowattsWhenActive = HEAT_SELF;

			res.ViewMode = OverlayModes.GasConduits.ID;

			res.AudioCategory = "HollowMetal";

			res.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));

			return res;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

			go.AddOrGet<Reservoir>();

			var st = go.AddComponent<Storage>();
			st.showDescriptor = st.allowItemRemoval = false;
			st.capacityKg = 1000f;

			var cc = go.AddOrGet<ConduitConsumer>();
			cc.storage = st;
			cc.capacityKG = cc.consumptionRate = 1f;
			cc.capacityTag = new Tag(nameof(SimHashes.Hydrogen));
			cc.conduitType = ConduitType.Gas;

			var ec = go.AddOrGet<ElementConsumer>();
			ec.storage = st;
			ec.capacityKG = ec.consumptionRate = 1f;
			ec.storeOnConsume = ec.showInStatusPanel = true;
			ec.isRequired = ec.showDescriptor = false;
			ec.consumptionRadius = 2;
			ec.sampleCellOffset = Vector3.zero;
			ec.elementToConsume = SimHashes.Oxygen;

			var cd = go.AddOrGet<ConduitDispenser>();
			cd.conduitType = ConduitType.Liquid;
			cd.elementFilter = new[] { SimHashes.Water };
			cd.alwaysDispense = true;

			var conv = go.AddOrGet<ElementConverter>();
			conv.consumedElements = new[]
			{
				new ce_(new Tag(nameof(SimHashes.Hydrogen)), USE_H2),
				new ce_(new Tag(nameof(SimHashes.Oxygen)), USE_O2)
			};
			conv.outputElements = new[]
			{
				new oe_(RATE_H2O, SimHashes.Water, 0f, false, true)
			};

			go.AddOrGet<AirFilter>().filterTag = cc.capacityTag;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LogicOperationalController>();
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}