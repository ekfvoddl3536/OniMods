using System;
using TUNING;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x0200000E RID: 14
	public abstract class LStorageBase : IBuildingConfig
	{
		// Token: 0x0600002D RID: 45 RVA: 0x000031C0 File Offset: 0x000013C0
		public BuildingDef Create(string id, int wi, int hi, string anim, float contime, string[] mates, float[] masskg, int capacity)
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, wi, hi, anim, 30, 30f, masskg, mates, 1600f, 1, DECOR.PENALTY.TIER1, NOISE_POLLUTION.NONE, 0.2f);
			buildingDef.Floodable = (buildingDef.Overheatable = false);
			buildingDef.AudioCategory = "Metal";
			this.StorageCapacity = capacity;
			this.CopyGroup = new Tag(id);
			return buildingDef;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003228 File Offset: 0x00001428
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", NOISE_POLLUTION.NOISY.TIER1);
			Prioritizable.AddRef(go);
			Storage storage = EntityTemplateExtensions.AddOrGet<Storage>(go);
			storage.capacityKg = (float)this.StorageCapacity;
			storage.showInUI = (storage.allowItemRemoval = (storage.showDescriptor = true));
			storage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
			storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
			storage.fetchCategory = 1;
			EntityTemplateExtensions.AddOrGet<CopyBuildingSettings>(go).copyGroupTag = this.CopyGroup;
			EntityTemplateExtensions.AddOrGet<StorageLocker>(go);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000032B4 File Offset: 0x000014B4
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGetDef<StorageController.Def>(go);
		}

		// Token: 0x0400000D RID: 13
		protected int StorageCapacity;

		// Token: 0x0400000E RID: 14
		protected Tag CopyGroup;
	}
}
