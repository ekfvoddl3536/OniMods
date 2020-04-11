using System;
using TUNING;
using UnityEngine;

namespace VacuumTools
{
	// Token: 0x02000002 RID: 2
	public class AnotherStorageLockerConfig : IBuildingConfig
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AnotherVoidStorage", 1, 2, "storagelocker_kanim", 30, 60f, VTCONST.TMass, VTCONST.TMate, 1600f, 1, DECOR.BONUS.TIER0, NOISE_POLLUTION.NONE, 0.2f);
			buildingDef.Floodable = (buildingDef.Overheatable = false);
			buildingDef.AudioCategory = "Metal";
			return buildingDef;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020B0 File Offset: 0x000002B0
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", NOISE_POLLUTION.NOISY.TIER1);
			Prioritizable.AddRef(go);
			Storage storage = EntityTemplateExtensions.AddOrGet<Storage>(go);
			storage.allowSettingOnlyFetchMarkedItems = true;
			storage.showInUI = (storage.showDescriptor = true);
			storage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
			storage.fetchCategory = 1;
			EntityTemplateExtensions.AddOrGet<CopyBuildingSettings>(go).copyGroupTag = VTCONST.AnotherVoidStorage.MyTag;
			EntityTemplateExtensions.AddOrGet<VoidStorageFilterLocker>(go);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002121 File Offset: 0x00000321
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGetDef<StorageController.Def>(go);
		}
	}
}
