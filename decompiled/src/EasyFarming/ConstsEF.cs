using System;
using STRINGS;

namespace EasyFarming
{
	// Token: 0x02000002 RID: 2
	internal sealed class ConstsEF
	{
		// Token: 0x02000016 RID: 22
		public class WildFarmTile
		{
			// Token: 0x04000015 RID: 21
			public const string ID = "WildFarmTile";

			// Token: 0x04000016 RID: 22
			public const string ANISTR = "farmtilerotating_kanim";

			// Token: 0x04000017 RID: 23
			public static readonly LocString NAME = UI.FormatAsLink("자연재배 타일", "WildFarmTile");

			// Token: 0x04000018 RID: 24
			public static readonly LocString DESC = "바닥 타일로 사용하고 건설 전에 회전할 수 있으며, 자연(Wild)상태로 자라게됩니다.";

			// Token: 0x04000019 RID: 25
			public static readonly LocString EFFC = "하나의 식물을 키웁니다.";

			// Token: 0x0400001A RID: 26
			public static readonly float[] TMass = new float[]
			{
				100f,
				25f
			};

			// Token: 0x0400001B RID: 27
			public static readonly string[] TMate = new string[]
			{
				"Farmable",
				"Metal"
			};

			// Token: 0x0400001C RID: 28
			public static readonly string ID_UPPER = "WildFarmTile".ToUpper();
		}
	}
}
