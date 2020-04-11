using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace ManualExhaustPump
{
	// Token: 0x02000004 RID: 4
	public class ManualLiquidPumpConfig : IBuildingConfig
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002290 File Offset: 0x00000490
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ManualLiquidPump", 2, 3, "liquidreservoir_kanim", 100, 150f, Constants.TMass, Constants.TMate, 1600f, 1, DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER1, 0.2f);
			buildingDef.OutputConduitType = 2;
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
			buildingDef.RequiresPowerInput = true;
			buildingDef.PowerInputOffset = new CellOffset(1, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
			buildingDef.EnergyConsumptionWhenActive = 90f;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
			{
				Constants.OUTPUT_PORT
			};
			return buildingDef;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002334 File Offset: 0x00000534
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			Prioritizable.AddRef(go);
			Storage storage = EntityTemplateExtensions.AddOrGet<Storage>(go);
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			storage.showInUI = (storage.showDescriptor = true);
			storage.capacityKg = 600f;
			storage.storageFilters = STORAGEFILTERS.LIQUIDS;
			ConduitDispenser conduitDispenser = EntityTemplateExtensions.AddOrGet<ConduitDispenser>(go);
			conduitDispenser.conduitType = 2;
			conduitDispenser.elementFilter = Array.Empty<SimHashes>();
			conduitDispenser.alwaysDispense = false;
			EntityTemplateExtensions.AddOrGet<SmartReservoir>(go);
			EntityTemplateExtensions.AddOrGet<CopyBuildingSettings>(go).copyGroupTag = Constants.ManualLiquidPump.StorageTag;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023B2 File Offset: 0x000005B2
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGetDef<StorageController.Def>(go);
		}
	}
}
