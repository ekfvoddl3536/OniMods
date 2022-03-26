using UnityEngine;
using TUNING;

namespace SupportPackage
{
	using static GlobalConsts;
	using static GlobalConsts.WastewaterSterilizer;
	using ce_ = ElementConverter.ConsumedElement;
	using oe_ = ElementConverter.OutputElement;
	public class WastewaterSterilizerConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
			BuildingDef res =
				BuildingTemplates.CreateBuildingDef(
					ID, 4, 3, ANISTR, 
					100, 45f,
					TMass, TMates,
					DEF_MELPT * 2f, BuildLocationRule.OnFloor, 
					DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER3);

			res.RequiresPowerInput = true;
			res.EnergyConsumptionWhenActive = USE_POWER;

			res.ExhaustKilowattsWhenActive = HEAT_EXHAUS;
			res.SelfHeatKilowattsWhenActive = HEAT_SELF;
			
			res.InputConduitType = ConduitType.Liquid;
			res.OutputConduitType = ConduitType.Liquid;

			res.ViewMode = OverlayModes.LiquidConduits.ID;
			
			res.AudioCategory = "HollowMetal";

			res.PowerInputOffset = new CellOffset(2, 0);
			res.UtilityInputOffset = new CellOffset(-1, 2);
			res.UtilityOutputOffset = new CellOffset(2, 2);

			res.PermittedRotations = PermittedRotations.FlipH;

			return res;
		}

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

			var st = go.AddOrGet<Storage>();
			st.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			st.capacityKg = 2000f;

			go.AddOrGet<WaterPurifier>();

			Prioritizable.AddRef(go);

			var ec = go.AddOrGet<ElementConverter>();
			ec.consumedElements = new[]
			{
				new ce_(GameTags.Filter, FILTER_USE_RATE),
				new ce_(GameTags.DirtyWater, LIQUID_RATE),
			};
			ec.outputElements = new[]
			{
				new oe_(LIQUID_RATE, SimHashes.Water, 0, false, true, 2f, 2f, float.NegativeInfinity),
				new oe_(FILTER_OUT_RATE, SimHashes.ToxicSand, 0, false, true, 0f, 0.5f, float.NegativeInfinity),
			};

			var ed = go.AddOrGet<ElementDropper>();
			ed.emitMass = DROP_MASS;
			ed.emitTag = new Tag(nameof(SimHashes.ToxicSand));
			ed.emitOffset = new Vector3(0f, 1f);

			var mdkg = go.AddOrGet<ManualDeliveryKG>();
			mdkg.allowPause = false;
			mdkg.SetStorage(st);
			mdkg.requestedItemTag = GameTags.Filter;
			mdkg.capacity = DELIVERY_CAPACITY;
			mdkg.refillMass = DELIVERY_REFILL;
			mdkg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;

			var cc = go.AddOrGet<ConduitConsumer>();
			cc.conduitType = ConduitType.Liquid;
			cc.consumptionRate = 10f;
			cc.capacityKG = 20f;
			cc.capacityTag = GameTags.DirtyWater;
			cc.forceAlwaysSatisfied = true;
			cc.wrongElementResult = 0;

			var cd = go.AddOrGet<ConduitDispenser>();
			cd.conduitType = ConduitType.Liquid;
			cd.elementFilter = new[] { SimHashes.Water };
		}

        public override void DoPostConfigureComplete(GameObject go) =>
			go.AddOrGetDef<PoweredActiveController.Def>();
    }
}
