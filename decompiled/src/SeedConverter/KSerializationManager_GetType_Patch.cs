using System;
using Harmony;
using KSerialization;

namespace SeedConverter
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(Manager))]
	[HarmonyPatch("GetType")]
	[HarmonyPatch(new Type[]
	{
		typeof(string)
	})]
	public static class KSerializationManager_GetType_Patch
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000021C4 File Offset: 0x000003C4
		public static void Postfix(string type_name, ref Type __result)
		{
			if (type_name == typeof(SeedConverterConfig).AssemblyQualifiedName)
			{
				__result = typeof(SeedConverterConfig);
			}
		}
	}
}
