using System;
using Harmony;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000010 RID: 16
	[HarmonyPatch(typeof(KAnimGraphTileVisualizer), "get_ConnectionManager")]
	public class PATCH5
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00002E6F File Offset: 0x0000106F
		public static void Postfix(ref IUtilityNetworkMgr __result)
		{
			if (__result == null)
			{
				__result = Constants.hupipeSystem;
			}
		}
	}
}
