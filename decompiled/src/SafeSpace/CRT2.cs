using System;
using Harmony;

namespace SafeSpace
{
	// Token: 0x02000003 RID: 3
	[HarmonyPatch(typeof(SeasonManager))]
	[HarmonyPatch("OnNewDay")]
	[HarmonyPatch(new Type[]
	{
		typeof(object)
	})]
	public static class CRT2
	{
		// Token: 0x06000002 RID: 2 RVA: 0x000020A7 File Offset: 0x000002A7
		public static bool Prefix()
		{
			return false;
		}
	}
}
