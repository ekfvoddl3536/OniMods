using System;
using STRINGS;

namespace SeedConverter
{
	// Token: 0x02000002 RID: 2
	public sealed class Consts
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static string FAL(string text, string id)
		{
			return UI.FormatAsLink(text, id);
		}

		// Token: 0x04000001 RID: 1
		public const string ID = "SeedConverter";

		// Token: 0x04000002 RID: 2
		public const string ANISTR = "fertilizer_maker_kanim";

		// Token: 0x04000003 RID: 3
		public const string AUDIO = "HollowMetal";

		// Token: 0x04000004 RID: 4
		public const int HITPT = 60;

		// Token: 0x04000005 RID: 5
		public const float CONTIME = 60f;

		// Token: 0x04000006 RID: 6
		public const float MELPT = 1600f;

		// Token: 0x04000007 RID: 7
		public const float USEPOWER = 160f;

		// Token: 0x04000008 RID: 8
		public const float HEAT_SELF = 6f;

		// Token: 0x04000009 RID: 9
		public const float HEAT_EXHAUS = 1f;

		// Token: 0x0400000A RID: 10
		public static readonly string[] TMate = new string[]
		{
			"BuildableRaw",
			"Metal"
		};

		// Token: 0x0400000B RID: 11
		public static readonly float[] TMass = new float[]
		{
			800f,
			400f
		};

		// Token: 0x0400000C RID: 12
		public static readonly string ID_UPPER = "SeedConverter".ToUpper();

		// Token: 0x0400000D RID: 13
		public static readonly LocString NAME = Consts.FAL("씨앗 압착기", "SeedConverter") ?? "";

		// Token: 0x0400000E RID: 14
		public static readonly LocString EFFC = "씨앗을 소량의 물이나 비료 또는 원유, 흙으로 변환합니다.";

		// Token: 0x0400000F RID: 15
		public static readonly LocString DESC = Consts.FAL("씨앗 압착기", "SeedConverter") + "를 이용하면, 씨앗을 압착해 소량의 물이나 비료 또는 원유, 흙을 얻을 수 있습니다.";
	}
}
