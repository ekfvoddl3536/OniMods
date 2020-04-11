using System;
using STRINGS;

namespace CarbonatedWater
{
	// Token: 0x02000005 RID: 5
	public sealed class Consts
	{
		// Token: 0x0400000E RID: 14
		public const string KPATH = "STRINGS.BUILDINGS.PREFABS.";

		// Token: 0x0400000F RID: 15
		public const string SPATH = "STRINGS.ITEMS.FOOD.";

		// Token: 0x04000010 RID: 16
		public const string CWG_ID = "CO2WaterGen";

		// Token: 0x04000011 RID: 17
		public static readonly LocString CWG_NAME = UI.FormatAsLink("탄산음료 제조기", "CO2WaterGen");

		// Token: 0x04000012 RID: 18
		public static readonly LocString CWG_DESC = UI.FormatAsLink("탄산음료 제조기", "CO2WaterGen") + "는 특별한 레시피를 이용하여 특별한 음료수를 만듭니다.\n\n레시피 별로 재료가 다릅니다.";

		// Token: 0x04000013 RID: 19
		public const string CWG_EFFC = "듀플리칸트는 먹어야합니다. 좋은 품질의 음식을 요구한다면, 탄산음료는 거의 필수 입니다.\n또한, 특별한 레시피를 통해 만들 수 있는 특별한 음료가 있습니다.";

		// Token: 0x04000014 RID: 20
		public const string CWG_ANST = "microbemusher_kanim";

		// Token: 0x04000015 RID: 21
		public const int CWG_HITPT = 30;

		// Token: 0x04000016 RID: 22
		public const float CWG_CONTIME = 30f;

		// Token: 0x04000017 RID: 23
		public const string TabGroup = "Food";

		// Token: 0x04000018 RID: 24
		public const string TechGroup = "FineDining";
	}
}
