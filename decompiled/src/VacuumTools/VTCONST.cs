using System;
using STRINGS;

namespace VacuumTools
{
	// Token: 0x02000007 RID: 7
	internal sealed class VTCONST
	{
		// Token: 0x06000017 RID: 23 RVA: 0x000022E4 File Offset: 0x000004E4
		internal static string fal(string text, string id)
		{
			return UI.FormatAsLink(text, id);
		}

		// Token: 0x04000007 RID: 7
		public const int DEF_HITPT = 30;

		// Token: 0x04000008 RID: 8
		public const int DEF_MELPT = 1600;

		// Token: 0x04000009 RID: 9
		public static readonly string[] TMate = new string[]
		{
			"RefinedMetal"
		};

		// Token: 0x0400000A RID: 10
		public static readonly float[] TMass = new float[]
		{
			50f
		};

		// Token: 0x02000008 RID: 8
		public class AnotherVoidStorage
		{
			// Token: 0x0400000B RID: 11
			public const string ID = "AnotherVoidStorage";

			// Token: 0x0400000C RID: 12
			public const int CON_TIME = 60;

			// Token: 0x0400000D RID: 13
			public const string ANISTR = "storagelocker_kanim";

			// Token: 0x0400000E RID: 14
			public static Tag MyTag = new Tag("AnotherVoidStorage");

			// Token: 0x0400000F RID: 15
			public static readonly LocString NAME = VTCONST.fal("또 다른 이공간 저장소", "AnotherVoidStorage");

			// Token: 0x04000010 RID: 16
			public const string DESC = "이 저장소에 저장되는 물질은 영원히 사라집니다!";

			// Token: 0x04000011 RID: 17
			public const string EFFC = "심각하게 위험한 저장소";

			// Token: 0x04000012 RID: 18
			public static readonly string ID_UPPER = "AnotherVoidStorage".ToUpper();
		}
	}
}
