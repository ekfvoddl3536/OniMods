using System;
using Harmony;
using UnityEngine;

namespace WaterfallSpace
{
	// Token: 0x02000007 RID: 7
	[HarmonyPatch(typeof(UnstableGroundManager), "SpawnOld", new Type[]
	{
		typeof(Vector3),
		typeof(Element),
		typeof(float),
		typeof(float),
		typeof(byte),
		typeof(int)
	})]
	public class UGM_OLD_WATERBOMB
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000024E6 File Offset: 0x000006E6
		public static bool Prefix(Vector3 pos, Element element, float mass, float temperature, byte disease_idx, int disease_count)
		{
			if (element.IsLiquid)
			{
				UGM_PATCH_WATERBOMB.Spawn(pos, mass, disease_idx, disease_count);
				return false;
			}
			return true;
		}
	}
}
