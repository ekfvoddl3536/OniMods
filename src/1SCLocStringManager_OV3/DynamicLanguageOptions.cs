// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#nullable enable
using PeterHan.PLib.Options;

namespace SuperComicLib.ModONI.Configurations.Views;

internal sealed class DynamicLanguageOptions<T> : DynamicLanguageOptionsBase
    where T : unmanaged
{
    private static DynamicLanguageOptions<T>? s_instance;
    
    private static unsafe bool IsPreferred => sizeof(T) == sizeof(int);

    public DynamicLanguageOptions(string field, IOptionSpec _) : base(field, new OptionSpec())
    {
        s_instance ??= this;
    }

    protected override int PrivateIndex
    {
        get => 
            IsPreferred 
            ? ModConfigV3.Instance!.PreferredLanguage 
            : ModConfigV3.Instance!.AlternateLanguage;

        set
        {
            if (IsPreferred)
                ModConfigV3.Instance!.PreferredLanguage = value;
            else
                ModConfigV3.Instance!.AlternateLanguage = value;

            _selected = value;
        }
    }

    protected override DynamicLanguageOptionsBase Opposite => 
        IsPreferred 
        ? DynamicLanguageOptions<char>.s_instance! 
        : DynamicLanguageOptions<int>.s_instance!;

    public override bool WriteTo(object settings)
    {
        // 한 번만 하면 된다
        if (IsPreferred)
        {
            // -- write out
            // var configActual = PreferredLanguageConfig.Shared;
            var clone = CloneArray(ModConfigV3.LangOptions);
            
            if (!Swap(clone, ModConfigV3.Instance!.PreferredLanguage, 0))
            {
                Debug.LogWarning("Preferred language option cannot be applied.");
            }
            if (!ModConfigV3.UnusableAlternate && !Swap(clone, ModConfigV3.Instance!.AlternateLanguage, 1))
            {
                Debug.LogWarning("Alternate language option cannot be applied.");
            }

            var configActual = new PreferredLanguageConfig(PreferredLanguageConfig.Shared.LimitFallbackDepth, clone);
            if (!configActual.TrySaveTo(ModFileOrDirectoryHints.LanguageConfigFile))
            {
                Debug.LogWarning($"Failed to save file to path '{ModFileOrDirectoryHints.LanguageConfigFile}'.");
            }
        }

        return true;
    }

    private U[] CloneArray<U>(ReadOnlySpan<U> vs)
    {
        if (vs.Length == 0) return [];

        var dst = new U[vs.Length];
        vs.CopyTo(dst);
        return dst;
    }

    private static bool Swap<U>(Span<U> arr, int selectIndex, int orderIndex)
    {
        if ((uint)(selectIndex | orderIndex) >= (uint)arr.Length) return false;

        // -- 이미 정렬돼있음
        if (selectIndex == orderIndex) return true;

        var m = arr[selectIndex];
        arr[selectIndex] = arr[orderIndex];
        arr[orderIndex] = m;

        return true;
    }

    private readonly struct OptionSpec : IOptionSpec
    {
        public readonly string Category => string.Empty;
        public readonly string Format => string.Empty;
        public readonly string Title => IsPreferred ? "Preferred Language" : "Alternate Language";
        public readonly string Tooltip =>
            IsPreferred
            ?
            """
            Specifies the user's preferred primary language.
            
            The specified language does not necessarily apply to all resources in the MOD.
            If a MOD provides multilingual resources designated as a preferred language, that resource will be used first.
            """
            :
            """
            Specifies the user's second preferred secondary language.

            This is the language the user prefers to use if the MOD does not provide resources in the user's preferred language.

            If the MOD does not provide resources in the preferred language or fallback language, the resources in the MOD's default language (typically Korean, not English) will be used.
            """;
    }
}
