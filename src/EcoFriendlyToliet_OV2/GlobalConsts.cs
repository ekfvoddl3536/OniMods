// MIT License
//
// Copyright (c) 2022-2023. SuperComic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

namespace EcoFriendlyToilet
{
    using static TUNING.BUILDINGS;
    using static TUNING.MATERIALS;
    internal static class GlobalConsts
    {
        public const int HITPT = 30;
        public const float MELPT = 800f;
        public const string DISEASE_ID = "FoodPoisoning";
        public const float CONTIME = CONSTRUCTION_TIME_SECONDS.TIER2;

        public static class EcoFriendlyToilet
        {
            public const string ID = nameof(EcoFriendlyToilet);

            public const string ANISTR = "outhouse_kanim";

            public static readonly string[] Materials = new[] { BUILDABLERAW };
            public static readonly float[] MassKG = new[] { MASS_KG.TIER5 };

            public const int MAX_FLUSHES = 24;
            public const float SOLID_WASTE_TEMP = 310.15f;
            public const int DISEASE_PER_FLUSH = 50000;

            public const float USE_PER_EMIT_KG = 9f;
            public const float GAS_WHEN_FULL_SIZE = 0.2f;
            public const float GAS_WHEN_FULL_INVER = 50f;

            public const int TOILET_CLEAN_TIME = 50;

            public const int MAX_CAPA = 500;
            public const int DIRT_CAPA = 200;
            public const int WATER_CAPA = 70;

            public const float HEAT_EXHAUST = 0.6f;
        }

        public static class AutoPurifyWashsink
        {
            public const string ID = nameof(AutoPurifyWashsink);

            public const string ANISTR = "wash_sink_kanim";

            public const int DISEASE_REMOVAL = 180_000;
            public const int WATER_USE = 4;
            public const int WORKTIME = 5;

            public const int MAX_STORED = 200;
            public const int USED_MAX = MAX_STORED / WATER_USE;

            public const int MIN_FILTER_DELIVERY_KG = 5;

            public const float UNPOWERED_FCON = 0.0025f;
            public const float UNPOWERED_CONV = 0.001f; // -60.0%

            public const float POWERED_FCON = 1.0f;
            public const float POWERED_CONV = 1.125f; // +12.5%

            public const int USE_POWER = 60;

            public static readonly string[] Materials = new[] { BUILDABLERAW, METAL };
            public static readonly float[] MassKG = new[] { MASS_KG.TIER4, MASS_KG.TIER1 };
        }

        public static class EcoFriendlyWildFarmTile
        {
            public const string ID = nameof(EcoFriendlyWildFarmTile);

            public const string ANISTR = "farmtilerotating_kanim";

            public static readonly string[] Materials = new[] { "Farmable", BUILDABLERAW };
            public static readonly float[] MassKG = new[] { MASS_KG.TIER2, MASS_KG.TIER1 };
        }

        public static class EcoFriendlyWildFlowerPot
        {
            public const string ID = nameof(EcoFriendlyWildFlowerPot);

            public const string ANISTR = "flowervase_kanim";

            public static readonly string[] Materials = new[] { BUILDABLERAW };
            public static readonly float[] MassKG = new[] { MASS_KG.TIER2 };
        }

        public static class EcoFriendlyWildHangingPot
        {
            public const string ID = nameof(EcoFriendlyWildHangingPot);

            public const string ANISTR = "flowervase_hanging_basic_kanim";

            public static readonly string[] Materials = new[] { METAL, BUILDABLERAW };
            public static readonly float[] MassKG = new[] { MASS_KG.TIER1, MASS_KG.TIER1 };
        }
    }
}
