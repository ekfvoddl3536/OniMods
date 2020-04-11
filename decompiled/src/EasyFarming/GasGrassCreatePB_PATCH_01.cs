using System;
using Harmony;
using UnityEngine;

namespace EasyFarming
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(GasGrassConfig), "CreatePrefab")]
	public static class GasGrassCreatePB_PATCH_01
	{
		// Token: 0x06000008 RID: 8 RVA: 0x0000224A File Offset: 0x0000044A
		public static void Postfix(ref GameObject __result)
		{
			StateMachineControllerExtensions.GetDef<CropSleepingMonitor.Def>(__result).lightIntensityThreshold = 100f;
		}
	}
}
