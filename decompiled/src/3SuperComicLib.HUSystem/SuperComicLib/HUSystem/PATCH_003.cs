using System;
using Harmony;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
	// Token: 0x0200000D RID: 13
	[HarmonyPatch(typeof(KAnimGraphTileVisualizer), "GetNeighbour")]
	public static class PATCH_003
	{
		// Token: 0x06000017 RID: 23 RVA: 0x000029B0 File Offset: 0x00000BB0
		public static bool Prefix(ref KAnimGraphTileVisualizer __result, Direction d, KAnimGraphTileVisualizer __instance)
		{
			__result = null;
			Vector2I v2;
			Grid.PosToXY(TransformExtensions.GetPosition(__instance.transform), ref v2);
			int idx = -1;
			switch (d)
			{
			case 0:
				if (v2.y < Grid.HeightInCells - 1)
				{
					idx = Grid.XYToCell(v2.x, v2.y + 1);
				}
				break;
			case 1:
				if (v2.x < Grid.WidthInCells - 1)
				{
					idx = Grid.XYToCell(v2.x + 1, v2.y);
				}
				break;
			case 2:
				if (v2.y > 0)
				{
					idx = Grid.XYToCell(v2.x, v2.y - 1);
				}
				break;
			case 3:
				if (v2.x > 0)
				{
					idx = Grid.XYToCell(v2.x - 1, v2.y);
				}
				break;
			}
			if (idx != -1)
			{
				ObjectLayer ol = 2;
				switch (__instance.connectionSource)
				{
				case 0:
					ol = 13;
					break;
				case 1:
					ol = 17;
					break;
				case 2:
					ol = 27;
					break;
				case 3:
					ol = 32;
					break;
				case 4:
					ol = 35;
					break;
				case 5:
					ol = 21;
					break;
				}
				GameObject go = Grid.Objects[idx, ol];
				if (go != null)
				{
					__result = go.GetComponent<KAnimGraphTileVisualizer>();
				}
			}
			return false;
		}
	}
}
