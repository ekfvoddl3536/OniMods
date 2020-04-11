using System;
using STRINGS;

namespace SuperSpeedRunner
{
	// Token: 0x02000002 RID: 2
	internal sealed class Constants
	{
		// Token: 0x02000007 RID: 7
		public class RunnerTile
		{
			// Token: 0x04000001 RID: 1
			public const string ID = "RunnerTile";

			// Token: 0x04000002 RID: 2
			public const string ANISTR = "floor_plastic_kanim";

			// Token: 0x04000003 RID: 3
			public const float Speed = 20f;

			// Token: 0x04000004 RID: 4
			public static readonly LocString NAME = UI.FormatAsLink("슈퍼 러너 타일", "RunnerTile");

			// Token: 0x04000005 RID: 5
			public const string DESC = "어마무시한 속도로 달릴 수 있습니다.";

			// Token: 0x04000006 RID: 6
			public const string EFFC = "더 빠르게 달릴 수 있습니다.";

			// Token: 0x04000007 RID: 7
			public static readonly string[] TMate = new string[]
			{
				"BuildableRaw",
				"Plastic"
			};

			// Token: 0x04000008 RID: 8
			public static readonly float[] TMass = new float[]
			{
				50f,
				50f
			};

			// Token: 0x04000009 RID: 9
			public static readonly string ID_UPPER = "RunnerTile".ToUpper();
		}

		// Token: 0x02000008 RID: 8
		public class RunnerLadder
		{
			// Token: 0x0400000A RID: 10
			public const string ID = "RunnerLadder";

			// Token: 0x0400000B RID: 11
			public const string ANISTR = "ladder_poi_kanim";

			// Token: 0x0400000C RID: 12
			public const float Speed = 40f;

			// Token: 0x0400000D RID: 13
			public static readonly LocString NAME = UI.FormatAsLink("슈퍼 러너 사다리", "RunnerLadder");

			// Token: 0x0400000E RID: 14
			public const string DESC = "매우 빠른속도로 오르거나 내려갈 수 있습니다.";

			// Token: 0x0400000F RID: 15
			public const string EFFC = "특별한 사다리입니다.";

			// Token: 0x04000010 RID: 16
			public static readonly string[] TMate = new string[]
			{
				"Plastic"
			};

			// Token: 0x04000011 RID: 17
			public static readonly float[] TMass = new float[]
			{
				100f
			};

			// Token: 0x04000012 RID: 18
			public static readonly string ID_UPPER = "RunnerLadder".ToUpper();
		}
	}
}
