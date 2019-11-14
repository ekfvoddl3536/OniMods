using System;
using System.IO;
using System.Text;
using Unity.Collections;

namespace AsynchronousAutosave
{
    public static class Extension
    {
        private const int isize = sizeof(int);
        private const int usize = sizeof(long);

        public static NativeArray<char> ToNativeArray(this string str) => new NativeArray<char>(str.ToCharArray(), Allocator.Persistent);

        public static string GetString(this NativeArray<char> vs) => new string(vs.ToArray());

        public static void WriteString(this MemoryStream ms, string str) => WriteString(ms, str, Encoding.UTF8);

        public static void WriteString(this MemoryStream ms, string str, Encoding encoding)
        {
            byte[] arr = encoding.GetBytes(str);
            ms.Write(BitConverter.GetBytes(arr.Length), 0, isize);
            ms.Write(arr, 0, arr.Length);
        }

        public static void WriteInt32(this MemoryStream ms, int val) => ms.Write(BitConverter.GetBytes(val), 0, isize);

        public static void WriteBytes(this MemoryStream ms, byte[] vs)
        {
            ms.Write(BitConverter.GetBytes(vs.Length), 0, isize);
            ms.Write(vs, 0, vs.Length);
        }

        public static void WriteBoolean(this MemoryStream ms, bool val) => ms.WriteByte(val ? byte.MaxValue : byte.MinValue);

        public static void WriteInt64(this MemoryStream ms, long val) => ms.Write(BitConverter.GetBytes(val), 0, usize);

        public static byte[] GetBytesSMART(this IReader reader) => reader.ReadBytes(BitConverter.ToInt32(reader.ReadBytes(isize), 0));

        public static int GetInt32SMART(this IReader reader) => BitConverter.ToInt32(reader.ReadBytes(isize), 0);

        public static long GetInt64SMART(this IReader reader) => BitConverter.ToInt64(reader.ReadBytes(usize), 0);

        public unsafe static string[] GetStringArray(this IReader reader)
        {
            int alen = BitConverter.ToInt32(reader.ReadBytes(isize), 0);
            string[] temp = new string[alen];
            for (int x = 0; x < alen; x++)
                temp[x] = reader.ReadKleiString();
            return temp;
        }

        public unsafe static void WriteStringArray(this MemoryStream ms, string[] arr)
        {
            ms.Write(BitConverter.GetBytes(arr.Length), 0, 4);
            foreach (string i in arr)
                WriteString(ms, i);
        }

        public static void GetHeader(this IReader reader, out SaveGame.GameInfo gameInfo, out SaveGame.Header header)
        {
            header = new SaveGame.Header();
            header.buildVersion = reader.ReadUInt32();
#if boost
            header.headerSize = reader.ReadInt32();
            header.headerVersion = reader.ReadUInt32();
            if (1U <= header.headerVersion)
                header.compression = reader.ReadInt32();
#endif

            gameInfo = new SaveGame.GameInfo();
            gameInfo.numberOfCycles = BitConverter.ToInt32(reader.ReadBytes(isize), 0);
            gameInfo.numberOfDuplicants = BitConverter.ToInt32(reader.ReadBytes(isize), 0);
            gameInfo.baseName = reader.ReadKleiString();
            gameInfo.isAutoSave = reader.ReadByte() == byte.MaxValue;
            gameInfo.originalSaveName = reader.ReadKleiString();
            gameInfo.saveMajorVersion = BitConverter.ToInt32(reader.ReadBytes(isize), 0);
            gameInfo.saveMinorVersion = BitConverter.ToInt32(reader.ReadBytes(isize), 0);
            gameInfo.worldID = reader.ReadKleiString();
            gameInfo.worldTraits = GetStringArray(reader);
            gameInfo.sandboxEnabled = reader.ReadByte() == byte.MaxValue;
        }
    }
}
