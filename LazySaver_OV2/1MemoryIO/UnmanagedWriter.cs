using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace LazySaver.MemoryIO
{
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class UnmanagedWriter : BinaryWriter
    {
        // caching
        internal UnmanagedStream m_stream;
        private Encoding m_encoding;
        private Encoder m_encoder;

        private byte* m_largeBuffer;
        private int m_largeBufferLength;
        private int m_maxChars;

        #region property
        public override Stream BaseStream => m_stream;
        #endregion

        #region constructor (method)
        private UnmanagedWriter Initialize(long initSize, long incSize, Encoding encoding)
        {
            UnityEngine.Debug.Assert(encoding != null);

            m_stream = new UnmanagedStream(initSize, incSize);

            m_encoding = encoding;
            m_encoder = encoding.GetEncoder();

            m_largeBuffer = null;
            m_largeBufferLength = 0;
            m_maxChars = 0;

            return this;
        }
        #endregion

        #region methods
        public override void Flush() { }
        public override void Close() => Dispose();
        protected override void Dispose(bool disposing)
        {
            m_stream.Dispose();

            if (m_largeBuffer != null)
                Marshal.FreeHGlobal((IntPtr)m_largeBuffer);

            if (disposing)
            {
                m_stream = null;
                m_largeBuffer = null;
            }
        }

        public override long Seek(int offset, SeekOrigin origin) => m_stream.Seek(offset, origin);

        #region write (simple)
        public override void Write(byte[] buffer) => m_stream.Write(buffer, 0, buffer.Length);
        public override void Write(byte[] buffer, int index, int count) => m_stream.Write(buffer, index, count);
        public override void Write(bool value) => m_stream.WriteByte((byte)(value ? 1u : 0u));
        public override void Write(byte value) => m_stream.WriteByte(value);
        public override void Write(sbyte value) => m_stream.WriteByte((byte)value);
        public override void Write(short value) => m_stream.Write((byte*)&value, sizeof(short));
        public override void Write(ushort value) => m_stream.Write((byte*)&value, sizeof(ushort));
        public override void Write(int value) => m_stream.Write((byte*)&value, sizeof(int));
        public override void Write(uint value) => m_stream.Write((byte*)&value, sizeof(uint));
        public override void Write(float value) => m_stream.Write((byte*)&value, sizeof(float));
        public override void Write(long value) => m_stream.Write((byte*)&value, sizeof(long));
        public override void Write(ulong value) => m_stream.Write((byte*)&value, sizeof(ulong));
        public override void Write(double value) => m_stream.Write((byte*)&value, sizeof(double));
        public override void Write(decimal value)
        {
            int[] vs = decimal.GetBits(value);
            fixed (int* p = &vs[0])
                m_stream.Write((byte*)p, 16);
        }
        #endregion

        #region write (complex)
        public override void Write(char ch)
        {
            if (char.IsSurrogate(ch)) throw new ArgumentException("Surrogate not allowed as single char");

            A16 _dummy;
            byte* buf = (byte*)&_dummy;

            int num = m_encoder.GetBytes(&ch, 1, buf, sizeof(A16), true);
            m_stream.Write(buf, num);
        }
        public override void Write(char[] chars) => Write(chars, 0, chars.Length);
        public override void Write(char[] chars, int index, int count)
        {
            byte[] bytes = m_encoding.GetBytes(chars, index, count);
            m_stream.Write(bytes, 0, bytes.Length);
        }
        public override void Write(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            int bCnt = m_encoding.GetByteCount(value);
            Write7BitEncodedInt(bCnt);

            if (m_largeBuffer == null)
            {
                m_largeBuffer = (byte*)Marshal.AllocHGlobal(m_largeBufferLength = 4096);
                m_maxChars = 4096 / m_encoding.GetMaxByteCount(1);
            }

            if (bCnt <= m_largeBufferLength)
            {
                fixed (char* p1 = value)
                    m_encoding.GetBytes(p1, value.Length, m_largeBuffer, m_largeBufferLength);

                m_stream.Write(m_largeBuffer, bCnt);
                return;
            }

            int charStart = 0;
            int numLeft = value.Length;

            fixed (char* pChars = value)
            {
                byte* pBytes = m_largeBuffer;
                int pBytesLen = m_largeBufferLength;

                while (numLeft > 0)
                {
                    int charCount = (numLeft > m_maxChars) ? m_maxChars : numLeft;
                    int byteLen;

                    if (charStart < 0 || charCount < 0 || checked(charStart + charCount) > value.Length)
                        throw new ArgumentOutOfRangeException("charCount");

                    byteLen = m_encoder.GetBytes(pChars + charStart, charCount, pBytes, pBytesLen, charCount == numLeft);

                    m_stream.Write(pBytes, byteLen);
                    charStart += charCount;
                    numLeft -= charCount;
                }
            }
        }
        #endregion
        #endregion

        #region create instance helper method
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnmanagedWriter Create(long initSize, long incSize, Encoding encoding) =>
            ((UnmanagedWriter)FormatterServices.GetUninitializedObject(typeof(UnmanagedWriter))).Initialize(initSize, incSize, encoding);
        #endregion

        #region private struct
        [StructLayout(LayoutKind.Sequential, Size = 16)]
        private readonly ref struct A16 { }
        #endregion
    }
}
