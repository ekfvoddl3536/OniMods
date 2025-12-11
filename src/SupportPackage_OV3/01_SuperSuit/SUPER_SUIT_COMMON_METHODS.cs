// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace SupportPackage;

using static GlobalConsts.SUPER_SUIT;
using static STRINGS.DUPLICANTS.ATTRIBUTES;
public static class SUPER_SUIT_COMMON_METHODS
{
    public static string Desc_thermal =
        $"{THERMALCONDUCTIVITYBARRIER.NAME}: {GameUtil.GetFormattedDistance(TEMP_DEF)}";

    public static string DESC_DECOR(int i) =>
        $"{DECOR.NAME}: {i}";
}
