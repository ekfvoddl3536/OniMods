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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SuperComicLib.MonoRuntime
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct UnmanagedArray<T> : IDisposable
        where T : unmanaged
    {
        public readonly long pFirst;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal UnmanagedArray(void* ptr) => pFirst = (long)ptr;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnmanagedArray(int length)
        {
            var ptr = UnmanagedArrayHelper.AllocateArray1Header(length, sizeof(T));
            pFirst = (long)ptr;

            var tmp = Array.Empty<T>();
            UnmanagedArrayHelper.InitArray1Header(ptr, **(long**)Unsafe.AsPointer(ref tmp), length);
        }

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => *(int*)(pFirst + 0x18L);
        }

        public T* Pointer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (T*)(pFirst + 0x20L);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] AsManaged() => Unsafe.As<long, T[]>(ref Unsafe.AsRef(pFirst));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Marshal.FreeHGlobal((IntPtr)pFirst);

        #region static
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnmanagedArray<T> MappingUnsafe(void* dst_mm, int length)
        {
            var tmp = Array.Empty<T>();
            UnmanagedArrayHelper.InitArray1Header(dst_mm, **(long**)Unsafe.AsPointer(ref tmp), length);
            return new UnmanagedArray<T>(dst_mm);
        }
        #endregion
    }
}