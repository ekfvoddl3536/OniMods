// MIT License
//
// Copyright (c) 2022-2023. SuperComic (ekfvoddl3535@naver.com)
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
using static TUNING.BUILDINGS;

namespace EcoFriendlyToilet
{
    using static GlobalConsts.EcoFriendlyWildFarmTile;
    public sealed class WildFarmTileConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID, 1, 1, ANISTR,
                HITPOINTS.TIER2, CONSTRUCTION_TIME_SECONDS.TIER2,
                MassKG, Materials,
                MELTING_POINT_KELVIN.TIER1, BuildLocationRule.Tile,
                TUNING.DECOR.BONUS.TIER0, TUNING.NOISE_POLLUTION.NONE);

            def.Floodable = false;
            def.Entombable = false;
            def.Overheatable = false;
            def.ForegroundLayer = Grid.SceneLayer.BuildingBack;
            def.AudioCategory = TUNING.AUDIO.CATEGORY.HOLLOW_METAL;
            def.AudioSize = TUNING.AUDIO.SIZE.SMALL;
            def.BaseTimeUntilRepair = -1f;
            def.SceneLayer = Grid.SceneLayer.TileMain;
            def.ConstructionOffsetFilter = BuildingDef.ConstructionOffsetFilter_OneDown;
            def.PermittedRotations = PermittedRotations.FlipV;
            def.DragBuild = true;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            GeneratedBuildings.MakeBuildingAlwaysOperational(go);

            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            var sco = go.AddOrGet<SimCellOccupier>();
            sco.notifyOnMelt = true;
            sco.doReplaceElement = true;

            go.AddOrGet<TileTemperature>();

            BuildingTemplates.CreateDefaultStorage(go).SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);

            var wpp = go.AddOrGet<WildPlantablePlot>();
            wpp.occupyingObjectRelativePosition = new Vector3(0, 1, 0);
            wpp.AddDepositTag(GameTags.CropSeed);
            wpp.AddDepositTag(GameTags.WaterSeed);
            wpp.SetFertilizationFlags(true, false);

            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
            go.AddOrGet<AnimTileable>();

            Prioritizable.AddRef(go);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            GeneratedBuildings.RemoveLoopingSounds(go);

            var kpid = go.GetComponent<KPrefabID>();
            kpid.AddTag(GameTags.FarmTiles);
            kpid.prefabSpawnFn += SetupFarmPlot;
        }

        private static void SetupFarmPlot(GameObject go)
        {
            var ori = go.GetComponent<Rotatable>().GetOrientation();
            var wpp = go.GetComponent<WildPlantablePlot>();

            switch (ori)
            {
                case Orientation.Neutral:
                case Orientation.FlipH:
                    wpp.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Top);
                    break;

                case Orientation.R90:
                case Orientation.R270:
                    wpp.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Side);
                    break;

                default:
                    wpp.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Bottom);
                    break;
            }
        }
    }
}
