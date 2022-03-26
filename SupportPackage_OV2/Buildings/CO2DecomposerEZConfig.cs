using UnityEngine;
using TUNING;

namespace SupportPackage
{
	using static GlobalConsts;
	using static GlobalConsts.CO2DecomposerEZ;
	using ce_ = ElementConverter.ConsumedElement;
	using oe_ = ElementConverter.OutputElement;
	public class CO2DecomposerEZConfig : IBuildingConfig
    {
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef res =
				BuildingTemplates.CreateBuildingDef(
					ID, 2, 2, ANISTR,
					100, 120f,
					TMass, TMate, 
					DEF_MELPT * 2f, BuildLocationRule.OnFloor, 
					DECOR.BONUS.TIER0, NOISE_POLLUTION.NOISY.TIER0);

			res.RequiresPowerInput = true;
			res.EnergyConsumptionWhenActive = USE_POWER;

			res.ExhaustKilowattsWhenActive = HEAT_EXHAUST;
			res.SelfHeatKilowattsWhenActive = HEAT_SELF;

			res.UtilityInputOffset = default; // (0, 0)
			res.InputConduitType = ConduitType.Liquid;

			res.UtilityOutputOffset = new CellOffset(1, 1);
			res.OutputConduitType = ConduitType.Liquid;

			res.ViewMode = OverlayModes.Power.ID;

			res.PermittedRotations = PermittedRotations.FlipH;

			res.AudioCategory = "HollowMetal";
			res.AudioSize = "large";

			res.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));

			return res;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<LoopingSounds>();

			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

			var st = go.AddComponent<Storage>();
			st.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			st.showDescriptor = false;
			st.capacityKg = 1000f;

			var ec = go.AddOrGet<PassiveElementConsumer>();
			ec.consumptionRate = 5f;
			ec.capacityKG = 2f;
			ec.showInStatusPanel = ec.showDescriptor = false;
			ec.storeOnConsume = true;
			ec.elementToConsume = SimHashes.CarbonDioxide;
			ec.consumptionRadius = 3;
			ec.sampleCellOffset = default;

			var cc = go.AddOrGet<ConduitConsumer>();
			cc.forceAlwaysSatisfied = true;
			cc.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
			cc.capacityKG = cc.consumptionRate = 1f;
			cc.capacityTag = GameTags.Water;
			cc.conduitType = ConduitType.Liquid;

			var cd = go.AddOrGet<ConduitDispenser>();
			cd.conduitType = ConduitType.Liquid;
			cd.elementFilter = new[] { SimHashes.DirtyWater };

			var ed = go.AddOrGet<ElementDropper>();
			ed.emitMass = 100f;
			ed.emitOffset = new Vector3(1f, 1f);
			ed.emitTag = GameTags.Carbon;

			var conv = go.AddOrGet<ElementConverter>();
			conv.consumedElements = new[]
			{
				new ce_(GameTags.CarbonDioxide, USE_CO2),
				new ce_(GameTags.Water, 0.01f),
			};
			conv.outputElements = new[]
			{
				new oe_(RATE_O2, SimHashes.Oxygen, 0f, true, false, 1f, 1f),
				new oe_(RATE_C2, SimHashes.Carbon, 0f, true, true, 0f, 0f),
				new oe_(0.01f, SimHashes.DirtyWater, 0f, true, true, 0f, 0f)
			};

			go.AddOrGet<AirFilter>().filterTag = GameTags.Water;

			go.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGetDef<PoweredActiveController.Def>();
		}
	}
}
