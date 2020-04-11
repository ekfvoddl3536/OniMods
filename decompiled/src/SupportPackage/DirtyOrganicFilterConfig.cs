using System;
using TUNING;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x02000008 RID: 8
	public class DirtyOrganicFilterConfig : IBuildingConfig
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002C2C File Offset: 0x00000E2C
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("DirtyOrganicFilter", 1, 2, "mineraldeoxidizer_kanim", 30, 60f, IN_Constants.DirtyOrganicFilter.TMass, IN_Constants.DirtyOrganicFilter.TMate, 1600f, 1, DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 90f;
			buildingDef.SelfHeatKilowattsWhenActive = 2f;
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			return buildingDef;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002CA0 File Offset: 0x00000EA0
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			Prioritizable.AddRef(go);
			Electrolyzer electrolyzer = EntityTemplateExtensions.AddOrGet<Electrolyzer>(go);
			electrolyzer.hasMeter = false;
			electrolyzer.maxMass = 1.5f;
			Storage st = EntityTemplateExtensions.AddOrGet<Storage>(go);
			st.showInUI = true;
			st.capacityKg = 10000f;
			ElementDropper elementDropper = EntityTemplateExtensions.AddOrGet<ElementDropper>(go);
			elementDropper.emitMass = 1000f;
			elementDropper.emitOffset = new Vector3(0f, 0.5f);
			elementDropper.emitTag = GameTagExtensions.CreateTag(1624244999);
			ManualDeliveryKG mdkg = EntityTemplateExtensions.AddOrGet<ManualDeliveryKG>(go);
			mdkg.SetStorage(st);
			mdkg.requestedItemTag = GameTagExtensions.CreateTag(869554203);
			mdkg.capacity = 1000f;
			mdkg.refillMass = 200f;
			mdkg.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.Id;
			ElementConverter ec = EntityTemplateExtensions.AddOrGet<ElementConverter>(go);
			ec.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(mdkg.requestedItemTag, 0.9f)
			};
			ec.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.448f, 721531317, 0f, false, false, 0f, 1f, 1f, byte.MaxValue, 0),
				new ElementConverter.OutputElement(0.452f, 1624244999, 348.15f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0)
			};
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002E05 File Offset: 0x00001005
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
