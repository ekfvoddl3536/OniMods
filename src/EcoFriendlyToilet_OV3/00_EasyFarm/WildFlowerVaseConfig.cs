// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine;
using static TUNING.BUILDINGS;

namespace EcoFriendlyToilet;

using static GlobalConsts.EcoFriendlyWildFlowerPot;
public sealed class WildFlowerVaseConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        var def = BuildingTemplates.CreateBuildingDef(
            ID, 1, 1, ANISTR,
            HITPOINTS.TIER0, CONSTRUCTION_TIME_SECONDS.TIER1,
            MassKG, Materials,
            MELTING_POINT_KELVIN.TIER0, BuildLocationRule.OnFloor,
            TUNING.DECOR.BONUS.TIER0, TUNING.NOISE_POLLUTION.NONE);

        def.Floodable = false;
        def.Overheatable = false;
        def.ViewMode = OverlayModes.Decor.ID;
        def.AudioCategory = TUNING.AUDIO.CATEGORY.GLASS;
        def.AudioSize = TUNING.AUDIO.SIZE.LARGE;

        return def;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        go.AddOrGet<Storage>();

        Prioritizable.AddRef(go);

        go.AddOrGet<WildPlantablePlot>().AddDepositTag(GameTags.DecorSeed);
        go.AddOrGet<FlowerVase>();

        go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
    }
}
