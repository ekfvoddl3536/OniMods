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
using SuperComicLib.ModONI;

namespace AdvancedGenerators.LocalStrings
{
    using static STRINGS.UI;
    using static GlobalConsts;
    internal sealed class KoreanDef : ModLocaleKorean<ModStringsKey>
    {
        protected override string[] OnLoadOriginalStrings() =>
            new string[]
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

                "산소를 필요로 하며, 나프타를 연료로 합니다.",

                // NAME, DESC, EFFECT
                FormatAsLink("친환경 천연가스 발전기", EcoFriendlyMethaneGenerator.ID),

                $"{FormatAsLink("천연가스", "METHANE")}와 {FormatAsLink("산소", "OXYGEN")}, 여과 매질을 이용해 전기와 " +
                $"{FormatAsLink("이산화 탄소", "CARBONDIOXIDE")} 및 {FormatAsLink("물", "WATER")}을 생산합니다.",

                "산소와 여과 매질을 필요로 하며, 오염된 물을 배출하지 않습니다.",
            };
    }
}
