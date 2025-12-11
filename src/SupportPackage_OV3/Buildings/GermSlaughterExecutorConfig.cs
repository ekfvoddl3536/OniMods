// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#if PRIVATE_CORE
using UnityEngine;

namespace SupportPackage;

using static GlobalConsts.GermSlaughterExecutor;
using static TUNING.BUILDINGS;

public sealed class GermSlaughterExecutorConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        var def = BuildingTemplates.CreateBuildingDef(
            ID, 1, 1, ANISTR,
            HITPOINTS.TIER1, CONSTRUCTION_TIME_SECONDS.TIER2,
            TMass, TMate,
            MELTING_POINT_KELVIN.TIER3,
            BuildLocationRule.Anywhere,
            DECOR.BONUS.TIER5,
            TUNING.NOISE_POLLUTION.NONE);

        def.ViewMode = OverlayModes.Disease.ID;

        def.Floodable = false;
        def.Overheatable = false;

        def.RequiresPowerInput = false;

        def.AudioCategory = TUNING.AUDIO.CATEGORY.METAL;

        def.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(default);
        return def;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        const float CAPACITY = 1f;

        go.AddOrGet<LoopingSounds>();

        Prioritizable.AddRef(go);

        var storage = BuildingTemplates.CreateDefaultStorage(go);
        storage.showInUI = true;
        storage.capacityKg = CAPACITY;
        storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);

        var ge = go.AddOrGet<WorldGermExecutor>();
        ge.storage = storage;
        ge.filterUseRate = FILTER_USE;
        ge.radius = RADIUS;

        var mdk = go.AddOrGet<ManualDeliveryKG>();
        mdk.SetStorage(storage);
        mdk.RequestedItemTag = SimHashes.BleachStone.CreateTag();
        mdk.capacity = CAPACITY;
        mdk.refillMass = FILTER_USE * 10;
        mdk.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;

        go.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
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
        go.AddOrGetDef<ActiveController.Def>();
        AddVisualizer(go);
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