using System;
using TUNING;
using UnityEngine;

namespace NewWirelessAutomatic
{
	// Token: 0x02000002 RID: 2
	public class EmitterConfig : IBuildingConfig
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("WirelessSignalEmitter", 1, 1, "critter_sensor_kanim", 30, 30f, NWA_CONST.TMass, NWA_CONST.TMate, 1600f, 0, DECOR.NONE, NOISE_POLLUTION.NONE, 0.2f);
			buildingDef.Overheatable = (buildingDef.Floodable = (buildingDef.Entombable = false));
			buildingDef.RequiresPowerInput = true;
			buildingDef.ViewMode = OverlayModes.Logic.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.SceneLayer = 19;
			buildingDef.EnergyConsumptionWhenActive = 25f;
			buildingDef.SelfHeatKilowattsWhenActive = (buildingDef.ExhaustKilowattsWhenActive = 0f);
			buildingDef.PowerInputOffset = new CellOffset(0, 0);
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
			SoundEventVolumeCache.instance.AddVolume("switchgaspressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
			GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "WirelessSignalEmitter");
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(default(CellOffset));
			return buildingDef;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002153 File Offset: 0x00000353
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000216A File Offset: 0x0000036A
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<SignalEmitter>(go).EmitChannel = 100;
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go).unNetworkedValue = 0;
		}
	}
}
