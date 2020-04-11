using System;
using Harmony;

namespace SuperComicLib.HUSystem
{
	// Token: 0x0200001F RID: 31
	[HarmonyPatch(typeof(EnergySim), "EnergySim200ms")]
	public class UPDATE_SIM_200ms
	{
		// Token: 0x06000086 RID: 134 RVA: 0x00004BA9 File Offset: 0x00002DA9
		public static void Postfix(float dt)
		{
			HUSimUpdater.Update(dt);
		}
	}
}
