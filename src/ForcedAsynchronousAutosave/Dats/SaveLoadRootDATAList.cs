using KSerialization;
using SuperComicLib.Threading;
using System.Collections.Generic;
using System.IO;

namespace AsynchronousAutosave
{
    public sealed class SaveLoadRootDATAList
    {
        public Dictionary<string, List<SaveLoadRootDATA>> cdts_0 = new Dictionary<string, List<SaveLoadRootDATA>>();
        public Dictionary<string, List<SaveLoadRootDATA>> cdts_null = new Dictionary<string, List<SaveLoadRootDATA>>();

        public unsafe void Add(bool isnull, string tagname, SaveLoadRootDATA data)
        {
            Dictionary<string, List<SaveLoadRootDATA>> temp = isnull ? cdts_null : cdts_0;
            if (!temp.ContainsKey(tagname))
                temp.Add(tagname, new List<SaveLoadRootDATA>());
            temp[tagname].Add(data);
        }

        public void WriteAuto(AwaitibleAsyncTask.Instance instance, BinaryWriter wr, ref uint count)
        {
            foreach (KeyValuePair<string, List<SaveLoadRootDATA>> kv in cdts_0)
                WriteArray(instance, wr, kv.Key, kv.Value, ref count);
            foreach (KeyValuePair<string, List<SaveLoadRootDATA>> kv in cdts_null)
                WriteArray(instance, wr, kv.Key, kv.Value, ref count);

            cdts_0.Clear();
            cdts_null.Clear();

            cdts_0 = null;
            cdts_null = null;
        }

        public void WriteArray(AwaitibleAsyncTask.Instance instance, BinaryWriter wr, string key, List<SaveLoadRootDATA> val, ref uint count)
        {
            wr.WriteKleiString(key);
            // wr.Write(val.Count);
            long pos1 = wr.BaseStream.Position;
            wr.Write(uint.MinValue);
            uint writed = 0;
            wr.WriteLengthWithAction(tmp =>
            {
                foreach (SaveLoadRootDATA d in val)
                    d.TWrite(instance, tmp, ref writed);
            });

            long posEnd = wr.BaseStream.Position;
            wr.BaseStream.Position = pos1;
            wr.Write(writed);
            wr.BaseStream.Position = posEnd;

            if (writed > 0)
                count++;
        }
    }
}
