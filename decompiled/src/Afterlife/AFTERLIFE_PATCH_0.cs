using System;
using Harmony;

namespace Afterlife
{
	// Token: 0x02000002 RID: 2
	[HarmonyPatch(typeof(GameClock), "Sim33ms")]
	public static class AFTERLIFE_PATCH_0
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void Prefix(float dt, GameClock __instance)
		{
			int cycle = __instance.GetCycle();
			if (__instance.GetTimeSinceStartOfCycle() + dt >= 600f && cycle < 396 && cycle % 3 == 0)
			{
				Constants.full.SetValue(Constants.full.Value + 250000f);
				Constants.delta.SetValue(Constants.delta.Value - 416.666656f);
				Constants.feedingTime = 2E-05f - 5E-08f * (float)cycle;
			}
		}
	}
}
