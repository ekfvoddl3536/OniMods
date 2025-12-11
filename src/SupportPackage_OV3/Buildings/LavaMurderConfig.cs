// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#if PRIVATE_CORE
using UnityEngine;
using static TUNING.BUILDINGS;

namespace SupportPackage;

using static GlobalConsts.LavaMurder;
public sealed class LavaMurderConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        var def = BuildingTemplates.CreateBuildingDef(
            ID, 1, 1, ANISTR,
            HITPOINTS.TIER1, CONSTRUCTION_TIME_SECONDS.TIER3,
            TMass, TMate,
            MELTING_POINT_KELVIN.TIER2,
            BuildLocationRule.Anywhere,
            DECOR.BONUS.TIER2,
            TUNING.NOISE_POLLUTION.NOISY.TIER4);

        def.RequiresPowerInput = true;
        def.EnergyConsumptionWhenActive = POWER_USE;

        def.ViewMode = OverlayModes.Power.ID;

        def.AudioCategory = TUNING.AUDIO.CATEGORY.METAL;
        def.AudioSize = TUNING.AUDIO.SIZE.SMALL;

        def.Floodable = false;
        def.Overheatable = false;
        def.Entombable = false;

        def.SceneLayer = Grid.SceneLayer.Building;

        return def;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
    }

    public override void DoPostConfigureUnderConstruction(GameObject go)
    {
        AddVisualizer(go);
    }

    public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
    {
        AddVisualizer(go);
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        AddVisualizer(go);

        go.AddOrGetDef<PoweredActiveController.Def>();

        var wee = go.AddOrGet<WorldElementExecutor>();
        wee.radius = RADIUS;
        wee.targetElement = SimHashes.Magma;
    }

    private static void AddVisualizer(GameObject go)
    {
        var rv = go.AddOrGet<RangeVisualizer>();
        rv.OriginOffset = default;
        rv.RangeMin = new Vector2I(-RADIUS, -RADIUS);
        rv.RangeMax = new Vector2I(+RADIUS, +RADIUS);
        rv.TestLineOfSight = false;
        rv.BlockingTileVisible = false;

        go.GetComponent<KPrefabID>().instantiateFn += static k => k.GetComponent<RangeVisualizer>().BlockingCb = static u => false;
    }
}
#endif