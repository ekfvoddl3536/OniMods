using System;
using SuperComicLib;
using TUNING;
using UnityEngine;

namespace AdvancedGeneratos
{
	// Token: 0x02000004 RID: 4
	public class EcoFriendlyMethaneGeneratorConfig : IBuildingConfig
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000022E4 File Offset: 0x000004E4
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("EcoFriendlyMethaneGenerator", 4, 3, "generatormethane_kanim", 100, 60f, Constans.EcoFriendlyMethaneGenerator.TMass, Constans.EcoFriendlyMethaneGenerator.TMate, 2400f, 1, DECOR.BONUS.TIER1, NOISE_POLLUTION.NOISY.TIER6, 0.2f);
			buildingDef.GeneratorBaseCapacity = (buildingDef.GeneratorWattageRating = 1000f);
			buildingDef.ExhaustKilowattsWhenActive = 2f;
			buildingDef.SelfHeatKilowattsWhenActive = 4f;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.PowerOutputOffset = (buildingDef.UtilityInputOffset = new CellOffset(0, 0));
			buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
			buildingDef.PermittedRotations = 3;
			buildingDef.InputConduitType = (buildingDef.OutputConduitType = 1);
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
			return buildingDef;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000023B4 File Offset: 0x000005B4
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(go);
			Storage ist = EntityTemplateExtensions.AddOrGet<Storage>(go);
			Storage ost = go.AddComponent<Storage>();
			ConduitConsumer cc = EntityTemplateExtensions.AddOrGet<ConduitConsumer>(go);
			cc.storage = ist;
			cc.conduitType = 1;
			cc.consumptionRate = 0.2f;
			cc.capacityKG = 1f;
			cc.capacityTag = GameTagExtensions.CreateTag(-841236436);
			cc.forceAlwaysSatisfied = true;
			cc.wrongElementResult = 1;
			ConduitDispenser conduitDispenser = EntityTemplateExtensions.AddOrGet<ConduitDispenser>(go);
			conduitDispenser.conduitType = 1;
			conduitDispenser.storage = ost;
			ElementConsumer ec = EntityTemplateExtensions.AddOrGet<ElementConsumer>(go);
			ec.storage = ist;
			ec.configuration = 0;
			ec.elementToConsume = -1528777920;
			ec.capacityKG = 1f;
			ec.consumptionRate = 0.1f;
			ec.consumptionRadius = 2;
			ec.isRequired = (ec.storeOnConsume = true);
			ManualDeliveryKG manualDeliveryKG = EntityTemplateExtensions.AddOrGet<ManualDeliveryKG>(go);
			manualDeliveryKG.SetStorage(ist);
			manualDeliveryKG.allowPause = false;
			manualDeliveryKG.capacity = 400f;
			manualDeliveryKG.refillMass = 10f;
			manualDeliveryKG.requestedItemTag = Constans.EcoFriendlyMethaneGenerator.Filter;
			manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.GeneratePower.Id;
			ConsumGasPowerGenerator adg = EntityTemplateExtensions.AddOrGet<ConsumGasPowerGenerator>(go);
			adg.Consumer = ec;
			AdvancedEnergyGenerator advancedEnergyGenerator = adg;
			EnergyGenerator.Formula inOutItems = default(EnergyGenerator.Formula);
			inOutItems.inputs = new EnergyGenerator.InputItem[]
			{
				new EnergyGenerator.InputItem(cc.capacityTag, 0.1f, 1f),
				new EnergyGenerator.InputItem(GameTagExtensions.CreateTag(-1528777920), 0.03f, 1f),
				new EnergyGenerator.InputItem(Constans.EcoFriendlyMethaneGenerator.Filter, 0.07f, 50f)
			};
			inOutItems.outputs = new EnergyGenerator.OutputItem[]
			{
				new EnergyGenerator.OutputItem(1836671383, 0.07f, false, new CellOffset(1, 1), 348.15f),
				new EnergyGenerator.OutputItem(1960575215, 0.03f, true, 383.15f)
			};
			advancedEnergyGenerator.InOutItems = inOutItems;
			adg.InStorage = ist;
			adg.OutStorage = ost;
			Tinkerable.MakeFarmTinkerable(go);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000025CD File Offset: 0x000007CD
		public override void DoPostConfigureComplete(GameObject go)
		{
			EntityTemplateExtensions.AddOrGet<LogicOperationalController>(go);
			EntityTemplateExtensions.AddOrGetDef<PoweredActiveController.Def>(go);
		}
	}
}
