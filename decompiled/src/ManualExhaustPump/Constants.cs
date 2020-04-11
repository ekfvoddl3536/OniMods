using System;
using STRINGS;

namespace ManualExhaustPump
{
	// Token: 0x02000005 RID: 5
	internal sealed class Constants
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000023C3 File Offset: 0x000005C3
		public static string FAL(string a1, string a2)
		{
			return UI.FormatAsLink(a1, a2);
		}

		// Token: 0x04000001 RID: 1
		public static readonly LogicPorts.Port OUTPUT_PORT = LogicPorts.Port.OutputPort(FilteredStorage.FULL_PORT_ID, new CellOffset(1, 2), BUILDINGS.PREFABS.STORAGELOCKERSMART.LOGIC_PORT, BUILDINGS.PREFABS.STORAGELOCKERSMART.LOGIC_PORT_ACTIVE, BUILDINGS.PREFABS.STORAGELOCKERSMART.LOGIC_PORT_INACTIVE, false, false);

		// Token: 0x04000002 RID: 2
		public const int USE_POWER = 90;

		// Token: 0x04000003 RID: 3
		public const float EXHAUST_HEAT = 0.15f;

		// Token: 0x04000004 RID: 4
		public const float SELF_HEAT = 0f;

		// Token: 0x04000005 RID: 5
		public const int HITPT = 100;

		// Token: 0x04000006 RID: 6
		public const float CONSTIME = 150f;

		// Token: 0x04000007 RID: 7
		public const float MELPT = 1600f;

		// Token: 0x04000008 RID: 8
		public const string AU_HOLLOWMETAL = "HollowMetal";

		// Token: 0x04000009 RID: 9
		public static readonly string[] TMate = new string[]
		{
			"Metal",
			"RefinedMetal"
		};

		// Token: 0x0400000A RID: 10
		public static readonly float[] TMass = new float[]
		{
			400f,
			25f
		};

		// Token: 0x0200000B RID: 11
		public class ManualLiquidPump
		{
			// Token: 0x04000019 RID: 25
			public const string ID = "ManualLiquidPump";

			// Token: 0x0400001A RID: 26
			public const string ANISTR = "liquidreservoir_kanim";

			// Token: 0x0400001B RID: 27
			public const int StorageCapacity = 600;

			// Token: 0x0400001C RID: 28
			public static Tag StorageTag = new Tag("ManualLiquidPump");

			// Token: 0x0400001D RID: 29
			public static readonly LocString NAME = UI.FormatAsLink("수동 배수 펌프", "ManualLiquidPump");

			// Token: 0x0400001E RID: 30
			public const string EFFC = "병에 든 액체를 액체관으로 내보냅니다.";

			// Token: 0x0400001F RID: 31
			public const string DESC = "액체를 임시 저장소에 저장했다가, 액체관을 통해 내보냅니다.\n받은 액체는 항상 내보내려고 시도합니다, 만약 배관이 막혀, 저장소에 액체가 가득찬 경우 자동화 신호를 활성화로 변경합니다.";

			// Token: 0x04000020 RID: 32
			public static readonly string ID_UPPER = "ManualLiquidPump".ToUpper();
		}

		// Token: 0x0200000C RID: 12
		public class ManualGasPump
		{
			// Token: 0x04000021 RID: 33
			public const string ID = "ManualGasPump";

			// Token: 0x04000022 RID: 34
			public const string ANISTR = "gasstorage_kanim";

			// Token: 0x04000023 RID: 35
			public const int StorageCapacity = 75;

			// Token: 0x04000024 RID: 36
			public static Tag StorageTag = new Tag("ManualGasPump");

			// Token: 0x04000025 RID: 37
			public static readonly LocString NAME = UI.FormatAsLink("수동 배기 펌프", "ManualGasPump");

			// Token: 0x04000026 RID: 38
			public const string EFFC = "병에 든 기체를 기체관으로 내보냅니다.";

			// Token: 0x04000027 RID: 39
			public const string DESC = "기체를 임시 저장소에 저장했다가, 기체관을 통해 내보냅니다.받은 기체는 항상 내보내려고 시도합니다, 만약 배관이 막혀, 저장소에 기체가 가득찬 경우 자동화 신호를 활성화로 변경합니다.";

			// Token: 0x04000028 RID: 40
			public static readonly string ID_UPPER = "ManualGasPump".ToUpper();
		}

		// Token: 0x0200000D RID: 13
		public class LiquidBottleCharger
		{
			// Token: 0x04000029 RID: 41
			public const string ID = "LiquidBottleCharger";

			// Token: 0x0400002A RID: 42
			public const string ANISTR = "valveliquid_kanim";

			// Token: 0x0400002B RID: 43
			public const float CAPA = 1000f;

			// Token: 0x0400002C RID: 44
			public static readonly LocString NAME = UI.FormatAsLink("액체 병 충전기", "LiquidBottleCharger");

			// Token: 0x0400002D RID: 45
			public const string EFFC = "배관을 통해 받은 액체를 저장하고, 병에 담습니다.";

			// Token: 0x0400002E RID: 46
			public const string DESC = "저장된 액체는 듀플이 가져갈 수 있습니다.";

			// Token: 0x0400002F RID: 47
			public static readonly string[] _Tmate = new string[]
			{
				"Metal"
			};

			// Token: 0x04000030 RID: 48
			public static readonly float[] _Tmass = new float[]
			{
				200f
			};

			// Token: 0x04000031 RID: 49
			public static readonly string ID_UPPER = "LiquidBottleCharger".ToUpper();
		}
	}
}
