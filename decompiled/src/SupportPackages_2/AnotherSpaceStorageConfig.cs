using System;
using TUNING;
using UnityEngine;

namespace SupportPackages
{
	// Token: 0x02000002 RID: 2
	public class AnotherSpaceStorageConfig : IBuildingConfig
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AnotherSpaceStorage", 1, 2, "storagelocker_kanim", 30, 10f, SS_CONST.AnotherSpaceStorage.TMass, SS_CONST.AnotherSpaceStorage.TMate, 1600f, 1, DECOR.BONUS.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
			buildingDef.Floodable = (buildingDef.Overheatable = false);
			buildingDef.AudioCategory = "Metal";
			return buildingDef;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020B0 File Offset: 0x000002B0
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", NOISE_POLLUTION.NOISY.TIER1);
			Prioritizable.AddRef(go);
			ShareStorage shareStorage = go.AddComponent<ShareStorage>();
			shareStorage.capacityKg = 50000f;
			shareStorage.showInUI = (shareStorage.showDescriptor = (shareStorage.allowItemRemoval = true));
			shareStorage.fetchCategory = 1;
			shareStorage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
			shareStorage.items = SS_CONST.AnotherSpaceStorage.STATIC_ITEMS;
			EntityTemplateExtensions.AddOrGet<CopyBuildingSettings>(go).copyGroupTag = SS_CONST.AnotherSpaceStorage.MyTag;
			go.AddComponent<SharedStorageLocker>();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000213B File Offset: 0x0000033B
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGetDef<StorageController.Def>(go);
		}
	}
}
