using System;
using TUNING;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x02000009 RID: 9
	public class H2OSynthesizerConfig : IBuildingConfig
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002E20 File Offset: 0x00001020
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("H2OSynthesizer", 2, 3, "liquidreservoir_kanim", 100, 120f, IN_Constants.H2OSynthesizer.TMass, IN_Constants.H2OSynthesizer.TMate, 1600f, 1, DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 1200f;
			buildingDef.UtilityInputOffset = new CellOffset(1, 0);
			buildingDef.InputConduitType = 1;
			buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
			buildingDef.OutputConduitType = 2;
			buildingDef.ExhaustKilowattsWhenActive = 4f;
			buildingDef.SelfHeatKilowattsWhenActive = 4f;
			buildingDef.ViewMode = OverlayModes.GasConduits.ID;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			return buildingDef;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002EDC File Offset: 0x000010DC
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
			EntityTemplateExtensions.AddOrGet<Reservoir>(go);
			Storage st = go.AddComponent<Storage>();
			st.showDescriptor = (st.allowItemRemoval = false);
			st.capacityKg = 1000f;
			ConduitConsumer cc = EntityTemplateExtensions.AddOrGet<ConduitConsumer>(go);
			cc.storage = st;
			cc.capacityKG = (cc.consumptionRate = 1f);
			cc.capacityTag = GameTagExtensions.CreateTag(-1046145888);
			cc.conduitType = 1;
			ElementConsumer ec = EntityTemplateExtensions.AddOrGet<ElementConsumer>(go);
			ec.storage = st;
			ec.capacityKG = (ec.consumptionRate = 1f);
			ec.storeOnConsume = (ec.showInStatusPanel = true);
			ec.isRequired = (ec.showDescriptor = false);
			ec.consumptionRadius = 2;
			ec.sampleCellOffset = Vector3.zero;
			ec.elementToConsume = -1528777920;
			ConduitDispenser cd = EntityTemplateExtensions.AddOrGet<ConduitDispenser>(go);
			cd.conduitType = 2;
			cd.elementFilter = new SimHashes[]
			{
				1836671383
			};
			cd.alwaysDispense = true;
			ElementConverter conv = EntityTemplateExtensions.AddOrGet<ElementConverter>(go);
			conv.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(cc.capacityTag, 0.8f),
				new ElementConverter.ConsumedElement(GameTagExtensions.CreateTag(ec.elementToConsume), 0.2f)
			};
			conv.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(1f, 1836671383, 0f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0)
			};
			EntityTemplateExtensions.AddOrGet<AirFilter>(go).filterTag = cc.capacityTag;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003086 File Offset: 0x00001286
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
