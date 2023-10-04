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

using SuperComicLib;
using System;

namespace SCMod
{
    internal static unsafe class PackedCultureName
    {
        public static ulong ToUInt64(string cultureName)
        {
            if (cultureName == null || cultureName.Length != 5)
                throw new ArgumentException("null or invalid length");

            // string packing
            uint max;
            ulong temp;
            fixed (char* pName = cultureName)
            {
                uint
                    a = *(uint*)pName | 0x0020_0020u,
                    b = *(uint*)(pName + 3) | 0x0020_0020u;

                temp = (ulong)a << 32;     // ko..
                temp |= b;    // ..kr

                max =
                    CMath.Max(
                        CMath.Max((ushort)a, a >> 16),
                        CMath.Max((ushort)b, b >> 16));
            }

            if (max - 'a' > 'z' - 'a')
                throw new InvalidOperationException("invalid character(s) -> " + cultureName);

            return temp;
        }

        public static string ToString(ulong id)
        {
            var res = new string('\0', 5);

            fixed (char* pName = res)
            {
                *(uint*)pName = (uint)(id >> 32);   // ko..
                pName[2] = '-';
                *(uint*)(pName + 3) = (uint)id;     // ..kr
            }

            return res;
        }
    }
}
