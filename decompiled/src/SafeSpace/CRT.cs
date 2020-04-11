using System;
using System.Collections;
using Harmony;

namespace SafeSpace
{
	// Token: 0x02000002 RID: 2
	[HarmonyPatch(typeof(SeasonManager))]
	[HarmonyPatch("OnSpawn")]
	public static class CRT
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void Postfix(SeasonManager __instance)
		{
			Traverse traverse = Traverse.Create(__instance);
			string[] vs = traverse.Field<string[]>("SeasonLoop").Value;
			IDictionary vs2 = traverse.Field<IDictionary>("seasons").Value;
			int x = 0;
			int max = vs.Length;
			while (x < max)
			{
				vs[x] = "Default";
				x++;
			}
			vs2.Remove("MeteorShower");
		}

		// Token: 0x04000001 RID: 1
		public const string SAFE = "Default";

		// Token: 0x04000002 RID: 2
		public const string DANGER = "MeteorShower";
	}
}
