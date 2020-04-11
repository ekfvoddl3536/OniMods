using System;
using Harmony;

namespace SuperSpeedRunner
{
	// Token: 0x02000003 RID: 3
	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	public class RUNNER_PATCH_01
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public static void Prefix()
		{
			RUNNER_PATCH_01.Set(Constants.RunnerTile.ID_UPPER, Constants.RunnerTile.NAME, "어마무시한 속도로 달릴 수 있습니다.", "더 빠르게 달릴 수 있습니다.");
			RUNNER_PATCH_01.Set(Constants.RunnerLadder.ID_UPPER, Constants.RunnerLadder.NAME, "매우 빠른속도로 오르거나 내려갈 수 있습니다.", "특별한 사다리입니다.");
			ModUtil.AddBuildingToPlanScreen("Base", "RunnerTile");
			ModUtil.AddBuildingToPlanScreen("Base", "RunnerLadder");
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020CC File Offset: 0x000002CC
		private static void Set(string id, string name, string desc, string eff)
		{
			string temp = "STRINGS.BUILDINGS.PREFABS." + id + ".";
			Strings.Add(new string[]
			{
				temp + "NAME",
				name
			});
			Strings.Add(new string[]
			{
				temp + "DESC",
				desc
			});
			Strings.Add(new string[]
			{
				temp + "EFFECT",
				eff
			});
		}
	}
}
