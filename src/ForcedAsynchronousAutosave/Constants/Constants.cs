using System;
using System.Collections.Generic;
using System.IO;

namespace AsynchronousAutosave
{
    public static class AsyncAutosaveCS
    {
        public const int
            WORLDHEADER = 0x80,
            isize = sizeof(int),
            usize = sizeof(long);
        public const string NameContains = "UnderConstruction";
        public static Dictionary<string, ISerializableComponentManager> serializableComs;

        public const string MODLOG_HEADER = "===== Asynchronous Autosave // by SuperComic (ekfvoddl3535@naver.com) =====";
        public const string MODTHD_HEADER = "===== Asynchronous Autosave (Thread-Safe) // by SuperComic (ekfvoddl3535@naver.com) =====";
        private const long isize64 = isize;

        public static readonly char[] SAVE_HEADER = "KSAV".ToCharArray(); // SaveManager.SAVE_HEADER 

        public static void WriteLengthWithAction(this BinaryWriter writer, Action<BinaryWriter> action)
        {
            long pos1 = writer.BaseStream.Position; // 0
            writer.Write(uint.MinValue); // fill 4 bytes
            action.Invoke(writer); // invoke
            long pos3 = writer.BaseStream.Position; // 10
            writer.BaseStream.Position = pos1;
            writer.Write((uint)(pos3 - (pos1 + isize64)));
            writer.BaseStream.Position = pos3;
        }
    }
}
