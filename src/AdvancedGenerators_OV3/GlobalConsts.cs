// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace AdvancedGenerators;

using static TUNING.BUILDINGS;
using static TUNING.MATERIALS;
internal static class GlobalConsts
{
    public const int HITPT = HITPOINTS.TIER1;
    public const float CONSTRUCT_TIME = CONSTRUCTION_TIME_SECONDS.TIER3;
    public const float MELT_PT = MELTING_POINT_KELVIN.TIER2;

    public const string TAB_CATEGORY = "Power";

    public const string AU_METAL = "Metal";
    public const string AU_HOLLOWMETAL = "HollowMetal";

    public static class RefinedCarbonGenerator
    {
        public const string ID = nameof(RefinedCarbonGenerator);
        public const string AnimSTR = "generatorphos_kanim";

        public const int WATT = 1440;

        public const int HEAT_SELF = 0;
        public const int HEAT_EXHAUST = 0;

        public const float CARBONE_BURN_RATE = 1f;
        public const float CARBONE_CAPACITY = 500f;
        public const float REFILL_CAPACITY = 100f;

        public const float CO2_GEN_RATE = 0.02f;
        public const float OUT_CO2_TEMP = 348.15f;

        public static readonly string[] Materials = [METAL, BUILDABLERAW];
        public static readonly float[] MateMassKg = [MASS_KG.TIER3, MASS_KG.TIER3];
    }

    public static class ThermoelectricGenerator
    {
        public const string ID = nameof(ThermoelectricGenerator);

        public const string ANISTR = "generatormerc_kanim";

        public const int WATT = 320;

        public const int HEAT_SELF = -16;
        public const int HEAT_EXHAUST = -140;

        public const float MINIMUM_TEMP = 223.15f;
        public const float MAXIMUM_TEMP = 473.15f;

        public static readonly string[] Materials = [REFINED_METAL];
        public static readonly float[] MateMassKg = [MASS_KG.TIER3];
    }

    public static class NaphthaGenerator
    {
        public const string ID = nameof(NaphthaGenerator);

        public const string ANISTR = "generatorpetrol_kanim";

        public const int HEAT_SELF = 0;
        public const int HEAT_EXHAUST = 0;

        public const float CONSUM_NAPHTHA_RATE = 1f;
        public const float NAPHTHA_MAXSTORED = 10;
        public const float OXYGEN_MAX_STORED = 1f;
        public const float OXYGEN_CONSUM_RATE = 0.1f;
        public const float EXHAUST_CO2_RATE = 0.04f;

        public static readonly string[] Materials = [REFINED_METAL, PLASTIC];
        public static readonly float[] MateMassKg = [MASS_KG.TIER2, MASS_KG.TIER1];

        public const int WATT = 2500;
    }

    public static class EcoFriendlyMethaneGenerator
    {
        public const string ID = nameof(EcoFriendlyMethaneGenerator);

        public const string ANISTR = "generatormethane_kanim";

        public const int HEAT_SELF = 0;
        public const int HEAT_EXHAUST = 0;

        public const float METHANE_CONSUM_RATE = 0.1f;
        public const float MaxStored = 60;
        public const float EXHAUST_H2O_RATE = 0.07f;
        public const float OXYGEN_MAXSTORED = 1f;
        public const float OXYGEN_CONSUM_RATE = 0.1f;
        public const float EXHAUST_CO2_RATE = 0.03f;

        public const int WATT = 1080;

        public static readonly string[] Materials = [METAL, REFINED_METAL];
        public static readonly float[] MateMassKg = [MASS_KG.TIER3, MASS_KG.TIER1];
    }

    public static class FusionPowerPlant
    {
        public const string ID = nameof(FusionPowerPlant);

        public const string ANISTR = "steamturbine_kanim";

        public const int MY_HITPT = HITPOINTS.TIER0;
        public const float MY_CONTIME = CONSTRUCTION_TIME_SECONDS.TIER6;
        public const float MY_MELTPT = MELTING_POINT_KELVIN.TIER4;

