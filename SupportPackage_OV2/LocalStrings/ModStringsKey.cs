using SuperComicLib.ModONI;
using System;

namespace SupportPackage.LocalStrings
{
    public readonly struct ModStringsKey : IModLocalizationKey
    {
        private static readonly Guid MY_UUID = Guid.Parse("94772e3f-836e-4560-8449-1b0b0ae8bce9");

        public Guid UUID => MY_UUID;
    }
}
