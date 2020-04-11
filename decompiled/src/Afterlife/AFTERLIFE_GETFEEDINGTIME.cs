using System;
using Harmony;

namespace Afterlife
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(Edible), "GetFeedingTime")]
	public static class AFTERLIFE_GETFEEDINGTIME
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000021E6 File Offset: 0x000003E6
		public static bool Prefix(ref float __result, Edible __instance, Worker worker)
		{
			__result = Funcs.GetFeedingTime(worker, __instance);
			return false;
		}
	}
}
