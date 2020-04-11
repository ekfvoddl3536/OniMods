using System;
using TUNING;
using UnityEngine;

namespace DupeWhatDiamond
{
	// Token: 0x02000003 RID: 3
	public sealed class DiamondCompressorConfig : IBuildingConfig
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020FC File Offset: 0x000002FC
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("DiamondCompressor", 3, 4, "oxylite_refinery_kanim", 100, 120f, MConstants.TMateMassKG, MConstants.TMaterials, 3200f, 1, DECOR.PENALTY.TIER3, NOISE_POLLUTION.NOISY.TIER3, 0.2f);
			buildingDef.Overheatable = false;
			buildingDef.RequiresPowerInput = true;
			buildingDef.PowerInputOffset = new CellOffset(1, 0);
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
			buildingDef.EnergyConsumptionWhenActive = 480f;
			buildingDef.ExhaustKilowattsWhenActive = 4f;
			buildingDef.SelfHeatKilowattsWhenActive = 16f;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.InputConduitType = 2;
			buildingDef.OutputConduitType = 2;
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(default(CellOffset));
			return buildingDef;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000021C4 File Offset: 0x000003C4
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
			OxyliteRefinery oxyliteRefinery = EntityTemplateExtensions.AddOrGet<OxyliteRefinery>(go);
			oxyliteRefinery.emitTag = GameTagExtensions.CreateTag(-2079931820);
			oxyliteRefinery.emitMass = 50f;
			oxyliteRefinery.dropOffset = new Vector3(0f, 1f);
			Storage st = EntityTemplateExtensions.AddOrGet<Storage>(go);
			st.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			st.showInUI = true;
			ConduitConsumer cc = EntityTemplateExtensions.AddOrGet<ConduitConsumer>(go);
			cc.storage = st;
			cc.capacityTag = GameTagExtensions.CreateTag(1836671383);
			cc.conduitType = 2;
			cc.consumptionRate = 10f;
			cc.capacityKG = 100f;
			cc.wrongElementResult = 1;
			ConduitDispenser cd = EntityTemplateExtensions.AddOrGet<ConduitDispenser>(go);
			cd.storage = st;
			cd.conduitType = 2;
			cd.elementFilter = new SimHashes[]
			{
				1836671383
			};
			cd.invertElementFilter = (cd.alwaysDispense = true);
			ManualDeliveryKG mdkg = EntityTemplateExtensions.AddOrGet<ManualDeliveryKG>(go);
			mdkg.allowPause = false;
			mdkg.SetStorage(st);
			mdkg.requestedItemTag = GameTagExtensions.CreateTag(-902240476);
			mdkg.refillMass = 50f;
			mdkg.capacity = 500f;
			mdkg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
			ElementConverter ec = EntityTemplateExtensions.AddOrGet<ElementConverter>(go);
			ec.SetStorage(st);
			ec.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(mdkg.requestedItemTag, 2f),
				new ElementConverter.ConsumedElement(cc.capacityTag, 5f)
			};
			ec.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.09f, -2079931820, 0f, true, true, 0f, 0.5f, 1f, byte.MaxValue, 0),
				new ElementConverter.OutputElement(5f, 1832607973, 338.15f, false, true, 1f, 1f, 1f, Db.Get().Diseases.GetIndex(Db.Get().Diseases.FoodGerms.IdHash), 100000)
			};
			Prioritizable.AddRef(go);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000023E6 File Offset: 0x000005E6
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
