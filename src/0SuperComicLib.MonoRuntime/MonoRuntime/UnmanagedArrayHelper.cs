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
using System.Security;

namespace SuperComicLib.MonoRuntime
{
    [SuppressUnmanagedCodeSecurity]
    internal static unsafe class UnmanagedArrayHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void* AllocateArray1Header(int length, int element_size) =>
            (void*)Marshal.AllocHGlobal((IntPtr)(length * (long)element_size + 0x20L));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void InitArray1Header(void* dst, long vt, int length)
        {
            *(long*)dst = vt;
            *((long*)dst + 1) = 0;
            *((long*)dst + 2) = 0;
            *((long*)dst + 3) = (uint)length;
        }
    }
}