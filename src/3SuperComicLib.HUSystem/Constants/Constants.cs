namespace SuperComicLib.HUSystem
{
    public static class Constants
    {
        public const int layer = (int)ObjectLayer.Backwall;
        public static UtilityNetworkManager<HUNetwork, HUPipe> hupipeSystem;
        public static HUPipeManager manager;

        public static readonly Operational.Flag connectedFlag = new Operational.Flag("HUPipeConnected", Operational.Flag.Type.Requirement);
    }
}
