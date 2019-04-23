using UnityEngine;
using TUNING;

namespace NewWirelessAutomatic
{
    using static NWA_CONST;
    using static NWA_CONST.Receiver;
    public class ReceiverConfig : IBuildingConfig
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
            bd.PowerInputOffset = new CellOffset(0, 0);

            SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
            SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);

            GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);

            return bd;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) =>
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go) => Func(go);

        public override void DoPostConfigureUnderConstruction(GameObject go) => Func(go);

        public override void DoPostConfigureComplete(GameObject go)
        {
            Func(go);
            go.AddOrGet<SignalReceiver>().ReceiveChannel = DEF_CHANNEL;
        }

        private void Func(GameObject go) => GeneratedBuildings.RegisterLogicPorts(go, LogicSwitchConfig.OUTPUT_PORT);
    }
}
