// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace FastWirelessAutomation;

using static TUNING.BUILDINGS;
using static TUNING.MATERIALS;
internal static class GlobalConsts
{
    public const string TOOLTIP_KEY = "STRINGS.UI.UISIDESCREENS.FASTWIRELESSAUTO_SCREEN.TOOLTIP";
    public const string TITLE_KEY = "STRINGS.UI.UISIDESCREENS.FASTWIRELESSAUTO_SCREEN.TITLE";

    public const string ANISTR = "critter_sensor_kanim";
    public const int HITPT = HITPOINTS.TIER1;
    public const float CONTIME = CONSTRUCTION_TIME_SECONDS.TIER2;
    public const float MELTPT = MELTING_POINT_KELVIN.TIER1;

    public const int USE_POWER = 5;

    public const int MAX_CHANNEL = 1024;

    public const string AU_CATE = "Metal";

    public static readonly string[] TMate = [REFINED_METAL];
    public static readonly float[] TMass = [MASS_KG.TIER1];

    public static class FastWirelessEmitter
    {
        public const string ID = nameof(FastWirelessEmitter);
    }

    public static class FastWirelessReceiver
    {
        public const string ID = nameof(FastWirelessReceiver);
    }
}