        public const int WATT = 580_500_000; // 580.5 MW (실제론 GW 단위)

        public const float USE_HYDROGEN = 0.002f; // 2g/s
        public const float EMIT_HELIUM = 0.00099355f; // D-D 핵융합의 질량 결손은 0.645%

        public const float HELIUM_TEMP = 9773.15f; // 9500℃

        public static readonly string[] Materials = [REFINED_METAL, GLASS, PLASTIC];
        public static readonly float[] MateMassKg = [MASS_KG.TIER7 * 8f, MASS_KG.TIER7 * 2f, MASS_KG.TIER7];
    }

    public static class ThermoconvertGenerator
    {
        public const string ID = nameof(ThermoconvertGenerator);

        public const string ANISTR = "airconditioner_kanim";

        private const string UIScreenPrefix = "STRINGS.UI.UISIDESCREENS.SUPERCOMIC_THERMOCONVERT_GEN.";
        public const string SLIDER_KEY = UIScreenPrefix + "TITLE";
        public const string SLIDER_TOOLTIP_KEY = UIScreenPrefix + "TOOLTIP";

        public const float MIN_TARGET_TEMP = 1f;    // -272.15f
        public const float MAX_TARGET_TEMP = 273.15f + 1000f;

        public const int InputFieldCharacterLimit = 8;

        // public const float SCALE_FACTOR = 0.0007324219f; // 2^-11 + 2^-12
        public const float SCALE_FACTOR = 3.0f;

        public const int WATT = 6000;

        public const float HEAT_EXHAUST = 0;
        public const float HEAT_SELF = -0.015625f;

        public const float CONDUCTIVITY = 5f;

        public static readonly string[] Materials = [METAL];
        public static readonly float[] MassKG = [MASS_KG.TIER4];

        public static readonly ConduitPortInfo PORT_GAS_IN = new ConduitPortInfo(ConduitType.Gas, InputOffset);
        public static readonly ConduitPortInfo PORT_GAS_OUT = new ConduitPortInfo(ConduitType.Gas, OutputOffset);

        public static readonly ConduitPortInfo PORT_LIQUID_IN = new ConduitPortInfo(ConduitType.Liquid, InputOffset);
        public static readonly ConduitPortInfo PORT_LIQUID_OUT = new ConduitPortInfo(ConduitType.Liquid, OutputOffset);

        public static readonly ConduitPortInfo PORT_SOLID_IN = new ConduitPortInfo(ConduitType.Solid, InputOffset);
        public static readonly ConduitPortInfo PORT_SOLID_OUT = new ConduitPortInfo(ConduitType.Solid, OutputOffset);

        private const string BuildingPrefix = "STRINGS.BUILDINGS.PREFABS.SUPERCOMIC_THERMOCONVERT_GEN.";
        public const string LOGIC_MODE_INPUT = BuildingPrefix + nameof(LOGIC_MODE_INPUT);
        public const string LOGIC_MODE_INPUT_ACTIVE = BuildingPrefix + nameof(LOGIC_MODE_INPUT_ACTIVE);
        public const string LOGIC_MODE_INPUT_INACTIVE = BuildingPrefix + nameof(LOGIC_MODE_INPUT_INACTIVE);

        public const string WATTAGE_ID = ID + "Wattage";

        public static CellOffset InputOffset => new CellOffset(0, 1);
        public static CellOffset OutputOffset => new CellOffset(1, 0);

        private static StatusItem? _statusWattage = null;
        public static StatusItem StatusWattage => _statusWattage ??= new StatusItem(WATTAGE_ID, "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID)
        {
            resolveStringCallback = static (str, data) =>
            {
                var gen = (ThermoconvertPowerGenerator)data;
                return str.Replace("{Wattage}", GameUtil.GetFormattedWattage(gen.CurrentWattage));
            }
        };
    }
}
