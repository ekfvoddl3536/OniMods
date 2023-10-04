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

using SuperComicLib.ModONI;
using System;

namespace AdvancedGenerators.LocalStrings
{
    using static STRINGS.UI;
    using static GlobalConsts;
    internal sealed class KoreanDef : ModStrings
    {
        public KoreanDef() : base(Guid.Parse("e75e01c8-6e42-4d8a-a395-c785040d24a7"))
        {
        }

        protected override string[] OnLoadOriginalStrings()
        {
            var nl = Environment.NewLine;
            return new[]
            {
                // NAME, DESC, EFFECT
                FormatAsLink("정제 탄소 발전기", RefinedCarbonGenerator.ID),
                $"{FormatAsLink("석탄", "COAL")} 발전기보다 더 많은 전기를 생산합니다.",
                "정제 탄소를 연소하고, 많은 전기를 생산합니다.",

                // NAME, DESC, EFFECT
                FormatAsLink("열전기 발전기", ThermoelectricGenerator.ID),
                $"초당 {-(ThermoelectricGenerator.HEAT_SELF + ThermoelectricGenerator.HEAT_EXHAUST)}kDTU의 열을 제거합니다.",
                "열을 제거하고 전기를 생산합니다.",

                // NAME, DESC, EFFECT
                FormatAsLink("나프타 발전기", NaphthaGenerator.ID),

                $"{FormatAsLink("나프타", "NAPHTHA")}와 {FormatAsLink("산소", "OXYGEN")}을 이용해 전기와 " +
                $"{FormatAsLink("이산화 탄소", "CARBONDIOXIDE")}를 생산합니다.",

                "연료 외 산소를 필요로 하며, 나프타를 연료로 합니다.",

                // NAME, DESC, EFFECT
                FormatAsLink("친환경 천연가스 발전기", EcoFriendlyMethaneGenerator.ID),

                $"{FormatAsLink("천연가스", "METHANE")}와 {FormatAsLink("산소", "OXYGEN")}, 여과 매질을 이용해 전기와 " +
                $"{FormatAsLink("이산화 탄소", "CARBONDIOXIDE")} 및 {FormatAsLink("물", "WATER")}을 생산합니다.",

                "연료 외 산소와 여과 매질을 필요로 하며, 오염된 물을 배출하지 않습니다.",

                // NAME, DESC, EFFECT
                FormatAsLink("핵융합로", FusionPowerPlant.ID),
                $"{FormatAsLink("액화수소", "LIQUIDHYDROGEN")}를 소모하여 {FormatAsLink("헬륨", "HELIUM")}과 " +
                $"대량의 {FormatAsLink("전기", "POWER")}를 생산합니다.",
                "꿈의 에너지",

                // NAME, DESC, EFFECT
                FormatAsLink("액체 에너지 흡수 발전기", ThermoconvertGenerator.ID),
                "액체가 가지고 있는 열 에너지를 흡수하여, 전력을 생산합니다." + nl +
                "흡수할 에너지양을 조절하여 전력 생산량을 조절할 수 있습니다." + nl +
                "3.125%의 질량이 변환 과정에서 손실됩니다.",
                "한 번 흡수한 에너지는 다시 돌려주지 않습니다!",

                // TITLE, TOOLTIP
                "변환할 에너지양",
                "이 값을 높이면, 입력된 액체의 열을 더 많이 흡수합니다.",
            };
        }
    }
}
