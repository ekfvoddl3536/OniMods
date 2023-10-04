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

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace SuperComicLib.MonoRuntime
{
    [SuppressUnmanagedCodeSecurity]
    public sealed unsafe class NativeStream : Stream
    {
        private readonly long align_size;

        internal byte* _Ptr;
        internal byte* _Data;
        internal byte* _Last;
        internal byte* _End;

        #region constructors
        public NativeStream(long init_size, long incr_size)
        {
            align_size = incr_size;

            _Ptr = (byte*)Marshal.AllocHGlobal((IntPtr)init_size);
            _Data = _Ptr;
            _Last = _Ptr;
            _End = _Ptr + init_size;
        }
        #endregion

        #region properties
        public long Capacity => _End - _Ptr;
        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;
        public override long Length => _Last - _Data;
        public override long Position
        {
            get => _Data - _Ptr;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _Data = _Ptr + value;
            }
        }
        #endregion

        #region default methods
        public override void Flush() { }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    break;

                case SeekOrigin.Current:
                    offset += Position;
                    break;

                case SeekOrigin.End:
                    offset += Length;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(origin));
            }

            _Data = _Ptr + offset;
            return offset;
        }

        public override void SetLength(long value)
        {
            var p = _Ptr + value;
            if (p > _End)
                IncreaseCapacity(value, _Last);
            else
            {
                _Data = (byte*)Math.Max((long)p, (long)_Data);
                _Last = p;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var p = _Last - _Data;
            if (p <= 0)
                return 0;

            count = (int)Math.Min(p, count);

            Marshal.Copy((IntPtr)_Data, buffer, offset, count);
            _Data += count;

            return count;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var data = WriteCore((uint)count);
            Marshal.Copy(buffer, offset, (IntPtr)data, count);
        }

        public override int ReadByte() => _Data < _Last ? *_Data++ : -1;

        public override void WriteByte(byte value) => *WriteCore(1) = value;

        private void IncreaseCapacity(long nextCapacity, byte* basePointer)
        {
            long cb = align_size - ((align_size - 1) & nextCapacity) + nextCapacity;

            var np = (byte*)Marshal.AllocHGlobal((IntPtr)cb);

            var old_cb = basePointer - _Ptr;
            Buffer.MemoryCopy(_Ptr, np, old_cb, old_cb);

            Marshal.FreeHGlobal((IntPtr)_Ptr);

            _Data = np + (_Data - _Ptr);
            _Last = np + (_Last - _Ptr);
            _End = np + cb;
            _Ptr = np;
        }
        #endregion

        #region custom methods
        public long Read(byte* buffer, long count)
        {
            var p = _Last - _Data;
            if (p == 0)
                return 0;

            count = Math.Min(p, count);

            Buffer.MemoryCopy(_Data, buffer, count, count);
            _Data += count;

            return count;
        }

        public void Write(byte* buffer, long count)
        {
            var data = WriteCore(count);
            Buffer.MemoryCopy(buffer, data, count, count);
        }

        public void WriteUnsafe<T>(T value) where T : unmanaged
        {
            var data = WriteCore((uint)sizeof(T));
            *(T*)data = value;
        }

        public void CopyTo(long count, Stream dest, byte[] buffer)
        {
            if ((ulong)count > (ulong)(_Last - _Data))
                throw new ArgumentOutOfRangeException(nameof(count));

            if (dest == null || !dest.CanWrite)
                throw new ArgumentException("Null or CanWrite == false", nameof(dest));

            if (buffer == null || buffer.Length < 0x1_0000)
                throw new ArgumentException("Null or too small length (Length < 0x1_0000)", nameof(dest));

            UnsafeCopyTo(count, dest, buffer);
        }

        public void UnsafeCopyTo(long count, Stream dest, byte[] buffer)
        {
            var bufLeni64 = buffer.Length;
            var end = _Data + count;
            for (var src = _Data; ;)
            {
                var cnt = (int)Math.Min(end - src, bufLeni64);
                if (cnt == 0)
                    break;

                Marshal.Copy((IntPtr)src, buffer, 0, cnt);
                dest.Write(buffer, 0, cnt);

                src += cnt;
            }

            _Data = end;
        }

        public void FastClear()
        {
            _Data = _Ptr;
            _Last = _Ptr;
        }
        #endregion

        #region core method
        private byte* WriteCore(long count)
        {
            var p = _Data + count;
            if (p > _End)
            {
                IncreaseCapacity(p - _Ptr, _Data);
                p = _Data + count;
            }

            _Last = (byte*)Math.Max((long)p, (long)_Last);

            var res = _Data;
            _Data = p;

            return res;
        }
        #endregion

        #region disposing
        protected override void Dispose(bool disposing)
        {
            Marshal.FreeHGlobal((IntPtr)_Ptr);

            if (disposing)
            {
                _Ptr = null;
                _Data = null;
                _Last = null;
                _End = null;
            }
        }
        #endregion
    }
}
