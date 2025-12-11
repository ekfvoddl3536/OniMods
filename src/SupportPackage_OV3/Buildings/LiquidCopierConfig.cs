// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#if PRIVATE_CORE
using UnityEngine;
using static TUNING.BUILDINGS;

namespace SupportPackage;

using static GlobalConsts.LiquidCopier;
public class LiquidCopierConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        var def = BuildingTemplates.CreateBuildingDef(
            ID, 1, 2, ANISTR,
            HITPOINTS.TIER1, CONSTRUCTION_TIME_SECONDS.TIER1,
            TMass, TMate,
            MELTING_POINT_KELVIN.TIER1,
            BuildLocationRule.Anywhere,
            DECOR.NONE,
            TUNING.NOISE_POLLUTION.NOISY.TIER1);

        def.RequiresPowerInput = true;
        def.Floodable = false;

        def.EnergyConsumptionWhenActive = POWER_USE;
        
        def.InputConduitType = ConduitType.Liquid;
        def.OutputConduitType = ConduitType.Liquid;

        def.ViewMode = OverlayModes.LiquidConduits.ID;
        def.AudioCategory = TUNING.AUDIO.CATEGORY.METAL;

        def.UtilityInputOffset = default;
        def.UtilityOutputOffset = new CellOffset(0, 1);

        def.PermittedRotations = PermittedRotations.R360;

        GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, ID);
        return def;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

        var ecp = go.AddOrGet<ElementCopier>();
        ecp.portType = ConduitType.Liquid;
        ecp.portOffset = default;
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
        go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
    }
}
#endif