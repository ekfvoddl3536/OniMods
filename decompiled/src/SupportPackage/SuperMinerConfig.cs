using System;
using SuperComicLib;
using TUNING;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x02000005 RID: 5
	public class SuperMinerConfig : IBuildingConfig
	{
		// Token: 0x0600000D RID: 13 RVA: 0x0000274C File Offset: 0x0000094C
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SuperMiner", 2, 2, "auto_miner_kanim", 10, 10f, IN_Constants.SuperMiner.TMass, IN_Constants.SuperMiner.TMates, 1600f, 14, DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER1, 0.2f);
			buildingDef.Floodable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 120f;
			buildingDef.ExhaustKilowattsWhenActive = 0f;
			buildingDef.SelfHeatKilowattsWhenActive = 2f;
			buildingDef.PermittedRotations = 2;
			buildingDef.PowerInputOffset = new CellOffset(0, 0);
			buildingDef.UtilityInputOffset = new CellOffset(1, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
			buildingDef.InputConduitType = (buildingDef.OutputConduitType = 2);
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SuperMiner");
			return buildingDef;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000282A File Offset: 0x00000A2A
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			EntityTemplateExtensions.AddOrGet<Operational>(go);
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(go);
			EntityTemplateExtensions.AddOrGet<MiningSounds>(go);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002841 File Offset: 0x00000A41
		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			SuperMinerConfig.AddVisualizer(go, true);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000284A File Offset: 0x00000A4A
		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			SuperMinerConfig.AddVisualizer(go, false);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002854 File Offset: 0x00000A54
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			Storage storage = EntityTemplateExtensions.AddOrGet<Storage>(go);
			storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			storage.showInUI = false;
			AutoMiner autoMiner = EntityTemplateExtensions.AddOrGet<AutoMiner>(go);
			autoMiner.x = -10;
			autoMiner.y = 0;
			autoMiner.width = 22;
			autoMiner.height = 12;
			autoMiner.vision_offset = new CellOffset(0, 1);
			SuperMinerConfig.AddVisualizer(go, false);
			Storage st = go.AddComponent<Storage>();
			st.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			st.capacityKg = 100f;
			st.showInUI = true;
			Storage ost = go.AddComponent<Storage>();
			ost.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			ost.capacityKg = 100f;
			ost.showInUI = true;
			ConduitConsumer conduitConsumer = EntityTemplateExtensions.AddOrGet<ConduitConsumer>(go);
			conduitConsumer.storage = st;
			conduitConsumer.capacityTag = GameTags.Liquid;
			conduitConsumer.capacityKG = 100f;
			conduitConsumer.consumptionRate = 5f;
			conduitConsumer.forceAlwaysSatisfied = true;
			ConduitDispenser conduitDispenser = EntityTemplateExtensions.AddOrGet<ConduitDispenser>(go);
			conduitDispenser.storage = ost;
			conduitDispenser.conduitType = 2;
			conduitDispenser.alwaysDispense = true;
			EntityTemplateExtensions.AddOrGet<RequireOutputs>(go).ignoreFullPipe = true;
			LiquidCoolingSystem liquidCoolingSystem = EntityTemplateExtensions.AddOrGet<LiquidCoolingSystem>(go);
			liquidCoolingSystem.CoolantTag = GameTags.Liquid;
			liquidCoolingSystem.OutStorage = ost;
			liquidCoolingSystem.InStorage = st;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002978 File Offset: 0x00000B78
		public static void AddVisualizer(GameObject prefab, bool movable)
		{
			StationaryChoreRangeVisualizer stationaryChoreRangeVisualizer = EntityTemplateExtensions.AddOrGet<StationaryChoreRangeVisualizer>(prefab);
			stationaryChoreRangeVisualizer.x = -10;
			stationaryChoreRangeVisualizer.y = 0;
			stationaryChoreRangeVisualizer.width = 22;
			stationaryChoreRangeVisualizer.height = 12;
			stationaryChoreRangeVisualizer.vision_offset = new CellOffset(0, 1);
			stationaryChoreRangeVisualizer.movable = movable;
			stationaryChoreRangeVisualizer.blocking_tile_visible = false;
			prefab.GetComponent<KPrefabID>().instantiateFn += new KPrefabID.PrefabFn(SuperMinerConfig.SuperMinerConfig_instantiateFn);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000029DB File Offset: 0x00000BDB
		private static void SuperMinerConfig_instantiateFn(GameObject go)
		{
			StationaryChoreRangeVisualizer component = go.GetComponent<StationaryChoreRangeVisualizer>();
			if (SuperMinerConfig.CacheFunc == null)
			{
				SuperMinerConfig.CacheFunc = new Func<int, bool>(AutoMiner.DigBlockingCB);
			}
			component.blocking_cb = SuperMinerConfig.CacheFunc;
		}

		// Token: 0x04000001 RID: 1
		private const int poX = -10;

		// Token: 0x04000002 RID: 2
		private const int widX = 22;

		// Token: 0x04000003 RID: 3
		private const int hieY = 12;

		// Token: 0x04000004 RID: 4
		public static Func<int, bool> CacheFunc;
	}
}
