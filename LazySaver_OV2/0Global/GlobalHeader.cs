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

using LazySaver.Options;
using SuperComicLib.MonoRuntime;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading.Tasks;

namespace LazySaver
{
    [SuppressUnmanagedCodeSecurity]
    internal unsafe static class GlobalHeader
    {
        public const long HEAP_INIT_SZ = 0x800_0000;    // 128MiB
        public const long HEAP_INCR_SZ = 0x100_0000;    //  16MiB

        #region fields
        //
        //  ==  saveloader save PATCH  ==
        //
        internal static byte[] m_tempBuffer;
        internal static UnmanagedWriter m_writer;
        internal static Task m_previousTask;

        // saveloader.save task
        internal static bool arg_isAutoSave;
        internal static string arg_filename;
        internal static string res_errMsg;
        internal static bool res_exceptionUnhandled;

        // =========================
        internal static ModConfig option;
        #endregion

        #region methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* GetAlign8(byte* s) => (byte*)(((long)s + 7L) & ~7L);
        #endregion
    }
}