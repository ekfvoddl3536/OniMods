using System.Collections.Generic;

namespace LazySaver
{
    public sealed class P1SaveFileRoot
    {
        public int WidthInCells;
        public int HeightInCells;
        public Dictionary<string, byte[]> streamed;
        public string clusterID;
        public List<ModInfo> requiredMods;
        public List<KMod.Label> active_mods;
    }
}
