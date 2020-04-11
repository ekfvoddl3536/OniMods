using System;
using System.Collections.Generic;
using STRINGS;

namespace SulfurDeoxydizer
{
	// Token: 0x02000002 RID: 2
	internal sealed class Constans
	{
		// Token: 0x04000001 RID: 1
		public const string Kpath = "STRINGS.BUILDINGS.PREFABS.";

		// Token: 0x04000002 RID: 2
		public const string TechGroup = "ImprovedOxygen";

		// Token: 0x04000003 RID: 3
		public const string TabGroup = "Utilities";

		// Token: 0x04000004 RID: 4
		public const int HITPT = 30;

		// Token: 0x04000005 RID: 5
		public const float ConstructTime = 60f;

		// Token: 0x04000006 RID: 6
		public const float MeltingPoint = 800f;

		// Token: 0x04000007 RID: 7
		public static readonly LocString SUFDE_Displyname = UI.FormatAsLink("메탄", "METHANE") + " 확산기";

		// Token: 0x04000008 RID: 8
		public static readonly LocString SUFDE_Effect = "유황을 소모하여, 소량의 천연가스를 생성합니다.";

		// Token: 0x04000009 RID: 9
		public static readonly LocString SUFDE_Descript = string.Concat(new string[]
		{
			UI.FormatAsLink("메탄", "METHANE"),
			" 확산기는 ",
			UI.FormatAsLink("유황", "SULFUR"),
			"을 소모하여 ",
			UI.FormatAsLink("천연가스", "METHANE"),
			"를 생산합니다."
		});

		// Token: 0x0400000A RID: 10
		public const string SUFDE_ID = "SulfurDeoxidizer";

		// Token: 0x0400000B RID: 11
		public const string SUFDE_AnimSTR = "mineraldeoxidizer_kanim";

		// Token: 0x0400000C RID: 12
		public const float SULFUR_BURN_RATE = 0.65f;

		// Token: 0x0400000D RID: 13
		public const float SULFUR_STORAGE = 200f;

		// Token: 0x0400000E RID: 14
		public const float STORAGE_REFILL = 75f;

		// Token: 0x0400000F RID: 15
		public const float METHANE_GENERATE_RATE = 0.5f;

		// Token: 0x04000010 RID: 16
		public const float METHANE_TEMPERATURE = 320.15f;

		// Token: 0x04000011 RID: 17
		public const int SUFDE_USE_POWER = 250;

		// Token: 0x04000012 RID: 18
		public const float OVERHEAT_TEMP = 363.15f;

		// Token: 0x04000013 RID: 19
		public static readonly string[] SUFDE_Materials = new string[]
		{
			"RefinedMetal"
		};

		// Token: 0x04000014 RID: 20
		public static readonly float[] SUFDE_MateMass = new float[]
		{
			200f
		};

		// Token: 0x04000015 RID: 21
		public static readonly string SUFDE_ID_UPPER = "SulfurDeoxidizer".ToUpper();

		// Token: 0x04000016 RID: 22
		public static readonly List<Storage.StoredItemModifier> storeditem = new List<Storage.StoredItemModifier>
		{
			1,
			2
		};

		// Token: 0x04000017 RID: 23
		public static readonly List<Tag> habitatTag = new List<Tag>
		{
			GameTagExtensions.Create(-729385479),
			GameTagExtensions.Create(-1528777920)
		};
	}
}
