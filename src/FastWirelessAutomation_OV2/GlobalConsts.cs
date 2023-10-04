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

namespace FastWirelessAutomation
{
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

        public static readonly string[] TMate = new[] { REFINED_METAL };
        public static readonly float[] TMass = new[] { MASS_KG.TIER1 };

        public static class FastWirelessEmitter
        {
            public const string ID = nameof(FastWirelessEmitter);
        }

        public static class FastWirelessReceiver
        {
            public const string ID = nameof(FastWirelessReceiver);
        }
    }
}
