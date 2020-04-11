using System;
using SuperComicLib;
using TUNING;
using UnityEngine;

namespace NuclearReaction
{
	// Token: 0x02000003 RID: 3
	public sealed class FusionPowerPlantConfig : IBuildingConfig
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000215C File Offset: 0x0000035C
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FusionPowerPlant", 5, 4, "steamturbine_kanim", 100, 480f, Constans.FS_MASS_KG, Constans.FS_Materials, 9999f, 1, BUILDINGS.DECOR.PENALTY.TIER5, NOISE_POLLUTION.NONE, 0.2f);
			buildingDef.GeneratorWattageRating = (buildingDef.GeneratorBaseCapacity = 2500000f);
			buildingDef.PermittedRotations = 3;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.Overheatable = (buildingDef.UseStructureTemperature = true);
			buildingDef.AudioCategory = "Metal";
			buildingDef.ModifiesTemperature = (buildingDef.RequiresPowerOutput = (buildingDef.Floodable = true));
			buildingDef.ExhaustKilowattsWhenActive = 0f;
			buildingDef.SelfHeatKilowattsWhenActive = 4f;
			buildingDef.PowerOutputOffset = new CellOffset(2, 1);
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(1, 2);
			buildingDef.InputConduitType = 2;
			buildingDef.OutputConduitType = 1;
			return buildingDef;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002248 File Offset: 0x00000448
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(go);
			EntityTemplateExtensions.AddOrGet<RequireOutputs>(go).ignoreFullPipe = false;
			Storage ist = EntityTemplateExtensions.AddOrGet<Storage>(go);
			ist.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			ist.capacityKg = 100f;
			ist.showInUI = true;
			EntityTemplateExtensions.AddOrGet<Prioritizable>(go);
			ConduitConsumer cc = EntityTemplateExtensions.AddOrGet<ConduitConsumer>(go);
			cc.storage = ist;
			cc.conduitType = 2;
			cc.consumptionRate = 10f;
			cc.capacityKG = 50f;
			cc.capacityTag = GameTagExtensions.CreateTag(-751997156);
			cc.forceAlwaysSatisfied = (cc.alwaysConsume = true);
			cc.wrongElementResult = 1;
			AdvancedEnergyGenerator advancedEnergyGenerator = go.AddComponent<AdvancedEnergyGenerator>();
			advancedEnergyGenerator.powerDistributionOrder = 8;
			advancedEnergyGenerator.OutStorage = (advancedEnergyGenerator.InStorage = ist);
			EnergyGenerator.Formula inOutItems = default(EnergyGenerator.Formula);
			inOutItems.inputs = new EnergyGenerator.InputItem[]
			{
				new EnergyGenerator.InputItem(cc.capacityTag, 1f, cc.capacityKG)
			};
			inOutItems.outputs = new EnergyGenerator.OutputItem[]
			{
				new EnergyGenerator.OutputItem(-1554872654, 0.9929f, true, new CellOffset(0, 1), 1473.15f)
			};
			advancedEnergyGenerator.InOutItems = inOutItems;
			ConduitDispenser conduitDispenser = EntityTemplateExtensions.AddOrGet<ConduitDispenser>(go);
			conduitDispenser.alwaysDispense = (conduitDispenser.invertElementFilter = true);
			conduitDispenser.conduitType = 1;
			conduitDispenser.elementFilter = new SimHashes[]
			{
				-1046145888
			};
			Tinkerable.MakePowerTinkerable(go);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000023B3 File Offset: 0x000005B3
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
