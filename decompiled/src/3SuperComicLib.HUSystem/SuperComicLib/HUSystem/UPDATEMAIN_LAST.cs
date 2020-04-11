using System;
using Harmony;

namespace SuperComicLib.HUSystem
{
	// Token: 0x0200001E RID: 30
	[HarmonyPatch(typeof(CircuitManager), "Sim200msLast")]
	public class UPDATEMAIN_LAST
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00004B94 File Offset: 0x00002D94
		public static void Postfix(float dt)
		{
			Constants.manager.Sim200msLast(dt);
		}
	}
}
