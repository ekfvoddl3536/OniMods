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
using static STRINGS.UI;

namespace EcoFriendlyToliet.LocalStrings
{
    using static GlobalConsts;
    internal sealed class KoreanDef : ModLocaleKorean<ModStringsKey>
    {
        protected override string[] OnLoadOriginalStrings() =>
            new[]
            {
                // 툴팁
                "이용 \"{FlushesRemaining}\"회 남음",
                "이 시설은 보수가 필요하기 전까지 \"{FlushesRemaining}\"명이 이용할 수 있습니다.",

                // NAME, DESC, EFFECT
                FormatAsLink("친환경 간이 화장실", EcoFriendlyToilet.ID),
                "복제체에게 배출할 친환경의 중요성에 대해서 교육하십시오.",
                
                "이 시설은 배관을 요구하지 않으며, 비료를 만듭니다.\n" +
                "주기적으로 만들어진 비료를 비워야합니다.",

                // NAME, DESC, EFFECT
                FormatAsLink("친환경 세면대", AutoPurifyWashsink.ID),
                "복제체로부터 세균을 제거하고, 만들어진 오염수를 정수하여 재사용합니다.",
                
                "세균에 덮인 복제체는 선택된 방향으로 지나갈 때 세면대를 사용합니다.\n" +
                "모래가 있으면, 오염된 물을 깨끗한 물로 정수합니다. 정수된 물은 재사용합니다.\n" +
                "전력을 공급하면 더 빨리 정수할 수 있고, 보다 효율적으로 사용할 수 있습니다."
            };
    }
}
