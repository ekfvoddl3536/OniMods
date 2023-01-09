using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace LazySaver.MemoryIO
{
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class UnmanagedStream : Stream
    {
        #region fields
        //  initial size of bytes
        //      medium: 0x800_0000 (128.0MiB)
        //      high:   0x2000_0000 (512.0MiB)
        //      ultra:  0x4000_0000 (1024.0MiB)
        internal long _initialSize;
        //  Size of bytes to increase when memory needs to be expanded
        //      low:    0x100_0000  (16.0MiB)
        //      medium: 0x200_0000  (32.0MiB)
        //      high:   0x400_0000  (64.0MiB)
        //      ultra:  0x800_0000  (128.0MiB)
        internal long _increaseSize;
        // profile (init+inc):
        //  low:    mid+low
        //  mid:    mid+mid
        //  mid+:   mid+high
        //  high:   high+mid
        //  high+:  high+high
        //  ultra:  ultra+high
        //  ultra+: ultra+ultra

        private byte* _Ptr;
        private byte* _Last;
        private byte* _End;
        #endregion

        #region constructor
        public UnmanagedStream(long initSize, long incSize /*, bool allocateMemoryImmediate*/)
        {
            UnityEngine.Debug.Assert(initSize > 0);
            UnityEngine.Debug.Assert(incSize > 0);

            _initialSize = initSize;
            _increaseSize = incSize;

            // if (allocateMemoryImmediate)
            // ReAllocateMemory();

            _Ptr = (byte*)Marshal.AllocHGlobal((IntPtr)_initialSize);
            _Last = _Ptr;
            _End = _Ptr + _initialSize;
        }

        // public void ReAllocateMemory()
        // {
        //     Marshal.FreeHGlobal((IntPtr)_Ptr);
        // 
        //     _Ptr = (byte*)Marshal.AllocHGlobal((IntPtr)_initialSize);
        //     _Last = _Ptr;
        //     _End = _Ptr + _initialSize;
        // }
        #endregion

        #region property
        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;
        public override long Length => _Last - _Ptr;
        public override long Position
        {
            get => _Last - _Ptr;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _Last = _Ptr + value;
            }
        }
        #endregion

        #region methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void FastClear() => _Last = _Ptr;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Flush() { }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (_Ptr == null) // Closed
                throw new ObjectDisposedException(typeof(UnmanagedStream).FullName);

            byte* temp;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    if (offset < 0)
                        throw new IOException("SeekBeforeBegin-Begin. invalid offset. #L57 (offset < 0) :: " + offset.ToString());
                    temp = _Ptr + offset;
                    break;

                case SeekOrigin.Current:
                    temp = _Last + offset;
                    break;

                case SeekOrigin.End:
                    temp = _End + offset;
                    break;

                default:
                    throw new ArgumentException(nameof(origin));
            }

            if (temp < _Ptr)
                throw new IOException("SeekBeforeBegin. invalid offset. #L73-Common :: " + origin.ToString());

            _Last = temp;
            return temp - _Ptr;
        }

        public override void SetLength(long value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            if (_Ptr + value > _End)
            {
                long align = _increaseSize - ((_increaseSize - 1) & value) + value;

                GC.AddMemoryPressure(align);

                byte* nbuf = (byte*)Marshal.AllocHGlobal((IntPtr)align);

                ulong numOfBytes = (ulong)(_Last - _Ptr);
                Buffer.MemoryCopy(_Ptr, nbuf, numOfBytes, numOfBytes);

                GC.RemoveMemoryPressure(_End - _Ptr);

                Marshal.FreeHGlobal((IntPtr)_Ptr);

                _Ptr = nbuf;
                _Last = nbuf + numOfBytes;
                _End = nbuf + align;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            long cnt = _End - _Last;

            if (cnt > count)
                cnt = count;

            Marshal.Copy((IntPtr)_Last, buffer, offset, (int)cnt);

            _Last += (int)cnt;
            return (int)cnt;
        }

        public override int ReadByte() => _Last < _End ? *_Last++ : -1;

        public override void Write(byte[] buffer, int offset, int count)
        {
            ReserveLength(count);

            Marshal.Copy(buffer, offset, (IntPtr)_Last, count);
            _Last += count;
        }

        public void Write(byte* buffer, int count)
        {
            ReserveLength(count);

            ulong numOfBytes = (uint)count;
            Buffer.MemoryCopy(buffer, _Last, numOfBytes, numOfBytes);

            _Last += count;
        }

        public override void WriteByte(byte value)
        {
            ReserveLength(1);
            *_Last++ = value;
        }
        #endregion

        #region Utils
        public byte[] ToArray(long startPosition)
        {
            byte* pBegin = _Ptr + startPosition;
            if (startPosition < 0 || pBegin > _Last)
                throw new ArgumentOutOfRangeException(nameof(startPosition));

            var length = (int)Math.Min(_Last - pBegin, 0x7FFFFFC7);
            byte[] buffer = new byte[length];

            Marshal.Copy((IntPtr)pBegin, buffer, 0, length);

            return buffer;
        }

        public void CopyTo(long startPosition, byte[] buffer, int index, int count)
        {
            if (_Last - _Ptr < (uint)count)
                count = (int)(_Last - _Ptr);

            if (startPosition < 0 || _Ptr + startPosition > _Last)
                throw new ArgumentOutOfRangeException(nameof(startPosition));

            Marshal.Copy((IntPtr)(_Ptr + startPosition), buffer, index, count);
        }

        public void ReserveLength(int addLength) => SetLength(_Last - _Ptr + (uint)addLength);
        #endregion

        #region dispose
        protected override void Dispose(bool disposing)
        {
            Marshal.FreeHGlobal((IntPtr)_Ptr);

            if (disposing)
            {
                _Ptr = null;
                _Last = null;
                _End = null;
            }
        }
        #endregion
    }
}
