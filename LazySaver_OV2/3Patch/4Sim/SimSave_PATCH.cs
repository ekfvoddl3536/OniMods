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
using System.Reflection;
using HarmonyLib;
using SuperComicLib.MonoRuntime;

namespace LazySaver
{
    using static GlobalHeader;
    [HarmonyPatch(typeof(Sim), nameof(Sim.Save))]
    public static unsafe class SimSave_PATCH
    {
        private static ExternSIM_BeginSave m_simBeginSave;
        private static ExternSIM_EndSave m_simEndSave;

        public static void Prepare()
        {
            const BindingFlags STATIC = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            // Sim
            var t0 = typeof(Sim);

            var m1 = t0.GetMethod("SIM_BeginSave", STATIC);
            m_simBeginSave = (ExternSIM_BeginSave)Delegate.CreateDelegate(typeof(ExternSIM_BeginSave), m1);

            m1 = t0.GetMethod("SIM_EndSave", STATIC);
            m_simEndSave = (ExternSIM_EndSave)Delegate.CreateDelegate(typeof(ExternSIM_EndSave), m1);
        }

        public static bool Prefix(BinaryWriter writer, int x, int y)
        {
            if (writer == m_writer)
            {
                int length;
                byte* source = m_simBeginSave.Invoke(&length, x, y);

                m_writer.Write(length);
                ((NativeStream)m_writer.BaseStream).Write(source, length);

                m_simEndSave.Invoke();
                return false;
            }

            return true;
        }
    }
}