#region LICENSE
/*
MIT License

Copyright (c) 2022. Super Comic (ekfvoddl3535@naver.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion
using HarmonyLib;
using SuperComicLib.ModONI;

namespace AdvancedGenerators.Patchs
{
    using static GlobalConsts;
    [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
    public static class GeneratedBuildings_LoadGeneratedBuildings_Patch_OV2
    {
        public static void Postfix()
        {
            var list = new StringsKeyList(12);

            new LocalStrings.KoreanDef().Apply(
                list
                    .ADD_NAME_DESC_EFFECT(RefinedCarbonGenerator.ID)
                    .ADD_NAME_DESC_EFFECT(ThermoelectricGenerator.ID)
                    .ADD_NAME_DESC_EFFECT(NaphthaGenerator.ID)
                    .ADD_NAME_DESC_EFFECT(EcoFriendlyMethaneGenerator.ID)
                    .Buffer
                );

            ModUtil.AddBuildingToPlanScreen(TAB_CATEGORY, RefinedCarbonGenerator.ID);
            ModUtil.AddBuildingToPlanScreen(TAB_CATEGORY, ThermoelectricGenerator.ID);
            ModUtil.AddBuildingToPlanScreen(TAB_CATEGORY, NaphthaGenerator.ID);
            ModUtil.AddBuildingToPlanScreen(TAB_CATEGORY, EcoFriendlyMethaneGenerator.ID);

            ModTechs.Insert("AdvancedPowerRegulation", RefinedCarbonGenerator.ID);
            ModTechs.Insert("Plastics", NaphthaGenerator.ID);
            ModTechs.Insert("RenewableEnergy", ThermoelectricGenerator.ID);
            ModTechs.Insert("ImprovedCombustion", EcoFriendlyMethaneGenerator.ID);
        }
    }
}
