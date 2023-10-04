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

namespace EcoFriendlyToilet
{
    using static GlobalConsts;
    public sealed class ModLoad : KMod.UserMod2
    {
        public override void OnLoad(Harmony _)
        {
            var list = new StringKeyList(FixedCapacity.SZ_32);

            new LocalStrings.KoreanDef().Apply(
                list
                    .Add($"STRINGS.BUILDING.STATUSITEMS.{EcoFriendlyToilet.ID}.NAME")
                    .Add($"STRINGS.BUILDING.STATUSITEMS.{EcoFriendlyToilet.ID}.TOOLTIP")
                    .ADD_BUILDINGS(EcoFriendlyToilet.ID)
                    .ADD_BUILDINGS(AutoPurifyWashsink.ID)
                    .ADD_BUILDINGS(EcoFriendlyWildFarmTile.ID)
                    .ADD_BUILDINGS(EcoFriendlyWildFlowerPot.ID)
                    .ADD_BUILDINGS(EcoFriendlyWildHangingPot.ID)
                );

            GlobalVars.FerzToilet_ =
                new StatusItem(
                    EcoFriendlyToilet.ID,
                    "BUILDING",
                    string.Empty,
                    StatusItem.IconType.Info,
                    NotificationType.Neutral,
                    false,
                    OverlayModes.None.ID)
                {
                    resolveStringCallback = STATIC_CALLBACK
                };

            ModUtil.AddBuildingToPlanScreen("Plumbing", EcoFriendlyToilet.ID);
            ModUtil.AddBuildingToPlanScreen("Medical", AutoPurifyWashsink.ID);
            ModUtil.AddBuildingToPlanScreen("Food", EcoFriendlyWildFarmTile.ID);
            ModUtil.AddBuildingToPlanScreen("Furniture", EcoFriendlyWildFlowerPot.ID);
            ModUtil.AddBuildingToPlanScreen("Furniture", EcoFriendlyWildHangingPot.ID);
        }

        private static string STATIC_CALLBACK(string data, object arg) =>
            arg is FerzToilet.StatesInstance smi
            ? data.Replace("{FlushesRemaining}", (EcoFriendlyToilet.MAX_FLUSHES - smi.master._flushesUsed).ToString())
            : data;
    }
}