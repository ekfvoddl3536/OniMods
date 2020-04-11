using System;
using STRINGS;

namespace SuperVest
{
	// Token: 0x02000009 RID: 9
	internal sealed class SV_CONST
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002BC7 File Offset: 0x00000DC7
		public static string DESC_THERMAL(float i)
		{
			return string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, GameUtil.GetFormattedDistance(i));
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002BDE File Offset: 0x00000DDE
		public static string DESC_DECOR(int i)
		{
			return string.Format("{0}: {1}", DUPLICANTS.ATTRIBUTES.DECOR.NAME, i);
		}

		// Token: 0x04000001 RID: 1
		public const string KPATH = "STRINGS.EQUIPMENT.PREFABS.";

		// Token: 0x04000002 RID: 2
		public const string Fabric = "BasicFabric";

		// Token: 0x04000003 RID: 3
		public const string R_ID = "ClothingFabricator";

		// Token: 0x04000004 RID: 4
		public const float TEMP_DEF = 0.002f;

		// Token: 0x04000005 RID: 5
		public const float BACKPACK_TEMP_DEF = 0.0035f;

		// Token: 0x04000006 RID: 6
		public const float EFFMP_DEF = -1.25f;

		// Token: 0x04000007 RID: 7
		public const string SNAPON0 = "snapTo_body";

		// Token: 0x04000008 RID: 8
		public const string SNAPON1 = "snapTo_arm";

		// Token: 0x04000009 RID: 9
		public const string FAB = "FABRICATOR";

		// Token: 0x0400000A RID: 10
		public const string at_Ahid = "Athletics";

		// Token: 0x0400000B RID: 11
		public const string at_Caid = "CarryAmount";

		// Token: 0x0200000A RID: 10
		public class Basic_SuperVest
		{
			// Token: 0x0400000C RID: 12
			public const string ID = "Basic_SuperVest";

			// Token: 0x0400000D RID: 13
			public const string ANISTR = "body_shirt_decor01_kanim";

			// Token: 0x0400000E RID: 14
			public const string ICON = "shirt_decor01_kanim";

			// Token: 0x0400000F RID: 15
			public const int CLOTH_MASS = 8;

			// Token: 0x04000010 RID: 16
			public const int FAB_TIME = 220;

			// Token: 0x04000011 RID: 17
			public static readonly LocString NAME = UI.FormatAsLink("기본 슈퍼 슈트", "Basic_SuperVest");

			// Token: 0x04000012 RID: 18
			public static readonly LocString DESC = "이 슈트는 듀플리칸트를 패션의 신으로 만들어줍니다.\n\n 이 슈트는 너무 빛나서 오래 쳐다볼 수가 없습니다.";

			// Token: 0x04000013 RID: 19
			public static readonly LocString EFFC = "주변의 장식 수치를 많이 증가시켜줍니다.";

			// Token: 0x04000014 RID: 20
			public static readonly LocString RECIPE = "주변의 장식 수치를 많이 증가시켜줍니다.";

			// Token: 0x04000015 RID: 21
			public static readonly ClothingWearer.ClothingInfo SUPER_VEST_1 = new ClothingWearer.ClothingInfo(SV_CONST.Basic_SuperVest.NAME, 120, 0.002f, -1.25f);

			// Token: 0x04000016 RID: 22
			public static readonly string ID_UPPER = "Basic_SuperVest".ToUpper();
		}

		// Token: 0x0200000B RID: 11
		public class Advance_SuperSuit
		{
			// Token: 0x04000017 RID: 23
			public const string ID = "Advance_SuperSuit";

			// Token: 0x04000018 RID: 24
			public const string ANISTR = "body_shirt_cold01_kanim";

			// Token: 0x04000019 RID: 25
			public const string ICON = "shirt_cold01_kanim";

			// Token: 0x0400001A RID: 26
			public const int CLOTH_MASS = 20;

			// Token: 0x0400001B RID: 27
			public const int FAB_TIME = 1000;

			// Token: 0x0400001C RID: 28
			public static readonly LocString NAME = UI.FormatAsLink("특별한 슈퍼 슈트", "Advance_SuperSuit");

			// Token: 0x0400001D RID: 29
			public static readonly LocString DESC = "이 슈트는 듀플리칸트를 패션의 전설로 만들어줍니다.\n\n 이 슈트는 마치 눈앞에 태양이 있는것 처럼 밝아서, 도무지 제대로 볼 수가 없습니다.";

			// Token: 0x0400001E RID: 30
			public static readonly LocString EFFC = "주변의 장식 수치를 최고수준으로 만들어줍니다.";

			// Token: 0x0400001F RID: 31
			public static readonly LocString RECIPE = "주변의 장식 수치를 최고수준으로 만들어줍니다.";

			// Token: 0x04000020 RID: 32
			public static readonly ClothingWearer.ClothingInfo SUPER_VEST_1 = new ClothingWearer.ClothingInfo(SV_CONST.Advance_SuperSuit.NAME, 9999, 0.002f, -1.25f);

			// Token: 0x04000021 RID: 33
			public static readonly string ID_UPPER = "Advance_SuperSuit".ToUpper();
		}

