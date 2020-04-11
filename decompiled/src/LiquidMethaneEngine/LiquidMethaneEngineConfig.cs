using System;
using TUNING;
using UnityEngine;

namespace LiquidMethaneEngine
{
	// Token: 0x02000003 RID: 3
	public sealed class LiquidMethaneEngineConfig : IBuildingConfig
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000212C File Offset: 0x0000032C
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LiquidMethaneEngine", 7, 5, "rocket_petroleum_engine_kanim", 1000, 480f, Consts.MateMassKG, Consts.Materials, 9999f, 1, BUILDINGS.DECOR.NONE, NOISE_POLLUTION.NOISY.TIER2, 0.2f);
			BuildingTemplates.CreateRocketBuildingDef(buildingDef);
			buildingDef.SceneLayer = 21;
			buildingDef.OverheatTemperature = 2273.15f;
			buildingDef.Floodable = (buildingDef.RequiresPowerInput = false);
			buildingDef.AttachmentSlotTag = GameTags.Rocket;
			buildingDef.ObjectLayer = 1;
			buildingDef.attachablePosition = new CellOffset(0, 0);
			return buildingDef;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000021BC File Offset: 0x000003BC
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
			EntityTemplateExtensions.AddOrGet<LoopingSounds>(go);
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
			EntityTemplateExtensions.AddOrGet<BuildingAttachPoint>(go).points = new BuildingAttachPoint.HardPoint[]
			{
				new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
			};
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002220 File Offset: 0x00000420
		public override void DoPostConfigureComplete(GameObject go)
		{
			RocketEngine rocketEngine = EntityTemplateExtensions.AddOrGet<RocketEngine>(go);
			rocketEngine.fuelTag = GameTagExtensions.CreateTag(371787440);
			rocketEngine.efficiency = 50f;
			rocketEngine.explosionEffectHash = -31719612;
			rocketEngine.requireOxidizer = false;
			rocketEngine.exhaustElement = -899515856;
			rocketEngine.exhaustTemperature = 398.15f;
			EntityTemplateExtensions.AddOrGet<RocketModule>(go).SetBGKAnim(Assets.GetAnim("rocket_petroleum_engine_kanim"));
			EntityTemplates.ExtendBuildingToRocketModule(go);
		}
	}
}
