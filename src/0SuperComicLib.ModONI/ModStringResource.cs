// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.ModONI.Configurations;
using SuperComicLib.YamlXX;

namespace SuperComicLib.ModONI;

public readonly ref struct ModStringResource
{
    public const string ModResourceDirectoryName = "resources.lang";
    public const string FileNamePrefix = "strings.";
    public const string FileExtensions = ".yaml";

    private readonly string _modDirectory;
    private readonly PreferredLanguageConfig _preferred;

    private ModStringResource(
        string hostAssemblyLocation,
        PreferredLanguageConfig preferred)
    {
        _preferred = preferred;
        _modDirectory = Path.GetDirectoryName(hostAssemblyLocation);
    }

    public readonly bool TryLoad()
    {
        using var __s = FDebug.EnterScope(nameof(ModStringResource));
        try
        {
            var resDirectory = Path.Combine(_modDirectory, ModResourceDirectoryName);
            if (!Directory.Exists(resDirectory))
            {
                return false;
            }

            var handler = StringResourceLoadContext.ScriptBlockHandler;
            
            var limits = (uint)_preferred.LimitFallbackDepth;
            if (limits > 0)
            {
                var preferences = _preferred.Preferences;
                for (int i = 0; i < preferences.Length; ++i)
                {
                    // is default language
                    if (preferences[i].Id.Equals(ConfigDefaultPreset.DefaultLangID, StringComparison.OrdinalIgnoreCase)) continue;
    
                    if (LoadLocalizedString(resDirectory, preferences[i].Id, ref handler))
                    {
                        FDebug.Log($"    '{preferences[i].Name}'({preferences[i].Id}) language string resource was loaded.");
    
                        if (--limits == 0U) break;
                    }
                }
            }

            // load default (ko-KR)
            if (!LoadLocalizedString(resDirectory, ConfigDefaultPreset.DefaultLangID, ref handler))
            {
                // 기본 언어는 로드에 실패해서는 안된다
                return false;
            }

            FDebug.Log($"    Default language '{ConfigDefaultPreset.DefaultLangName}'({ConfigDefaultPreset.DefaultLangID}) string resource was loaded.");
            FDebug.Log($"    {handler.ResultStringDict.Count} string resources are ready and will be applied.");

            var ignoredCount = 0U;

            var quickIndex = STRINGSQuickIndex.Prefixes;
            var tempBuffer = new string[2]; // param 재활용 용도의 임시 배열
            ref var __key = ref tempBuffer[0];
            ref var __val = ref tempBuffer[1];
            foreach (var kv in handler.ResultStringDict)
            {
                // null이나 빈 문자열의 키 값은 무시한다
                var text = kv.Key.AsSpan().Trim();
                if (text.Length < 1)
                {
                    ++ignoredCount;
                    continue;
                }
                
                // 미리 할당 해두기
                __key = kv.Key;
                __val = kv.Value;
                
                // 숫자로 시작하는 경우 prefixes에 대한 참조로 인식한다
                if ((uint)(text[0] - '0') <= ('9' - '0'))
                {
                    var idx = text.IndexOf('.');
                    
                    // 1. '0.'과 같은 prefxi를 가지면서
                    // 2. 정상적인 숫자이고
                    // 3. 인덱스 범위를 초과하지 않는것
                    if (idx > 0 && 
                        int.TryParse(text[..idx], out var refIndex) && 
                        (uint)refIndex < (uint)quickIndex.Length)
                    {
                        // string과 ReadOnlySpan을 연결하는 좋은 방법이 .netstandard2.1에 없다
                        __key = $"{quickIndex[refIndex]}{text[(idx + 1)..].ToString()}";
                    }
                }
                
                Strings.Add(tempBuffer);
            }

            if (ignoredCount > 0)
            {
                FDebug.LogWarning($"    A total of {ignoredCount} string resources were not applied.");
            }

            FDebug.Log($"    String resource loading succeeded.");
        }
        catch (Exception ex)
        {
            FDebug.LogWarning(ex.ToString());
            return false;
        }

        return true;
    }

    private static bool LoadLocalizedString(string resDirectory, string langID, ref StringInterpolatedScriptBlockHandler handler)
    {
        var langFile = Path.Combine(resDirectory, $"{FileNamePrefix}{langID}{FileExtensions}");
        if (!File.Exists(langFile)) return false;

        bool succeess = false;
        try
        {
            handler.ResetLastError();
            PSLikeYaml.TryParseFromBytes(File.ReadAllBytes(langFile), ref handler);
            succeess = true;
        }
        catch (Exception ex1)
        {
            FDebug.LogWarning(ex1.ToString());
        }

        if (!handler.Success)
        {
            FDebug.LogWarning(
                $"""
                Parsing error. Meesage: {handler.LastErrorMessage}
                    Error Code: {handler.LastError}.
                    File pos.: {handler.LastErrorMark}
                """);
            return false;
        }

        // catch 블록에서 오류 출력 후, handler 내 오류를 출력하기 위함
        return succeess;
    }

    public static ModStringResource Create(in StringResourceLoadContext context)
    {
        return new(context.Owner.assembly.Location, PreferredLanguageConfig.Shared);
    }
}