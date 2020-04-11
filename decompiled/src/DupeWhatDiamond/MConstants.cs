using System;
using STRINGS;

namespace DupeWhatDiamond
{
	// Token: 0x02000002 RID: 2
	internal sealed class MConstants
	{
		// Token: 0x04000001 RID: 1
		public const string TechGroup = "Smelting";

		// Token: 0x04000002 RID: 2
		public const string TabGroup = "Utilities";

		// Token: 0x04000003 RID: 3
		public const string DWD_ID = "DiamondCompressor";

		// Token: 0x04000004 RID: 4
		public const string KANIMSTR = "oxylite_refinery_kanim";

		// Token: 0x04000005 RID: 5
		public const int HITPT = 100;

		// Token: 0x04000006 RID: 6
		public const float CONTIME = 120f;

		// Token: 0x04000007 RID: 7
		public const float MELPT = 3200f;

		// Token: 0x04000008 RID: 8
		public const float USEPOWER = 480f;

		// Token: 0x04000009 RID: 9
		public const float USE_IN = 2f;

		// Token: 0x0400000A RID: 10
		public const float USE_WAT = 5f;

		// Token: 0x0400000B RID: 11
		public const float OUT_RATE = 0.09f;

		// Token: 0x0400000C RID: 12
		public const float DIADROP_SIZE = 50f;

		// Token: 0x0400000D RID: 13
		public const float CL_CONSU_MASS_KG = 10f;

		// Token: 0x0400000E RID: 14
		public const float CL_CAPACITY = 100f;

		// Token: 0x0400000F RID: 15
		public const float ST_CAPC = 1000f;

		// Token: 0x04000010 RID: 16
		public const float OST_CAPC = 100f;

		// Token: 0x04000011 RID: 17
		public static readonly string DWD_ID_UPPER = "DiamondCompressor".ToUpper();

		// Token: 0x04000012 RID: 18
		public static readonly LocString NAME = UI.FormatAsLink("탄소 압축기", MConstants.DWD_ID_UPPER);

		// Token: 0x04000013 RID: 19
		public static readonly LocString DESC = UI.FormatAsLink("정제 탄소", "REFINEDCARBON") + "를 압축하여 " + UI.FormatAsLink("다이아몬드", "DIAMOND") + "를 생산합니다.";

		// Token: 0x04000014 RID: 20
		public const string EFFC = "이 시설은 다이아몬드를 만들기 위해 존재합니다.";

		// Token: 0x04000015 RID: 21
		public static readonly string[] TMaterials = new string[]
		{
			"Metal",
			"BuildableRaw"
		};

		// Token: 0x04000016 RID: 22
		public static readonly float[] TMateMassKG = new float[]
		{
			800f,
			800f
		};
	}
}
