using SuperComicLib.Oni;
using System.Collections.Generic;
using System.IO;

namespace MoreSpecialGeysers
{
    internal static class CustomGeysers
    {
        public static List<GeyserSandfileScanner> scanners = new List<GeyserSandfileScanner>();

        public static void Load(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                return;
            }

            Load(Directory.EnumerateFiles(directory, "*.sndf", SearchOption.AllDirectories), scanners);
            Load(Directory.EnumerateFiles(directory, "*.txt", SearchOption.AllDirectories), scanners);
        }

        private static void Load(IEnumerable<string> files, List<GeyserSandfileScanner> list)
        {
            IEnumerator<string> e1 = files.GetEnumerator();

            while (e1.MoveNext())
            {
                GeyserSandfileScanner val = GeyserSandfileScanner.Load(e1.Current);
                SimHashes sh = val.Element;

                string name = val.GetStringValue(IDList.Name);
                string desc = val.GetStringValue(IDList.Desc);
                if (sh == 0 ||
                    sh == SimHashes.Void || 
                    sh == SimHashes.Vacuum || 
                    val.GetStringValue(IDList.Anistr) == null ||
                    name == null ||
                    desc == null)
                {
                    Debug.LogWarning("Invalid Geyser Config: " + e1.Current);
                    continue;
                }

                list.Add(val);
                StringLoader.Set(StringLoader.defaultGeyser, val.ID.ToUpper(), name, desc);
            }

            e1.Dispose();
        }

        public static void GetList(List<GeyserGenericConfig.GeyserPrefabParams> list)
        {
            ref List<GeyserSandfileScanner> scanners = ref CustomGeysers.scanners;
            if (scanners == null)
                return;

            for (int x = 0, max = scanners.Count; x < max; x++)
            {
                GeyserSandfileScanner ps = scanners[x];
                list.Add(ps.Create());
                ps.Dispose();
            }

            scanners.Clear();
            scanners = null;
        }
    }
}
