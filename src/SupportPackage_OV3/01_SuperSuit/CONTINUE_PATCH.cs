// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using SuperComicLib.ModONI;
using ci_ = ClothingWearer.ClothingInfo;

namespace SupportPackage.SuperSuitOrigin;

using static GlobalConsts.SUPER_SUIT;
internal static class CONTINUE_PATCH
{
    public static void OnLoad()
    {
        Basic_SuperSuit.Info = setup(Basic_SuperSuit.ID, Basic_SuperSuit.DECOR_MOD);
        Advance_SuperSuit.Info = setup(Advance_SuperSuit.ID, Advance_SuperSuit.DECOR_MOD);

        SmallBackpack.Info = setup(SmallBackpack.ID);
        MediumBackpack.Info = setup(MediumBackpack.ID);
        LargeBackpack.Info = setup(LargeBackpack.ID);
    }

    private static ci_ setup(string id, int decorMod = DECOR_MOD_DEF)
    {
        const string KPATH = STRINGSQuickIndex.PrefixEquipmentPrefabs;

        id = KPATH + id.ToUpper();

        Strings.Add($"{id}.GENERICNAME", Strings.Get($"{KPATH}COOL_VEST.GENERICNAME"));
        Strings.Add($"{id}.RECIPE_DESC", Strings.Get($"{id}.EFFECT"));

        return new ci_(Strings.Get($"{id}.NAME"), decorMod, TEMP_DEF, -1.25f);
    }
}
