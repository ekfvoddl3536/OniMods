using System.IO;
using System.Reflection;
using Harmony;
using SuperComicLib.Oni;
using SuperComicLib.Sandfile;

namespace MoreSpecialGeysers.patch
{
    using static MSG_CONST;

    [HarmonyPatch(typeof(EntityConfigManager), nameof(EntityConfigManager.LoadGeneratedEntities))]
    public static class EntityConfMgrPatch
    {
        public static void Prefix()
        {
            string dname = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string p1 = Path.Combine(dname, "textpatch.txt");
            if (File.Exists(p1))
                UpdateString(p1);
            else
                DefaultString();

            CustomGeysers.Load(Path.Combine(dname, "custom_geysers"));
        }

        private static void UpdateString(string path)
        {
            using (SandfileScanner sp = new SandfileScanner(path))
            {
                sp.Start();
                SetString(sp, TUNGSTEN.ID_UPPER, TUNGSTEN.NAME, TUNGSTEN.DESC);
                SetString(sp, COOLWATER.ID_UPPER, COOLWATER.NAME, COOLWATER.DESC);
                SetString(sp, LIQHYDROGEN.ID_UPPER, LIQHYDROGEN.NAME, LIQHYDROGEN.DESC);
                SetString(sp, COOLGASOXYGEN.ID_UPPER, COOLGASOXYGEN.NAME, COOLGASOXYGEN.DESC);
            }
        }

        private static void DefaultString()
        {
            StringLoader.Geyser(typeof(TUNGSTEN));
            StringLoader.Geyser(typeof(COOLWATER));
            StringLoader.Geyser(typeof(LIQHYDROGEN));
            StringLoader.Geyser(typeof(COOLGASOXYGEN));
        }

        private static void SetString(SandfileScanner sp, string id_upper, string def_name, string def_desc) => 
            StringLoader.Set(
                StringLoader.defaultGeyser,
                id_upper,
                sp.GetStringValue(id_upper + ".NAME") ?? def_name,
                sp.GetStringValue(id_upper + ".DESC") ?? def_desc);
    }
}
