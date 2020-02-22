#if savemgrSave
using Harmony;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using KSerialization;

namespace AsynchronousAutosave
{
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.Save))]
    public class SaveManagerSAVEPATCH
    {
        private static char[] save_header;
        private static FieldInfo sceneObjects;

        public static bool Prepare()
        {
            save_header = "KSAV".ToCharArray(); // Check SaveManager.SAVE_HEADER
            sceneObjects = typeof(SaveManager).GetField("sceneObjects", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            return true;
        }

        private static volatile int num;

        public static bool Prefix(SaveManager __instance, BinaryWriter writer)
        {
            Dictionary<Tag, List<SaveLoadRoot>> objs = (Dictionary<Tag, List<SaveLoadRoot>>)sceneObjects.GetValue(__instance);
            writer.Write(save_header);
            writer.Write(SaveManager.SAVE_MAJOR_VERSION);
            writer.Write(SaveManager.SAVE_MINOR_VERSION);
            num = 0;
            Parallel.ForEach(objs.Values, a =>
            {
                if (a.Count > 0)
                    num++;
            });
            writer.Write(num);
            HashSet<Tag> tempkeys = new HashSet<Tag>(objs.Keys);
            tempkeys.Remove(SaveGame.Instance.PrefabID());

            IOrderedEnumerable<Tag> tree = tempkeys.OrderBy(x => x.Name.Contains("UnderConstruction"));
            TWrite(SaveGame.Instance.PrefabID(), new List<SaveLoadRoot> { SaveGame.Instance.GetComponent<SaveLoadRoot>() }, writer);

            // simcell 머시기가 null이 아닐때
            foreach (Tag t in tree)
            {
                List<SaveLoadRoot> co = objs[t];
                if (co.Count > 0 && co.Find(x => x != null && x.GetComponent<SimCellOccupier>() != null) is SaveLoadRoot root)
                {
                    TWrite(t, co, writer);
                    break;
                }
            }

            // simcell 머시기가 null일때
            // 순서 상관있을까 싶다가도, 상관 있을거라 생각됨 (서.순)
            foreach (Tag t in tree)
            {
                List<SaveLoadRoot> co = objs[t];
                if (co.Count > 0 && co.Find(x => x != null && x.GetComponent<SimCellOccupier>() == null) is SaveLoadRoot root)
                {
                    TWrite(t, co, writer);
                    break;
                }
            }

            tempkeys.Clear();
            tempkeys = null;
            return false;
        }

        private static void TWrite(Tag key, List<SaveLoadRoot> roots, BinaryWriter writer)
        {
            writer.WriteKleiString(key.Name);
            writer.Write(roots.Count);
            writer.Write(0);
            long pos1 = writer.BaseStream.Position;
            foreach (SaveLoadRoot root in roots)
                if (root != null)
                    root.Save(writer);
                else
                    Debug.LogWarning("Null game object when saving");
            long pos2 = writer.BaseStream.Position;
            writer.BaseStream.Position = pos1;
            writer.Write((int)(pos2 - pos1));
            writer.BaseStream.Position = pos2;
        }
    }
}
#endif
