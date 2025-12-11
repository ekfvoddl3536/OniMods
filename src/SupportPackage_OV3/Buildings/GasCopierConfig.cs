// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#if PRIVATE_CORE
using UnityEngine;
using static TUNING.BUILDINGS;

namespace SupportPackage;

using static GlobalConsts.GasCopier;
public class GasCopierConfig : IBuildingConfig
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

        def.InputConduitType = ConduitType.Gas;
        def.OutputConduitType = ConduitType.Gas;

        def.ViewMode = OverlayModes.GasConduits.ID;
        def.AudioCategory = TUNING.AUDIO.CATEGORY.METAL;

        def.UtilityInputOffset = default;
        def.UtilityOutputOffset = new CellOffset(0, 1);

        def.PermittedRotations = PermittedRotations.R360;

        GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, ID);
        return def;
    }

    private void AttachPort(GameObject go)
    {
        go.AddComponent<ConduitSecondaryOutput>().portInfo = new ConduitPortInfo(ConduitType.Gas, default);
    }

    public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
    {
        AttachPort(go);
    }

    public override void DoPostConfigureUnderConstruction(GameObject go)
    {
        AttachPort(go);
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

        var ecp = go.AddOrGet<ElementCopier>();
        ecp.portType = ConduitType.Gas;
        ecp.portOffset = default;
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        go.AddOrGetDef<PoweredActiveController.Def>().showWorkingStatus = true;
        go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
    }
}
#endif