#pragma warning disable IDE1006 // 명명 스타일
using ci_ = ClothingWearer.ClothingInfo;

namespace SupportPackage.Merge_SUPERSUIT
{
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
            const string KPATH = "STRINGS.EQUIPMENT.PREFABS.";

            id = KPATH + id.ToUpper();

            Strings.Add($"{id}.GENERICNAME", STRINGS.EQUIPMENT.PREFABS.COOL_VEST.GENERICNAME);
            Strings.Add($"{id}.RECIPE_DESC", Strings.Get($"{id}.EFFECT"));

            return new ci_(Strings.Get($"{id}.NAME"), decorMod, TEMP_DEF, -1.25f);
        }
    }
}
