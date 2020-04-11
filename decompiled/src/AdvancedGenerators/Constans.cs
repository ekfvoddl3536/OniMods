using System;
using STRINGS;

namespace AdvancedGeneratos
{
	// Token: 0x02000002 RID: 2
	internal class Constans
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static LogicPorts.Port[] GetPorts(CellOffset offset)
		{
			return new LogicPorts.Port[]
			{
				LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, offset, UI.LOGIC_PORTS.CONTROL_OPERATIONAL, UI.LOGIC_PORTS.CONTROL_OPERATIONAL_ACTIVE, UI.LOGIC_PORTS.CONTROL_OPERATIONAL_INACTIVE, false, false)
			};
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002095 File Offset: 0x00000295
		public static string Fal(string text, string id)
		{
			return UI.FormatAsLink(text, id);
		}

		// Token: 0x04000001 RID: 1
		public const string Kpath = "STRINGS.BUILDINGS.PREFABS.";

		// Token: 0x04000002 RID: 2
		public const int HITPT = 30;

		// Token: 0x04000003 RID: 3
		public const float ConstructTime = 60f;

		// Token: 0x04000004 RID: 4
		public const float MeltPT = 2400f;

		// Token: 0x04000005 RID: 5
		public const string TabCategory = "Power";

		// Token: 0x04000006 RID: 6
		public const string AU_METAL = "Metal";

		// Token: 0x04000007 RID: 7
		public const string AU_HOLLOWMETAL = "HollowMetal";

		// Token: 0x0200000B RID: 11
		public class RefinedCarbonGenerator
		{
			// Token: 0x0400000E RID: 14
			public const string ID = "RefinedCarbonGenerator";

			// Token: 0x0400000F RID: 15
			public const string AnimSTR = "generatorphos_kanim";

			// Token: 0x04000010 RID: 16
			public static readonly LocString NAME = Constans.Fal("정제 탄소 발전기", "RefinedCarbonGenerator");

			// Token: 0x04000011 RID: 17
			public static readonly LocString DESC = Constans.Fal("석탄", "COAL") + " 발전기보다 더 많은 전기를 생산합니다.";

			// Token: 0x04000012 RID: 18
			public const string EFFC = "정제 탄소를 연소하고, 많은 전기를 생산합니다.";

			// Token: 0x04000013 RID: 19
			public const int WATT = 1200;

			// Token: 0x04000014 RID: 20
			public const float CARBONE_BURN_RATE = 1f;

			// Token: 0x04000015 RID: 21
			public const float CARBONE_CAPACITY = 500f;

			// Token: 0x04000016 RID: 22
			public const float REFILL_CAPACITY = 100f;

			// Token: 0x04000017 RID: 23
			public const float CO2_GEN_RATE = 0.02f;

			// Token: 0x04000018 RID: 24
			public const float OUT_CO2_TEMP = 348.15f;

			// Token: 0x04000019 RID: 25
			public static readonly string[] Materials = new string[]
			{
				"Metal",
				"BuildableRaw"
			};

			// Token: 0x0400001A RID: 26
			public static readonly float[] MateMassKg = new float[]
			{
				800f,
				400f
			};

			// Token: 0x0400001B RID: 27
			public static readonly string ID_UPPER = "RefinedCarbonGenerator".ToUpper();
		}

		// Token: 0x0200000C RID: 12
		public class ThermoelectricGenerator
		{
			// Token: 0x0400001C RID: 28
			public const string ID = "ThermoelectricGenerator";

			// Token: 0x0400001D RID: 29
			public const string ANISTR = "generatormerc_kanim";

			// Token: 0x0400001E RID: 30
			public const int Watt = 250;

			// Token: 0x0400001F RID: 31
			public const int Heat_Self = -8;

			// Token: 0x04000020 RID: 32
			public const int Heat_Exhaust = -120;

			// Token: 0x04000021 RID: 33
			public const float MinimumTemp = 283.15f;

			// Token: 0x04000022 RID: 34
			public static readonly LogicPorts.Port[] INPUT_PORTS = Constans.GetPorts(new CellOffset(1, 0));

			// Token: 0x04000023 RID: 35
			public static readonly LocString NAME = Constans.Fal("열전기 발전기", "ThermoelectricGenerator");

			// Token: 0x04000024 RID: 36
			public static readonly LocString DESC = string.Format("초당 {0}kDTU의 열을 제거합니다.", 128);

			// Token: 0x04000025 RID: 37
			public const string EFFC = "열을 제거하고 전기를 생산합니다.";

			// Token: 0x04000026 RID: 38
			public static readonly string[] Materials = new string[]
			{
				"RefinedMetal"
			};

			// Token: 0x04000027 RID: 39
			public static readonly float[] MateMassKg = new float[]
			{
				400f
			};

			// Token: 0x04000028 RID: 40
			public static readonly string ID_UPPER = "ThermoelectricGenerator".ToUpper();
		}

