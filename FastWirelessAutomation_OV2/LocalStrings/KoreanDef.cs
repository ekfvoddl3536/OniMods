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

namespace FastWirelessAutomation.LocalStrings
{
    internal sealed class KoreanDef : ModLocaleKorean<ModStringsKey>
    {
        protected override string[] OnLoadOriginalStrings() =>
            new[]
            {
                // TOOLTIP, TITLE
                "원하는 채널로 변경할 수 있습니다. OR 채널은 0 ~ 127 입니다.",
                "채널",

                // NAME, DESC, EFFECT
                "자동화 신호 송신기 (E)",
                "입력 받은 자동화 신호를 지정된 채널에 전송합니다.",
                "유선 신호를 받아서 무선으로 변환합니다.",

                // NAME, DESC, EFFECT
                "자동화 신호 수신기 (R)",
                "지정된 채널에서 신호를 수신받습니다.",
                "무선 신호를 받아서 유선 신호로 변환합니다.",
            };
    }
}
