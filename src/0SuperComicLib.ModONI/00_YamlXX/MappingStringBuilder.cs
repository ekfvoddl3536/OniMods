// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace SuperComicLib.YamlXX;

public sealed class MappingStringBuilder
{
    private readonly List<(string Key, string Value)> _mappingList;
    
    public MappingStringBuilder()
    {
        _mappingList = new(32);
    }
    
    public MappingStringBuilder AddId(params ReadOnlySpan<string> IDs)
    {
        for (int i = 0; i < IDs.Length; ++i)
        {
            _mappingList.Add(($"id:{IDs[i]}", IDs[i]));
        }
        return this;
    }
    
    public MappingStringBuilder AddVar<T>(string name, T value)
    {
        _mappingList.Add(($"var:{name}", value.ToString()));
        return this;
    }

    internal (string Key, string Value)[] ToArray() => [.. _mappingList];
}