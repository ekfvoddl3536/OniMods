// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace SuperComicLib.ModONI.Configurations;

public static class ConfigDefaultPreset
{
    public const string DefaultLangID = "ko-KR";
    public const string DefaultLangName = "한국어 (Korean)";

    public const int DefaultFallbackDepth = 2;
    
    public static readonly ConfigOptionItem[] DefaultOptionSet = [
        new(DefaultLangID, DefaultLangName),
        new("en-US", "English (US)"),
        new("ja-JP", "日本語 (Japanese)"),
        new("zh-CN", "中文 (Chinese)"),
    ];
}