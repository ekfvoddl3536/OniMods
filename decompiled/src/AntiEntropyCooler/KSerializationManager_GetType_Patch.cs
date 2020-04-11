using System;
using Harmony;
using KSerialization;

namespace AntiEntropyCooler
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(Manager))]
	[HarmonyPatch("GetType")]
	[HarmonyPatch(new Type[]
	{
		typeof(string)
	})]
	public static class KSerializationManager_GetType_Patch
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002332 File Offset: 0x00000532
		public static void Postfix(string type_name, ref Type __result)
		{
			if (type_name == typeof(AntiEntropyCoolerConfig).AssemblyQualifiedName)
			{
				__result = typeof(AntiEntropyCoolerConfig);
			}
		}
	}
}
