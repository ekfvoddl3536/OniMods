using System;
using Harmony;

namespace WaterfallSpace
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(UnstableGroundManager), "Spawn", new Type[]
	{
		typeof(int),
		typeof(Element),
		typeof(float),
		typeof(float),
		typeof(byte),
		typeof(int)
	})]
	public class UGM_SP_WATERBOMB
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000024BF File Offset: 0x000006BF
		public static bool Prefix(int cell, Element element, float mass, float temperature, byte disease_idx, int disease_count)
		{
			if (element.IsLiquid)
			{
				UGM_PATCH_WATERBOMB.Spawn(Grid.CellToPosCCC(cell, 30), mass, disease_idx, disease_count);
				return false;
			}
			return true;
		}
	}
}
