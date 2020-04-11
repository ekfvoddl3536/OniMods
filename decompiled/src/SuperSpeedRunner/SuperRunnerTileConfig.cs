using System;
using TUNING;
using UnityEngine;

namespace SuperSpeedRunner
{
	// Token: 0x02000006 RID: 6
	public class SuperRunnerTileConfig : IBuildingConfig
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002264 File Offset: 0x00000464
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("RunnerTile", 1, 1, "floor_plastic_kanim", 100, 5f, Constants.RunnerTile.TMass, Constants.RunnerTile.TMate, 800f, 6, DECOR.BONUS.TIER0, NOISE_POLLUTION.NONE, 0.2f);
			BuildingTemplates.CreateFoundationTileDef(buildingDef);
			buildingDef.Floodable = (buildingDef.Entombable = (buildingDef.Overheatable = (buildingDef.UseStructureTemperature = false)));
			buildingDef.isKAnimTile = (buildingDef.isSolidTile = true);
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.SceneLayer = 30;
			buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_plastic");
			buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_plastic_place");
			buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
			buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_plastic_tops_decor_info");
			buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_plastic_tops_place_decor_info");
			buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
			buildingDef.DragBuild = true;
			return buildingDef;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000236C File Offset: 0x0000056C
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			SimCellOccupier simCellOccupier = EntityTemplateExtensions.AddOrGet<SimCellOccupier>(go);
			simCellOccupier.movementSpeedMultiplier = 20f;
			simCellOccupier.notifyOnMelt = true;
			EntityTemplateExtensions.AddOrGet<TileTemperature>(go);
			EntityTemplateExtensions.AddOrGet<KAnimGridTileVisualizer>(go).blockTileConnectorID = PlasticTileConfig.BlockTileConnectorID;
			EntityTemplateExtensions.AddOrGet<BuildingHP>(go).destroyOnDamaged = true;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000023CE File Offset: 0x000005CE
		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<KAnimGridTileVisualizer>(go);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000023D7 File Offset: 0x000005D7
		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
		}
	}
}
