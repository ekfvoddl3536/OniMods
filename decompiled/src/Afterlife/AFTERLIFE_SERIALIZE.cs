using System;
using Harmony;
using Klei.AI;

namespace Afterlife
{
	// Token: 0x02000003 RID: 3
	[HarmonyPatch(typeof(Game), "OnSpawn")]
	public static class AFTERLIFE_SERIALIZE
	{
		// Token: 0x06000002 RID: 2 RVA: 0x000020C8 File Offset: 0x000002C8
		public static void Prefix()
		{
			int cycle = Math.Min(GameClock.Instance.GetCycle() / 3, 396);
			Constants.full = new AttributeModifier("CaloriesMax", 250000f * (float)cycle, "After-Life (EXTREME HARDCORE)", false, false, true);
			Constants.delta = new AttributeModifier("CaloriesDelta", -(416.666656f * (float)cycle), "After-Life (EXTREME HARDCORE)", false, false, true);
			Constants.feedingTime = 2E-05f - 5E-08f * (float)cycle;
		}
	}
}
