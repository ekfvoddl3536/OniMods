// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#nullable enable
namespace SuperComicLib.ModONI;

internal static class ModTechs
{
    private static List<(string TechID, string BuildingID)>? s_lazyTemporaryList;

    internal static List<(string TechID, string BuildIngID)> LazyTemporaryList => s_lazyTemporaryList ??= [];

    internal static List<(string TechID, string BuildingID)>? Move()
    {
        var list = s_lazyTemporaryList;
        s_lazyTemporaryList = null;
        return list;
    }
}
