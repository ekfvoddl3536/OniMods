using System;
using System.IO;
using System.Text;

namespace AsynchronousAutosave
{
    using static AsyncAutosaveCS;
    public static class Extension
    {
        public static void WriteBytes(this MemoryStream ms, byte[] vs)
        {
            ms.Write(BitConverter.GetBytes(vs.Length), 0, isize);
            ms.Write(vs, 0, vs.Length);
        }

        public static void WriteString(this MemoryStream ms, string str) => ms.WriteBytes(Encoding.UTF8.GetBytes(str));

        public static void WriteBool(this MemoryStream ms, bool val) => ms.WriteByte(val ? byte.MaxValue : byte.MinValue);

        public static void WriteInt32(this MemoryStream ms, int val) => ms.Write(BitConverter.GetBytes(val), 0, isize);

        public static void WriteInt64(this MemoryStream ms, long val) => ms.Write(BitConverter.GetBytes(val), 0, usize);

        public static byte[] GetBytes(this IReader reader) => reader.ReadBytes(reader.ReadInt32());

        public static string GetString(this IReader reader) => Encoding.UTF8.GetString(reader.GetBytes());

        public unsafe static string[] GetStringArray(this IReader reader)
        {
            int alen = reader.ReadInt32();
            string[] temp = new string[alen];
            for (int x = 0; x < alen; x++)
                temp[x] = reader.GetString();
            return temp;
        }

        public unsafe static void WriteStringArray(this MemoryStream ms, string[] arr)
        {
            ms.WriteInt32(arr.Length);
            foreach (string i in arr)
                ms.WriteString(i);
        }

        public static void GetHeader(this IReader reader, out SaveGame.GameInfo gameInfo, out SaveGame.Header header)
        {
            header = new SaveGame.Header();
            header.buildVersion = reader.ReadUInt32();
            header.headerSize = reader.ReadInt32();
            header.headerVersion = reader.ReadUInt32();
            if (1U <= header.headerVersion)
                header.compression = reader.ReadInt32();

            gameInfo = new SaveGame.GameInfo();
            gameInfo.numberOfCycles = reader.ReadInt32();
            gameInfo.numberOfDuplicants = reader.ReadInt32();
            gameInfo.baseName = reader.GetString();
            gameInfo.isAutoSave = reader.ReadByte() > 0;
            gameInfo.originalSaveName = reader.GetString();
            gameInfo.saveMajorVersion = reader.ReadInt32();
            gameInfo.saveMinorVersion = reader.ReadInt32();
            gameInfo.worldID = reader.GetString();
            gameInfo.worldTraits = reader.GetStringArray();
            gameInfo.sandboxEnabled = reader.ReadByte() > 0;
        }
    }
}
