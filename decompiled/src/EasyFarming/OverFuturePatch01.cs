using System;
using Harmony;

namespace EasyFarming
{
	// Token: 0x02000011 RID: 17
	[HarmonyPatch(typeof(OvercrowdingMonitor), "IsFutureOvercrowded", new Type[]
	{
		typeof(OvercrowdingMonitor.Instance)
	})]
	public static class OverFuturePatch01
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002FA1 File Offset: 0x000011A1
		public static bool Prefix(ref bool __result)
		{
			__result = false;
			return false;
		}
	}
}
