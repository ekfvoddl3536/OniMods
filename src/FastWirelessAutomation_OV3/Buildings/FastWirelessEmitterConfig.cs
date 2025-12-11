// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using TUNING;
using UnityEngine;

namespace FastWirelessAutomation;

using static GlobalConsts;
using static GlobalConsts.FastWirelessEmitter;
using static STRINGS.UI.LOGIC_PORTS;
using LPORT = LogicPorts.Port;
public class FastWirelessEmitterConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        BuildingDef bd = BuildingTemplates.CreateBuildingDef(ID, 1, 1, ANISTR, HITPT, CONTIME,
            TMass, TMate, MELTPT, BuildLocationRule.Anywhere, DECOR.NONE, NOISE_POLLUTION.NONE);

        bd.Overheatable = bd.Floodable = bd.Entombable = false;

        bd.RequiresPowerInput = true;

        bd.ViewMode = OverlayModes.Logic.ID;

        bd.AudioCategory = AU_CATE;
        bd.SceneLayer = Grid.SceneLayer.Building;

        bd.EnergyConsumptionWhenActive = USE_POWER;
        bd.SelfHeatKilowattsWhenActive = bd.ExhaustKilowattsWhenActive = 0;

        bd.PowerInputOffset = default; // (0, 0)

        bd.DragBuild = true;

        bd.LogicInputPorts = new List<LPORT>(1)
        {
            new LPORT(
                LogicOperationalController.PORT_ID,
                default,
                INPUT_PORTS,
                GATE_SINGLE_INPUT_ONE_ACTIVE,
                GATE_SINGLE_INPUT_ONE_INACTIVE,
                false,
                LogicPortSpriteType.Input)
        };

        var inst = SoundEventVolumeCache.instance;
        inst.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
        inst.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);

        GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);

        return bd;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) =>
        go.AddOrGet<BuildingComplete>().isManuallyOperated = false;

    public override void DoPostConfigureComplete(GameObject go)
    {
        go.AddOrGet<CopyBuildingSettings>();

        go.AddOrGet<SignalEmitter>();

        go.AddOrGet<LogicOperationalController>().unNetworkedValue = 0;

        go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
    }
}
