namespace SuperComicLib.HUSystem
{
    public static class ObjdctEXPlus
    {
        // public static bool HasFlag(this HUEndpointType target, HUEndpointType val) => (target & val) != 0;
        public static int GetRotatedOffsetCell(this Building bd, CellOffset cell) =>
            Grid.OffsetCell(Grid.PosToCell(bd.transform.GetPosition()), cell);

        public static bool HasFlag(this UtilityConnections target, UtilityConnections val) => (target & val) != 0;
    }
}
