// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.ModONI;
using TUNING;
using static TUNING.BUILDINGS;

namespace SupportPackage;

internal static partial class GlobalConsts
{
    public const float DEF_CON_TIME = 30f;
    public const int DEF_HITPT = 30;
    public const float DEF_MELPT = 800f;
    public const string AU_HLLOWMETAL = "HollowMetal";
    public const string AU_METAL = "Metal";

    // 개조된 전해조
    public static class EasyElectrolyzer
    {
        public const string ID = nameof(EasyElectrolyzer);

        public const string ANISTR = "electrolyzer_kanim";

        public const float WATER_USE_RATE = 1f;
        public const float O2_RATE = 0.88f;
        public const float H2_RATE = 0.12f;

        public const float HEAT_EXHAUS = 0f;
        public const float HEAT_SELF = 0f;

        public const int USE_POWER = 90;

        public static readonly string[] TMates = MATERIALS.REFINED_METALS;
        public static readonly float[] TMass = [MASS_KG.TIER4];
    }

    // 2 in 1 전기그릴
    public static class TwoInOneGrill
    {
        public const string ID = nameof(TwoInOneGrill);

        public const string ANISTR = "cookstation_kanim";

        public const int USE_POWER = 90;

        public const float CAPA = 50f;
        public const float STDCK = 50f;
        public const float SMDCK = 30f;

        public const float HEAT_EXHAUS = 0.25f;
        public const float HEAT_SELF = 0.75f;

        public const float STORED_MAX = 900;

        public static readonly string[] TMates = MATERIALS.RAW_METALS;
        public static readonly float[] TMass = [MASS_KG.TIER5];
    }

    // 폐수 살균기
    public static class WastewaterSterilizer
    {
        public const string ID = nameof(WastewaterSterilizer);

        public const string ANISTR = "waterpurifier_kanim";

        public const int USE_POWER = 120;

        public const float HEAT_EXHAUS = 2f;
        public const float HEAT_SELF = 2f;

        public const float LIQUID_RATE = 5f;
        public const float FILTER_USE_RATE = 1f;
        public const float FILTER_OUT_RATE = 0.2f;

        public const float DROP_MASS = 100f;

        public const float DELIVERY_CAPACITY = 1200f;
        public const float DELIVERY_REFILL = 300f;

        public static readonly string[] TMates = MATERIALS.RAW_METALS;
        public static readonly float[] TMass = [MASS_KG.TIER4];
    }

    // 매-직 타일
    public static class MagicTile
    {
        public const string ID = nameof(MagicTile);

        public const string ANISTR = "floor_gasperm_kanim";

        public static readonly string[] TMates = MATERIALS.ALL_METALS;
        public static readonly float[] TMass = [MASS_KG.TIER2];
    }

    // 미네랄 분해기
    public static class OrganicDeoxidizer
    {
        public const string ID = nameof(OrganicDeoxidizer);

        public const string ANISTR = "rust_deoxidizer_kanim";

        public const int STORED_DIRT = 2000;
        public const int REFILL_DIRT = 200;
        public const int USE_POWER = 240;

        public const float USE_ORGANIC = 4f;
        public const float EMIT_OXYGEN = 0.375f;

        public const float HEAT_EXHAUST = 0.25f;
        public const float HEAT_SELF = 1.25f;

        public static readonly string[] TMate = MATERIALS.RAW_METALS;
        public static readonly float[] TMass = [MASS_KG.TIER4];
    }

    // 물 합성기
    public static class H2OSynthesizer
    {
        public const string ID = nameof(H2OSynthesizer);

        public const string ANISTR = "liquidreservoir_kanim";

        public const int USE_POWER = 1200;

        public const float USE_H2 = 0.5f;
        public const float USE_O2 = 0.25f;
        public const float RATE_H2O = 0.25f;

        public const int HEAT_EXHAUST = 4;
        public const int HEAT_SELF = 4;

        public static readonly string[] TMate = MATERIALS.REFINED_METALS;
        public static readonly float[] TMass = [MASS_KG.TIER5];
    }

    // 이산화탄소 분해기
    public static class CO2DecomposerEZ
    {
        public const string ID = nameof(CO2DecomposerEZ);

        public const string ANISTR = "co2scrubber_kanim";

        public const int USE_POWER = 60;

        public const float USE_CO2 = 0.75f;

        public const float RATE_O2 = 0.5f;
        public const float RATE_C2 = 0.25f;

        public const float HEAT_EXHAUST = 0.1f;
        public const float HEAT_SELF = 0.2f;

