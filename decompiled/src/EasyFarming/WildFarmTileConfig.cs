using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace EasyFarming
{
	// Token: 0x02000013 RID: 19
	public sealed class WildFarmTileConfig : IBuildingConfig
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002FB0 File Offset: 0x000011B0
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("WildFarmTile", 1, 1, "farmtilerotating_kanim", 100, 30f, ConstsEF.WildFarmTile.TMass, ConstsEF.WildFarmTile.TMate, 1600f, 6, DECOR.NONE, NOISE_POLLUTION.NONE, 0.2f);
			buildingDef.Floodable = (buildingDef.Entombable = (buildingDef.Overheatable = false));
			buildingDef.IsFoundation = (buildingDef.isSolidTile = (buildingDef.DragBuild = true));
			buildingDef.TileLayer = 9;
			buildingDef.ReplacementLayer = 11;
			buildingDef.ForegroundLayer = 18;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.SceneLayer = 30;
			buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
			buildingDef.PermittedRotations = 4;
			buildingDef.ReplacementTags = new List<Tag>
			{
				GameTags.FloorTiles
			};
			return buildingDef;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003090 File Offset: 0x00001290
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			SimCellOccupier simCellOccupier = EntityTemplateExtensions.AddOrGet<SimCellOccupier>(go);
			simCellOccupier.doReplaceElement = (simCellOccupier.notifyOnMelt = true);
			EntityTemplateExtensions.AddOrGet<TileTemperature>(go);
			BuildingTemplates.CreateDefaultStorage(go, false).SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			WildPlantablePlot wildPlantablePlot = EntityTemplateExtensions.AddOrGet<WildPlantablePlot>(go);
			wildPlantablePlot.occupyingObjectRelativePosition = new Vector3(0f, 1f, 0f);
			wildPlantablePlot.AddDepositTag(GameTags.CropSeed);
			wildPlantablePlot.AddDepositTag(GameTags.WaterSeed);
			wildPlantablePlot.SetFertilizationFlags(true, false);
			EntityTemplateExtensions.AddOrGet<CopyBuildingSettings>(go).copyGroupTag = GameTags.Farm;
			EntityTemplateExtensions.AddOrGet<AnimTileable>(go);
			Prioritizable.AddRef(go);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000313F File Offset: 0x0000133F
		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			KPrefabID component = go.GetComponent<KPrefabID>();
			component.AddTag(GameTags.FarmTiles, false);
			component.prefabSpawnFn += new KPrefabID.PrefabFn(this.SetupFarmPlot);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000316C File Offset: 0x0000136C
		private void SetupFarmPlot(GameObject go)
		{
			Rotatable component = go.GetComponent<Rotatable>();
			WildPlantablePlot wd = go.GetComponent<WildPlantablePlot>();
			switch (component.GetOrientation())
			{
			case 0:
			case 5:
				wd.SetReceptacleDirection(0);
				return;
			case 1:
			case 3:
				wd.SetReceptacleDirection(1);
				return;
			}
			wd.SetReceptacleDirection(2);
		}
	}
}
