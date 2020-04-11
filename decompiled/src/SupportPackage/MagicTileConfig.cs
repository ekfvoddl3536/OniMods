using System;
using TUNING;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x02000013 RID: 19
	public sealed class MagicTileConfig : IBuildingConfig
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00003834 File Offset: 0x00001A34
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MagicTile", 1, 1, "floor_gasperm_kanim", 100, 30f, IN_Constants.MagicTile.TMass, IN_Constants.MagicTile.TMates, 1600f, 6, DECOR.PENALTY.TIER0, NOISE_POLLUTION.NONE, 0.2f);
			BuildingTemplates.CreateFoundationTileDef(buildingDef);
			buildingDef.Floodable = (buildingDef.Entombable = (buildingDef.Overheatable = (buildingDef.UseStructureTemperature = false)));
			buildingDef.IsFoundation = (buildingDef.isKAnimTile = true);
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.SceneLayer = 30;
			buildingDef.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
			buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_gasmembrane");
			buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_gasmembrane_place");
			buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
			buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_mesh_tops_decor_info");
			buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_mesh_tops_decor_place_info");
			return buildingDef;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003934 File Offset: 0x00001B34
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			SimCellOccupier simCellOccupier = EntityTemplateExtensions.AddOrGet<SimCellOccupier>(go);
			simCellOccupier.setGasImpermeable = true;
			simCellOccupier.doReplaceElement = false;
			EntityTemplateExtensions.AddOrGet<KAnimGridTileVisualizer>(go).blockTileConnectorID = MeshTileConfig.BlockTileConnectorID;
			EntityTemplateExtensions.AddOrGet<BuildingHP>(go).destroyOnDamaged = true;
			go.AddComponent<SimTemperatureTransfer>();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003992 File Offset: 0x00001B92
		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<KAnimGridTileVisualizer>(go);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000399B File Offset: 0x00001B9B
		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddComponent<PublicZoneTile>();
			go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
		}
	}
}
