using System;
using Harmony;
using KSerialization;

namespace DupeWhatDiamond
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
		// Token: 0x06000009 RID: 9 RVA: 0x000024E2 File Offset: 0x000006E2
		public static void Postfix(string type_name, ref Type __result)
		{
			if (type_name == typeof(DiamondCompressorConfig).AssemblyQualifiedName)
			{
				__result = typeof(DiamondCompressorConfig);
			}
		}
	}
}
