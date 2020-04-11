using System;
using Harmony;
using KSerialization;

namespace ManualExhaustPump
{
	// Token: 0x02000008 RID: 8
	[HarmonyPatch(typeof(Manager), "GetType", new Type[]
	{
		typeof(string)
	})]
	public class KSerializationMgr_Patch
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00002618 File Offset: 0x00000818
		public static void Postfix(string type_name, ref Type __result)
		{
			if (type_name == typeof(SmartReservoir).AssemblyQualifiedName)
			{
				__result = typeof(SmartReservoir);
				return;
			}
			if (type_name == typeof(SCValve).AssemblyQualifiedName)
			{
				__result = typeof(SCValve);
			}
		}
	}
}
