// MIT License
//
// Copyright (c) 2023. Super Comic (ekfvoddl3535@naver.com)
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

using PeterHan.PLib.Options;
using PeterHan.PLib.UI;
using System.Linq;
using UnityEngine;

namespace SCMod
{
    internal sealed class F1LanguageSelectOne : OptionsEntry
    {
        private static int maxLength;
        private static LangF1Option _selected;
        private static LangF1Option[] _list;
        private static GameObject _combo;

        public F1LanguageSelectOne() : base(nameof(ModConfigV2_2.Language), new Spec())
        {
        }

        public F1LanguageSelectOne(string field, IOptionSpec attr) : base(field, attr) { }
        
        public override object Value
        {
            get => _selected?._info.packedName ?? 0;

            set
            {
                if (!(value is ulong pckLang) || _list == null)
                    return;

                var list = _list;
                for (int i = 0; i < list.Length; ++i)
                    if (list[i]._info.packedName == pckLang)
                    {
                        _selected = list[i];
                        Update();
                        break;
                    }
            }
        }

        public override GameObject GetUIComponent()
        {
            var options = PatchInfo.cultures.Select(x =>
            {
                var len = x.nativeName?.Length ?? 0;

                if (maxLength < len)
                    maxLength = len;

                return new LangF1Option(x);
            }).ToArray();

            _list = options;

            var cb = new PComboBox<LangF1Option>()
            {
                BackColor = PUITuning.Colors.ButtonPinkStyle,
                EntryColor = PUITuning.Colors.ButtonBlueStyle,
                InitialItem = options.FirstOrDefault(),
                Content = options,
                TextStyle = PUITuning.Fonts.TextLightStyle,
                OnOptionSelected = OnUpdateSelected
            }.SetMinWidthInCharacters(maxLength).Build();

            _combo = cb;

            Update();

            return cb;
        }

        private void Update()
        {
            if (_combo != null && _selected != null)
            {
                PComboBox<LangF1Option>.SetSelectedItem(_combo, _selected);
                POptions.WriteSettings(ModConfigV2_2.instance);
            }
        }

        private void OnUpdateSelected(GameObject _, LangF1Option choice)
        {
            if (choice != null)
                _selected = choice;
        }

        private sealed class LangF1Option : ITooltipListableOption
        {
            public readonly SimpleCultureInfo _info;

            public LangF1Option(in SimpleCultureInfo info) => _info = info;

            public string GetToolTipText() => string.Empty;
            public string GetProperName() => _info.nativeName;
        }

        private readonly struct Spec : IOptionSpec
        {
            public string Category => "Basic";
            public string Format => null;
            public string Title => nameof(ModConfigV2_2.Language);
            public string Tooltip => null;
        }

    }
}
