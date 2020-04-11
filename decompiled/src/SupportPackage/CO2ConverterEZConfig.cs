using System;
using TUNING;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x02000002 RID: 2
	public class CO2ConverterEZConfig : IBuildingConfig
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CO2ConverterEZ", 2, 2, "co2scrubber_kanim", 100, 120f, IN_Constants.CO2ConverterEZ.TMass, IN_Constants.CO2ConverterEZ.TMate, 1600f, 1, DECOR.BONUS.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 10f;
			buildingDef.ExhaustKilowattsWhenActive = 0f;
			buildingDef.SelfHeatKilowattsWhenActive = 0.2f;
			buildingDef.UtilityInputOffset = default(CellOffset);
			buildingDef.InputConduitType = 2;
			buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
			buildingDef.OutputConduitType = 2;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.PermittedRotations = 3;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.AudioSize = "large";
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
			return buildingDef;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002120 File Offset: 0x00000320
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(go);
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
			Storage storage = go.AddComponent<Storage>();
			storage.showDescriptor = false;
			storage.capacityKg = 255f;
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			PassiveElementConsumer passiveElementConsumer = EntityTemplateExtensions.AddOrGet<PassiveElementConsumer>(go);
			passiveElementConsumer.consumedMass = (passiveElementConsumer.consumptionRate = (passiveElementConsumer.capacityKG = 5f));
			passiveElementConsumer.elementToConsume = 1960575215;
			passiveElementConsumer.storeOnConsume = true;
			passiveElementConsumer.isRequired = (passiveElementConsumer.showInStatusPanel = (passiveElementConsumer.showDescriptor = false));
			passiveElementConsumer.consumptionRadius = 3;
			passiveElementConsumer.sampleCellOffset = default(Vector3);
			ConduitConsumer conduitConsumer = EntityTemplateExtensions.AddOrGet<ConduitConsumer>(go);
			conduitConsumer.forceAlwaysSatisfied = true;
			conduitConsumer.wrongElementResult = 1;
			conduitConsumer.capacityKG = (conduitConsumer.consumptionRate = 1f);
			conduitConsumer.capacityTag = GameTags.Water;
			conduitConsumer.conduitType = 2;
			ConduitDispenser conduitDispenser = EntityTemplateExtensions.AddOrGet<ConduitDispenser>(go);
			conduitDispenser.invertElementFilter = true;
			conduitDispenser.conduitType = 2;
			conduitDispenser.elementFilter = new SimHashes[]
			{
				1836671383
			};
			ElementDropper elementDropper = EntityTemplateExtensions.AddOrGet<ElementDropper>(go);
			elementDropper.emitMass = 10f;
			elementDropper.emitOffset = new Vector3(1f, 1f);
			elementDropper.emitTag = GameTagExtensions.CreateTag(947100397);
			ElementConverter conv = EntityTemplateExtensions.AddOrGet<ElementConverter>(go);
			conv.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(GameTagExtensions.CreateTag(1960575215), 0.9f),
				new ElementConverter.ConsumedElement(GameTags.Water, 0.1f)
			};
			conv.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.6f, -1528777920, 0f, true, false, 1f, 1f, 1f, byte.MaxValue, 0),
				new ElementConverter.OutputElement(0.299999952f, 947100397, 0f, true, true, 0f, 0f, 1f, byte.MaxValue, 0),
				new ElementConverter.OutputElement(0.1f, 1832607973, 0f, true, true, 0f, 0f, 1f, byte.MaxValue, 0)
			};
			EntityTemplateExtensions.AddOrGet<AirFilter>(go).filterTag = GameTags.Water;
			EntityTemplateExtensions.AddOrGet<KBatchedAnimController>(go).randomiseLoopedOffset = true;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000235E File Offset: 0x0000055E
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
