using System;
using TUNING;
using UnityEngine;

namespace AdvancedExcavatorAndPump
{
	// Token: 0x02000003 RID: 3
	public class AdvancedExcavatorConfig : IBuildingConfig
	{
		// Token: 0x06000011 RID: 17 RVA: 0x000023C0 File Offset: 0x000005C0
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AdvancedExcavatorAndPump", 1, 1, "sc_aeap_kanim", 100, 120f, Constants.TMass, Constants.TMate, 1600f, 0, DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER4, 0.2f);
			BuildingTemplates.CreateFoundationTileDef(buildingDef);
			buildingDef.Overheatable = (buildingDef.UseStructureTemperature = true);
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.SceneLayer = 31;
			buildingDef.isSolidTile = true;
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 240f;
			buildingDef.SelfHeatKilowattsWhenActive = 0f;
			buildingDef.ExhaustKilowattsWhenActive = 0f;
			buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
			buildingDef.OutputConduitType = 2;
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(default(CellOffset));
			return buildingDef;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002490 File Offset: 0x00000690
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			EntityTemplateExtensions.AddOrGet<Storage>(go).showInUI = true;
			EntityTemplateExtensions.AddOrGet<Operational>(go);
			EntityTemplateExtensions.AddOrGet<MiningSounds>(go);
			SimCellOccupier simCellOccupier = EntityTemplateExtensions.AddOrGet<SimCellOccupier>(go);
			simCellOccupier.setGasImpermeable = (simCellOccupier.setLiquidImpermeable = true);
			simCellOccupier.doReplaceElement = false;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000024E8 File Offset: 0x000006E8
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			EntityTemplateExtensions.AddOrGet<ConduitDispenser>(go).conduitType = 2;
			ElementConsumer elementConsumer = EntityTemplateExtensions.AddOrGet<ElementConsumer>(go);
			elementConsumer.configuration = 1;
			elementConsumer.sampleCellOffset = new Vector3(0f, -1f);
			elementConsumer.storeOnConsume = true;
			elementConsumer.showDescriptor = (elementConsumer.isRequired = false);
			elementConsumer.consumptionRadius = 2;
			elementConsumer.consumptionRate = 10f;
			AdvancedExcavator advancedExcavator = EntityTemplateExtensions.AddOrGet<AdvancedExcavator>(go);
			advancedExcavator.maxLength = 100;
			advancedExcavator.hardnessLv = 250;
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
			go.GetComponent<KPrefabID>().AddTag(GameTags.FloorTiles, false);
			Prioritizable.AddRef(go);
		}
	}
}
