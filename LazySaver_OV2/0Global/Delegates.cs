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
using System.Security;
using System.Collections.Generic;

namespace LazySaver
{
    //
    // SaveLoader
    //
    internal delegate void MethodReportSaveMetrics(SaveLoader @this, bool isAutoSave);
    internal delegate void MethodSave(SaveLoader @this, BinaryWriter writer);

    // 
    // Sim (DLLImport + Unmanaged source)
    //
    [SuppressUnmanagedCodeSecurity]
    internal unsafe delegate byte* ExternSIM_BeginSave(int* size, int x, int y);
    [SuppressUnmanagedCodeSecurity]
    internal unsafe delegate void ExternSIM_EndSave();

    //
    // SaveManager
    //
    internal delegate void MethodWrite(SaveManager @this, Tag key, List<SaveLoadRoot> value, BinaryWriter writer);
}
