// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using UnityEngine;
using VYaml.Serialization;

namespace SuperComicLib.ModONI.Configurations;

partial class PreferredLanguageConfig
{
    #region load from file or default
    public static PreferredLanguageConfig FromFileOrDefault(string inputFile)
    {
        using var __s = FDebug.EnterScope(nameof(PreferredLanguageConfig));
        if (!File.Exists(inputFile)) goto _Default;

        try
        {
            var inst = YamlSerializer.Deserialize<PreferredLanguageConfig>(File.ReadAllBytes(inputFile));
            if (inst is null) goto _Default;

            var values = inst.Preferences.AsSpan();
            var original = values;

            inst.LimitFallbackDepth = Mathf.Clamp(inst.LimitFallbackDepth, -1, short.MaxValue);

            for (int i = 0; i < values.Length; ++i)
            {
                // 1. skip null item
                // 2. trim Id string
                // 3. test Id
                // 4. trim Name string
                ref var item = ref values[i];

                var temp = item.Id.AsSpan().Trim();
                if (!IsValidLanguageCultureExpr(temp)) goto _RemoveItem;

                // if modified, set
                if (temp.Length != item.Id.Length)
                {
                    item.Id = temp.ToString();
                }

                temp = item.Name.AsSpan().Trim();
                if (temp.Length != item.Name.Length)
                {
                    item.Name = temp.ToString();
                }

                continue;

            _RemoveItem:
                values[(i + 1)..].CopyTo(values[i..]);
                values[^1] = default;
                values = values[..^1];
            }

            if (values.Length == 0)
            {
                // default set
                goto _Default;
            }
            else if (values.Length != original.Length)
            {
                inst.Preferences = values.ToArray();
            }

            // DefaultOptionSet이 지원하는 언어가 더 많은지? (즉, 파일이 오래됐는지?)
            if (inst.Preferences.Length < ConfigDefaultPreset.DefaultOptionSet.Length)
            {
                // merge 및 업데이트 작업
                MergeAndUpdate(inst, inputFile);
            }

            FDebug.Log($"Preferred language settings were successfully loaded from file.");
            s_commonDefault = inst;
            return inst;
        }
        catch (Exception ex)
        {
            FDebug.LogWarning($"An error occurred while loading from file. Exception: {ex}");
        }

    _Default:
        // default value
        return Default;
    }

    public static bool IsValidLanguageCultureExpr(ReadOnlySpan<char> s) =>
        s.Length == 5 && s[2] == '-' &&
        TestAscii(s[0], 'a') && TestAscii(s[1], 'a') &&     // ISO 639-1 , not allowed upper case
        TestAscii(s[3], 'A') && TestAscii(s[4], 'A');       // ISO 3166-1, not allowed lower case

    private static bool TestAscii(char v, char m) => (uint)(v - m) <= ('z' - 'a');
    #endregion

    #region sort and save
    private static void MergeAndUpdate(PreferredLanguageConfig inst, string outputFile)
    {
        FDebug.Log("Found a new element in the list of supported languages.");

        inst.Preferences = [.. inst.Preferences.Union(ConfigDefaultPreset.DefaultOptionSet)];
        if (!inst.TrySaveTo(outputFile))
        {
            FDebug.LogWarning("Found updated culture resource, but failed to save the updates.");
        }
    }

    public bool TrySaveTo(string filepath)
    {
        try
        {
            var mem = YamlSerializer.Serialize(this);
            using var fs = File.Create(filepath);
            fs.Write(mem.Span);
            fs.Close();
        }
        catch
        {
            return false;
        }
        
        return true;
    }
    #endregion
}
