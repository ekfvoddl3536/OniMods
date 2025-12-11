// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#nullable enable
using HarmonyLib;
using PeterHan.PLib.Options;

namespace SuperComicLib.ModONI.Configurations.Views;

public sealed class ModLoad : V1XXModImpl
{
    public const string TRASH_FILE = ".Trash";

    protected override void OnLoadCore(Harmony harmony, ref StringResourceLoadContext __)
    {
        // -- 폴더에 있는 .Trash 파일은 지운다
        var trash = Path.Combine(path, TRASH_FILE);
        if (File.Exists(trash))
        {
            File.Delete(trash);
        }

        // -- 이미 로드됐으면 건너뛴다
        if (ModConfigV3.Instance is not null) return;

        // OnPatch(harmony);

        new POptions().RegisterOptions(this, typeof(ModConfigV3));

        ModConfigV3.LangOptions = PreferredLanguageConfig.Shared.Preferences;

        var conf = new ModConfigV3();
        
        // -- 싱글톤 할당
        ModConfigV3.Instance = conf;

        conf.OnValieUpdated();
    }
}
