using System;
using Harmony;
using UnityEngine;

namespace EasyFarming
{
	// Token: 0x02000010 RID: 16
	[HarmonyPatch(typeof(EvilFlowerConfig))]
	[HarmonyPatch("CreatePrefab")]
	public static class Patchs3
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002F98 File Offset: 0x00001198
		public static void Postfix(ref GameObject __result)
		{
			CON.FUNC(__result);
		}
	}
}
