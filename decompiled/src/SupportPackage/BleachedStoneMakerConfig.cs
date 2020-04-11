using System;
using TUNING;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x02000003 RID: 3
	public sealed class BleachedStoneMakerConfig : IBuildingConfig
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002378 File Offset: 0x00000578
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("BleachedStoneMaker", 2, 2, "rust_deoxidizer_kanim", 30, 30f, IN_Constants.BleachedStoneMaker.TMass, IN_Constants.BleachedStoneMaker.TMate, 800f, 1, DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER3, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.PowerInputOffset = new CellOffset(1, 0);
			buildingDef.InputConduitType = 1;
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.EnergyConsumptionWhenActive = 150f;
			buildingDef.SelfHeatKilowattsWhenActive = 8f;
			buildingDef.ViewMode = OverlayModes.GasConduits.ID;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			return buildingDef;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002424 File Offset: 0x00000624
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			KPrefabIDExtensions.AddTag(go.GetComponent<KSelectable>(), RoomConstraints.ConstraintTags.IndustrialMachinery);
			EntityTemplateExtensions.AddOrGet<WaterPurifier>(go);
			Storage storage = EntityTemplateExtensions.AddOrGet<Storage>(go);
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			storage.showInUI = true;
			ElementDropper elementDropper = EntityTemplateExtensions.AddOrGet<ElementDropper>(go);
			elementDropper.emitMass = 25f;
			elementDropper.emitOffset = new Vector2(1f, 1f);
			elementDropper.emitTag = GameTagExtensions.CreateTag(-839728230);
			ConduitConsumer cc = EntityTemplateExtensions.AddOrGet<ConduitConsumer>(go);
			cc.alwaysConsume = false;
			cc.capacityKG = 1f;
			cc.capacityTag = GameTagExtensions.CreateTag(-1324664829);
			cc.consumptionRate = 0.2f;
			ElementConverter ec = EntityTemplateExtensions.AddOrGet<ElementConverter>(go);
			ec.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(cc.capacityTag, 0.5f)
			};
			ec.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.5f, -839728230, 323.15f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0)
			};
			Prioritizable.AddRef(go);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000253B File Offset: 0x0000073B
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
