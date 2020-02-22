using KSerialization;
using SuperComicLib.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AsynchronousAutosave
{
    partial class SaveLoadRootDATA
    {
        public SaveLoadRootDATA(Vector3 pos, Quaternion rotation, Vector3 localScale, KMonoBehaviour[] objs, GameObject go)
        {
            this.pos = pos;
            this.rotation = rotation;
            scale = localScale;
            compos = objs;
            this.go = go;
        }

        public void TWrite(AwaitibleAsyncTask.Instance instance, BinaryWriter writer, ref uint writed)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bwr = new BinaryWriter(ms))
            {
                if (UnsafeTWrite_Internal(instance, bwr))
                {
                    writed++;
                    writer.Write(ms.ToArray());
                }
            }
        }

        private bool UnsafeTWrite_Internal(AwaitibleAsyncTask.Instance instance, BinaryWriter writer)
        {
            writer.Write(pos);
            writer.Write(rotation);
            writer.Write(scale);
            writer.Write(byte.MinValue);
            if (compos == null || go.Equals(null))
                return false;
            long pos1 = writer.BaseStream.Position;
            writer.Write(uint.MinValue);
            uint count = 0;

            // Debug.Log("AL1");
            bool flag = false;
            instance.Invoke(() =>
            {
                foreach (KMonoBehaviour kmb in compos)
                    if (!kmb.Equals(null) /*&& !kmb.gameObject*/)
                    {
                        Type t = kmb.GetType();
                        if (!t.IsDefined(typeof(SkipSaveFileSerialization), false))
                        {
                            writer.WriteKleiString(t.ToString());
                            writer.WriteLengthWithAction(tmp =>
                            {
                                if (kmb is ISaveLoadableDetails sld)
                                {
                                    Serializer.SerializeTypeless(kmb, tmp);
                                    sld.Serialize(writer);
                                }
                                else
                                    Serializer.SerializeTypeless(kmb, tmp); // 잘 됨
                                                                            // SerializeTypeless(instance, t, kmb, writer);
                            });
                            count++;
                        }
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
            }).Wait();
            if (flag)
                return false;
            // Debug.Log("AL1 END");

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
            return true;
        }
    }
}
