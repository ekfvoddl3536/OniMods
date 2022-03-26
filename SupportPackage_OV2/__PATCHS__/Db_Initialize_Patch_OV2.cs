using HarmonyLib;
using SuperComicLib.ModONI;

namespace SupportPackage
{
    using static GlobalConsts;
    using static GlobalConsts.SUPER_SUIT;
    [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
    public static class Db_Initialize_Patch_OV2
    {
        public static void Postfix()
        {
            var list = new StringsKeyList(21 + 15);

            new LocalStrings.KoreanDef().Apply(
                list
                    .ADD_NAME_DESC_EFFECT(EasyElectrolyzer.ID)
                    .ADD_NAME_DESC_EFFECT(TwoInOneGrill.ID)
                    .ADD_NAME_DESC_EFFECT(WastewaterSterilizer.ID)
                    .ADD_NAME_DESC_EFFECT(MagicTile.ID)
                    .ADD_NAME_DESC_EFFECT(OrganicDeoxidizer.ID)
                    .ADD_NAME_DESC_EFFECT(H2OSynthesizer.ID)
                    .ADD_NAME_DESC_EFFECT(CO2DecomposerEZ.ID)
                    .ADD_EQUIPMENT_K4(Basic_SuperSuit.ID)
                    .ADD_EQUIPMENT_K4(Advance_SuperSuit.ID)
                    .ADD_EQUIPMENT_K4(SmallBackpack.ID)
                    .ADD_EQUIPMENT_K4(MediumBackpack.ID)
                    .ADD_EQUIPMENT_K4(LargeBackpack.ID)
                    .Buffer
                );

            const string TAB_OXYGEN = "Oxygen";

            ModUtil.AddBuildingToPlanScreen(TAB_OXYGEN, EasyElectrolyzer.ID);
            ModUtil.AddBuildingToPlanScreen("Food", TwoInOneGrill.ID);
            ModUtil.AddBuildingToPlanScreen("Refining", WastewaterSterilizer.ID);
            ModUtil.AddBuildingToPlanScreen("Base", MagicTile.ID);
            ModUtil.AddBuildingToPlanScreen(TAB_OXYGEN, OrganicDeoxidizer.ID);
            ModUtil.AddBuildingToPlanScreen("Utilities", H2OSynthesizer.ID);
            ModUtil.AddBuildingToPlanScreen(TAB_OXYGEN, CO2DecomposerEZ.ID);

            Merge_SUPERSUIT.CONTINUE_PATCH.OnLoad();
        }
    }
}
