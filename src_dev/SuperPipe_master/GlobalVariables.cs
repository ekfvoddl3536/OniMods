using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPipe
{
    internal static class GlobalConstants
    {
        public const int layer = (int)ObjectLayer.Backwall;

        public static readonly Operational.Flag connectedFlag =
            new Operational.Flag("SPUtilityConnected", Operational.Flag.Type.Requirement);
    }

    internal static class GlobalVariables
    {
        public static UtilityNetworkManager<>
    }
}
