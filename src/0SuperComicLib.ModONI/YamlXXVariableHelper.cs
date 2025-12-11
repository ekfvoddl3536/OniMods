// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.YamlXX;
using static STRINGS.UI;

namespace SuperComicLib.ModONI;

internal static class YamlXXVariableHelper
{
    public static FuncTargetDict? s_funcTargets;
    public static MappedStringDict? s_mappedStrings;
    public static MappedStringDict? s_intermediateResults;
    
    public static unsafe void AddDefaultFuncTargets(FuncTargetDict dict)
    {
        ReadOnlySpan<(string, VFuncRStr)> items = [
            VFuncItem(&FormatAsLink),
            VFuncItem(&FormatAsKeyWord),
            VFuncItem(&FormatAsPositiveModifier),
            VFuncItem(&FormatAsNegativeModifier),
            VFuncItem(&FormatAsPositiveRate),
            VFuncItem(&FormatAsNegativeRate),
            VFuncItem(&FormatAsAutomationState),
            VFuncItem(&FormatAsHotkey),
            VFuncItem(&FormatAsTool),
            VFuncItem(&FormatAsCaps),
            VFuncItem(&GetString, "Get-String")
        ];
        
        InsertAll(items, dict);
    }

    private static string GetString(string text) => text;

    private static string FormatAsAutomationState(string text, string state)
    {
        return 
            state.Equals(nameof(AutomationState.Active), StringComparison.OrdinalIgnoreCase)
            ? STRINGS.UI.FormatAsAutomationState(text, AutomationState.Active)
            : STRINGS.UI.FormatAsAutomationState(text, AutomationState.Standby);
    }

    private static unsafe (string, VFuncRStr) VFuncItem(delegate* managed<string, string> fp, [CallerArgumentExpression(nameof(fp))] ReadOnlySpan<char> name = default)
    {
        return (GetFuncName(name), new(fp));
    }
    
    private static unsafe (string, VFuncRStr) VFuncItem(delegate* managed<string, string, string> fp, [CallerArgumentExpression(nameof(fp))] ReadOnlySpan<char> name = default)
    {
        return (GetFuncName(name), new(fp));
    }
    
    private static string GetFuncName(ReadOnlySpan<char> s)
    {
        // remove pointer reference character
        s = s.Trim('&');
        
        // remove class name, namespace etc.
        // keep left only method name
        var idx = s.LastIndexOf('.');
        if (idx >= 0)
            s = s[(idx + 1)..];
        return s.ToString();
    }
    
    public static void InsertAll<TKey, TValue>(ReadOnlySpan<(TKey Key, TValue Value)> items, Dictionary<TKey, TValue> destination)
    {
        for (int i = 0; i < items.Length; ++i)
        {
            if (!destination.TryAdd(items[i].Key, items[i].Value))
            {
                // 이전에 추가된 모든걸 제거한다
                RemoveAll(items[..i], destination);
                throw new InvalidOperationException("duplicate key!");
            }
        }
    }

    public static void RemoveAll<TKey, TValue>(ReadOnlySpan<(TKey Key, TValue Value)> items, Dictionary<TKey, TValue> source)
    {
        for (int i = 0; i < items.Length; ++i)
        {
            source.Remove(items[i].Key);
        }
    }
}