		// Token: 0x0200000C RID: 12
		public class SmallBackpack
		{
			// Token: 0x04000022 RID: 34
			public const string ID = "SmallBackpack";

			// Token: 0x04000023 RID: 35
			public const string ANISTR = "body_shirt_hot01_kanim";

			// Token: 0x04000024 RID: 36
			public const string ICON = "shirt_hot01_kanim";

			// Token: 0x04000025 RID: 37
			public const int CLOTH_MASS = 4;

			// Token: 0x04000026 RID: 38
			public const int FAB_TIME = 30;

			// Token: 0x04000027 RID: 39
			public const int AddKg = 160;

			// Token: 0x04000028 RID: 40
			public const int SpeedMn = -2;

			// Token: 0x04000029 RID: 41
			public static readonly LocString NAME = UI.FormatAsLink("작은 배낭", "SmallBackpack");

			// Token: 0x0400002A RID: 42
			public static readonly LocString DESC = string.Format("듀플이 재료를 {0}kg 더 많이 운반할 수 있게됩니다.\n이동속도가 약간 감소합니다!", 160);

			// Token: 0x0400002B RID: 43
			public static readonly LocString EFFC = "재료를 더 많이 운반하세요!";

			// Token: 0x0400002C RID: 44
			public static readonly LocString RECIPE = "재료를 더 많이 운반하세요!";

			// Token: 0x0400002D RID: 45
			public static readonly ClothingWearer.ClothingInfo SUPER_VEST_1 = new ClothingWearer.ClothingInfo(SV_CONST.SmallBackpack.NAME, -10, 0.0035f, -1.25f);

			// Token: 0x0400002E RID: 46
			public static readonly string ID_UPPER = "SmallBackpack".ToUpper();
		}

		// Token: 0x0200000D RID: 13
		public class MediumBackpack
		{
			// Token: 0x0400002F RID: 47
			public const string ID = "MediumBackpack";

			// Token: 0x04000030 RID: 48
			public const string ANISTR = "body_shirt_hot01_kanim";

			// Token: 0x04000031 RID: 49
			public const string ICON = "shirt_hot01_kanim";

			// Token: 0x04000032 RID: 50
			public const int CLOTH_MASS = 8;

			// Token: 0x04000033 RID: 51
			public const int FAB_TIME = 60;

			// Token: 0x04000034 RID: 52
			public const int AddKg = 400;

			// Token: 0x04000035 RID: 53
			public const int SpeedMn = -2;

			// Token: 0x04000036 RID: 54
			public static readonly LocString NAME = UI.FormatAsLink("여행용 큰 배낭", "MediumBackpack");

			// Token: 0x04000037 RID: 55
			public static readonly LocString DESC = string.Format("듀플이 재료를 {0}kg 더 많이 운반할 수 있게됩니다.\n이동속도가 감소합니다!", 400);

			// Token: 0x04000038 RID: 56
			public static readonly LocString EFFC = "재료를 더 많이 운반하세요!";

			// Token: 0x04000039 RID: 57
			public static readonly LocString RECIPE = "재료를 더 많이 운반하세요!";

			// Token: 0x0400003A RID: 58
			public static readonly ClothingWearer.ClothingInfo SUPER_VEST_1 = new ClothingWearer.ClothingInfo(SV_CONST.MediumBackpack.NAME, -10, 0.0035f, -1.25f);

			// Token: 0x0400003B RID: 59
			public static readonly string ID_UPPER = "MediumBackpack".ToUpper();
		}

		// Token: 0x0200000E RID: 14
		public class LargeBackpack
		{
			// Token: 0x0400003C RID: 60
			public const string ID = "LargeBackpack";

			// Token: 0x0400003D RID: 61
			public const string ANISTR = "body_shirt_cold01_kanim";

			// Token: 0x0400003E RID: 62
			public const string ICON = "shirt_cold01_kanim";

			// Token: 0x0400003F RID: 63
			public const int CLOTH_MASS = 20;

			// Token: 0x04000040 RID: 64
			public const int FAB_TIME = 700;

			// Token: 0x04000041 RID: 65
			public const int AddKg = 3200;

			// Token: 0x04000042 RID: 66
			public const int SpeedMn = -4;

			// Token: 0x04000043 RID: 67
			public static readonly LocString NAME = UI.FormatAsLink("전설적인 슈퍼 배낭", "LargeBackpack");

			// Token: 0x04000044 RID: 68
			public static readonly LocString DESC = string.Format("선택받은 듀플만이 이 배낭을 멜 수 있습니다.\n   최대 {0}kg 만큼 더 운반할 수 있습니다.", 3200);

			// Token: 0x04000045 RID: 69
			public static readonly LocString EFFC = "다리가 부러지지 않도록 주의하세요!";

			// Token: 0x04000046 RID: 70
			public static readonly LocString RECIPE = "다리가 부러지지 않도록 주의하세요!";

			// Token: 0x04000047 RID: 71
			public static readonly ClothingWearer.ClothingInfo SUPER_VEST_1 = new ClothingWearer.ClothingInfo(SV_CONST.LargeBackpack.NAME, -10, 0.004f, -1.25f);

			// Token: 0x04000048 RID: 72
			public static readonly string ID_UPPER = "LargeBackpack".ToUpper();
		}
	}
}
