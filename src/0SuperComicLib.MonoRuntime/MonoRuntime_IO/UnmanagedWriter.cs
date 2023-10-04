// MIT License
//
// Copyright (c) 2023. Super Comic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.IO;
using System.Text;

namespace SuperComicLib.MonoRuntime
{
    public sealed unsafe class UnmanagedWriter : BinaryWriter
    {
        public readonly NativeStream m_stream;

        public UnmanagedWriter(NativeStream stream) : base(stream) => m_stream = stream;
        public UnmanagedWriter(NativeStream stream, Encoding encoding) : base(stream, encoding) => m_stream = stream;
        public UnmanagedWriter(NativeStream stream, Encoding encoding, bool leaveOpen) : base(stream, encoding, leaveOpen) => m_stream = stream;

        public override Stream BaseStream => m_stream;

        public override void Flush() { }
        public override long Seek(int offset, SeekOrigin origin) => m_stream.Seek(offset, origin);

        public override void Write(bool value) => m_stream.WriteUnsafe(value);
        public override void Write(byte value) => m_stream.WriteUnsafe(value);
        public override void Write(sbyte value) => m_stream.WriteUnsafe(value);
        public override void Write(short value) => m_stream.WriteUnsafe(value);
        public override void Write(ushort value) => m_stream.WriteUnsafe(value);
        public override void Write(int value) => m_stream.WriteUnsafe(value);
        public override void Write(uint value) => m_stream.WriteUnsafe(value);
        public override void Write(long value) => m_stream.WriteUnsafe(value);
        public override void Write(ulong value) => m_stream.WriteUnsafe(value);
        public override void Write(float value) => m_stream.WriteUnsafe(value);
        public override void Write(double value) => m_stream.WriteUnsafe(value);

        public void WriteUnsafe(byte* ptr, long count) => m_stream.Write(ptr, count);
        public void WriteUnsafe<T>(T value) where T : unmanaged => m_stream.WriteUnsafe(value);
        public void WriteUnsafe<T>(UnmanagedArray<T> array) where T : unmanaged => m_stream.Write((byte*)array.Pointer, array.Length * (long)sizeof(T));
    }
}
