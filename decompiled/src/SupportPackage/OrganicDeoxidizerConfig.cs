using System;
using TUNING;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x02000007 RID: 7
	public sealed class OrganicDeoxidizerConfig : IBuildingConfig
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002A6C File Offset: 0x00000C6C
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("OrganicDeoxidizer", 2, 2, "rust_deoxidizer_kanim", 30, 30f, IN_Constants.OrganicDeoxidizer.TMass, IN_Constants.OrganicDeoxidizer.TMate, 800f, 1, DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER3, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.PowerInputOffset = new CellOffset(1, 0);
			buildingDef.EnergyConsumptionWhenActive = 280f;
			buildingDef.ExhaustKilowattsWhenActive = 3f;
			buildingDef.SelfHeatKilowattsWhenActive = 3f;
			buildingDef.ViewMode = OverlayModes.Oxygen.ID;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
			return buildingDef;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002B10 File Offset: 0x00000D10
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			KPrefabIDExtensions.AddTag(go.GetComponent<KSelectable>(), RoomConstraints.ConstraintTags.IndustrialMachinery);
			EntityTemplateExtensions.AddOrGet<RustDeoxidizer>(go).maxMass = 2f;
			Storage st = EntityTemplateExtensions.AddOrGet<Storage>(go);
			st.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			st.showInUI = true;
			ManualDeliveryKG mdkg = EntityTemplateExtensions.AddOrGet<ManualDeliveryKG>(go);
			mdkg.SetStorage(st);
			mdkg.allowPause = false;
			mdkg.requestedItemTag = GameTags.Dirt;
			mdkg.capacity = 2000f;
			mdkg.refillMass = 200f;
			mdkg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
			ElementConverter ec = EntityTemplateExtensions.AddOrGet<ElementConverter>(go);
			ec.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(mdkg.requestedItemTag, 5f)
			};
			ec.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.33f, -1528777920, 0f, true, false, 1f, 1f, 1f, byte.MaxValue, 0)
			};
			Prioritizable.AddRef(go);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002C14 File Offset: 0x00000E14
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
