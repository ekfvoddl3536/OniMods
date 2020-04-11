using System;
using Harmony;

namespace SafeSpace
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(SeasonManager))]
	[HarmonyPatch("Sim200ms")]
	[HarmonyPatch(new Type[]
	{
		typeof(float)
	})]
	public static class CRT6
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020AD File Offset: 0x000002AD
		public static bool Prefix()
		{
			return false;
		}
	}
}
