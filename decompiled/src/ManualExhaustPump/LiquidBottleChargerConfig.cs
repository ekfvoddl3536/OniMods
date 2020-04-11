using System;
using TUNING;
using UnityEngine;

namespace ManualExhaustPump
{
	// Token: 0x02000002 RID: 2
	public sealed class LiquidBottleChargerConfig : IBuildingConfig
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LiquidBottleCharger", 1, 2, "valveliquid_kanim", 30, 30f, Constants.LiquidBottleCharger._Tmass, Constants.LiquidBottleCharger._Tmate, 1600f, 1, DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
			buildingDef.Floodable = false;
			buildingDef.InputConduitType = 2;
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.PermittedRotations = 2;
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
			return buildingDef;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020C4 File Offset: 0x000002C4
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			Storage st = BuildingTemplates.CreateDefaultStorage(go, false);
			st.showDescriptor = (st.allowItemRemoval = true);
			st.storageFilters = STORAGEFILTERS.LIQUIDS;
			st.capacityKg = 1000f;
			st.SetDefaultStoredItemModifiers(GasReservoirConfig.ReservoirStoredItemModifiers);
			EntityTemplateExtensions.AddOrGet<SCValve>(go);
			ConduitConsumer conduitConsumer = EntityTemplateExtensions.AddOrGet<ConduitConsumer>(go);
			conduitConsumer.conduitType = 2;
			conduitConsumer.ignoreMinMassCheck = (conduitConsumer.forceAlwaysSatisfied = (conduitConsumer.alwaysConsume = true));
			conduitConsumer.capacityKG = st.capacityKg;
			conduitConsumer.keepZeroMassObject = false;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002149 File Offset: 0x00000349
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGetDef<StorageController.Def>(go);
		}
	}
}
