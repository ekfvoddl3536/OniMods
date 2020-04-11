using System;
using STRINGS;

namespace AntiEntropyCooler
{
	// Token: 0x02000002 RID: 2
	internal sealed class Consts
	{
		// Token: 0x04000001 RID: 1
		public const string KPATH = "STRINGS.BUILDINGS.PREFABS.";

		// Token: 0x04000002 RID: 2
		public const string ID = "AntiEntropyCooler";

		// Token: 0x04000003 RID: 3
		public const string ANIM = "massiveheatsink_kanim";

		// Token: 0x04000004 RID: 4
		public const string TabGroup = "Utilities";

		// Token: 0x04000005 RID: 5
		public const string TechGroup = "HVAC";

		// Token: 0x04000006 RID: 6
		public const int HITPT = 100;

		// Token: 0x04000007 RID: 7
		public const float CONTIME = 240f;

		// Token: 0x04000008 RID: 8
		public const float USEPOWER = 30f;

		// Token: 0x04000009 RID: 9
		public const float EXHEAT = -192f;

		// Token: 0x0400000A RID: 10
		public const float SFHEAT = -256f;

		// Token: 0x0400000B RID: 11
		public const float MINTEMP = 20f;

		// Token: 0x0400000C RID: 12
		public const int STGE_CAPACITY = 2;

		// Token: 0x0400000D RID: 13
		public const float CONSUM_RATE = 0.5f;

		// Token: 0x0400000E RID: 14
		public const float CONSUM_CAPAITY = 10f;

		// Token: 0x0400000F RID: 15
		public const float CONV_RATE = 0.01f;

		// Token: 0x04000010 RID: 16
		public static readonly string[] TMate = new string[]
		{
			"RefinedMetal",
			"Glass"
		};

		// Token: 0x04000011 RID: 17
		public static readonly float[] TMass = new float[]
		{
			400f,
			50f
		};

		// Token: 0x04000012 RID: 18
		public static readonly string ID_UPPER = "AntiEntropyCooler".ToUpper();

		// Token: 0x04000013 RID: 19
		public static readonly LocString NAME = UI.FormatAsLink("개조 항엔트로피열무효화장치", Consts.ID_UPPER);

		// Token: 0x04000014 RID: 20
		public const string EFFC = "항엔트로피열무효화장치는 오래전, 이 지구를 창조한 누군가가 개발한 것입니다.";

		// Token: 0x04000015 RID: 21
		public const string DESC = "이 장치는 듀플이 유적에서 발견한 항엔트로피열무효화장치의 설계도를 카피하여 개발했습니다.\n튜닝을 통해 이 장치는 기존의 항엔트로피열무효화장치의 성능을 압도합니다!";
	}
}
