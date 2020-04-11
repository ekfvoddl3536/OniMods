using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace NewWirelessAutomatic
{
	// Token: 0x02000009 RID: 9
	public class ReceiverConfig : IBuildingConfig
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000023DC File Offset: 0x000005DC
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("WirelessSignalReceiver", 1, 1, "critter_sensor_kanim", 30, 30f, NWA_CONST.TMass, NWA_CONST.TMate, 1600f, 0, DECOR.NONE, NOISE_POLLUTION.NONE, 0.2f);
			buildingDef.Overheatable = (buildingDef.Floodable = false);
			buildingDef.RequiresPowerInput = true;
			buildingDef.ViewMode = OverlayModes.Logic.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.SceneLayer = 19;
			buildingDef.EnergyConsumptionWhenActive = 25f;
			buildingDef.SelfHeatKilowattsWhenActive = (buildingDef.ExhaustKilowattsWhenActive = 0f);
			buildingDef.PowerInputOffset = new CellOffset(0, 0);
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "WirelessSignalReceiver");
			buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
			{
				LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, default(CellOffset), "Wireless Signal", UI.LOGIC_PORTS.CONTROL_OPERATIONAL_ACTIVE, UI.LOGIC_PORTS.CONTROL_OPERATIONAL_INACTIVE, true, false)
			};
			return buildingDef;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002501 File Offset: 0x00000701
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002518 File Offset: 0x00000718
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<SignalReceiver>(go).ReceiveChannel = 100;
		}
	}
}
