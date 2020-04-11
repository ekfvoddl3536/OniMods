using System;
using TUNING;
using UnityEngine;

namespace AntiEntropyCooler
{
	// Token: 0x02000003 RID: 3
	public sealed class AntiEntropyCoolerConfig : IBuildingConfig
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C4 File Offset: 0x000002C4
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AntiEntropyCooler", 4, 4, "massiveheatsink_kanim", 100, 240f, Consts.TMass, Consts.TMate, 2400f, 1, BUILDINGS.DECOR.BONUS.TIER4, NOISE_POLLUTION.NOISY.TIER4, 0.2f);
			buildingDef.ExhaustKilowattsWhenActive = -192f;
			buildingDef.SelfHeatKilowattsWhenActive = -256f;
			buildingDef.RequiresPowerInput = true;
			buildingDef.Floodable = false;
			buildingDef.Entombable = true;
			buildingDef.Overheatable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "large";
			buildingDef.EnergyConsumptionWhenActive = 30f;
			buildingDef.PowerInputOffset = new CellOffset(0, 0);
			buildingDef.UtilityInputOffset = new CellOffset(1, 0);
			buildingDef.InputConduitType = 1;
			buildingDef.ViewMode = OverlayModes.Temperature.ID;
			return buildingDef;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002188 File Offset: 0x00000388
		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
			EntityTemplateExtensions.AddOrGet<MassiveHeatSink>(go);
			EntityTemplateExtensions.AddOrGet<MinimumOperatingTemperature>(go).minimumTemperature = 20f;
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(go);
			Storage storage = EntityTemplateExtensions.AddOrGet<Storage>(go);
			storage.capacityKg = 2f;
			storage.showInUI = true;
			ConduitConsumer cc = EntityTemplateExtensions.AddOrGet<ConduitConsumer>(go);
			cc.conduitType = 1;
			cc.consumptionRate = 0.5f;
			cc.capacityKG = 10f;
			cc.capacityTag = GameTagExtensions.CreateTag(-1046145888);
			cc.forceAlwaysSatisfied = true;
			cc.wrongElementResult = 1;
			EntityTemplateExtensions.AddOrGet<ElementConverter>(go).consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(cc.capacityTag, 0.01f)
			};
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
