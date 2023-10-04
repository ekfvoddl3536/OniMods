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

//namespace SuperComicLib
//{
//    [SuppressUnmanagedCodeSecurity]
//    public static unsafe class BitOperation
//    {
//        // https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Numerics/BitOperations.cs,1f415bab5cc6fe00
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static int PopCount(ulong n)
//        {
//            const ulong c1 = 0x_55555555_55555555ul;
//            const ulong c2 = 0x_33333333_33333333ul;
//            const ulong c3 = 0x_0F0F0F0F_0F0F0F0Ful;
//            const ulong c4 = 0x_01010101_01010101ul;

//            n -= (n >> 1) & c1;
//            n = (n & c2) + ((n >> 2) & c2);
//            n = (((n + (n >> 4)) & c3) * c4) >> 56;

//            return (int)n;
//        }

//        /// <returns>0 = not defined</returns>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static int MSB64Index(ulong n)
//        {
//            n |= n >> 1;
//            n |= n >> 2;
//            n |= n >> 4;
//            n |= n >> 8;
//            n |= n >> 16;
//            n |= n >> 32;

//            return PopCount(n);
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static ulong MSB64(ulong n)
//        {
//            n |= n >> 1;
//            n |= n >> 2;
//            n |= n >> 4;
//            n |= n >> 8;
//            n |= n >> 16;
//            n |= n >> 32;

//            return n ^ (n >> 1);
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static int MSB64Index(long n) => MSB64Index((ulong)n);

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static long MSB64(long n) => (long)MSB64((ulong)n);

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static long Align8(long v) => (v + 7L) & ~7L;
//    }
//}
