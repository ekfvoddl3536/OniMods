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
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LazySaver
{
    [HarmonyPatch(typeof(SaveManager), "OnPrefabInit")]
    public static class OnPrefabInit_PATCH_1
    {
        public static void Postfix(SaveManager __instance)
        {
            const BindingFlags INST = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var t0 = typeof(SaveManager);

            SaveBw_PATCH_2.m_prefabMap = (Dictionary<Tag, GameObject>)t0.GetField("prefabMap", INST).GetValue(__instance);

            t0.GetField("orderedKeys", INST).SetValue(__instance, null);
        }
    }
}
