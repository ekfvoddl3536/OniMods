using System;
using Harmony;

namespace SuperComicLib.HUSystem
{
	// Token: 0x0200001A RID: 26
	[HarmonyPatch(typeof(Game), "OnSpawn")]
	public class P2
	{
		// Token: 0x0600007B RID: 123 RVA: 0x000049A2 File Offset: 0x00002BA2
		public static void Postfix()
		{
			HUSimUpdater.Clear();
		}
	}
}
