using TUNING;

namespace SupportPackage
{
	using static TUNING.BUILDINGS;
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

			public const float HEAT_EXHAUS = 0.25f;
			public const float HEAT_SELF = 1f;

			public const int USE_POWER = 90;

			public static readonly string[] TMates = MATERIALS.REFINED_METALS;
			public static readonly float[] TMass = new[] { MASS_KG.TIER4 };
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

			public const float HEAT_EXHAUS = 0.5f;
			public const float HEAT_SELF = 3.5f;

			public const float  STORED_MAX = 1000f;

			public static readonly string[] TMates = MATERIALS.RAW_METALS;
			public static readonly float[] TMass = new[] { MASS_KG.TIER5 };
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
			public static readonly float[] TMass = new[] { MASS_KG.TIER4 };
		}

		// 매-직 타일
		public static class MagicTile
        {
			public const string ID = nameof(MagicTile);

			public const string ANISTR = "floor_gasperm_kanim";

			public static readonly string[] TMates = MATERIALS.ALL_METALS;
			public static readonly float[] TMass = new[] { MASS_KG.TIER2 };
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
			public static readonly float[] TMass = new[] { MASS_KG.TIER4 };
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
			public static readonly float[] TMass = new[] { MASS_KG.TIER5 };
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
			public static readonly float[] TMass = new[] { MASS_KG.TIER5 };
		}
	}
}
