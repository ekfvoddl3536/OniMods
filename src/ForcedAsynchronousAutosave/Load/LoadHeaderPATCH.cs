using Harmony;
using System.IO;

namespace AsynchronousAutosave
{
    [HarmonyPatch(typeof(SaveLoader), "LoadHeader")]
    public class LoadHeaderPATCH
    {
        public static bool Prefix(ref SaveGame.GameInfo __result, string filename, out SaveGame.Header header)
        {
            new FastReader(File.ReadAllBytes(filename)).GetHeader(out __result, out header);
            return false;
        }
    }
}
