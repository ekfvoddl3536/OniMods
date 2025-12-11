// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using static EventSystem;

namespace AdvancedGenerators;

public abstract class AdvancedGeneratorBase : Generator
{
    protected static readonly IntraObjectHandler<AdvancedGeneratorBase> OnActivateChangeDelegate =
        new IntraObjectHandler<AdvancedGeneratorBase>(OnActivateChangedStatic);

    protected static readonly IntraObjectHandler<AdvancedGeneratorBase> OnCopySettingsDelegate =
        new IntraObjectHandler<AdvancedGeneratorBase>(OnCopySettingsStatic);

    public bool UseCopySettings = false;
    public bool HasMeter = true;
    public Meter.Offset MeterOffset;
    protected MeterController meter;

    private static void OnActivateChangedStatic(AdvancedGeneratorBase gen, object data) =>
        gen.OnActivateChanged((Operational)data);

    private static void OnCopySettingsStatic(AdvancedGeneratorBase gen, object data) =>
        gen.OnCopySettings((UnityEngine.GameObject)data);

    protected virtual void OnActivateChanged(Operational op)
    {
        var db = Db.Get();

        selectable.SetStatusItem(
            db.StatusItemCategories.Power,
            op.IsActive
            ? db.BuildingStatusItems.Wattage
            : db.BuildingStatusItems.GeneratorOffline,
            this);
    }

    protected virtual void OnCopySettings(UnityEngine.GameObject from) { }

    protected override void OnPrefabInit()
    {
        base.OnPrefabInit();
        Subscribe((int)GameHashes.ActiveChanged, OnActivateChangeDelegate);

        if (UseCopySettings)
            Subscribe((int)GameHashes.CopySettings, OnCopySettingsDelegate);
    }

    protected override void OnSpawn()
    {
        base.OnSpawn();
        OnSpawnPost();
    }

    protected virtual void OnSpawnPost()
    {
        if (HasMeter)
            meter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", MeterOffset, Grid.SceneLayer.NoLayer,
            [
                "meter_target",
                "meter_fill",
                "meter_frame",
                "meter_OL"
            ]);
    }
}