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
	using static GlobalConsts.OrganicDeoxidizer;
	using ce_ = ElementConverter.ConsumedElement;
	using oe_ = ElementConverter.OutputElement;
	public sealed class OrganicDeoxidizerConfig : IBuildingConfig
	{
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef res = 
				BuildingTemplates.CreateBuildingDef(
					ID, 2, 2, ANISTR, 
					DEF_HITPT, DEF_CON_TIME, 
					TMass, TMate, 
					DEF_MELPT, BuildLocationRule.OnFloor, 
					DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER3);

			res.RequiresPowerInput = true;
			res.PowerInputOffset = new CellOffset(1, 0);

			res.EnergyConsumptionWhenActive = USE_POWER;

			res.ExhaustKilowattsWhenActive = HEAT_EXHAUST;
			res.SelfHeatKilowattsWhenActive = HEAT_SELF;

			res.ViewMode = OverlayModes.Oxygen.ID;

			res.AudioCategory = "HollowMetal";

			res.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));

			return res;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KSelectable>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

			go.AddOrGet<RustDeoxidizer>().maxMass = 2f;

			var st = go.AddOrGet<Storage>();
			st.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			st.showInUI = true;

			var mdkg = go.AddOrGet<ManualDeliveryKG>();
			mdkg.SetStorage(st);
			mdkg.allowPause = true;
			mdkg.RequestedItemTag = GameTags.Dirt;
			mdkg.capacity = STORED_DIRT;
			mdkg.refillMass = REFILL_DIRT;
			mdkg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;

			var ec = go.AddOrGet<ElementConverter>();
			ec.consumedElements = new[]
			{
				new ce_(mdkg.RequestedItemTag, USE_ORGANIC)
			};
			ec.outputElements = new[]
			{
				new oe_(EMIT_OXYGEN, SimHashes.Oxygen, 0f, true, false, 1f, 1f)
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