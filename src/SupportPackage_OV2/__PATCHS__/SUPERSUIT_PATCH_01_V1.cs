// MIT License
//
// Copyright (c) 2022-2023. Super Comic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using HarmonyLib;
using System.Collections.Generic;
using re_ = ComplexRecipe.RecipeElement;

namespace SupportPackage
{
    using static GlobalConsts.SUPER_SUIT;
    using static ComplexRecipeManager;
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
            re_[] inp = new[] { new re_(bf_tag, mass) };
            re_[] outp = new[] { new re_(new Tag(id), 1f, 0) };

            new ComplexRecipe(MakeRecipeID(ClothingFabricatorConfig.ID, inp, outp), inp, outp)
            {
                time = fabTime,
                description = id.equipmentEffect(), // effect 와 recipe의 string이 같다
                nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
                fabricators = new List<Tag>(1) { me },
                sortOrder = 2, 
            };
        }
    }
}
