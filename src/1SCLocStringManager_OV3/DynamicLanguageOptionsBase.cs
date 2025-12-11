// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#nullable enable
using PeterHan.PLib.Options;
using PeterHan.PLib.UI;
using UnityEngine;

namespace SuperComicLib.ModONI.Configurations.Views;

internal abstract class DynamicLanguageOptionsBase : OptionsEntry
{
    private bool _Unavailable;
    private GameObject? _uiComponent;
    private LangOption[]? _list;
    protected int _selected;

    protected DynamicLanguageOptionsBase(string field, IOptionSpec attr) : base(field, attr) { }

    protected abstract int PrivateIndex { get; set; }

    protected abstract DynamicLanguageOptionsBase Opposite { get; }

    public override object Value
    {
        get => _selected;

        set
        {
            if (value is not int id || _list is null || (uint)id >= (uint)_list.Length) return;

            _selected = id;
            OnUpdated(id);
        }
    }

    private void OnUpdated(int index)
    {
        if (_Unavailable)
        {
            Debug.LogWarning("Unavailable option. ignore.");
            return;
        }

        // -- 반대편의 현재 선택이 뭔지 알아야한다
        var _opposite = Opposite;
        if (_opposite.PrivateIndex == index)
        {
            // 반대편 옵션과 같은 경우에 두 값을 스왑한다
            _opposite.PrivateIndex = PrivateIndex;
        }

        PrivateIndex = index;

        // -- validation
        ModConfigV3.Instance!.OnValieUpdated();

        _opposite.Refresh();
        Refresh();
    }

    private void Refresh()
    {
        var index = PrivateIndex;

        if (_list is null) return;
        if ((uint)index >= (uint)_list.Length) return;

        var choice = _list[index];
        _selected = choice.Index;
        PComboBox<LangOption>.SetSelectedItem(_uiComponent, choice);
    }

    public override GameObject GetUIComponent()
    {
        System.Diagnostics.Debug.Assert(ModConfigV3.Instance is not null);

        var index = PrivateIndex;
        _selected = index;

        if (ModConfigV3.UnusableAlternate || (uint)index >= (uint)ModConfigV3.LangOptions!.Length)
        {
            _Unavailable = true;
            return _uiComponent = new PLabel() { Text = "<Unavailable>" }.Build();
        }
        
        var maxlen = 0;
        var _options = ModConfigV3.LangOptions.Select((x, i) =>
        {
            var len = x.Name.Length;
            maxlen = maxlen >= len ? maxlen : len;
            return new LangOption(x, i);
        }).ToArray();

        _list = _options;

        _uiComponent = new PComboBox<LangOption>
        {
            BackColor = PUITuning.Colors.ButtonBlueStyle,
            EntryColor = PUITuning.Colors.ButtonBlueStyle,
            TextStyle = PUITuning.Fonts.TextLightStyle,
            OnOptionSelected = OnUpdateSelected,
            InitialItem = _options[index],
            Content = _options
        }.SetMinWidthInCharacters(maxlen + 1).Build();

        return _uiComponent;
    }

    private void OnUpdateSelected(GameObject _, LangOption choice)
    {
        if (choice is not null)
        {
            _selected = choice.Index;
            OnUpdated(choice.Index);
        }
    }

    protected sealed class LangOption : ITooltipListableOption
    {
        public LangOption(ConfigOptionItem item, int index)
        {
            Item = item;
            Index = index;
        }

        public ConfigOptionItem Item { get; }

        public int Index { get; }

        public string GetToolTipText() => string.Empty;
        public string GetProperName() => Item.Name;
    }
}
