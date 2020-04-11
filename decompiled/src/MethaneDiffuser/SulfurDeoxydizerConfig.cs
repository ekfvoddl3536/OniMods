using System;
using TUNING;
using UnityEngine;

namespace SulfurDeoxydizer
{
	// Token: 0x02000005 RID: 5
	public class SulfurDeoxydizerConfig : IBuildingConfig
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002260 File Offset: 0x00000460
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SulfurDeoxidizer", 1, 2, "mineraldeoxidizer_kanim", 30, 60f, Constans.SUFDE_MateMass, Constans.SUFDE_Materials, 800f, 1, DECOR.PENALTY.TIER3, NOISE_POLLUTION.NOISY.TIER3, 0.2f);
			buildingDef.EnergyConsumptionWhenActive = 250f;
			buildingDef.PermittedRotations = 0;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.Overheatable = true;
			buildingDef.OverheatTemperature = 363.15f;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.RequiresPowerInput = (buildingDef.Breakable = true);
			buildingDef.PowerInputOffset = new CellOffset(0, 0);
			buildingDef.ExhaustKilowattsWhenActive = 0.5f;
			buildingDef.SelfHeatKilowattsWhenActive = 1f;
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(default(CellOffset));
			return buildingDef;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002328 File Offset: 0x00000528
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			Prioritizable.AddRef(go);
			Electrolyzer electrolyzer = EntityTemplateExtensions.AddOrGet<Electrolyzer>(go);
			electrolyzer.maxMass = 3f;
			electrolyzer.hasMeter = false;
			Storage st = EntityTemplateExtensions.AddOrGet<Storage>(go);
			st.capacityKg = 200f;
			st.showInUI = true;
			ManualDeliveryKG mdkg = EntityTemplateExtensions.AddOrGet<ManualDeliveryKG>(go);
			mdkg.allowPause = false;
			mdkg.SetStorage(st);
			mdkg.requestedItemTag = GameTagExtensions.CreateTag(-729385479);
			mdkg.capacity = 200f;
			mdkg.refillMass = 75f;
			mdkg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
			ElementConverter conv = EntityTemplateExtensions.AddOrGet<ElementConverter>(go);
			conv.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(mdkg.requestedItemTag, 0.65f)
			};
			conv.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(0.5f, -841236436, 320.15f, false, false, 0f, 1f, 1f, byte.MaxValue, 0)
			};
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002428 File Offset: 0x00000628
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
