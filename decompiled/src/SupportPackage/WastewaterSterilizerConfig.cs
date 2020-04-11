using System;
using TUNING;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x02000019 RID: 25
	public class WastewaterSterilizerConfig : IBuildingConfig
	{
		// Token: 0x06000063 RID: 99 RVA: 0x0000483C File Offset: 0x00002A3C
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("WastewaterSterilizer", 4, 3, "waterpurifier_kanim", 100, 45f, IN_Constants.WastewaterSterilizer.TMass, IN_Constants.WastewaterSterilizer.TMates, 800f, 1, DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER3, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 180f;
			buildingDef.ExhaustKilowattsWhenActive = 2.5f;
			buildingDef.SelfHeatKilowattsWhenActive = 6f;
			buildingDef.InputConduitType = 2;
			buildingDef.OutputConduitType = 2;
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.PowerInputOffset = new CellOffset(2, 0);
			buildingDef.UtilityInputOffset = new CellOffset(-1, 2);
			buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
			buildingDef.PermittedRotations = 3;
			return buildingDef;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000048FC File Offset: 0x00002AFC
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
			Storage st = BuildingTemplates.CreateDefaultStorage(go, false);
			st.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			EntityTemplateExtensions.AddOrGet<WaterPurifier>(go);
			Prioritizable.AddRef(go);
			ElementConverter ec = EntityTemplateExtensions.AddOrGet<ElementConverter>(go);
			ec.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(IN_Constants.WastewaterSterilizer.UseFilter, 1f),
				new ElementConverter.ConsumedElement(IN_Constants.WastewaterSterilizer.InputLiq, 5f)
			};
			ec.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.2f, 869554203, 313.15f, false, true, 0f, 0.5f, 0f, byte.MaxValue, 0),
				new ElementConverter.OutputElement(5f, 1836671383, 313.15f, false, true, 2f, 2f, 0f, byte.MaxValue, 0)
			};
			ElementDropper elementDropper = EntityTemplateExtensions.AddOrGet<ElementDropper>(go);
			elementDropper.emitMass = 10f;
			elementDropper.emitTag = GameTagExtensions.CreateTag(869554203);
			elementDropper.emitOffset = new Vector3(0f, 1f);
			ManualDeliveryKG manualDeliveryKG = EntityTemplateExtensions.AddOrGet<ManualDeliveryKG>(go);
			manualDeliveryKG.allowPause = false;
			manualDeliveryKG.SetStorage(st);
			manualDeliveryKG.requestedItemTag = IN_Constants.WastewaterSterilizer.UseFilter;
			manualDeliveryKG.capacity = 1200f;
			manualDeliveryKG.refillMass = 300f;
			manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
			ConduitConsumer conduitConsumer = EntityTemplateExtensions.AddOrGet<ConduitConsumer>(go);
			conduitConsumer.conduitType = 2;
			conduitConsumer.consumptionRate = 10f;
			conduitConsumer.capacityKG = 20f;
			conduitConsumer.capacityTag = IN_Constants.WastewaterSterilizer.InputLiq;
			conduitConsumer.forceAlwaysSatisfied = true;
			conduitConsumer.wrongElementResult = 0;
			ConduitDispenser conduitDispenser = EntityTemplateExtensions.AddOrGet<ConduitDispenser>(go);
			conduitDispenser.conduitType = 2;
			conduitDispenser.invertElementFilter = true;
			conduitDispenser.elementFilter = new SimHashes[]
			{
				1832607973
			};
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004ACE File Offset: 0x00002CCE
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
