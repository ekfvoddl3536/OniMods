using System;
using STRINGS;

namespace AdvancedExcavatorAndPump
{
	// Token: 0x02000006 RID: 6
	internal sealed class Constants
	{
		// Token: 0x04000010 RID: 16
		public const string ID = "AdvancedExcavatorAndPump";

		// Token: 0x04000011 RID: 17
		public const string ANISTR = "sc_aeap_kanim";

		// Token: 0x04000012 RID: 18
		public static readonly LocString NAME = UI.FormatAsLink("마법의 채굴상자", "AdvancedExcavatorAndPump");

		// Token: 0x04000013 RID: 19
		public const string DESC = "최대 3x100 칸을 채굴할 수 있으며, 채굴 중 진로를 방해하는 액체를 퍼 올립니다.";

		// Token: 0x04000014 RID: 20
		public const string EFFC = "시험적.. 버그 많음";

		// Token: 0x04000015 RID: 21
		public const float SLEF_HEAT = 0f;

		// Token: 0x04000016 RID: 22
		public const float EXHAUST_HEAT = 0f;

		// Token: 0x04000017 RID: 23
		public const int POWER = 240;

		// Token: 0x04000018 RID: 24
		public static readonly string[] TMate = new string[]
		{
			"Metal",
			"PreciousRock"
		};

		// Token: 0x04000019 RID: 25
		public static readonly float[] TMass = new float[]
		{
			400f,
			100f
		};

		// Token: 0x0400001A RID: 26
		public static readonly string ID_UPPER = "AdvancedExcavatorAndPump".ToUpper();
	}
}
