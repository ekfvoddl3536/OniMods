// MIT License
//
// Copyright (c) 2022-2023. Super Comic (ekfvoddl3535@naver.com)
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

namespace SupportPackage.LocalStrings
{
    using static GlobalConsts;
    using static GlobalConsts.SUPER_SUIT;
    using static STRINGS.UI;
    internal sealed class KoreanDef : ModStrings
    {
        public KoreanDef() : base(Guid.Parse("94772e3f-836e-4560-8449-1b0b0ae8bce9"))
        {
        }

        protected override string[] OnLoadOriginalStrings() =>
            new[]
            {
                // NAME, DESC, EFFECT
                FormatAsLink("향상된 전해조 2.0", EasyElectrolyzer.ID),
                "이 기계는 만들어진 수소를 배관으로 내보냅니다.",
                "개조된 전해조",

                // NAME, DESC, EFFECT
                FormatAsLink("2 in 1 전기 그릴", TwoInOneGrill.ID),
                "냉장고와 전기 그릴의 역할을 동시에 수행합니다.",
                "냉장고 + 전기 그릴",

                // NAME, DESC, EFFECT
                FormatAsLink("폐수 살균기", WastewaterSterilizer.ID),
                "오염된 물에 존재하는 모든 세균을 죽이고, 초미세 필터를 거쳐 깨끗한 물을 배출합니다.",
                "세균을 박멸하는 미지의 강력한 기술",

                // NAME, DESC, EFFECT
                FormatAsLink("매-직 타일", MagicTile.ID),
                "미지의 기술을 사용하여, 액체는 투과하지만 기체는 투과하지 못하게 만든 타일입니다.",
                "액체는 투과하지만, 기체 흐름은 차단합니다",

                // NAME, DESC, EFFECT
                FormatAsLink("미네랄 분해 산소 확산기", OrganicDeoxidizer.ID),
                "흙 속에 존재하는 미생물에게 에너지를 제공하고, 약간의 산소를 얻어냅니다.",
                "흙을 비효율적으로 산소로 변환합니다",

                // NAME, DESC, EFFECT
                FormatAsLink("물 합성기", H2OSynthesizer.ID),
                $"{FormatAsLink("수소", "HYDROGEN")}와 {FormatAsLink("산소", "OXYGEN")}로 {FormatAsLink("물", "WATER")}을 합성합니다.",
                "미지의 기술로 물을 합성합니다",

                // NAME, DESC, EFFECT
                FormatAsLink("이산화 탄소 분해기", CO2DecomposerEZ.ID),
                $"{FormatAsLink("이산화 탄소", "CARBONDIOXIDE")}를 {FormatAsLink("산소", "OXYGEN")}와 {FormatAsLink("석탄", "CARBON")}로 분해합니다",
                "미지의 기술로 이산화 탄소를 분해합니다",

                // begin)) SUPER_SUIT --->
                // NAME, DESC, EFFECT
                FormatAsLink("슈퍼 슈트", Basic_SuperSuit.ID),
                "이 슈트는 쳐다보기만 해도 기분이 좋아집니다.",
                "주변의 장식 수치를 크게 증가시킵니다.",

                 // NAME, DESC, EFFECT
                FormatAsLink("전설적인 슈퍼 슈트", Advance_SuperSuit.ID),
                "이 슈트는 어느 노련한 디자이너의 손에서 탄생했다고 전해지는 특별한 슈트입니다.",
                "주변의 장식 수치를 최고 수준으로 증가시킵니다.",

                // NAME, DESC, EFFECT
                FormatAsLink("조잡한 작은 배낭", SmallBackpack.ID),
                "조금 더 많이 운반할 수 있게됩니다.",
                "조잡하지만, 쓸만한 배낭입니다.",

                // NAME, DESC, EFFECT
                FormatAsLink("현대적인 큰 배낭", MediumBackpack.ID),
                "작은 배낭보다 더 많이 운반할 수 있게되지만, 기술적인 한계로 이동 속도가 약간 감소합니다.",
                "튼튼한 배낭을 찾습니까? 이 배낭을 유심히 살펴보십시오.",

                // NAME, DESC, EFFECT
                FormatAsLink("전설적인 슈퍼 배낭", LargeBackpack.ID),
                "더 효율적으로, 엄청나게 많은 무게를 추가적으로 운반할 수 있게됩니다.",
                "미래의 기술로 만들어진 전설적인 배낭을 사용해보세요.",
            };
    }
}
