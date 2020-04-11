using System;
using STRINGS;

namespace LiquidMethaneEngine
{
	// Token: 0x02000002 RID: 2
	internal sealed class Consts
	{
		// Token: 0x04000001 RID: 1
		public const string Kpath = "STRINGS.BUILDINGS.PREFABS.";

		// Token: 0x04000002 RID: 2
		public const string TechGroup = "BasicRocketry";

		// Token: 0x04000003 RID: 3
		public const string TabGroup = "Rocketry";

		// Token: 0x04000004 RID: 4
		public const string Id = "LiquidMethaneEngine";

		// Token: 0x04000005 RID: 5
		public const string AnSTR = "rocket_petroleum_engine_kanim";

		// Token: 0x04000006 RID: 6
		public const int HITPT = 1000;

		// Token: 0x04000007 RID: 7
		public const float MeltPT = 9999f;

		// Token: 0x04000008 RID: 8
		public const float ConstructTime = 480f;

		// Token: 0x04000009 RID: 9
		public const float Overtemp = 2273.15f;

		// Token: 0x0400000A RID: 10
		public const float OutputElemetTemp = 398.15f;

		// Token: 0x0400000B RID: 11
		public const float EngineEfficiency = 50f;

		// Token: 0x0400000C RID: 12
		public static readonly string[] Materials = new string[]
		{
			-899253461.ToString()
		};

		// Token: 0x0400000D RID: 13
		public static readonly float[] MateMassKG = new float[]
		{
			1200f
		};

		// Token: 0x0400000E RID: 14
		public static readonly string ID_UPPER = "LiquidMethaneEngine".ToUpper();

		// Token: 0x0400000F RID: 15
		public static readonly LocString Displayname = UI.FormatAsLink("액체 메테인", "LIQUIDMETHANE") + " 엔진";

		// Token: 0x04000010 RID: 16
		public static readonly LocString Effect = UI.FormatAsLink("액체 메테인", "LIQUIDMETHANE") + " 엔진은 " + UI.FormatAsLink("석유", "PETROLEUM") + " 엔진과 동일한 효율을 가집니다.";

		// Token: 0x04000011 RID: 17
		public static readonly LocString Descript = "우주 탐사에 필요한 로켓을 추진하기 위해 " + UI.FormatAsLink("액체 메테인", "LIQUIDMETHANE") + "을 연료로 연소합니다.\n\n다른 로켓 모듈을 쌓기전에 엔진을 먼저 건설해야합니다.\n\n";
	}
}
