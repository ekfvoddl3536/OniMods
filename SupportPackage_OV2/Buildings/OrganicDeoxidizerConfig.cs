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
			mdkg.requestedItemTag = GameTags.Dirt;
			mdkg.capacity = STORED_DIRT;
			mdkg.refillMass = REFILL_DIRT;
			mdkg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;

			var ec = go.AddOrGet<ElementConverter>();
			ec.consumedElements = new[]
			{
				new ce_(mdkg.requestedItemTag, USE_ORGANIC)
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