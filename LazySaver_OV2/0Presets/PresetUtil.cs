namespace LazySaver.Presets
{
    internal static class PresetUtil
    {
        private const long
            INIT_MD = 0x800_0000,
            INIT_HI = 0x2000_0000,
            INIT_UL = 0x4000_0000;

        private const long
            INC_LO = 0x100_0000,
            INC_MD = INC_LO << 1,
            INC_HI = INC_LO << 2,
            INC_UL = INC_LO << 3;

        private static readonly IOProfile[] values =
        {
            new IOProfile(INIT_MD, INC_LO), // mid + lo

            new IOProfile(INIT_MD, INC_MD), // mid + mid
            new IOProfile(INIT_MD, INC_HI), // mid + high

            new IOProfile(INIT_HI, INC_MD), // high + mid
            new IOProfile(INIT_HI, INC_HI), // high + high

            new IOProfile(INIT_UL, INC_HI), // ultra + high
            new IOProfile(INIT_UL, INC_UL)  // ultra + ultra
        };

        public static IOProfile GetIOProfile(ProfilePreset v) => values[(int)v];
    }
}
