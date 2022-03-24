using SuperComicLib.ModONI;
using System;

namespace FastWirelessAutomation.LocalStrings
{
    public readonly struct ModStringsKey : IModLocalizationKey
    {
        private static readonly Guid MY_UUID = Guid.Parse("cbe181c0-ab80-11ec-a11b-bd4026f8440a");

        public Guid UUID => MY_UUID;
    }
}
