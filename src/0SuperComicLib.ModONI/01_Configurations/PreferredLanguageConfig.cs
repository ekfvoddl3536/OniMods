// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using VYaml.Annotations;

namespace SuperComicLib.ModONI.Configurations;

[YamlObject(NamingConvention.SnakeCase)]
public sealed partial class PreferredLanguageConfig
{
    private static PreferredLanguageConfig? s_commonDefault;

    public static PreferredLanguageConfig Shared => s_commonDefault ??= FromFileOrDefault(ModFileOrDirectoryHints.LanguageConfigFile);
    
    public static PreferredLanguageConfig Default => new(-1, ConfigDefaultPreset.DefaultOptionSet)
    {
        IsDefault = true
    };

    public int LimitFallbackDepth { get; private set; }
    public ConfigOptionItem[] Preferences { get; private set; }
    
    [YamlIgnore]
    public bool IsDefault { get; private init; }

    [YamlConstructor]
    public PreferredLanguageConfig(int limitFallbackDepth, ConfigOptionItem[] preferences)
    {
        LimitFallbackDepth = limitFallbackDepth;
        Preferences = preferences;
    }
}