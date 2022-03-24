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
using FastWirelessAutomation.Networks;
using HarmonyLib;
using SuperComicLib.ModONI;

namespace FastWirelessAutomation
{
    using static GlobalConsts;
    [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
    public static class Db_Initialize_Patch_OV2
    {
        public static void Postfix()
        {
            var list = new StringsKeyList(8);

            new LocalStrings.KoreanDef().Apply(
                list
                    .AddItem(TOOLTIP_KEY)
                    .AddItem(TITLE_KEY)
                    .ADD_NAME_DESC_EFFECT(FastWirelessEmitter.ID)
                    .ADD_NAME_DESC_EFFECT(FastWirelessReceiver.ID)
                    .Buffer
                );

            const string _TAB_ = "Automation";
            ModUtil.AddBuildingToPlanScreen(_TAB_, FastWirelessEmitter.ID);
            ModUtil.AddBuildingToPlanScreen(_TAB_, FastWirelessReceiver.ID);

            const string _TECH_ID_ = "DupeTrafficControl";
            ModTechs.Insert(_TECH_ID_, FastWirelessEmitter.ID);
            ModTechs.Insert(_TECH_ID_, FastWirelessReceiver.ID);
        }
    }

    [HarmonyPatch(typeof(Game), "OnSpawn")]
    public static class Game_OnSpawn_Patch_OV2
    {
        public static void Postfix() => ChannelManager.Reset();
    }
}
