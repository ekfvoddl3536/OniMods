using System;
using Harmony;

namespace SafeSpace
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(SeasonManager), "DoBombardment")]
	public static class DoBombardment_PATCH_01
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020B0 File Offset: 0x000002B0
		public static bool Prefix()
		{
			return false;
		}
	}
}
