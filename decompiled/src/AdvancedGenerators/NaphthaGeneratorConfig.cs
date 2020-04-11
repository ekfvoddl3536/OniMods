using System;
using TUNING;
using UnityEngine;

namespace AdvancedGeneratos
{
	// Token: 0x02000003 RID: 3
	public class NaphthaGeneratorConfig : IBuildingConfig
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020A8 File Offset: 0x000002A8
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("NaphthaGenerator", 3, 4, "generatorpetrol_kanim", 100, 100f, Constans.NaphthaGenerator.MateMassKg, Constans.NaphthaGenerator.Materials, 2400f, 1, DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER4, 0.2f);
			buildingDef.GeneratorWattageRating = (buildingDef.GeneratorBaseCapacity = 850f);
			buildingDef.ExhaustKilowattsWhenActive = 1f;
			buildingDef.SelfHeatKilowattsWhenActive = 8f;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
			buildingDef.PowerOutputOffset = new CellOffset(0, 0);
			buildingDef.RequiresPowerOutput = true;
			buildingDef.InputConduitType = 2;
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			return buildingDef;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002168 File Offset: 0x00000368
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(go);
			Storage st = EntityTemplateExtensions.AddOrGet<Storage>(go);
			st.showInUI = true;
			ConduitConsumer conduitConsumer = EntityTemplateExtensions.AddOrGet<ConduitConsumer>(go);
			conduitConsumer.conduitType = 2;
			conduitConsumer.consumptionRate = 10f;
			conduitConsumer.capacityKG = 100f;
			conduitConsumer.forceAlwaysSatisfied = true;
			conduitConsumer.wrongElementResult = 1;
			ElementConsumer ec = EntityTemplateExtensions.AddOrGet<ElementConsumer>(go);
			ec.storage = st;
			ec.configuration = 0;
			ec.elementToConsume = -1528777920;
			ec.capacityKG = 1f;
			ec.consumptionRate = 0.1f;
			ec.consumptionRadius = 2;
			ec.isRequired = (ec.storeOnConsume = true);
			ConsumGasPowerGenerator consumGasPowerGenerator = EntityTemplateExtensions.AddOrGet<ConsumGasPowerGenerator>(go);
			consumGasPowerGenerator.InStorage = st;
			consumGasPowerGenerator.OutStorage = go.AddComponent<Storage>();
			consumGasPowerGenerator.Consumer = ec;
			EnergyGenerator.Formula inOutItems = default(EnergyGenerator.Formula);
			inOutItems.inputs = new EnergyGenerator.InputItem[]
			{
				new EnergyGenerator.InputItem(GameTagExtensions.CreateTag(1157157570), 1f, 10f),
				new EnergyGenerator.InputItem(GameTagExtensions.CreateTag(-1528777920), 0.04f, 1f)
			};
			inOutItems.outputs = new EnergyGenerator.OutputItem[]
			{
				new EnergyGenerator.OutputItem(1960575215, 0.04f, false, new CellOffset(0, 0), 320.15f)
			};
			consumGasPowerGenerator.InOutItems = inOutItems;
			Tinkerable.MakePowerTinkerable(go);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022CA File Offset: 0x000004CA
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
