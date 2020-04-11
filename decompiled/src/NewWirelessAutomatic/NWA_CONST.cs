using System;

namespace NewWirelessAutomatic
{
	// Token: 0x02000004 RID: 4
	internal sealed class NWA_CONST
	{
		// Token: 0x04000002 RID: 2
		public const string PREFAB = "STRINGS.BUILDINGS.PREFABS.";

		// Token: 0x04000003 RID: 3
		public const string ToolTipKey = "STRINGS.UI.UISIDESCREENS.NEW_WIRELESS_AUTO_SCREEN.TOOLTIP";

		// Token: 0x04000004 RID: 4
		public const string ToolTip = "채널을 지정해주세요.";

		// Token: 0x04000005 RID: 5
		public const string TitleKey = "STRINGS.UI.UISIDESCREENS.NEW_WIRELESS_AUTO_SCREEN.TITLE";

		// Token: 0x04000006 RID: 6
		public const string Title = "채널";

		// Token: 0x04000007 RID: 7
		public const int OperationalChanged = -592767678;

		// Token: 0x04000008 RID: 8
		public const int SliderMax = 100000;

		// Token: 0x04000009 RID: 9
		public const string ANISTR = "critter_sensor_kanim";

		// Token: 0x0400000A RID: 10
		public const int HITPT = 30;

		// Token: 0x0400000B RID: 11
		public const float CONTIME = 30f;

		// Token: 0x0400000C RID: 12
		public const float MELTPT = 1600f;

		// Token: 0x0400000D RID: 13
		public const int USE_POWER = 25;

		// Token: 0x0400000E RID: 14
		public const int DEF_CHANNEL = 100;

		// Token: 0x0400000F RID: 15
		public const string AU_CATE = "Metal";

		// Token: 0x04000010 RID: 16
		public static readonly string[] TMate = new string[]
		{
			"RefinedMetal"
		};

		// Token: 0x04000011 RID: 17
		public static readonly float[] TMass = new float[]
		{
			50f
		};

		// Token: 0x0200000C RID: 12
		public class Emitter
		{
			// Token: 0x0400001A RID: 26
			public const string ID = "WirelessSignalEmitter";

			// Token: 0x0400001B RID: 27
			public const string NAME = "자동화 신호 송신기 (E)";

			// Token: 0x0400001C RID: 28
			public const string DESC = "입력받은 자동화 신호를 지정된 채널로 송신합니다.";

			// Token: 0x0400001D RID: 29
			public const string EFFC = "미래식 자동화 제어, 신호는 (E)에서 (R)로 향합니다.";

			// Token: 0x0400001E RID: 30
			public static readonly string ID_UPPER = "WirelessSignalEmitter".ToUpper();
		}

		// Token: 0x0200000D RID: 13
		public class Receiver
		{
			// Token: 0x0400001F RID: 31
			public const string ID = "WirelessSignalReceiver";

			// Token: 0x04000020 RID: 32
			public const string NAME = "자동화 신호 수신기 (R)";

			// Token: 0x04000021 RID: 33
			public const string DESC = "지정된 채널로 부터 신호를 수신받습니다.";

			// Token: 0x04000022 RID: 34
			public const string EFFC = "미래식 자동화 제어, 신호는 (E)에서 (R)로 향합니다.";

			// Token: 0x04000023 RID: 35
			public static readonly string ID_UPPER = "WirelessSignalReceiver".ToUpper();
		}
	}
}
