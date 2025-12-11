// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using VYaml.Parser;

namespace SuperComicLib.YamlXX;

public ref struct StringInterpolatedScriptBlockHandler
{
    private readonly List<string> _items;
    private readonly FuncTargetDict _fpTargets;
    private readonly MappedStringDict _mappedStrings;
    private readonly MappedStringDict _resultStringDict;
    
    private string _lastErrorMessage;
    private YamlXXParseError _lastError;
    private Marker _lastErrorMark;
    
    public StringInterpolatedScriptBlockHandler(
        FuncTargetDict target, 
        MappedStringDict hashtable,
        MappedStringDict? resultMap = null)
    {
        if (target is null) throw new ArgumentNullException(nameof(target));
        if (hashtable is null) throw new ArgumentNullException(nameof(hashtable));
        
        // readonly shared items
        _fpTargets = target;
        _mappedStrings = hashtable;
        _resultStringDict = resultMap ?? new(StringComparer.OrdinalIgnoreCase);
        
        // local scope list
        _items = new(48);
        
        // set to default
        _lastError = YamlXXParseError.Success;
        _lastErrorMark = default;
        _lastErrorMessage = string.Empty;
    }
    
    public readonly bool Success => _lastError == YamlXXParseError.Success;
    
    public readonly YamlXXParseError LastError => _lastError;

    public readonly Marker LastErrorMark => _lastErrorMark;
    
    public readonly string LastErrorMessage => _lastErrorMessage;
    
    public readonly MappedStringDict ResultStringDict => _resultStringDict;

    internal void SetLastError(string message, YamlXXParseError lastError, Marker mark)
    {
        _lastErrorMessage = message;
        _lastError = lastError;
        _lastErrorMark = mark;
    }
    
    internal void ResetLastError() => SetLastError(string.Empty, YamlXXParseError.Success, default);
    
    internal readonly unsafe YamlXXParseError ParseAppend(string v)
    {
        var items = _items;

        var target = _fpTargets;
        var strmap = _mappedStrings;

        items.Clear();
        
        var s = v.AsSpan();

        var errno = YamlXXParseError.Success;
        while (s.Length > 0)
        {
            var ni = s.IndexOf("$(", StringComparison.Ordinal);
            if (ni >= 0)
            {
                items.Add(s[..ni].ToString());
                s = s[(ni + 2)..];

                var t = LiteralHelpers.Expression(ref s, target, strmap, &errno);
                items.Add(t);

                if (errno != YamlXXParseError.Success) 
                    return errno;
            }
            else
            {
                if (s.Length == v.Length)
                    items.Add(v);
                else
                    items.Add(s.ToString());
                break;
            }
        }
        
        return YamlXXParseError.Success;
    }

    internal readonly void AddString(string key)
    {
        Debug.Assert(_lastError == YamlXXParseError.Success);
        
        string value = 
            (_items.Count == 1)
            ? _items[0]
            : string.Concat(_items);
            
        _resultStringDict.TryAdd(key, value);
    }
}
