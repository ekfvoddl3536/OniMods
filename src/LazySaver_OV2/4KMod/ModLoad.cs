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

using HarmonyLib;
using LazySaver.Options;
using PeterHan.PLib.Options;
using System;
using SuperComicLib.MonoRuntime;

namespace LazySaver
{
    using static GlobalHeader;
    public sealed unsafe class ModLoad : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            try
            {
                m_writer = new UnmanagedWriter(new NativeStream(HEAP_INIT_SZ, HEAP_INCR_SZ));
                m_tempBuffer = new UnmanagedArray<byte>((int)HEAP_INCR_SZ).AsManaged();
            }
            catch (OutOfMemoryException outOfMemExcept)
            {
                UnityEngine.Debug.LogError("Not enough memory capacity. [Lazy Saver+] ------> " + outOfMemExcept.Message);
                throw outOfMemExcept;
            }

            new POptions().RegisterOptions(this, typeof(ModConfig));

            option = POptions.ReadSettings<ModConfig>() ?? new ModConfig();

            harmony.PatchAll();
        }
    }
}
