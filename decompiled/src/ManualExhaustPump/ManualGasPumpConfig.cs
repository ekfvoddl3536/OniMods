using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace ManualExhaustPump
{
	// Token: 0x02000003 RID: 3
	public class ManualGasPumpConfig : IBuildingConfig
	{
		// Token: 0x06000005 RID: 5 RVA: 0x0000215C File Offset: 0x0000035C
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ManualGasPump", 5, 3, "gasstorage_kanim", 100, 150f, Constants.TMass, Constants.TMate, 1600f, 1, DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER1, 0.2f);
			buildingDef.OutputConduitType = 1;
			buildingDef.ViewMode = OverlayModes.GasConduits.ID;
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

		// Token: 0x06000006 RID: 6 RVA: 0x00002200 File Offset: 0x00000400
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			Prioritizable.AddRef(go);
			Storage storage = EntityTemplateExtensions.AddOrGet<Storage>(go);
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			storage.showInUI = (storage.showDescriptor = true);
			storage.capacityKg = 75f;
			storage.storageFilters = STORAGEFILTERS.GASES;
			ConduitDispenser conduitDispenser = EntityTemplateExtensions.AddOrGet<ConduitDispenser>(go);
			conduitDispenser.conduitType = 1;
			conduitDispenser.elementFilter = Array.Empty<SimHashes>();
			conduitDispenser.alwaysDispense = false;
			EntityTemplateExtensions.AddOrGet<SmartReservoir>(go);
			EntityTemplateExtensions.AddOrGet<CopyBuildingSettings>(go).copyGroupTag = Constants.ManualGasPump.StorageTag;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000227E File Offset: 0x0000047E
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGetDef<StorageController.Def>(go);
		}
	}
}
