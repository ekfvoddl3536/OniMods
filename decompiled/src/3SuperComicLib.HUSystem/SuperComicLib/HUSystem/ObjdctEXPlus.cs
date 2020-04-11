using System;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000021 RID: 33
	public static class ObjdctEXPlus
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00004C4A File Offset: 0x00002E4A
		public static int GetRotatedOffsetCell(this Building bd, CellOffset cell)
		{
			return Grid.OffsetCell(Grid.PosToCell(TransformExtensions.GetPosition(bd.transform)), cell);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004C62 File Offset: 0x00002E62
		public static bool HasFlag(this UtilityConnections target, UtilityConnections val)
		{
			return (target & val) > 0;
		}
	}
}
