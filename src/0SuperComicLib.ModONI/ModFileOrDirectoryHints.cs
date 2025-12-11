// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using KMod;

namespace SuperComicLib.ModONI;

public static class ModFileOrDirectoryHints
{
    public const string UserModManifestFileName = "user_mod_manifest.yaml";

    public const string GlobalConfigFileName = "scoxni-global.common.yaml";

    public const string CommonConfigFileName = "scoxni-language.common.yaml";
    public const string PresetCommonConfigFileName = "preset." + CommonConfigFileName;
    
    private static string? s_modsRootDirectory;
    private static string? s_sharedConfigDirectory;
    private static string? s_privateCommonConfigFile;
    private static string? s_privatePresetCommonConfigFile;

    private static string? s_globalCommonConfigFile;
    
    /// <summary>
    /// Gets the location of the game's root 'mods/' directory.
    /// </summary>
    public static string ModsRootDirectory => s_modsRootDirectory ??= Manager.GetDirectory();

    /// <summary>
    /// Get shared 'config/' directory path
    /// </summary>
    public static string SharedConfigDirectory => s_sharedConfigDirectory ??= Path.Combine(ModsRootDirectory, "config");
    
    /// <summary>
    /// 언어 설정이 있는 파일
    /// </summary>
    public static string LanguageConfigFile => s_privateCommonConfigFile ??= Path.Combine(SharedConfigDirectory, CommonConfigFileName);
    
    /// <summary>
    /// 'preset.' prefix filename
    /// </summary>
    public static string LanguagePresetConfigFile => s_privatePresetCommonConfigFile ??= Path.Combine(SharedConfigDirectory, PresetCommonConfigFileName);

    /// <summary>
    /// 전역적인 설정이 포함돼있는 파일
    /// </summary>
    public static string GlobalCommonConfigFile => s_globalCommonConfigFile ??= Path.Combine(SharedConfigDirectory, GlobalConfigFileName);
}
