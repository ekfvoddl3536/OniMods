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
using System;
using System.IO;
using System.Text;

namespace SuperComicLib.ModONI
{
    /// <summary>
    /// 가장 우선순위가 낮은 기본 표시 언어
    /// </summary>
    public abstract class ModLocaleKorean<TKey> : ModLocalization
        where TKey : unmanaged, IModLocalizationKey
    {
        protected ModLocaleKorean() : base(default(TKey).UUID)
        {
        }

        protected override sealed void OnInitialize()
        {
            // 기본 텍스트 가져오기
            var res = OnLoadOriginalStrings();

            var instance = GetInstance(default(TKey).UUID);
            if (instance == null)
            {
                // 폴더로 출력 (반드시 순차 출력)
                var fi_info = new FileInfo(Path.Combine(ModDirectory.GetPath<TKey>(), "original_strings_UTF16LE.txt"));

                // 기존 출력물이 존재할 경우 작업은 건너뛰기
                if (fi_info.Exists == false)
                {
                    var w = new StreamWriter(fi_info.Create(), Encoding.Unicode, 4096, false);

                    w.WriteLine("# Localization UUID");
                    w.WriteLine(default(TKey).UUID.ToString());
                    w.WriteLine();
                    w.WriteLine();

                    w.Flush();

                    for (int i = 0, max = res.Length; i < max; ++i)
                    {
                        w.WriteLine($"# strings index: [{i}]");
                        w.WriteLine(res[i]);
                        w.WriteLine();
                        w.WriteLine();

                        w.Flush();
                    }

                    w.Close();
                }
            }
            else if (instance is ModLocaleKorean<TKey>)
                throw new System.InvalidOperationException($"GUID: {default(TKey).UUID} is alredy exist");

            texts_ = res;
        }

        // 기본 표시 텍스트를 로딩
        protected abstract string[] OnLoadOriginalStrings();

        public override sealed Localization.Language MainLanguage => Localization.Language.Korean;

        protected override sealed void OnMergeTexts(ModLocalization other) =>
            // 기존의 번역 텍스트가 더 높은 우선 순위를 갖는다
            texts_ = MergeTexts(texts_, other.Texts);

        // 모든 텍스트를 게임에 적용
        public void Apply(string[] STRING_keys)
        {
            string[] texts = texts_;
            int length = STRING_keys.Length;

            if (length > texts.Length)
            {
                length = texts.Length;
                UnityEngine.Debug.LogWarning($"missing some strings: [Key: {default(TKey).GetType().FullName}]");
            }

            for (int i = 0; i < length; ++i)
                Strings.Add(STRING_keys[i], texts[i]);
        }
    }
}