		// Token: 0x0200000D RID: 13
		public class NaphthaGenerator
		{
			// Token: 0x04000029 RID: 41
			public const string ID = "NaphthaGenerator";

			// Token: 0x0400002A RID: 42
			public const string ANISTR = "generatorpetrol_kanim";

			// Token: 0x0400002B RID: 43
			public const int Heat_Self = 8;

			// Token: 0x0400002C RID: 44
			public const int Heat_Exhaust = 1;

			// Token: 0x0400002D RID: 45
			public const float UseNaphtha = 1f;

			// Token: 0x0400002E RID: 46
			public const float Naphtha_MaxStored = 10f;

			// Token: 0x0400002F RID: 47
			public const float Oxygen_MaxStored = 1f;

			// Token: 0x04000030 RID: 48
			public const float OxygenCosumRate = 0.1f;

			// Token: 0x04000031 RID: 49
			public const float ExhaustCO2 = 0.04f;

			// Token: 0x04000032 RID: 50
			public static readonly LocString NAME = Constans.Fal("나프타 발전기", "NaphthaGenerator");

			// Token: 0x04000033 RID: 51
			public static readonly LocString DESC = string.Concat(new string[]
			{
				Constans.Fal("나프타", "NAPHTHA"),
				"와 ",
				Constans.Fal("산소", "OXYGEN"),
				"를 이용해 전기와 ",
				Constans.Fal("이산화 탄소", "CARBONDIOXIDE"),
				"를 생산합니다."
			});

			// Token: 0x04000034 RID: 52
			public const string EFFC = "산소를 필요로 하며, 나프타를 연료로 합니다.";

			// Token: 0x04000035 RID: 53
			public static readonly string[] Materials = new string[]
			{
				"RefinedMetal",
				"Plastic"
			};

			// Token: 0x04000036 RID: 54
			public static readonly float[] MateMassKg = new float[]
			{
				200f,
				200f
			};

			// Token: 0x04000037 RID: 55
			public const int Watt = 850;

			// Token: 0x04000038 RID: 56
			public static readonly string ID_UPPER = "NaphthaGenerator".ToUpper();
		}

		// Token: 0x0200000E RID: 14
		public class EcoFriendlyMethaneGenerator
		{
			// Token: 0x04000039 RID: 57
			public const string ID = "EcoFriendlyMethaneGenerator";

			// Token: 0x0400003A RID: 58
			public const string ANISTR = "generatormethane_kanim";

			// Token: 0x0400003B RID: 59
			public const int Heat_Self = 4;

			// Token: 0x0400003C RID: 60
			public const int Heat_Exhaust = 2;

			// Token: 0x0400003D RID: 61
			public const int FilterMaxStored = 400;

			// Token: 0x0400003E RID: 62
			public const float UseMethane = 0.1f;

			// Token: 0x0400003F RID: 63
			public const float MaxStored = 60f;

			// Token: 0x04000040 RID: 64
			public const float ExhaustH2O = 0.07f;

			// Token: 0x04000041 RID: 65
			public const float Oxygen_MaxStored = 1f;

			// Token: 0x04000042 RID: 66
			public const float OxygenCosumRate = 0.1f;

			// Token: 0x04000043 RID: 67
			public const float ExhaustCO2 = 0.03f;

			// Token: 0x04000044 RID: 68
			public static readonly LocString NAME = Constans.Fal("친환경 천연가스 발전기", "EcoFriendlyMethaneGenerator");

			// Token: 0x04000045 RID: 69
			public static readonly LocString DESC = string.Concat(new string[]
			{
				Constans.Fal("천연가스", "METHANE"),
				"와 ",
				Constans.Fal("산소", "OXYGEN"),
				", 여과 매질을 이용해 전기와 ",
				Constans.Fal("이산화 탄소", "CARBONDIOXIDE"),
				", 그리고 ",
				Constans.Fal("물", "WATER"),
				"을 생산합니다."
			});

			// Token: 0x04000046 RID: 70
			public const string EFFC = "산소와 여과 매질을 필요로 하며, 오염된 물을 배출하지 않습니다.";

			// Token: 0x04000047 RID: 71
			public static readonly string[] TMate = new string[]
			{
				"Metal",
				"RefinedMetal"
			};

			// Token: 0x04000048 RID: 72
			public static readonly float[] TMass = new float[]
			{
				800f,
				50f
			};

			// Token: 0x04000049 RID: 73
			public static readonly Tag Filter = new Tag("Filter");

			// Token: 0x0400004A RID: 74
			public const int Watt = 1000;

			// Token: 0x0400004B RID: 75
			public static readonly LogicPorts.Port[] INPUT = Constans.GetPorts(new CellOffset(0, 0));

			// Token: 0x0400004C RID: 76
			public static readonly string ID_UPPER = "EcoFriendlyMethaneGenerator".ToUpper();
		}
	}
}
