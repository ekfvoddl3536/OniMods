#pragma warning disable CA1034

namespace MoreSpecialGeysers
{
    public static class MSG_CONST
    {
        public const string ROOT = "STRINGS.CREATURES.SPECIES.GEYSER.";

        public static class COOLWATER
        {
            public const string ID = "cool_water";

            public const string ANISTR = "geyser_gas_steam_kanim";

            public static readonly LocString NAME = "차가운 저압 증기 벤트".AsLink(ID);

            public static readonly LocString DESC = $"주기적으로 0.001 기압의 {"증기".AsLink("STEAM")}가 새어나오는 침착한 김치";

            public static readonly string ID_UPPER = ID.ToUpper();
        }

        public static class COOLGASOXYGEN
        {
            public const string ID = "cool_oxygen";

            public const string ANISTR = "geyser_liquid_water_slush_kanim";

            public static readonly LocString NAME = "시원한 산소 분출구".AsLink(ID);

            public static readonly LocString DESC = $"주기적으로 시원한 {"산소".AsLink("OXYGEN")}을 뿜어대는 얼어붙은 브로콜리";

            public static readonly string ID_UPPER = ID.ToUpper();
        }

        public static class LIQHYDROGEN
        {
            public const string ID = "liq_hydrogen";

            public const string ANISTR = "geyser_liquid_oil_kanim";

            public static readonly LocString NAME = "차가운 수소 분출구".AsLink(ID);

            public static readonly LocString DESC = $"주기적으로 매우 차가운 {"액체 수소".AsLink("LIQHYDROGEN")}가 터져나오는 화가난 연근";

            public static readonly string ID_UPPER = ID.ToUpper();
        }

        public static class TUNGSTEN
        {
            public const string ID = "mt_tungsten";

            public const string ANISTR = "geyser_molten_iron_kanim";

            public static readonly LocString NAME = "텅스텐 화산".AsLink(ID);

            public static readonly LocString DESC = $"주기적으로 {"녹은 텅스텐".AsLink("MOLTENTUNGSTEN")}이 흘러내리는 구멍난 당근";

            public static readonly string ID_UPPER = ID.ToUpper();
        }
    }
}
