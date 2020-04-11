using System;
using Harmony;

namespace EasyFarming
{
	// Token: 0x02000012 RID: 18
	[HarmonyPatch(typeof(OvercrowdingMonitor), "IsOvercrowded", new Type[]
	{
		typeof(OvercrowdingMonitor.Instance)
	})]
	public static class OvercrowdedPatch01
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002FA7 File Offset: 0x000011A7
		public static bool Prefix(ref bool __result)
		{
			__result = false;
			return false;
		}
	}
}
