#pragma warning disable IDE1006 // 명명 스타일
namespace SupportPackage
{
    internal static class StringExtension
    {
        private const string KPATH = "STRINGS.EQUIPMENT.PREFABS.";

        public static string equipmentName(this string id) =>
            Strings.Get($"{KPATH}{id.ToUpper()}.NAME");

        public static string equipmentEffect(this string id) =>
            Strings.Get($"{KPATH}{id.ToUpper()}.EFFECT");
    }
}
