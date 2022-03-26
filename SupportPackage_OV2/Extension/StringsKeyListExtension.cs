using SuperComicLib.ModONI;

namespace SupportPackage
{
    internal static class StringsKeyListExtension
    {
        public static StringsKeyList ADD_EQUIPMENT_K4(this StringsKeyList value, string equipment_id)
        {
            const string KPATH = "STRINGS.EQUIPMENT.PREFABS.";

            var buf = value.Buffer;
            var x = value.count_;

            string path = KPATH + equipment_id.ToUpper();

            // NAME, DESC, EFFECT, RECIPE
            buf[x++] = $"{path}.NAME";
            buf[x++] = $"{path}.DESC";
            buf[x++] = $"{path}.EFFECT";

            value.count_ = x;

            return value;
        }
    }
}
