using System;
using STRINGS;
using TUNING;

namespace SupportPackage
{
	// Token: 0x02000016 RID: 22
	internal sealed class IN_Constants
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00003F8A File Offset: 0x0000218A
		public static LogicPorts.Port GetOutputPort(string desc, string act, string inact, CellOffset off)
		{
			return LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, off, desc, act, inact, false, false);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003F9C File Offset: 0x0000219C
		public static LocString PortNameMagic(string text, string id)
		{
			return "Internal " + UI.FormatAsLink(text, id) + " Presence";
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003FBC File Offset: 0x000021BC
		public static LocString PortActiveMagic(string p, string text, string id)
		{
			return string.Concat(new string[]
			{
				"Sends an ",
				UI.FormatAsLink(p, "LOGIC"),
				" signal while the configured ",
				UI.FormatAsLink(text, id),
				" is detected within the pipe"
			});
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000400C File Offset: 0x0000220C
		public static LocString PortInActiveMagic(string p, string text, string id)
		{
			return string.Concat(new string[]
			{
				"Sends an ",
				UI.FormatAsLink(p, "LOGIC"),
				" signal while the configured ",
				UI.FormatAsLink(text, id),
				" is not detected within the pipe"
			});
		}

		// Token: 0x04000017 RID: 23
		public const string STRPREFAB = "STRINGS.BUILDINGS.PREFABS.";

		// Token: 0x04000018 RID: 24
		public const float DEF_CON_TIME = 30f;

		// Token: 0x04000019 RID: 25
		public const int DEF_HITPT = 30;

		// Token: 0x0400001A RID: 26
		public const float DEF_MELPT = 800f;

		// Token: 0x0400001B RID: 27
		public const string AU_HLLOWMETAL = "HollowMetal";

		// Token: 0x0400001C RID: 28
		public const string AU_METAL = "Metal";

		// Token: 0x0200001A RID: 26
		public class EasyElectrolyzer
		{
			// Token: 0x0400001D RID: 29
			public const string ID = "EasyElectrolyzer";

			// Token: 0x0400001E RID: 30
			public const float WATER_USE_RATE = 1f;

			// Token: 0x0400001F RID: 31
			public const float OXY_RATE = 0.888f;

			// Token: 0x04000020 RID: 32
			public const float H2_RATe = 0.111999989f;

			// Token: 0x04000021 RID: 33
			public const float OXY_TEMP = 308.15f;

			// Token: 0x04000022 RID: 34
			public const string ANISTR = "electrolyzer_kanim";

			// Token: 0x04000023 RID: 35
			public static readonly string[] TMates = MATERIALS.ALL_METALS;

			// Token: 0x04000024 RID: 36
			public static readonly float[] TMass = new float[]
			{
				800f
			};

			// Token: 0x04000025 RID: 37
			public const int POWER_USE = 160;

			// Token: 0x04000026 RID: 38
			public const float EXTEMP = 0.25f;

			// Token: 0x04000027 RID: 39
			public const float SFTEMP = 1f;

			// Token: 0x04000028 RID: 40
			public static readonly LocString NAME = UI.FormatAsLink("개조된 전해조", "EasyElectrolyzer");

			// Token: 0x04000029 RID: 41
			public static readonly LocString DESC = UI.FormatAsLink("개조된 전해조", "EasyElectrolyzer") + "는 수소를 기체 파이프를 통해 내보냅니다.";

			// Token: 0x0400002A RID: 42
			public const string EFFC = "개조된 전해조입니다. 재료값이 조금 비싸지만, 그만큼 가치가 있습니다.";

			// Token: 0x0400002B RID: 43
			public static readonly string ID_UPPER = "EasyElectrolyzer".ToUpper();

			// Token: 0x0400002C RID: 44
			public static readonly CellOffset INPUT_PORT = new CellOffset(1, 1);
		}

		// Token: 0x0200001B RID: 27
		public class SuperDeodorizer
		{
			// Token: 0x0400002D RID: 45
			public const string ID = "SuperDeodorizer";

			// Token: 0x0400002E RID: 46
			public const float AIR_RATE = 2f;

			// Token: 0x0400002F RID: 47
			public const float SAND_RATE = 0.85f;

			// Token: 0x04000030 RID: 48
			public const float CLAY_RATE = 0.4f;

			// Token: 0x04000031 RID: 49
			public const int RADIUS = 9;

			// Token: 0x04000032 RID: 50
			public const float SAND_ST_SIZE = 550f;

			// Token: 0x04000033 RID: 51
			public const float TEMP_ST_SIZE = 400f;

			// Token: 0x04000034 RID: 52
			public const float REF_RATE = 55f;

			// Token: 0x04000035 RID: 53
			public const string ANISTR = "co2filter_kanim";

			// Token: 0x04000036 RID: 54
			public static readonly string[] TMates = MATERIALS.RAW_METALS;

			// Token: 0x04000037 RID: 55
			public static readonly float[] TMass = new float[]
			{
				200f
			};

			// Token: 0x04000038 RID: 56
			public static readonly LocString NAME = UI.FormatAsLink("개조된 탈취기", "SuperDeodorizer");

			// Token: 0x04000039 RID: 57
			public static readonly LocString DESC = UI.FormatAsLink("개조된 탈취기", "SuperDeodorizer") + "는 더 넓은 범위의 오염된 산소를 정화합니다.\n\n또한, 흡입한 기체는 소멸시키지만 정화된 공기는 배출하며, 점토에 균이 묻어나오지 않습니다.";

			// Token: 0x0400003A RID: 58
			public static readonly string EFFC = string.Format("이 장치는 '주변 {0}칸'까지 오염된 산소를 제거할 수 있습니다.", 9);

			// Token: 0x0400003B RID: 59
			public static readonly string ID_UPPER = "SuperDeodorizer".ToUpper();
		}

		// Token: 0x0200001C RID: 28
		public class TwoInOneGrill
		{
			// Token: 0x0400003C RID: 60
			public const string ID = "TwoInOneGrill";

			// Token: 0x0400003D RID: 61
			public const float CAPA = 50f;

			// Token: 0x0400003E RID: 62
			public const float STDCK = 50f;

			// Token: 0x0400003F RID: 63
			public const float SMDCK = 30f;

			// Token: 0x04000040 RID: 64
			public const int USE_POWER = 100;

			// Token: 0x04000041 RID: 65
			public const float EXHAUS = 1f;

			// Token: 0x04000042 RID: 66
			public const float SELF = 4f;

			// Token: 0x04000043 RID: 67
			public const string ANISTR = "cookstation_kanim";

			// Token: 0x04000044 RID: 68
			public static readonly string[] TMates = MATERIALS.RAW_METALS;

			// Token: 0x04000045 RID: 69
			public static readonly float[] TMass = new float[]
			{
				800f
			};

			// Token: 0x04000046 RID: 70
			public static readonly LocString NAME = UI.FormatAsLink("2 in 1 전기 그릴", "TwoInOneGrill");

			// Token: 0x04000047 RID: 71
			public static readonly LocString DESC = UI.FormatAsLink("2 in 1 전기 그릴", "TwoInOneGrill") + "은 냉장고 역할과 전기 그릴 역할을 동시에 합니다.";

			// Token: 0x04000048 RID: 72
			public const string EFFC = "냉장고 + 전기 그릴";

			// Token: 0x04000049 RID: 73
			public static readonly string ID_UPPER = "TwoInOneGrill".ToUpper();
		}

		// Token: 0x0200001D RID: 29
		public class WastewaterSterilizer
		{
			// Token: 0x0400004A RID: 74
			public const string ID = "WastewaterSterilizer";

			// Token: 0x0400004B RID: 75
			public const string ANISTR = "waterpurifier_kanim";

			// Token: 0x0400004C RID: 76
			public const int USE_POWER = 180;

			// Token: 0x0400004D RID: 77
			public const float EXHAUS = 2.5f;

			// Token: 0x0400004E RID: 78
			public const float SELF = 6f;

			// Token: 0x0400004F RID: 79
			public static readonly Tag UseFilter = new Tag("Filter");

			// Token: 0x04000050 RID: 80
			public static readonly Tag InputLiq = GameTagExtensions.CreateTag(1832607973);

			// Token: 0x04000051 RID: 81
			public const SimHashes OutFilter = 869554203;

			// Token: 0x04000052 RID: 82
			public const SimHashes OutLiq = 1836671383;

			// Token: 0x04000053 RID: 83
			public const float LiqInOutRate = 5f;

			// Token: 0x04000054 RID: 84
			public const float FilterUseRate = 1f;

			// Token: 0x04000055 RID: 85
			public const float FilterOutRate = 0.2f;

			// Token: 0x04000056 RID: 86
			public const float DropMass = 10f;

			// Token: 0x04000057 RID: 87
			public const float DeliveryMax = 1200f;

			// Token: 0x04000058 RID: 88
			public const float DeliveryRefill = 300f;

			// Token: 0x04000059 RID: 89
			public static readonly string[] TMates = MATERIALS.RAW_METALS;

			// Token: 0x0400005A RID: 90
			public static readonly float[] TMass = new float[]
			{
				400f
			};

			// Token: 0x0400005B RID: 91
			public static readonly LocString NAME = UI.FormatAsLink("폐수 살균기", "WastewaterSterilizer");

			// Token: 0x0400005C RID: 92
			public static readonly LocString DESC = UI.FormatAsLink("폐수 살균기", "WastewaterSterilizer") + "는 폐수에 존재하는 모든 미생물을 죽이고, 초미세 필터를 거쳐 깨끗한 물을 배출합니다.";

			// Token: 0x0400005D RID: 93
			public const string EFFC = "폐수가 세균없는 순수한 물이 될 수 있는 가장 간단한 방법입니다.";

			// Token: 0x0400005E RID: 94
			public static readonly string ID_UPPER = "WastewaterSterilizer".ToUpper();
		}

		// Token: 0x0200001E RID: 30
		public class MagicTile
		{
			// Token: 0x0400005F RID: 95
			public const string ID = "MagicTile";

			// Token: 0x04000060 RID: 96
			public const string ANISTR = "floor_gasperm_kanim";

			// Token: 0x04000061 RID: 97
			public static readonly string[] TMates = MATERIALS.ALL_METALS;

			// Token: 0x04000062 RID: 98
			public static readonly float[] TMass = new float[]
			{
				100f
			};

			// Token: 0x04000063 RID: 99
			public static readonly LocString NAME = UI.FormatAsLink("매-직 타일", "MagicTile");

			// Token: 0x04000064 RID: 100
			public static readonly LocString DESC = UI.FormatAsLink("매-직 타일", "MagicTile") + "은 어떤 미지의 기술을 이용하여, 액체만 투과시킵니다.";

			// Token: 0x04000065 RID: 101
			public const string EFFC = "액체는 투과되지만, 기체 흐름은 막습니다.";

			// Token: 0x04000066 RID: 102
			public static readonly string ID_UPPER = "MagicTile".ToUpper();
		}

		// Token: 0x0200001F RID: 31
		public class SuperMiner
		{
			// Token: 0x04000067 RID: 103
			public const string ID = "SuperMiner";

			// Token: 0x04000068 RID: 104
			public const string ANISTR = "auto_miner_kanim";

			// Token: 0x04000069 RID: 105
			public static readonly string[] TMates = MATERIALS.ALL_METALS;

			// Token: 0x0400006A RID: 106
			public static readonly float[] TMass = new float[]
			{
				400f
			};

			// Token: 0x0400006B RID: 107
			public static readonly LocString NAME = UI.FormatAsLink("수냉쿨링 로보광부", "SuperMiner");

			// Token: 0x0400006C RID: 108
			public static readonly LocString DESC = "로보광부의 범위는 건조물을 클릭하여, 언제든지 볼 수 있습니다.\n\n유입된 액체를 이용해 건조물을 냉각합니다.";

			// Token: 0x0400006D RID: 109
			public const string EFFC = "범위내 있는 모든 광물을 자동으로 채굴합니다.\n\n받은 액체는 가열될 수도 있습니다.";

			// Token: 0x0400006E RID: 110
			public static readonly string ID_UPPER = "SuperMiner".ToUpper();
		}

		// Token: 0x02000020 RID: 32
		public class SolidElementSensor
		{
			// Token: 0x0400006F RID: 111
			public const string ID = "SolidElementSensor";

			// Token: 0x04000070 RID: 112
			public const string ANISTR = "gas_element_sensor_kanim";

			// Token: 0x04000071 RID: 113
			public static readonly string[] TMates = MATERIALS.REFINED_METALS;

			// Token: 0x04000072 RID: 114
			public static readonly float[] TMass = new float[]
			{
				25f
			};

			// Token: 0x04000073 RID: 115
			public static readonly LocString NAME = UI.FormatAsLink("고체 원소 센서", "SolidElementSensor");

			// Token: 0x04000074 RID: 116
			public static readonly LocString DESC = "지정된 원소를 탐지하면 활성화 신호를 내보냅니다.";

			// Token: 0x04000075 RID: 117
			public const string EFFC = "고체 수송 자동화";

			// Token: 0x04000076 RID: 118
			public static readonly LocString LOC = IN_Constants.PortNameMagic("Solid", "ELEMENTS_SOLID");

			// Token: 0x04000077 RID: 119
			public static readonly LocString LOC_ON = IN_Constants.PortActiveMagic("Active", "Solid", "ELEMENTS_SOLID");

			// Token: 0x04000078 RID: 120
			public static readonly LocString LOC_INON = IN_Constants.PortInActiveMagic("Standby", "Solid", "ELEMENTS_SOLID");

			// Token: 0x04000079 RID: 121
			public static readonly LogicPorts.Port OUTPUT_PORT = IN_Constants.GetOutputPort(IN_Constants.SolidElementSensor.LOC, IN_Constants.SolidElementSensor.LOC_ON, IN_Constants.SolidElementSensor.LOC_INON, new CellOffset(0, 0));

			// Token: 0x0400007A RID: 122
			public static readonly string ID_UPPER = "SolidElementSensor".ToUpper();
		}

		// Token: 0x02000021 RID: 33
		public class LargeStorage
		{
			// Token: 0x0400007B RID: 123
			public const string ID = "LargeStorage";

			// Token: 0x0400007C RID: 124
			public const string ANISTR = "liquidreservoir_kanim";

			// Token: 0x0400007D RID: 125
			public const int SIZE = 80000;

			// Token: 0x0400007E RID: 126
			public static readonly string[] TMates = MATERIALS.RAW_METALS;

			// Token: 0x0400007F RID: 127
			public static readonly float[] TMass = new float[]
			{
				100f
			};

			// Token: 0x04000080 RID: 128
			public static readonly LocString NAME = UI.FormatAsLink("큰 압축 저장소", "LargeStorage");

			// Token: 0x04000081 RID: 129
			public const string EFFC = "선택한 자원을 운반합니다.";

			// Token: 0x04000082 RID: 130
			public const string DESC = "바닥에 남겨진 자원은 치우지 않으면 \"잔해\"가 되고 장식이 낮아집니다.";

			// Token: 0x04000083 RID: 131
			public static readonly string ID_UPPER = "LargeStorage".ToUpper();
		}

		// Token: 0x02000022 RID: 34
		public class ExtremeLargeStorage
		{
			// Token: 0x04000084 RID: 132
			public const string ID = "ExtremeLargeStorage";

			// Token: 0x04000085 RID: 133
			public const string ANISTR = "gasstorage_kanim";

			// Token: 0x04000086 RID: 134
			public const int SIZE = 200000;

			// Token: 0x04000087 RID: 135
			public static readonly string[] TMates = MATERIALS.REFINED_METALS;

			// Token: 0x04000088 RID: 136
			public static readonly float[] TMass = new float[]
			{
				100f
			};

			// Token: 0x04000089 RID: 137
			public static readonly LocString NAME = UI.FormatAsLink("초대형 압축 저장소", "ExtremeLargeStorage");

			// Token: 0x0400008A RID: 138
			public const string EFFC = "선택한 자원을 운반합니다.";

			// Token: 0x0400008B RID: 139
			public const string DESC = "바닥에 남겨진 자원은 치우지 않으면 \"잔해\"가 되고 장식이 낮아집니다.";

			// Token: 0x0400008C RID: 140
			public static readonly string ID_UPPER = "ExtremeLargeStorage".ToUpper();
		}

		// Token: 0x02000023 RID: 35
		public class OrganicDeoxidizer
		{
			// Token: 0x0400008D RID: 141
			public const string ID = "OrganicDeoxidizer";

			// Token: 0x0400008E RID: 142
			public const string ANISTR = "rust_deoxidizer_kanim";

			// Token: 0x0400008F RID: 143
			public const int STORED_DIRT = 2000;

			// Token: 0x04000090 RID: 144
			public const int REFILL_DIRT = 200;

			// Token: 0x04000091 RID: 145
			public const int USE_POWER = 280;

			// Token: 0x04000092 RID: 146
			public const float USE_ORGANIC = 5f;

			// Token: 0x04000093 RID: 147
			public const float EMIT_OXYGEN = 0.33f;

			// Token: 0x04000094 RID: 148
			public const float EXHAST_HEAT = 3f;

			// Token: 0x04000095 RID: 149
			public const float SELF_HEAT = 3f;

			// Token: 0x04000096 RID: 150
			public static readonly string[] TMate = MATERIALS.RAW_METALS;

			// Token: 0x04000097 RID: 151
			public static readonly float[] TMass = new float[]
			{
				400f
			};

			// Token: 0x04000098 RID: 152
			public static readonly LocString NAME = UI.FormatAsLink("미네랄 분해 산소 확산기", "OrganicDeoxidizer");

			// Token: 0x04000099 RID: 153
			public const string EFFC = "매우 비효율적으로 흙을 산소로 만듭니다.";

			// Token: 0x0400009A RID: 154
			public const string DESC = "흙과 전기를 소모하여, 약간의 산소를 만듭니다.";

			// Token: 0x0400009B RID: 155
			public static readonly string ID_UPPER = "OrganicDeoxidizer".ToUpper();
		}

		// Token: 0x02000024 RID: 36
		public class DirtyOrganicFilter
		{
			// Token: 0x0400009C RID: 156
			public const string ID = "DirtyOrganicFilter";

			// Token: 0x0400009D RID: 157
			public const string ANISTR = "mineraldeoxidizer_kanim";

			// Token: 0x0400009E RID: 158
			public const int USE_POWER = 90;

			// Token: 0x0400009F RID: 159
			public const float USE_ORGANIC = 0.9f;

			// Token: 0x040000A0 RID: 160
			public const float EMIT_OXYGEN = 0.448f;

			// Token: 0x040000A1 RID: 161
			public const float EMIT_DIRT = 0.452f;

			// Token: 0x040000A2 RID: 162
			public const float SELF_HEAT = 2f;

			// Token: 0x040000A3 RID: 163
			public static readonly LocString NAME = UI.FormatAsLink("오염된 산소 확산기", "DirtyOrganicFilter");

			// Token: 0x040000A4 RID: 164
			public static readonly LocString EFFC = UI.FormatAsLink("오염된 흙", "TOXICSAND") + "을 " + UI.FormatAsLink("오염된 산소", "CONTAMINATEDOXYGEN") + "로 변환합니다.";

			// Token: 0x040000A5 RID: 165
			public static readonly LocString DESC = string.Concat(new string[]
			{
				UI.FormatAsLink("오염된 흙", "TOXICSAND"),
				"을 ",
				UI.FormatAsLink("오염된 산소", "CONTAMINATEDOXYGEN"),
				"와 ",
				UI.FormatAsLink("흙", "DIRT"),
				"으로 변환합니다."
			});

			// Token: 0x040000A6 RID: 166
			public static readonly string[] TMate = MATERIALS.RAW_METALS;

			// Token: 0x040000A7 RID: 167
			public static readonly float[] TMass = new float[]
			{
				400f
			};

			// Token: 0x040000A8 RID: 168
			public static readonly string ID_UPPER = "DirtyOrganicFilter".ToUpper();
		}

		// Token: 0x02000025 RID: 37
		public class BleachedStoneMaker
		{
			// Token: 0x040000A9 RID: 169
			public const string ID = "BleachedStoneMaker";

			// Token: 0x040000AA RID: 170
			public const string ANISTR = "rust_deoxidizer_kanim";

			// Token: 0x040000AB RID: 171
			public const int USE_POWER = 150;

			// Token: 0x040000AC RID: 172
			public const float USE_IN = 0.5f;

			// Token: 0x040000AD RID: 173
			public const float EMIT = 0.5f;

			// Token: 0x040000AE RID: 174
			public const float SELF_HEAT = 8f;

			// Token: 0x040000AF RID: 175
			public static readonly LocString NAME = UI.FormatAsLink("염소 압축기", "BleachedStoneMaker");

			// Token: 0x040000B0 RID: 176
			public static readonly LocString EFFC = UI.FormatAsLink("표백석", "BLEACHEDSTONE") + "을 생산";

			// Token: 0x040000B1 RID: 177
			public static readonly LocString DESC = UI.FormatAsLink("염소", "CHLORINEGAS") + "를 " + UI.FormatAsLink("표백석", "BLEACHEDSTONE") + "으로 변환합니다.";

			// Token: 0x040000B2 RID: 178
			public static readonly string[] TMate = MATERIALS.REFINED_METALS;

			// Token: 0x040000B3 RID: 179
			public static readonly float[] TMass = new float[]
			{
				200f
			};

			// Token: 0x040000B4 RID: 180
			public static readonly string ID_UPPER = "BleachedStoneMaker".ToUpper();
		}

		// Token: 0x02000026 RID: 38
		public class H2OSynthesizer
		{
			// Token: 0x040000B5 RID: 181
			public const string ID = "H2OSynthesizer";

			// Token: 0x040000B6 RID: 182
			public const string ANISTR = "liquidreservoir_kanim";

			// Token: 0x040000B7 RID: 183
			public const int USE_POWER = 1200;

			// Token: 0x040000B8 RID: 184
			public const float USE_H2 = 0.8f;

			// Token: 0x040000B9 RID: 185
			public const float USE_O2 = 0.2f;

			// Token: 0x040000BA RID: 186
			public const float RATE_H2O = 1f;

			// Token: 0x040000BB RID: 187
			public const int EX_HEAT = 4;

			// Token: 0x040000BC RID: 188
			public const int EX_SELF = 4;

			// Token: 0x040000BD RID: 189
			public static readonly LocString NAME = UI.FormatAsLink("물 합성기", "H2OSynthesizer");

			// Token: 0x040000BE RID: 190
			public static readonly LocString EFFC = string.Concat(new string[]
			{
				UI.FormatAsLink("수소", "HYDROGEN"),
				"와 ",
				UI.FormatAsLink("산소", "OXYGEN"),
				"로 ",
				UI.FormatAsLink("물", "WATER"),
				"을 합성합니다."
			});

			// Token: 0x040000BF RID: 191
			public static readonly LocString DESC = UI.FormatAsLink("물", "WATER") + " 만드는 기계";

			// Token: 0x040000C0 RID: 192
			public static readonly string[] TMate = MATERIALS.REFINED_METALS;

			// Token: 0x040000C1 RID: 193
			public static readonly float[] TMass = new float[]
			{
				800f
			};

			// Token: 0x040000C2 RID: 194
			public static readonly string ID_UPPER = "H2OSynthesizer".ToUpper();
		}

		// Token: 0x02000027 RID: 39
		public class CO2ConverterEZ
		{
			// Token: 0x040000C3 RID: 195
			public const string ID = "CO2ConverterEZ";

			// Token: 0x040000C4 RID: 196
			public const string ANISTR = "co2scrubber_kanim";

			// Token: 0x040000C5 RID: 197
			public const int USE_POWER = 10;

			// Token: 0x040000C6 RID: 198
			public const float USE_CO2 = 0.9f;

			// Token: 0x040000C7 RID: 199
			public const float RATE_O2 = 0.6f;

			// Token: 0x040000C8 RID: 200
			public const float USE_RATE_H2O = 0.1f;

			// Token: 0x040000C9 RID: 201
			public const float RATE_C2 = 0.299999952f;

			// Token: 0x040000CA RID: 202
			public const float EX_HEAT = 0f;

			// Token: 0x040000CB RID: 203
			public const float EX_SELF = 0.2f;

			// Token: 0x040000CC RID: 204
			public static readonly LocString NAME = UI.FormatAsLink("이산화 탄소 분해기 (EZ)", "CO2ConverterEZ");

			// Token: 0x040000CD RID: 205
			public static readonly LocString EFFC = string.Concat(new string[]
			{
				UI.FormatAsLink("이산화 탄소", "CARBONDIOXIDE"),
				"를 ",
				UI.FormatAsLink("산소", "OXYGEN"),
				"와 ",
				UI.FormatAsLink("석탄", "CARBON"),
				"으로 분해합니다."
			});

			// Token: 0x040000CE RID: 206
			public static readonly LocString DESC = UI.FormatAsLink("이산화 탄소", "CARBONDIOXIDE") + "를 외계의 기술로 분해하는 기계";

			// Token: 0x040000CF RID: 207
			public static readonly string[] TMate = MATERIALS.REFINED_METALS;

			// Token: 0x040000D0 RID: 208
			public static readonly float[] TMass = new float[]
			{
				800f
			};

			// Token: 0x040000D1 RID: 209
			public static readonly string ID_UPPER = "CO2ConverterEZ".ToUpper();
		}
	}
}
