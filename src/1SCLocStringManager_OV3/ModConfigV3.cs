// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#nullable enable
using Newtonsoft.Json;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using UnityEngine;

namespace SuperComicLib.ModONI.Configurations.Views;

[ConfigFile(ModLoad.TRASH_FILE, false, false)]
[ModInfo(@"https://github.com/ekfvoddl3536/OniMods", collapse: true), RestartRequired]
public sealed partial class ModConfigV3
{
    internal static ModConfigV3? Instance { get; set; }
    internal static ConfigOptionItem[]? LangOptions { get; set; }
    internal static bool UnusableAlternate { get; set; }

    [DynamicOption(typeof(DynamicLanguageOptions<int>), category: "Preferences"), JsonIgnore]
    public int PreferredLanguage { get; set; } = 0;

    [DynamicOption(typeof(DynamicLanguageOptions<char>), category: "Preferences"), JsonIgnore]
    public int AlternateLanguage { get; set; } = 1;

    [Option("OPEN SHARED CONFIG DIRECTORY", category: "Advanced"), JsonIgnore]
    public Action<object> OpenConfigDir => static _ => Application.OpenURL(new Uri(ModFileOrDirectoryHints.SharedConfigDirectory).AbsoluteUri);

    [Option("MANUAL LANGUAGE PREFERENCES CONFIG", category: "Advanced"), JsonIgnore]
    public Action<object> OpenConfigFile => static _ => Application.OpenURL(new Uri(ModFileOrDirectoryHints.LanguageConfigFile).AbsoluteUri);

    public void OnValieUpdated()
    {
        // -- 잘못된 값이 있다면 해결한다
        var pl = PreferredLanguage;
        var al = AlternateLanguage;

        // -- 음수는 존재할 수 없다
        pl = pl >= 0 ? pl : 0;
        al = al >= 0 ? al : 0;

        // -- 두 값이 같은 경우는 없어야 한다
        al = pl != al ? al : (al + 1);

        // -- 둘 중 어느것도 지원되는 언어 리스트의 인덱스를 초과할 수 없다
        var set = LangOptions!;
        if ((uint)(pl | al) >= (uint)set.Length)
        {
            // 그러나, set 자체가 하나밖에 없다면 0으로 초기화한다
            // 다만, 이 경우 AlternateLanguage는 비활성화되어야 한다.
            if (set.Length < 2)
            {
                UnusableAlternate = true;
            }
            else
            {
                pl = 0;
                al = 1;
            }
        }

        PreferredLanguage = pl;
        AlternateLanguage = al;
    }
}
