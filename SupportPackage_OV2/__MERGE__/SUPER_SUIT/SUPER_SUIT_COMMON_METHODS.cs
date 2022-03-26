namespace SupportPackage
{
    using static STRINGS.DUPLICANTS.ATTRIBUTES;
    using static GlobalConsts.SUPER_SUIT;
    public static class SUPER_SUIT_COMMON_METHODS
    {
        public static string Desc_thermal =
            $"{THERMALCONDUCTIVITYBARRIER.NAME}: {GameUtil.GetFormattedDistance(TEMP_DEF)}";

        public static string DESC_DECOR(int i) =>
            $"{DECOR.NAME}: {i}";
    }
}
