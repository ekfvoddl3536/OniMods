// MIT License
//
// Copyright (c) 2022-2023. SuperComic (ekfvoddl3535@naver.com)
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

namespace AdvancedGenerators
{
    using static GlobalConsts;
    public sealed class ModLoad : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            var list = new StringKeyList(FixedCapacity.SZ_32);

            new LocalStrings.KoreanDef().Apply(
                list
                    .ADD_BUILDINGS(RefinedCarbonGenerator.ID)
                    .ADD_BUILDINGS(ThermoelectricGenerator.ID)
                    .ADD_BUILDINGS(NaphthaGenerator.ID)
                    .ADD_BUILDINGS(EcoFriendlyMethaneGenerator.ID)
                    .ADD_BUILDINGS(FusionPowerPlant.ID)
                    .ADD_BUILDINGS(ThermoconvertGenerator.ID)
                    .Add(ThermoconvertGenerator.SLIDER_KEY)
                    .Add(ThermoconvertGenerator.SLIDER_TOOLTIP_KEY)
                );

            ModUtil.AddBuildingToPlanScreen(TAB_CATEGORY, RefinedCarbonGenerator.ID);
            ModUtil.AddBuildingToPlanScreen(TAB_CATEGORY, ThermoelectricGenerator.ID);
            ModUtil.AddBuildingToPlanScreen(TAB_CATEGORY, NaphthaGenerator.ID);
            ModUtil.AddBuildingToPlanScreen(TAB_CATEGORY, EcoFriendlyMethaneGenerator.ID);
            ModUtil.AddBuildingToPlanScreen(TAB_CATEGORY, FusionPowerPlant.ID);
            ModUtil.AddBuildingToPlanScreen(TAB_CATEGORY, ThermoconvertGenerator.ID);

            harmony.PatchAll();
        }
    }
}