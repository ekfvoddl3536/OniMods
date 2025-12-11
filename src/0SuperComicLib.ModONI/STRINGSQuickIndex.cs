// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace SuperComicLib.ModONI;

public static class STRINGSQuickIndex
{
    public const string PrefixBuildingsPrefabs = "STRINGS.BUILDINGS.PREFABS.";
    public const string PrefixUISideScreens = "STRINGS.UI.UISIDESCREENS.";
    public const string PrefixBuildingsStatusItems = "STRINGS.BUILDING.STATUSITEMS.";
    public const string PrefixEquipmentPrefabs = "STRINGS.EQUIPMENT.PREFABS.";
    
    private static readonly string[] s_references = [
        /* (0.) */ PrefixBuildingsPrefabs,
        /* (1.) */ PrefixUISideScreens,
        /* (2.) */ PrefixBuildingsStatusItems,
        /* (3.) */ PrefixEquipmentPrefabs
    ];
    
    internal static string[] Prefixes => s_references;
}