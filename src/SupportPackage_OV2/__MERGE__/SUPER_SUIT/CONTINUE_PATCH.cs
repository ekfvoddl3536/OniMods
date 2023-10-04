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
