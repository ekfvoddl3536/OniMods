using System;
using STRINGS;

namespace NuclearReaction
{
	// Token: 0x02000002 RID: 2
	internal sealed class Constans
	{
		// Token: 0x04000001 RID: 1
		public const string Kpath = "STRINGS.BUILDINGS.PREFABS.";

		// Token: 0x04000002 RID: 2
		public const string FSPP_ID = "FusionPowerPlant";

		// Token: 0x04000003 RID: 3
		public const string AnimSTR = "steamturbine_kanim";

		// Token: 0x04000004 RID: 4
		public const string TechGroup = "RenewableEnergy";

		// Token: 0x04000005 RID: 5
		public const string TabCategory = "Power";

		// Token: 0x04000006 RID: 6
		public const int WIDTH = 5;

		// Token: 0x04000007 RID: 7
		public const int HEIGHT = 4;

		// Token: 0x04000008 RID: 8
		public const int HITPT = 100;

		// Token: 0x04000009 RID: 9
		public const float ConstructTime = 480f;

		// Token: 0x0400000A RID: 10
		public const float MeltingPoint = 9999f;

		// Token: 0x0400000B RID: 11
		public const float FS_HEAT_SELF = 4f;

		// Token: 0x0400000C RID: 12
		public const float MASS_KG_TIER1 = 800f;

		// Token: 0x0400000D RID: 13
		public const float MASS_KG_TIER2 = 2000f;

		// Token: 0x0400000E RID: 14
		public static readonly string[] FS_Materials = new string[]
		{
			"RefinedMetal",
			"Plastic",
			"Glass"
		};

		// Token: 0x0400000F RID: 15
		public static readonly float[] FS_MASS_KG = new float[]
		{
			2000f,
			800f,
			800f
		};

		// Token: 0x04000010 RID: 16
		public const float FS_OutWattRate = 2500000f;

		// Token: 0x04000011 RID: 17
		public static readonly string FSPP_ID_UPPER = "FusionPowerPlant".ToUpper();

		// Token: 0x04000012 RID: 18
		public static readonly LocString FSPP_NAME = UI.FormatAsLink("핵융합로", "FusionPowerPlant");

		// Token: 0x04000013 RID: 19
		public static readonly LocString FSPP_EFFC = "핵융합로는 매우 효율적으로 더 많은 " + UI.FormatAsLink("전기", "POWER") + "를 생산합니다.";

		// Token: 0x04000014 RID: 20
		public static readonly LocString FSPP_DESC = string.Concat(new string[]
		{
			"핵융합로는 ",
			UI.FormatAsLink("액체수소", "LIQUIDHYDROGEN"),
			"를 이용하여 대량의 ",
			UI.FormatAsLink("전기", "POWER"),
			"를 생산하고, ",
			UI.FormatAsLink("헬륨", "HELIUM"),
			"을 만듭니다."
		});
	}
}