        public static readonly string[] TMate = MATERIALS.REFINED_METALS;
        public static readonly float[] TMass = [MASS_KG.TIER5];
    }

#if PRIVATE_CORE
    // 간편한 유리 화덕
    public static class EasyGlassForge
    {
        public const string ID = nameof(EasyGlassForge);

        public const string ANISTR = "glassrefinery_kanim";

        public const int USE_POWER = 60;

        public static readonly string[] TMate = MATERIALS.ALL_MINERALS;
        public static readonly float[] TMass = [MASS_KG.TIER5];

        public const float TIME = 10f;
        public const float PRODUCE = 2000f;

        public const float HEAT_SELF = 0f;
    }

    // 태양열 온수기
    public static class SolarLiquidHeater
    {
        public const string ID = nameof(SolarLiquidHeater);

        public const string ANISTR = "solar_panel_kanim";

        public const float HEAT_PER_LUX = 0.079f; // 0.375f;

        public const float MIN_TEMP = 30.0f + 273.15f;
        public const float MAX_TEMP = 2000.0f + 273.15f;
        
        private const string SLIDER_PREFIX = STRINGSQuickIndex.PrefixUISideScreens + "SOLARLIQUIDHEATER.";
        public const string SLIDER_TITLE =  SLIDER_PREFIX + nameof(SLIDER_TITLE);
        public const string SLIDER_TOOLTIP = SLIDER_PREFIX + nameof(SLIDER_TOOLTIP);

        public static readonly string[] TMate = [MATERIALS.REFINED_METAL, MATERIALS.GLASS];
        public static readonly float[] TMass = [MASS_KG.TIER3, MASS_KG.TIER3];

        public static CellOffset InputOffset => new(3, 0);
        public static CellOffset OutputOffset => new(-3, 0);

        public static CellOffset GasOutputOffset => new(-2, 0);

        public const int SOLAR_CELLS_COUNT = 5 + 5;
        public static readonly CellOffset[] SolarCells = [
            new CellOffset(-3, 2), new CellOffset(-2, 2), new CellOffset(-1, 2),
            new CellOffset(0, 2),
            new CellOffset(1, 2), new CellOffset(2, 2), new CellOffset(3, 2),

            new CellOffset(-3, 1), new CellOffset(-2, 1), new CellOffset(-1, 1),
            new CellOffset(0, 1),
            new CellOffset(1, 1), new CellOffset(2, 1), new CellOffset(3, 1)
        ];
    }

    // 세균 학살 집행자
    public static class GermSlaughterExecutor
    {
        public const string ID = nameof(GermSlaughterExecutor);

        public const string ANISTR = "co2filter_kanim";

        public const float FILTER_USE = 0.00001f;
        public const int RADIUS = 20;

        public static string[] TMate => MATERIALS.RAW_MINERALS;
        public static float[] TMass => CONSTRUCTION_MASS_KG.TIER2;
    }

    public static class EasyOilWellCap
    {
        public const string ID = nameof(EasyOilWellCap);

        public const string ANISTR = "geyser_oil_cap_kanim";

        public static string[] TMate => MATERIALS.REFINED_METALS;
        public static float[] TMass => CONSTRUCTION_MASS_KG.TIER3;

        public const int POWER_USE = 60;

        public const float OVERHEAT_TEMP = 2000 + 273.15f;

        public const float OIL_RATIO = 35f;
        public const float OIL_TEMP = 15 + 273.15f;

        public const float GAS_TEMP = 30 + 273.15f;
    }

    //  마그마 살인자
    public static class LavaMurder
    {
        public const string ID = nameof(LavaMurder);

        public const string ANISTR = "logiclightsensor_kanim";

        public const int POWER_USE = 1;

        public const int RADIUS = 20;

        public static string[] TMate => MATERIALS.RAW_MINERALS;
        public static float[] TMass => CONSTRUCTION_MASS_KG.TIER1;
    }

    //  액체 복사기
    public static class LiquidCopier
    {
        public const string ID = nameof(LiquidCopier);

        public const string ANISTR = "valveliquid_kanim";

        public const int POWER_USE = 10;

        public static string[] TMate => MATERIALS.REFINED_METALS;
        public static float[] TMass => CONSTRUCTION_MASS_KG.TIER2;
    }

    //  기체 복사기
    public static class GasCopier
    {
        public const string ID = nameof(GasCopier);

        public const string ANISTR = "valvegas_kanim";

        public const int POWER_USE = 10;

        public static string[] TMate => MATERIALS.REFINED_METALS;
        public static float[] TMass => CONSTRUCTION_MASS_KG.TIER2;
    }
#endif
}