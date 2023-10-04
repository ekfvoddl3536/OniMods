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

#if !HIDDEN
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace SuperComicLib.MonoRuntime
{
    [SuppressUnmanagedCodeSecurity]
    public unsafe static class MemoryBlock
    {
#region methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte* LargeClearBlock(byte* pCur, long cb)
        {
            const int BLOCK = 0x100_0000;

            var pEnd = pCur + (cb & -0x100_0000L);

            for (; pCur != pEnd; pCur += BLOCK)
                Unsafe.InitBlock(pCur, 0, BLOCK);

            return pEnd;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ZeroMemory(byte* ptr, long cb)
        {
            const long ALIGN = 16L;
            const long MASK = ALIGN - 1;

            if (cb == 0)
                return;

            // pointer address alignment
            if (((long)ptr & MASK) != 0)
            {
                var t0 = Math.Min(cb, ALIGN - ((long)ptr & MASK));

                Unsafe.InitBlockUnaligned(ptr, 0, (uint)t0);
                
                ptr += t0;
                cb -= t0;
            }

            var pEnd = LargeClearBlock(ptr, cb);

            var count = (uint)(cb & 0xFF_FFF0L);
            Unsafe.InitBlock(pEnd, 0, count);

            Unsafe.InitBlockUnaligned(pEnd + count, 0, (uint)(cb & MASK));
        }
#endregion
    }
}
#endif