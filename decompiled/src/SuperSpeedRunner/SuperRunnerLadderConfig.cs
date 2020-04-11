using System;
using TUNING;
using UnityEngine;

namespace SuperSpeedRunner
{
	// Token: 0x02000005 RID: 5
	public class SuperRunnerLadderConfig : IBuildingConfig
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000021A4 File Offset: 0x000003A4
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("RunnerLadder", 1, 1, "ladder_poi_kanim", 30, 10f, Constants.RunnerLadder.TMass, Constants.RunnerLadder.TMate, 1600f, 0, DECOR.PENALTY.TIER0, NOISE_POLLUTION.NONE, 0.2f);
			BuildingTemplates.CreateLadderDef(buildingDef);
			buildingDef.Floodable = (buildingDef.Overheatable = (buildingDef.Entombable = false));
			buildingDef.AudioCategory = "Metal";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.DragBuild = true;
			return buildingDef;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000222F File Offset: 0x0000042F
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			Ladder ladder = EntityTemplateExtensions.AddOrGet<Ladder>(go);
			ladder.upwardsMovementSpeedMultiplier = 40f;
			ladder.downwardsMovementSpeedMultiplier = 40f;
			EntityTemplateExtensions.AddOrGet<AnimTileable>(go);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002259 File Offset: 0x00000459
		public override void DoPostConfigureComplete(GameObject go)
		{
		}
	}
}
