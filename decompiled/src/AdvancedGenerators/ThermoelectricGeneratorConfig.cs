using System;
using TUNING;
using UnityEngine;

namespace AdvancedGeneratos
{
	// Token: 0x02000009 RID: 9
	public class ThermoelectricGeneratorConfig : IBuildingConfig
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002A0C File Offset: 0x00000C0C
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ThermoelectricGenerator", 4, 3, "generatormerc_kanim", 100, 45f, Constans.ThermoelectricGenerator.MateMassKg, Constans.ThermoelectricGenerator.Materials, 2400f, 1, DECOR.BONUS.TIER0, NOISE_POLLUTION.NOISY.TIER6, 0.2f);
			buildingDef.GeneratorWattageRating = (buildingDef.GeneratorBaseCapacity = 250f);
			buildingDef.ExhaustKilowattsWhenActive = -120f;
			buildingDef.SelfHeatKilowattsWhenActive = -8f;
			buildingDef.ViewMode = OverlayModes.Temperature.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.PowerOutputOffset = new CellOffset(1, 0);
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			return buildingDef;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002AB0 File Offset: 0x00000CB0
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(go);
			EntityTemplateExtensions.AddOrGet<ThermoelectricPowerGenerator>(go);
			EntityTemplateExtensions.AddOrGet<MinimumOperatingTemperature>(go).minimumTemperature = 283.15f;
			Tinkerable.MakePowerTinkerable(go);
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
