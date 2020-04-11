using System;
using STRINGS;

namespace CarbonatedWater
{
	// Token: 0x02000003 RID: 3
	internal sealed class CarbonatedWaterList
	{
		// Token: 0x04000001 RID: 1
		public const float CALMULTIPLY = 1000f;

		// Token: 0x04000002 RID: 2
		public const string INITANI = "object";

		// Token: 0x04000003 RID: 3
		public const Grid.SceneLayer LAYER = 26;

		// Token: 0x04000004 RID: 4
		public const EntityTemplates.CollisionShape COLLISION = 1;

		// Token: 0x04000005 RID: 5
		public const float FAST_COOK_TIME = 30f;

		// Token: 0x04000006 RID: 6
		public const float NORMAL_COOK_TIME = 50f;

		// Token: 0x04000007 RID: 7
		public const float SLOW_COOK_TIME = 75f;

		// Token: 0x04000008 RID: 8
		public const string DEF_RECIPEDESC = "탄산수 제조기를 통해 얻을 수 있습니다.";

		// Token: 0x04000009 RID: 9
		public const string VITAMIN_EFFECT = "Medicine_BasicBooster";

		// Token: 0x0400000A RID: 10
		public const string KORSOJU_EFFECT = "Korsoju_Drink_Effect";

		// Token: 0x0200000E RID: 14
		public class COLA
		{
			// Token: 0x0400001E RID: 30
			public const string ID = "Cola";

			// Token: 0x0400001F RID: 31
			public const float CALORIES = 320000f;

			// Token: 0x04000020 RID: 32
			public const int RATE = 4;

			// Token: 0x04000021 RID: 33
			public static readonly LocString NAME = UI.FormatAsLink("잇-츠 콜라", "Cola");

			// Token: 0x04000022 RID: 34
			public static readonly LocString DESC = UI.FormatAsLink("잇-츠 콜라", "Cola") + "는 미각을 마비시킬 수 있을 정도로 달거나 씁니다. 그래도 맛있습니다.";

			// Token: 0x04000023 RID: 35
			public const float MASS = 1.2f;

			// Token: 0x04000024 RID: 36
			public const string ANISTR = "pill_1_kanim";
		}

		// Token: 0x0200000F RID: 15
		public class DOCTORBERRY
		{
			// Token: 0x04000025 RID: 37
			public const string ID = "DoctorBerry";

			// Token: 0x04000026 RID: 38
			public const float CALORIES = 2200000f;

			// Token: 0x04000027 RID: 39
			public const int RATE = 5;

			// Token: 0x04000028 RID: 40
			public const string EFFECT = "GoodEats";

			// Token: 0x04000029 RID: 41
			public static readonly LocString NAME = UI.FormatAsLink("닥터베리", "DoctorBerry");

			// Token: 0x0400002A RID: 42
			public static readonly LocString DESC = UI.FormatAsLink("닥터베리", "DoctorBerry") + "는 센털베리를 적당한 크기로 썰어넣어 만든 탄산 음료 입니다.\n모습은 끼니절임이랑 같지만, 맛은 전혀 다릅니다.";

			// Token: 0x0400002B RID: 43
			public const float MASS = 1.8f;

			// Token: 0x0400002C RID: 44
			public const string ANISTR = "pickledmeal_kanim";
		}

		// Token: 0x02000010 RID: 16
		public class ITSTERINE
		{
			// Token: 0x0400002D RID: 45
			public const string ID = "ItsTerine";

			// Token: 0x0400002E RID: 46
			public const float CALORIES = 90000f;

			// Token: 0x0400002F RID: 47
			public const int RATE = 0;

			// Token: 0x04000030 RID: 48
			public static readonly LocString NAME = UI.FormatAsLink("잇-츠 테리", "ItsTerine");

			// Token: 0x04000031 RID: 49
			public static readonly LocString DESC = UI.FormatAsLink("잇-츠 테리", "ItsTerine") + "는 엄청 쓰고 맛이 없습니다. 만약, 건강을 위해 이 이상한 음료를 마시려고 시도한다면, 눈을 감고 코를 막아야 겨우 마실 수 있을겁니다.\n\n  ++면역계에 부스트를 줍니다.";

			// Token: 0x04000032 RID: 50
			public const float MASS = 1f;

			// Token: 0x04000033 RID: 51
			public const string ANISTR = "pill_2_kanim";
		}
	}
}
