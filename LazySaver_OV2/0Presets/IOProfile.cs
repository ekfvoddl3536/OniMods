namespace LazySaver.Presets
{
    public readonly struct IOProfile
    {
        public readonly long InitialSize;
        public readonly long IncreaseSize;

        public IOProfile(long init, long inc)
        {
            InitialSize = init;
            IncreaseSize = inc;
        }
    }
}
