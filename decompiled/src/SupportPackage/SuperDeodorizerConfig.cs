using System;
using TUNING;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x02000017 RID: 23
	public class SuperDeodorizerConfig : IBuildingConfig
	{
		// Token: 0x06000058 RID: 88 RVA: 0x00004064 File Offset: 0x00002264
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SuperDeodorizer", 1, 1, "co2filter_kanim", 30, 30f, IN_Constants.SuperDeodorizer.TMass, IN_Constants.SuperDeodorizer.TMates, 1600f, 0, BUILDINGS.DECOR.NONE, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
			buildingDef.Overheatable = false;
			buildingDef.ViewMode = OverlayModes.Oxygen.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.UtilityInputOffset = (buildingDef.UtilityOutputOffset = new CellOffset(0, 0));
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 10f;
			return buildingDef;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000040F0 File Offset: 0x000022F0
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(go);
			Prioritizable.AddRef(go);
			Storage st = EntityTemplateExtensions.AddOrGet<Storage>(go);
			st.showInUI = true;
			st.capacityKg = 400f;
			st.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			ElementConsumer elementConsumer = EntityTemplateExtensions.AddOrGet<ElementConsumer>(go);
			elementConsumer.elementToConsume = 721531317;
			elementConsumer.consumptionRate = 2f;
			elementConsumer.consumptionRadius = 9;
			elementConsumer.sampleCellOffset = Vector3.zero;
			elementConsumer.isRequired = (elementConsumer.showDescriptor = false);
			elementConsumer.showInStatusPanel = (elementConsumer.storeOnConsume = true);
			ElementDropper elementDropper = EntityTemplateExtensions.AddOrGet<ElementDropper>(go);
			elementDropper.emitMass = 25f;
			elementDropper.emitTag = GameTagExtensions.CreateTag(867327137);
			elementDropper.emitOffset = Vector3.zero;
			Tag filter;
			filter..ctor("Filter");
			Tag coxy = GameTagExtensions.CreateTag(721531317);
			ElementConverter ev = EntityTemplateExtensions.AddOrGet<ElementConverter>(go);
			ev.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(filter, 0.85f),
				new ElementConverter.ConsumedElement(coxy, 2f)
			};
			ev.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.4f, 867327137, 0f, false, true, 0f, 0.5f, 0f, byte.MaxValue, 0),
				new ElementConverter.OutputElement(1.98f, -1528777920, 0f, false, false, 0f, 0f, 0f, byte.MaxValue, 0)
			};
			ManualDeliveryKG manualDeliveryKG = EntityTemplateExtensions.AddOrGet<ManualDeliveryKG>(go);
			manualDeliveryKG.allowPause = false;
			manualDeliveryKG.SetStorage(st);
			manualDeliveryKG.requestedItemTag = filter;
			manualDeliveryKG.capacity = 550f;
			manualDeliveryKG.refillMass = 55f;
			manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
			EntityTemplateExtensions.AddOrGet<AirFilter>(go).filterTag = filter;
			EntityTemplateExtensions.AddOrGet<KBatchedAnimController>(go).randomiseLoopedOffset = true;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000042CB File Offset: 0x000024CB
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGetDef<ActiveController.Def>(go);
		}
	}
}
