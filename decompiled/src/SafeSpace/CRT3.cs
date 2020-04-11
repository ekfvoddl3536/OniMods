using System;
using Harmony;

namespace SafeSpace
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(SeasonManager))]
	[HarmonyPatch("UpdateState")]
	public static class CRT3
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020AA File Offset: 0x000002AA
		public static bool Prefix()
		{
			return false;
		}
	}
}
