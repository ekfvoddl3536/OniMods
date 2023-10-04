// MIT License
//
// Copyright (c) 2022-2023. Super Comic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using UnityEngine;
using TUNING;

namespace SupportPackage
{
    using static GlobalConsts;
    using static GlobalConsts.MagicTile;
    public class MagicTileConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
			BuildingDef res = 
				BuildingTemplates.CreateBuildingDef(
					ID, 1, 1, ANISTR,
					100, DEF_CON_TIME,
					TMass, TMates,
					DEF_MELPT * 2f, BuildLocationRule.Tile, 
					DECOR.PENALTY.TIER0, NOISE_POLLUTION.NONE);

			BuildingTemplates.CreateFoundationTileDef(res);

            res.Floodable = res.Entombable = res.Overheatable = res.UseStructureTemperature = false;
			res.IsFoundation = res.isKAnimTile = true;

			res.AudioCategory = "Metal";
			res.AudioSize = "small";

			res.BaseTimeUntilRepair = -1f;

			res.SceneLayer = Grid.SceneLayer.TileMain;

			res.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;

			res.BlockTileAtlas = Assets.GetTextureAtlas("tiles_gasmembrane");
			res.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_gasmembrane_place");
			res.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
			res.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_mesh_tops_decor_info");
			res.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_mesh_tops_decor_place_info");

			return res;
		}

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);

			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			var cell = go.AddOrGet<SimCellOccupier>();
			cell.setGasImpermeable = true;
			cell.doReplaceElement = false;

			go.AddOrGet<TileTemperature>();

			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MeshTileConfig.BlockTileConnectorID;
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
        }

        public override void DoPostConfigureUnderConstruction(GameObject go) =>
			go.AddOrGet<KAnimGridTileVisualizer>();

        public override void DoPostConfigureComplete(GameObject go)
        {
			GeneratedBuildings.RemoveLoopingSounds(go);

			go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles);

			go.AddComponent<SimTemperatureTransfer>();

			go.AddComponent<PublicZoneTile>();
		}
    }
}
