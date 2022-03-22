#region LICENSE
/*
MIT License

Copyright (c) 2022. Super Comic (ekfvoddl3535@naver.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion
namespace AdvancedGenerators
{
    using static TUNING.BUILDINGS;
    using static TUNING.MATERIALS;
    internal static class GlobalConsts
    {
        public const string KPATH = "STRINGS.BUILDINGS.PREFABS.";

        public const int HITPT = HITPOINTS.TIER1;
        public const float CONSTRUCT_TIME = CONSTRUCTION_TIME_SECONDS.TIER3;
        public const float MELT_PT = MELTING_POINT_KELVIN.TIER2;

        public const string TAB_CATEGORY = "Power";

        public const string AU_METAL = "Metal";
        public const string AU_HOLLOWMETAL = "HollowMetal";

        public class RefinedCarbonGenerator
        {
            public const string ID = nameof(RefinedCarbonGenerator);
            public const string AnimSTR = "generatorphos_kanim";

            public const int WATT = 1200;

            public const int HEAT_SELF = 2;
            public const int HEAT_EXHAUST = 2;

            public const float CARBONE_BURN_RATE = 1f;
            public const float CARBONE_CAPACITY = 500f;
            public const float REFILL_CAPACITY = 100f;

            public const float CO2_GEN_RATE = 0.02f;
            public const float OUT_CO2_TEMP = 348.15f;

            public static readonly string[] Materials = new[] { METAL, BUILDABLERAW };
            public static readonly float[] MateMassKg = new[] { MASS_KG.TIER5, MASS_KG.TIER4 };
        }

        public class ThermoelectricGenerator
        {
            public const string ID = nameof(ThermoelectricGenerator);

            public const string ANISTR = "generatormerc_kanim";

            public const int WATT = 300;

            public const int HEAT_SELF = -8;
            public const int HEAT_EXHAUST = -140;

            public const float MINIMUM_TEMP = 253.15f;
            public const float MAXIMUM_TEMP = 473.15f;

            public static readonly string[] Materials = new[] { REFINED_METAL };
            public static readonly float[] MateMassKg = new[] { MASS_KG.TIER4 };
        }

        public class NaphthaGenerator
        {
            public const string ID = nameof(NaphthaGenerator);

            public const string ANISTR = "generatorpetrol_kanim";

            public const int HEAT_SELF = 12;
            public const int HEAT_EXHAUST = 4;

            public const float CONSUM_NAPHTHA_RATE = 1f;
            public const float NAPHTHA_MAXSTORED = 10;
            public const float OXYGEN_MAX_STORED = 1f;
            public const float OXYGEN_CONSUM_RATE = 0.1f;
            public const float EXHAUST_CO2_RATE = 0.04f;

            public static readonly string[] Materials = new[] { REFINED_METAL, PLASTIC };
            public static readonly float[] MateMassKg = new[] { MASS_KG.TIER3, MASS_KG.TIER3 };

            public const int WATT = 2200;
        }

        public class EcoFriendlyMethaneGenerator
        {
            public const string ID = nameof(EcoFriendlyMethaneGenerator);

            public const string ANISTR = "generatormethane_kanim";

            public const int HEAT_SELF = 6;
            public const int HEAT_EXHAUST = 4;

            public const int FILTER_MAXSTORED = 400;

            public const float METHANE_CONSUM_RATE = 0.1f;
            public const float MaxStored = 60;
            public const float EXHAUST_H2O_RATE = 0.07f;
            public const float OXYGEN_MAXSTORED = 1f;
            public const float OXYGEN_CONSUM_RATE = 0.1f;
            public const float EXHAUST_CO2_RATE = 0.03f;

            public const int WATT = 800;

            public static readonly string[] Materials = new[] { METAL, REFINED_METAL };
            public static readonly float[] MateMassKg = new[] { MASS_KG.TIER5, MASS_KG.TIER1 };

            public static readonly Tag TagFilter = new Tag("Filter");
        }
    }
}
