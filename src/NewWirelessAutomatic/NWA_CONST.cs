namespace NewWirelessAutomatic
{
    using static TUNING.BUILDINGS;
    using static TUNING.MATERIALS;
    using static STRINGS.UI.LOGIC_PORTS;
    using static LogicPorts;
    using static LogicOperationalController;
    internal sealed class NWA_CONST
    {
        public const string PREFAB = "STRINGS.BUILDINGS.PREFABS.";

        public const string ToolTipKey = "STRINGS.UI.UISIDESCREENS.NEW_WIRELESS_AUTO_SCREEN.TOOLTIP";
        public const string ToolTip = "채널을 지정해주세요.";

        public const string TitleKey = "STRINGS.UI.UISIDESCREENS.NEW_WIRELESS_AUTO_SCREEN.TITLE";
        public const string Title = "채널";

        public const int OperationalChanged = (int)GameHashes.OperationalChanged;

        public const int SliderMax = 100_000;

        public const string ANISTR = "critter_sensor_kanim";
        public const int HITPT = HITPOINTS.TIER1;
        public const float CONTIME = CONSTRUCTION_TIME_SECONDS.TIER2;
        public const float MELTPT = MELTING_POINT_KELVIN.TIER1;

        public const int USE_POWER = 25;
        public const int DEF_CHANNEL = 100;

        public const string AU_CATE = "Metal";

        public static readonly string[] TMate = new[] { REFINED_METAL };
        public static readonly float[] TMass = new[] { MASS_KG.TIER1 };

        public static readonly Port[] INPUT =
            new[] { Port.InputPort(PORT_ID, new CellOffset(0, 0), CONTROL_OPERATIONAL, CONTROL_OPERATIONAL_ACTIVE, CONTROL_OPERATIONAL_INACTIVE, true) };

        public class Emitter
        {
            public const string ID = "WirelessSignalEmitter";

            public const string NAME = "자동화 신호 송신기 (E)";
            public const string DESC = "입력받은 자동화 신호를 지정된 채널로 송신합니다.";
            public const string EFFC = "미래식 자동화 제어, 신호는 (E)에서 (R)로 향합니다.";

            public static readonly string ID_UPPER = ID.ToUpper();
        }

        public class Receiver
        {
            public const string ID = "WirelessSignalReceiver";

            public const string NAME = "자동화 신호 수신기 (R)";
            public const string DESC = "지정된 채널로 부터 신호를 수신받습니다.";
            public const string EFFC = "미래식 자동화 제어, 신호는 (E)에서 (R)로 향합니다.";

            public static readonly string ID_UPPER = ID.ToUpper();
        }
    }
}
