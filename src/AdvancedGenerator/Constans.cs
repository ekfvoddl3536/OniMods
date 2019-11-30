namespace AdvancedGeneratos
{
    using static STRINGS.UI;
    using static TUNING.BUILDINGS;
    using static TUNING.MATERIALS;
    internal class Constans
    {
        public const string Kpath = "STRINGS.BUILDINGS.PREFABS.";
        public const int HITPT = HITPOINTS.TIER1;
        public const float ConstructTime = CONSTRUCTION_TIME_SECONDS.TIER3;
        public const float MeltPT = MELTING_POINT_KELVIN.TIER2;
        public const string TabCategory = "Power";

        public const string AU_METAL = "Metal";
        public const string AU_HOLLOWMETAL = "HollowMetal";

        public static LogicPorts.Port[] GetPorts(CellOffset offset) =>
            new[] { LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, offset, LOGIC_PORTS.CONTROL_OPERATIONAL, LOGIC_PORTS.CONTROL_OPERATIONAL_ACTIVE, LOGIC_PORTS.CONTROL_OPERATIONAL_INACTIVE) };


        public static readonly LogicPorts.Port[] INPUT_PORT_00 = GetPorts(new CellOffset(0, 0));

        public static string Fal(string text, string id) => FormatAsLink(text, id);

        public class RefinedCarbonGenerator
        {
            public const string ID = nameof(RefinedCarbonGenerator);
            public const string AnimSTR = "generatorphos_kanim";

            public static readonly LocString NAME = Fal("정제 탄소 발전기", ID);
            public static readonly LocString DESC = $"{Fal("석탄", "COAL")} 발전기보다 더 많은 전기를 생산합니다.";
            public const string EFFC = "정제 탄소를 연소하고, 많은 전기를 생산합니다.";

            public const int WATT = 1200;
            public const float CARBONE_BURN_RATE = 1f;
            public const float CARBONE_CAPACITY = 500f;
            public const float REFILL_CAPACITY = 100f;

            public const float CO2_GEN_RATE = 0.02f;
            public const float OUT_CO2_TEMP = 348.15f;

            public static readonly string[] Materials = new[] { METAL, BUILDABLERAW };
            public static readonly float[] MateMassKg = new[] { MASS_KG.TIER5, MASS_KG.TIER4 };

            public static readonly string ID_UPPER = ID.ToUpper();
        }

        public class ThermoelectricGenerator
        {
            public const string ID = nameof(ThermoelectricGenerator);

            public const string ANISTR = "generatormerc_kanim";

            public const int Watt = 250;

            public const int Heat_Self = -8;
            public const int Heat_Exhaust = -120;

            public const float MinimumTemp = 283.15f;

            public static readonly LogicPorts.Port[] INPUT_PORTS = GetPorts(new CellOffset(1, 0));

            public static readonly LocString NAME = Fal("열전기 발전기", ID);
            public static readonly LocString DESC = $"초당 {-Heat_Self - Heat_Exhaust}kDTU의 열을 제거합니다.";
            public const string EFFC = "열을 제거하고 전기를 생산합니다.";

            public static readonly string[] Materials = new[] { REFINED_METAL };
            public static readonly float[] MateMassKg = new[] { MASS_KG.TIER4 };

            public static readonly string ID_UPPER = ID.ToUpper();
        }

        public class NaphthaGenerator
        {
            public const string ID = nameof(NaphthaGenerator);

            public const string ANISTR = "generatorpetrol_kanim";

            public const int Heat_Self = 8;
            public const int Heat_Exhaust = 1;

            public const float UseNaphtha = 1f;
            public const float Naphtha_MaxStored = 10;
            public const float Oxygen_MaxStored = 1f;
            public const float OxygenCosumRate = 0.1f;
            public const float ExhaustCO2 = 0.04f;

            public static readonly LocString NAME = Fal("나프타 발전기", ID);
            public static readonly LocString DESC = $"{Fal("나프타", "NAPHTHA")}와 {Fal("산소", "OXYGEN")}를 이용해 전기와 {Fal("이산화 탄소", "CARBONDIOXIDE")}를 생산합니다.";
            public const string EFFC = "산소를 필요로 하며, 나프타를 연료로 합니다.";

            public static readonly string[] Materials = new[] { REFINED_METAL, PLASTIC };
            public static readonly float[] MateMassKg = new[] { MASS_KG.TIER3, MASS_KG.TIER3 };

            public const int Watt = 850;

            public static readonly string ID_UPPER = ID.ToUpper();
        }

        public class EcoFriendlyMethaneGenerator
        {
            public const string ID = nameof(EcoFriendlyMethaneGenerator);

            public const string ANISTR = "generatormethane_kanim";

            public const int Heat_Self = 4;
            public const int Heat_Exhaust = 2;

            public const int FilterMaxStored = 400;

            public const float UseMethane = 0.1f;
            public const float MaxStored = 60;
            public const float ExhaustH2O = 0.07f;
            public const float Oxygen_MaxStored = 1f;
            public const float OxygenCosumRate = 0.1f;
            public const float ExhaustCO2 = 0.03f;

            public static readonly LocString NAME = Fal("친환경 천연가스 발전기", ID);
            public static readonly LocString DESC = $"{Fal("천연가스", "METHANE")}와 {Fal("산소", "OXYGEN")}, 여과 매질을 이용해 전기와 {Fal("이산화 탄소", "CARBONDIOXIDE")}, 그리고 {Fal("물", "WATER")}을 생산합니다.";
            public const string EFFC = "산소와 여과 매질을 필요로 하며, 오염된 물을 배출하지 않습니다.";

            public static readonly string[] TMate = new[] { METAL, REFINED_METAL };
            public static readonly float[] TMass = new[] { MASS_KG.TIER5, MASS_KG.TIER1 };

            public static readonly Tag Filter = new Tag("Filter");

            public const int Watt = 1000;

            public static readonly LogicPorts.Port[] INPUT = GetPorts(new CellOffset(0, 0));

            public static readonly string ID_UPPER = ID.ToUpper();
        }
    }
}
