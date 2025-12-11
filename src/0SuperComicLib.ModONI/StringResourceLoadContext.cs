// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using KMod;
using SuperComicLib.YamlXX;

namespace SuperComicLib.ModONI;

using static YamlXXVariableHelper;
public ref struct StringResourceLoadContext
{
    internal static StringInterpolatedScriptBlockHandler ScriptBlockHandler => new(s_funcTargets, s_mappedStrings, s_intermediateResults);
    
    private MappingStringBuilder? _mappingStringBuilder;
    
    internal StringResourceLoadContext(UserMod2 owner)
    {
        Owner = owner;
        LocalFuncTargets = default;
        LocalMappingItems = default;
        _mappingStringBuilder = null;
    }

    internal UserMod2 Owner { get; }

    public ReadOnlySpan<(string Key, VFuncRStr Value)> LocalFuncTargets { get; set; }

    public ReadOnlySpan<(string Key, string Value)> LocalMappingItems { get; set; }

    public MappingStringBuilder MappingStringBuilder => _mappingStringBuilder ??= new();

    public bool Cancel { get; set; }

    internal readonly void Init()
    {
        if (s_funcTargets is null) 
        {
            s_funcTargets = new FuncTargetDict(StringComparer.OrdinalIgnoreCase);
            AddDefaultFuncTargets(s_funcTargets);
        }

        s_mappedStrings ??= new MappedStringDict(StringComparer.OrdinalIgnoreCase);
        s_mappedStrings.Clear();

        s_intermediateResults ??= new MappedStringDict(StringComparer.OrdinalIgnoreCase);
        s_intermediateResults.Clear();
    }

    internal readonly void Build()
    {
        System.Diagnostics.Debug.Assert(s_funcTargets != null);
        System.Diagnostics.Debug.Assert(s_mappedStrings != null);
        
        InsertAll(LocalFuncTargets, s_funcTargets);
        InsertAll(LocalMappingItems, s_mappedStrings);
        
        if (_mappingStringBuilder != null)
        {
            InsertAll(_mappingStringBuilder.ToArray(), s_mappedStrings);
        }
    }

    internal readonly void Restore()
    {
        System.Diagnostics.Debug.Assert(s_funcTargets != null);

        RemoveAll(LocalFuncTargets, s_funcTargets);
    }
}
