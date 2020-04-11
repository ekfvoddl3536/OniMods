using System;
using TUNING;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x02000004 RID: 4
	public class EasyElectrolyzerConfig : IBuildingConfig
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002554 File Offset: 0x00000754
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("EasyElectrolyzer", 2, 2, "electrolyzer_kanim", 30, 30f, IN_Constants.EasyElectrolyzer.TMass, IN_Constants.EasyElectrolyzer.TMates, 800f, 1, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER3, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.PowerInputOffset = new CellOffset(1, 0);
			buildingDef.EnergyConsumptionWhenActive = 160f;
			buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
			buildingDef.OutputConduitType = 1;
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.InputConduitType = 2;
			buildingDef.ExhaustKilowattsWhenActive = 0.25f;
			buildingDef.SelfHeatKilowattsWhenActive = 1f;
			buildingDef.ViewMode = OverlayModes.Oxygen.ID;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(IN_Constants.EasyElectrolyzer.INPUT_PORT);
			return buildingDef;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000261C File Offset: 0x0000081C
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
			Electrolyzer electrolyzer = EntityTemplateExtensions.AddOrGet<Electrolyzer>(go);
			electrolyzer.maxMass = 1.8f;
			electrolyzer.hasMeter = true;
			ConduitConsumer cc = EntityTemplateExtensions.AddOrGet<ConduitConsumer>(go);
			cc.conduitType = 2;
			cc.capacityTag = GameTagExtensions.CreateTag(1836671383);
			cc.consumptionRate = 1f;
			cc.wrongElementResult = 1;
			ConduitDispenser conduitDispenser = EntityTemplateExtensions.AddOrGet<ConduitDispenser>(go);
			conduitDispenser.alwaysDispense = true;
			conduitDispenser.conduitType = 1;
			ElementConverter ev = EntityTemplateExtensions.AddOrGet<ElementConverter>(go);
			ev.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(cc.capacityTag, 1f)
			};
			ev.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.888f, -1528777920, 308.15f, false, false, 0f, 1f, 1f, byte.MaxValue, 0),
				new ElementConverter.OutputElement(0.111999989f, -1046145888, 308.15f, false, true, 0f, 1f, 1f, byte.MaxValue, 0)
			};
			Prioritizable.AddRef(go);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002734 File Offset: 0x00000934
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
