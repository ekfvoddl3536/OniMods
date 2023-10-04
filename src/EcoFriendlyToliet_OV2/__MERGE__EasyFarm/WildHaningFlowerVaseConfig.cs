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
    using static GlobalConsts.EcoFriendlyWildHangingPot;
    public sealed class WildHaningFlowerVaseConfig : IBuildingConfig
    {
        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID, 1, 2, ANISTR,
                HITPOINTS.TIER0, CONSTRUCTION_TIME_SECONDS.TIER1,
                MassKG, Materials,
                MELTING_POINT_KELVIN.TIER0, BuildLocationRule.OnCeiling,
                TUNING.DECOR.BONUS.TIER0, TUNING.NOISE_POLLUTION.NONE);

            def.Floodable = false;
            def.Overheatable = false;
            def.ViewMode = OverlayModes.Decor.ID;
            def.AudioCategory = TUNING.AUDIO.CATEGORY.GLASS;
            def.AudioSize = TUNING.AUDIO.SIZE.LARGE;
            def.GenerateOffsets(1, 1);

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<Storage>();

            Prioritizable.AddRef(go);

            var wpp = go.AddOrGet<WildPlantablePlot>();
            wpp.AddDepositTag(GameTags.DecorSeed);
            wpp.occupyingObjectVisualOffset = new Vector3(0, -0.25f, 0);

            go.AddOrGet<FlowerVase>();

            go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
        }
    }
}
