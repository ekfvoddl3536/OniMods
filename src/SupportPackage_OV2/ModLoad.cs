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
using SuperComicLib.ModONI;

namespace SupportPackage
{
    using static GlobalConsts;
    using static GlobalConsts.SUPER_SUIT;
    public sealed class ModLoad : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            RegisterStrings();
            RegisterBuildings();

            Merge_SUPERSUIT.CONTINUE_PATCH.OnLoad();

            InOneRefrigeratorController.StatusItemInit();

            harmony.PatchAll();
        }

        private static void RegisterStrings()
        {
            var list = new StringKeyList(FixedCapacity.SZ_64);

            new LocalStrings.KoreanDef().Apply(
                list
                    .ADD_BUILDINGS(EasyElectrolyzer.ID)
                    .ADD_BUILDINGS(TwoInOneGrill.ID)
                    .ADD_BUILDINGS(WastewaterSterilizer.ID)
                    .ADD_BUILDINGS(MagicTile.ID)
                    .ADD_BUILDINGS(OrganicDeoxidizer.ID)
                    .ADD_BUILDINGS(H2OSynthesizer.ID)
                    .ADD_BUILDINGS(CO2DecomposerEZ.ID)
                    .ADD_EQUIPMENT(Basic_SuperSuit.ID)
                    .ADD_EQUIPMENT(Advance_SuperSuit.ID)
                    .ADD_EQUIPMENT(SmallBackpack.ID)
                    .ADD_EQUIPMENT(MediumBackpack.ID)
                    .ADD_EQUIPMENT(LargeBackpack.ID)
                );
        }

        private static void RegisterBuildings()
        {
            const string TAB_OXYGEN = "Oxygen";

            ModUtil.AddBuildingToPlanScreen(TAB_OXYGEN, EasyElectrolyzer.ID);
            ModUtil.AddBuildingToPlanScreen("Food", TwoInOneGrill.ID);
            ModUtil.AddBuildingToPlanScreen("Refining", WastewaterSterilizer.ID);
            ModUtil.AddBuildingToPlanScreen("Base", MagicTile.ID);
            ModUtil.AddBuildingToPlanScreen(TAB_OXYGEN, OrganicDeoxidizer.ID);
            ModUtil.AddBuildingToPlanScreen("Utilities", H2OSynthesizer.ID);
            ModUtil.AddBuildingToPlanScreen(TAB_OXYGEN, CO2DecomposerEZ.ID);
        }
    }
}
