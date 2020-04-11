using System;
using STRINGS;

namespace MoreSpecialGeysers
{
	// Token: 0x02000005 RID: 5
	public sealed class MSG_CONST
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000024AE File Offset: 0x000006AE
		public static string FAL(string text, string link)
		{
			return UI.FormatAsLink(text, link);
		}

		// Token: 0x04000018 RID: 24
		public const string ROOT = "STRINGS.CREATURES.SPECIES.GEYSER.";

		// Token: 0x02000008 RID: 8
		public class TUNGSTEN
		{
			// Token: 0x04000019 RID: 25
			public const string ID = "mt_tungsten";

			// Token: 0x0400001A RID: 26
			public const string ANISTR = "geyser_molten_iron_kanim";

			// Token: 0x0400001B RID: 27
			public static readonly LocString NAME = MSG_CONST.FAL("텅스텐 화산", "mt_tungsten") ?? "";

			// Token: 0x0400001C RID: 28
			public static readonly LocString DESC = "주기적으로 " + MSG_CONST.FAL("녹은 텅스텐", "MOLTENTUNGSTEN") + "을 분출하는 큰 화산";

			// Token: 0x0400001D RID: 29
			public static readonly string ID_UPPER = "mt_tungsten".ToUpper();
		}

		// Token: 0x02000009 RID: 9
		public class COOLWATER
		{
			// Token: 0x0400001E RID: 30
			public const string ID = "cool_water";

			// Token: 0x0400001F RID: 31
			public const string ANISTR = "geyser_gas_steam_kanim";

			// Token: 0x04000020 RID: 32
			public static readonly LocString NAME = MSG_CONST.FAL("차가운 저압 증기 벤트", "cool_water") ?? "";

			// Token: 0x04000021 RID: 33
			public static readonly LocString DESC = "주기적으로 0.001 기압의 " + MSG_CONST.FAL("증기", "STEAM") + "을 분출하는 저압 벤트";

			// Token: 0x04000022 RID: 34
			public static readonly string ID_UPPER = "cool_water".ToUpper();
		}

		// Token: 0x0200000A RID: 10
		public class LIQOXYGEN
		{
			// Token: 0x04000023 RID: 35
			public const string ID = "liq_oxygen";

			// Token: 0x04000024 RID: 36
			public const string ANISTR = "geyser_liquid_water_slush_kanim";

			// Token: 0x04000025 RID: 37
			public static readonly LocString NAME = MSG_CONST.FAL("차가운 산소 분출구", "liq_oxygen") ?? "";

			// Token: 0x04000026 RID: 38
			public static readonly LocString DESC = "주기적으로 차가운 " + MSG_CONST.FAL("액체 산소", "LIQUIDOXYGEN") + "를 분출하는 매우 압력이 높은 분출구";

			// Token: 0x04000027 RID: 39
			public static readonly string ID_UPPER = "liq_oxygen".ToUpper();
		}

		// Token: 0x0200000B RID: 11
		public class LIQHELIUM
		{
			// Token: 0x04000028 RID: 40
			public const string ID = "liq_helium";

			// Token: 0x04000029 RID: 41
			public const string ANISTR = "geyser_liquid_oil_kanim";

			// Token: 0x0400002A RID: 42
			public static readonly LocString NAME = MSG_CONST.FAL("차가운 헬륨 분출구", "liq_helium") ?? "";

			// Token: 0x0400002B RID: 43
			public static readonly LocString DESC = "주기적으로 차가운 " + MSG_CONST.FAL("액체 헬륨", "LIQUIDHELIUM") + "을 분출하는 중간 압력의 분출구";

			// Token: 0x0400002C RID: 44
			public static readonly string ID_UPPER = "liq_helium".ToUpper();
		}
	}
}
