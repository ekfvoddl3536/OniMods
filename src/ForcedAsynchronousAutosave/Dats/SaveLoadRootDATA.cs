using KSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AsynchronousAutosave
{
    public sealed partial class SaveLoadRootDATA
    {
        public string tagname;
        public Vector3 pos;
        public Quaternion rotation;
        public Vector3 scale;
        public KMonoBehaviour[] compos;
        // https://overworks.github.io/unity/2019/07/16/null-of-unity-object.html
        // null일 가능성은 없는듯 함
        public GameObject go;

        public void SafeTWrite(BinaryWriter writer)
        {
            writer.Write(pos);
            writer.Write(rotation);
            writer.Write(scale);
            writer.Write(byte.MinValue);
            if (compos == null)
                return;
            long pos1 = writer.BaseStream.Position;
            writer.Write(uint.MaxValue);
            uint count = 0;
            foreach (KMonoBehaviour kmb in compos)
                if (kmb != null)
                {
                    Type t = kmb.GetType();
                    if (!t.IsDefined(typeof(SkipSaveFileSerialization), false))
                    {
                        writer.WriteKleiString(t.ToString());
                        writer.WriteLengthWithAction(tmp =>
                        {
                            if (kmb is ISaveLoadableDetails sld)
                            {
                                // SerializeObject(tmp, kmb);
                                Serializer.SerializeTypeless(kmb, writer);
                                sld.Serialize(writer);
                            }
                            else
                                Serializer.SerializeTypeless(kmb, writer);
                        });
                        count++;
                    }
                }

            foreach (KeyValuePair<string, ISerializableComponentManager> kv in AsyncAutosaveCS.serializableComs)
            {
                ISerializableComponentManager v = kv.Value;
                if (v.Has(go))
                {
                    writer.WriteKleiString(kv.Key);
                    v.Serialize(go, writer);
                    count++;
                }
            }

            long pos3 = writer.BaseStream.Position;
            writer.BaseStream.Position = pos1;
            writer.Write(count);
            writer.BaseStream.Position = pos3;
        }
    }
}
