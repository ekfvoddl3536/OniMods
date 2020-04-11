using System;
using TUNING;
using UnityEngine;

namespace AdvancedGeneratos
{
	// Token: 0x02000008 RID: 8
	internal class RefinedCarbonGeneratorConfig : IBuildingConfig
	{
		// Token: 0x06000016 RID: 22 RVA: 0x0000283C File Offset: 0x00000A3C
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("RefinedCarbonGenerator", 3, 3, "generatorphos_kanim", 30, 60f, Constans.RefinedCarbonGenerator.MateMassKg, Constans.RefinedCarbonGenerator.Materials, 2400f, 1, DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER5, 0.2f);
			buildingDef.GeneratorWattageRating = 1200f;
			buildingDef.GeneratorBaseCapacity = 1200f;
			buildingDef.ExhaustKilowattsWhenActive = 0f;
			buildingDef.SelfHeatKilowattsWhenActive = 4f;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.AudioSize = "large";
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			return buildingDef;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000028E0 File Offset: 0x00000AE0
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
			Tag t = GameTagExtensions.Create(-902240476);
			EnergyGenerator eg = EntityTemplateExtensions.AddOrGet<EnergyGenerator>(go);
			EnergyGenerator energyGenerator = eg;
			EnergyGenerator.Formula formula = default(EnergyGenerator.Formula);
			formula.inputs = new EnergyGenerator.InputItem[]
			{
				new EnergyGenerator.InputItem(t, 1f, 500f)
			};
			formula.outputs = new EnergyGenerator.OutputItem[]
			{
				new EnergyGenerator.OutputItem(1960575215, 0.02f, false, new CellOffset(1, 0), 348.15f)
			};
			energyGenerator.formula = formula;
			eg.powerDistributionOrder = 8;
			Storage st = EntityTemplateExtensions.AddOrGet<Storage>(go);
			st.capacityKg = 500f;
			st.showInUI = true;
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(go);
			Prioritizable.AddRef(go);
			ManualDeliveryKG manualDeliveryKG = EntityTemplateExtensions.AddOrGet<ManualDeliveryKG>(go);
			manualDeliveryKG.allowPause = false;
			manualDeliveryKG.SetStorage(st);
			manualDeliveryKG.requestedItemTag = t;
			manualDeliveryKG.capacity = st.capacityKg;
			manualDeliveryKG.refillMass = 100f;
			manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.PowerFetch.IdHash;
			Tinkerable.MakePowerTinkerable(go);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000029F2 File Offset: 0x00000BF2
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
