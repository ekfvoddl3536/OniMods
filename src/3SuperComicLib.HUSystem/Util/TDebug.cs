namespace SuperComicLib.HUSystem
{
    public static class TDebug
    {
#if TDEBUG
        public static void Log(object obj) => Debug.Log(obj);
#else
        public static void Log(object obj) { }
#endif
    }
}
