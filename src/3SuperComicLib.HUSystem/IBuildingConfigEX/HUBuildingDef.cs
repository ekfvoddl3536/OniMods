using System;

namespace SuperComicLib.HUSystem
{
    [Flags]
    public enum HUPipeTypes : byte
    {
        None, InputOnly, OutputOnly, InOutput
    }

    public class HUBuildingDef : BuildingDef
    {
        public HUPipeTypes hutypes;
        public CellOffset huInputOffset, huOutputOffset;
    }
}
