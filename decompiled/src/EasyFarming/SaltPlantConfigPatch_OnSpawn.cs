using System;
using Harmony;

namespace EasyFarming
{
	// Token: 0x0200000E RID: 14
	[HarmonyPatch(typeof(SaltPlantConfig), "OnSpawn")]
	public static class SaltPlantConfigPatch_OnSpawn
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002F2E File Offset: 0x0000112E
		public static bool Prefix()
		{
			return false;
		}
	}
}
