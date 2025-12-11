// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;
using re_ = ComplexRecipe.RecipeElement;

namespace SupportPackage;

using static ComplexRecipeManager;
using static GlobalConsts.SUPER_SUIT;
[HarmonyPatch(typeof(ClothingFabricatorConfig), "ConfigureRecipes")]
public static class SUPERSUIT_PATCH_01_V1
{
    public static void Postfix()
    {
        var me = new Tag(ClothingFabricatorConfig.ID);
        var bf = new Tag(BASICFABRIC_ID);

        Add(Basic_SuperSuit.CLOTH_MASS, Basic_SuperSuit.ID, Basic_SuperSuit.FAB_TIME, ref me, ref bf);
        Add(Advance_SuperSuit.CLOTH_MASS, Advance_SuperSuit.ID, Advance_SuperSuit.FAB_TIME, ref me, ref bf);

        Add(SmallBackpack.CLOTH_MASS, SmallBackpack.ID, SmallBackpack.FAB_TIME, ref me, ref bf);
        Add(MediumBackpack.CLOTH_MASS, MediumBackpack.ID, MediumBackpack.FAB_TIME, ref me, ref bf);
        Add(LargeBackpack.CLOTH_MASS, LargeBackpack.ID, LargeBackpack.FAB_TIME, ref me, ref bf);
    }

    private static void Add(int mass, string id, int fabTime, ref Tag me, ref Tag bf_tag)
    {
        re_[] inp = [new re_(bf_tag, mass)];
        re_[] outp = [new re_(new Tag(id), 1f, 0)];

        new ComplexRecipe(MakeRecipeID(ClothingFabricatorConfig.ID, inp, outp), inp, outp)
        {
            time = fabTime,
            description = id.equipmentEffect(), // effect 와 recipe의 string이 같다
            nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
            fabricators = [me],
            sortOrder = 2,
        };
    }
}
