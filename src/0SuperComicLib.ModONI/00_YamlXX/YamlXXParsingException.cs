// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace SuperComicLib.YamlXX;

public sealed class YamlXXParsingException : InvalidOperationException
{
    public YamlXXParsingException() { }
    public YamlXXParsingException(string message) : base(message) { }
}
