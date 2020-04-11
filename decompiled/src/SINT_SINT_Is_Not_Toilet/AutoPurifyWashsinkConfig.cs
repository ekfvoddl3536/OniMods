using System;
using TUNING;
using UnityEngine;

namespace SINT_SINT_Is_Not_Toilet
{
	// Token: 0x02000002 RID: 2
	public class AutoPurifyWashsinkConfig : IBuildingConfig
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AutoPurifyWashsink", 2, 3, "wash_sink_kanim", 30, 40f, Consts.AutoPurifyWashsink.MASSKG, Consts.AutoPurifyWashsink.Materials, 1600f, 1, DECOR.BONUS.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.PowerInputOffset = new CellOffset(1, 1);
			buildingDef.EnergyConsumptionWhenActive = 60f;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.AudioCategory = "Metal";
			return buildingDef;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020CC File Offset: 0x000002CC
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation, false);
			Storage st = EntityTemplateExtensions.AddOrGet<Storage>(go);
			st.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
			st.showInUI = true;
			st.doDiseaseTransfer = false;
			st.capacityKg = 20000f;
			st.fetchCategory = 0;
			HandSanitizer hs = EntityTemplateExtensions.AddOrGet<HandSanitizer>(go);
			hs.massConsumedPerUse = 5f;
			hs.consumedElement = 1836671383;
			hs.outputElement = 1832607973;
			hs.diseaseRemovalCount = 120000;
			hs.maxUses = 40;
			hs.dirtyMeterOffset = 1;
			hs.dumpWhenFull = true;
			EntityTemplateExtensions.AddOrGet<DirectionControl>(go);
			HandSanitizer.Work work = EntityTemplateExtensions.AddOrGet<HandSanitizer.Work>(go);
			work.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_washbasin_kanim")
			};
			work.workTime = 5f;
			work.trackUses = true;
			ManualDeliveryKG manualDeliveryKG = EntityTemplateExtensions.AddOrGet<ManualDeliveryKG>(go);
			manualDeliveryKG.SetStorage(st);
			manualDeliveryKG.requestedItemTag = GameTagExtensions.CreateTag(1836671383);
			manualDeliveryKG.minimumMass = 5f;
			manualDeliveryKG.capacity = 200f;
			manualDeliveryKG.refillMass = 40f;
			manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
			ManualDeliveryKG mdkg2 = go.AddComponent<ManualDeliveryKG>();
			mdkg2.SetStorage(st);
			mdkg2.ShowStatusItem = false;
			mdkg2.requestedItemTag = GameTags.Filter;
			mdkg2.minimumMass = 50f;
			mdkg2.capacity = 200f;
			mdkg2.refillMass = mdkg2.minimumMass;
			mdkg2.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;
			ElementConverter ec = EntityTemplateExtensions.AddOrGet<ElementConverter>(go);
			ec.SetStorage(st);
			ec.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(GameTagExtensions.CreateTag(hs.outputElement), 0.001575f),
				new ElementConverter.ConsumedElement(mdkg2.requestedItemTag, 0.0025f)
			};
			ec.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.001575f, hs.consumedElement, 0f, true, true, 0f, 0.5f, 0f, byte.MaxValue, 0),
				new ElementConverter.OutputElement(0.0025f, -1396791454, 333.15f, false, true, 0f, 0.5f, 1f, Db.Get().Diseases.GetIndex(Db.Get().Diseases.FoodGerms.IdHash), 120)
			};
			ElementDropper elementDropper = EntityTemplateExtensions.AddOrGet<ElementDropper>(go);
			elementDropper.emitMass = 2000f;
			elementDropper.emitOffset = new Vector2(0f, 1f);
			elementDropper.emitTag = GameTagExtensions.CreateTag(-1396791454);
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(go);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002378 File Offset: 0x00000578
		public override void DoPostConfigureComplete(GameObject go)
		{
			this.Func(go);
			Object.DestroyImmediate(go.GetComponent<EnergyConsumer>());
			EntityTemplateExtensions.AddOrGet<AutoPurify>(go);
			go.GetComponent<RequireInputs>().SetRequirements(false, false);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000023A0 File Offset: 0x000005A0
		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			this.Func(go);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000023A9 File Offset: 0x000005A9
		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			this.Func(go);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000023B2 File Offset: 0x000005B2
		private void Func(GameObject go)
		{
			Object.DestroyImmediate(go.GetComponent<LogicOperationalController>());
			Object.DestroyImmediate(go.GetComponent<LogicPorts>());
		}
	}
}
