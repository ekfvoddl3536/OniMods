using System;
using Harmony;
using KSerialization;

namespace LiquidMethaneEngine
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
		// Token: 0x0600000B RID: 11 RVA: 0x00002392 File Offset: 0x00000592
		public static void Postfix(string type_name, ref Type __result)
		{
			if (type_name == typeof(LiquidMethaneEngineConfig).AssemblyQualifiedName)
			{
				__result = typeof(LiquidMethaneEngineConfig);
			}
		}
	}
}
