using System;
using STRINGS;

namespace SINT_SINT_Is_Not_Toilet
{
	// Token: 0x02000003 RID: 3
	internal sealed class Consts
	{
		// Token: 0x04000001 RID: 1
		public const string Kpath = "STRINGS.BUILDINGS.PREFABS.";

		// Token: 0x04000002 RID: 2
		public static StatusItem FerzToilet;

		// Token: 0x04000003 RID: 3
		public const string TabGroup = "Plumbing";

		// Token: 0x02000008 RID: 8
		public class Toilet
		{
			// Token: 0x04000016 RID: 22
			public const string ID = "SintIsNotToilet";

			// Token: 0x04000017 RID: 23
			public const string TOOLTIP_NAME_ID = "STRINGS.BUILDING.STATUSITEMS.FERZTOILET.NAME";

			// Token: 0x04000018 RID: 24
			public const string TOOLTIP_EFFC_ID = "STRINGS.BUILDING.STATUSITEMS.FERZTOILET.TOOLTIP";

			// Token: 0x04000019 RID: 25
			public const string TOOLTIP_DESC_NAME = "이용 \"{FlushesRemaining}\"회 남음";

			// Token: 0x0400001A RID: 26
			public const string TOOLTIP_DESC_TOLP = "이 시설은 보수가 필요하기 전까지 \"{FlushesRemaining}\"명이 이용할 수 있습니다.";

			// Token: 0x0400001B RID: 27
			public const string ANISTR = "outhouse_kanim";

			// Token: 0x0400001C RID: 28
			public const int HITPT = 30;

			// Token: 0x0400001D RID: 29
			public const int CONTIME = 30;

			// Token: 0x0400001E RID: 30
			public const float MELPT = 800f;

			// Token: 0x0400001F RID: 31
			public static readonly string[] Materials = new string[]
			{
				"BuildableRaw"
			};

			// Token: 0x04000020 RID: 32
			public static readonly float[] MASSKG = new float[]
			{
				800f
			};

			// Token: 0x04000021 RID: 33
			public const string VirusName = "FoodPoisoning";

			// Token: 0x04000022 RID: 34
			public const int MAX_FLUSH = 24;

			// Token: 0x04000023 RID: 35
			public const float USE_PER_MAKE_ITEM_SIZE = 9f;

			// Token: 0x04000024 RID: 36
			public const float GAS_WHEN_FULL_SIZE = 0.2f;

			// Token: 0x04000025 RID: 37
			public const float GAS_WHEN_FULL_INVER = 50f;

			// Token: 0x04000026 RID: 38
			private const int MULTIPLY = 1000;

			// Token: 0x04000027 RID: 39
			public const int FLUSH_PER = 100000;

			// Token: 0x04000028 RID: 40
			public const int DUPE_PER = 100000;

			// Token: 0x04000029 RID: 41
			public const int TOILET_CLEAM_TIME = 50;

			// Token: 0x0400002A RID: 42
			public const int STGE_CAPA = 500;

			// Token: 0x0400002B RID: 43
			public const int DIRT_CAPA = 200;

			// Token: 0x0400002C RID: 44
			public const int WATER_CAP = 70;

			// Token: 0x0400002D RID: 45
			public const string VirusAddReason = "FerzToilet.Flush";

			// Token: 0x0400002E RID: 46
			public const float EXHEATING = 0.6f;

			// Token: 0x0400002F RID: 47
			public static readonly string ID_UPPER = "SintIsNotToilet".ToUpper();

			// Token: 0x04000030 RID: 48
			public static readonly LocString NAME = UI.FormatAsLink("간이 화장실", "SintIsNotToilet");

			// Token: 0x04000031 RID: 49
			public const string EFFC = "듀플에게 배출할 공간을 마련해줍니다.";

			// Token: 0x04000032 RID: 50
			public const string DESC = "복체제는 배출할 곳이 필요합니다.\n\n배관을 요구하지 않으며, 비료를 만듭니다.\n주기적으로 만들어진 비료를 비워야합니다.\n\n";
		}

		// Token: 0x02000009 RID: 9
		public class AutoPurifyWashsink
		{
			// Token: 0x04000033 RID: 51
			public const string ID = "AutoPurifyWashsink";

			// Token: 0x04000034 RID: 52
			public const string ANISTR = "wash_sink_kanim";

			// Token: 0x04000035 RID: 53
			public const int disease_removal = 120000;

			// Token: 0x04000036 RID: 54
			public const float water_use = 5f;

			// Token: 0x04000037 RID: 55
			public const int used_max = 40;

			// Token: 0x04000038 RID: 56
			public const float worktime = 5f;

			// Token: 0x04000039 RID: 57
			public const float unpowered_fcon = 0.0025f;

			// Token: 0x0400003A RID: 58
			public const float powered_fcon = 1.9124999f;

			// Token: 0x0400003B RID: 59
			public const int max_filter_stor = 200;

			// Token: 0x0400003C RID: 60
			public const float unpowered_conv = 0.001575f;

			// Token: 0x0400003D RID: 61
			public const float powered_conv = 1.624475f;

			// Token: 0x0400003E RID: 62
			public const int USE_POWER = 60;

			// Token: 0x0400003F RID: 63
			public static readonly string[] Materials = new string[]
			{
				"BuildableRaw",
				"Metal"
			};

			// Token: 0x04000040 RID: 64
			public static readonly float[] MASSKG = new float[]
			{
				400f,
				50f
			};

			// Token: 0x04000041 RID: 65
			public static readonly string ID_UPPER = "AutoPurifyWashsink".ToUpper();

			// Token: 0x04000042 RID: 66
			public static readonly LocString NAME = UI.FormatAsLink("오염수 정화 세면대", "AutoPurifyWashsink");

			// Token: 0x04000043 RID: 67
			public const string EFFC = "복제체로부터 세균을 제거하고, 건조물 스스로 오염된 물을 정화합니다.";

			// Token: 0x04000044 RID: 68
			public const string DESC = "세균에 덮인 복제체는 선택된 방향으로 지나갈 때 세면대를 사용합니다.\n모래가 있으면, 오염된 물을 깨끗한 물로 천천히 정화합니다. 정화된 물은 재사용됩니다.\n전력을 공급해주면, 더 빠른속도로 오염을 정화합니다.";
		}
	}
}
