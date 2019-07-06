namespace SuperComicLib.Script
{
    public static class EnumEX
    {
        public static bool IsFlagged(this TriggerType tr, TriggerType other) => (tr & other) != 0;
    }
}
