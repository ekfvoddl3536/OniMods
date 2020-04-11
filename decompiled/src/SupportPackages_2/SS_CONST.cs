using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace SupportPackages
{
	// Token: 0x02000003 RID: 3
	internal sealed class SS_CONST
	{
		// Token: 0x06000005 RID: 5 RVA: 0x0000214C File Offset: 0x0000034C
		internal static string T(string a, string b)
		{
			return UI.FormatAsLink(a, b);
		}

		// Token: 0x0200000A RID: 10
		public class AnotherSpaceStorage
		{
			// Token: 0x04000017 RID: 23
			internal static volatile List<GameObject> STATIC_ITEMS = new List<GameObject>();

			// Token: 0x04000018 RID: 24
			public const string ID = "AnotherSpaceStorage";

			// Token: 0x04000019 RID: 25
			public const string ANISTR = "storagelocker_kanim";

			// Token: 0x0400001A RID: 26
			public static readonly string[] TMate = new string[]
			{
				"BuildableRaw",
				"Metal"
			};

			// Token: 0x0400001B RID: 27
			public static readonly float[] TMass = new float[]
			{
				200f,
				50f
			};

			// Token: 0x0400001C RID: 28
			public static Tag MyTag = new Tag("AnotherSpaceStorage");

			// Token: 0x0400001D RID: 29
			public static readonly LocString NAME = SS_CONST.T("이공간 공유 저장소", "AnotherSpaceStorage");

			// Token: 0x0400001E RID: 30
			public const string DESC = "이 저장소에 저장되는 물질은 어떤 이공간으로 복사되며, 언제든지 다시 가져올 수 있습니다!";

			// Token: 0x0400001F RID: 31
			public const string EFFC = "자원 공유가 가능한 저장소!";

			// Token: 0x04000020 RID: 32
			public static readonly string ID_UPPER = "AnotherSpaceStorage".ToUpper();

			// Token: 0x04000021 RID: 33
			public const float SIZE = 50000f;
		}
	}
}
