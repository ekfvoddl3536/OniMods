using System;
using Harmony;
using UnityEngine;

namespace WaterfallSpace
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(UnstableGroundManager), "Spawn", new Type[]
	{
		typeof(Vector3),
		typeof(Element),
		typeof(float),
		typeof(float),
		typeof(byte),
		typeof(int)
	})]
	public class UGM_PATCH_WATERBOMB
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002460 File Offset: 0x00000660
		internal static void Spawn(Vector3 pos, float mass, byte disease_idx, int disease_count)
		{
			FallingWater.instance.AddParticle(pos, (byte)ElementLoader.GetElementIndex(1836671383), mass, (float)Random.Range(272, 370), disease_idx, disease_count, false, false, false, false);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000249F File Offset: 0x0000069F
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
