using System;
using Harmony;

namespace SuperComicLib.HUSystem
{
	// Token: 0x0200001D RID: 29
	[HarmonyPatch(typeof(CircuitManager), "Sim200msFirst")]
	public class UPDATEMAIN_00
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00004B7F File Offset: 0x00002D7F
		public static void Postfix(float dt)
		{
			Constants.manager.Refresh(dt);
		}
	}
}
