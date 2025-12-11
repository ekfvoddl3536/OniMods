// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#nullable enable
using HarmonyLib;

namespace SuperComicLib.ModONI;

[HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
internal static class Db_Initialize_Patch
{
    private static bool s_isPatched;

    public static bool Prepare() => !s_isPatched && (s_isPatched = true);

    public static void Postfix(Db __instance)
    {
        var techs = __instance.Techs;

        var list = ModTechs.Move();
        if (list is null) return;

        for (int i = 0; i < list.Count; ++i)
        {
            var (TechID, BuildingID) = list[i];
            list[i] = default;

            var item = techs.TryGet(TechID);
            if (item is not null)
            {
                item.unlockedItemIDs.Add(BuildingID);
            }
            else
            {
                FDebug.LogWarning($"TechID Not Found -> '{TechID}'");
            }
        }

        GC.Collect(0, GCCollectionMode.Optimized, false, false);
    }
}
