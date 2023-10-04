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

#pragma warning disable IDE0028
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace LazySaver
{
    using static GlobalHeader;
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.Save))]
    public static class SaveBw_PATCH_2
    {
        // OnPrefabInit -> assign
        internal static Dictionary<Tag, GameObject> m_prefabMap;
        private static char[] m_SAVE_HEADER;
        // orderedKeys
        private static Tag[] m_orderedKeys;
        private static int m_orderedKeysCount;

        // methods
        private static MethodWrite m_instWrite;

        // OrderBy method delegates
        private static Func<Tag, bool> _stickerBomb_first, _underConstruction_second;
        private static Func<List<SaveLoadRoot>, bool> _countPredicate;

        public static void Prepare()
        {
            const BindingFlags COMMON = BindingFlags.Public | BindingFlags.NonPublic;
            const BindingFlags STATIC = COMMON | BindingFlags.Static;
            const BindingFlags INST = COMMON | BindingFlags.Instance;

            var t0 = typeof(SaveManager);

            m_SAVE_HEADER = (char[])t0.GetField("SAVE_HEADER", STATIC).GetValue(null);

            var mi = t0.GetMethod("Write", INST);
            m_instWrite = (MethodWrite)Delegate.CreateDelegate(typeof(MethodWrite), mi);

            m_orderedKeys = Array.Empty<Tag>();

            _stickerBomb_first = CB_StickerBomb;
            _underConstruction_second = CB_UnderConstruction;
            _countPredicate = CB_SceneObjectsCount;
        }

        public static bool Prefix(SaveManager __instance)
        {
            var wr = m_writer;

            wr.Write(m_SAVE_HEADER);
            wr.Write(SaveManager.SAVE_MAJOR_VERSION);
            wr.Write(SaveManager.SAVE_MINOR_VERSION);

            var sceneObjs = __instance.GetLists();
            wr.Write(sceneObjs.Values.Count(_countPredicate));

            var saveInstTag = SaveGame.Instance.PrefabID();
            sceneObjs.TryGetValue(saveInstTag, out var saveGameValues);
            sceneObjs.Remove(saveInstTag);

            // LOOP
            var orderedKeys =
                OrderedKeysAddRange(sceneObjs.Keys)
                    .Take(m_orderedKeysCount)
                    .OrderBy(_stickerBomb_first)
                    .OrderBy(_underConstruction_second)
                    .GetEnumerator();

            var _cached_L0 = new List<SaveLoadRoot>(1);
            _cached_L0.Add(SaveGame.Instance.GetComponent<SaveLoadRoot>());

            m_instWrite.Invoke(__instance, saveInstTag, _cached_L0, wr);

            if (option.isForcedDelay && arg_isAutoSave)
                while (orderedKeys.MoveNext())
                {
                    var currentTag = orderedKeys.Current;
                    var currentObjs = sceneObjs[currentTag];

                    WriteMatchWithDelay(__instance, currentObjs, currentTag, false);
                    WriteMatchWithDelay(__instance, currentObjs, currentTag, true);
                }
            else
                while (orderedKeys.MoveNext())
                {
                    var currentTag = orderedKeys.Current;
                    var currentObjs = sceneObjs[currentTag];

                    WriteMatch(__instance, currentObjs, currentTag, false);
                    WriteMatch(__instance, currentObjs, currentTag, true);
                }

            orderedKeys.Dispose();

            // backuped, re-add
            if (saveGameValues != null)
                sceneObjs.Add(saveInstTag, saveGameValues);

            return false;
        }

        private static void WriteMatchWithDelay(SaveManager @this, List<SaveLoadRoot> objs, in Tag tag, bool match)
        {
            var dt = System.DateTime.Now;

            WriteMatch(@this, objs, tag, match);

            if ((System.DateTime.Now - dt).Ticks > 20 * TimeSpan.TicksPerMillisecond)
                Thread.Sleep(option.delayDuration);
        }

        private static void WriteMatch(SaveManager @this, List<SaveLoadRoot> objs, in Tag tag, bool match)
        {
            for (int i = 0, cnt = objs.Count; i < cnt; i++)
            {
                var currentRoot = objs[i];
                if (currentRoot && currentRoot.GetComponent<SimCellOccupier>() ^ match)
                {
                    m_instWrite.Invoke(@this, tag, objs, m_writer);
                    break;
                }
            }
        }

        private static Tag[] OrderedKeysAddRange(ICollection<Tag> src)
        {
            var dst = m_orderedKeys;
            if (dst.Length < (m_orderedKeysCount = src.Count))
            {
                // align 192bytes
                int nextCnt = Math.Max(dst.Length << 1, m_orderedKeysCount);
                int align16sz = 16 - (15 & nextCnt) + nextCnt;

                dst = new Tag[align16sz];
                m_orderedKeys = dst;
            }

            src.CopyTo(dst, 0);

            return dst;
        }

        private static bool CB_StickerBomb(Tag tag) => tag.Name == nameof(StickerBomb);
        private static bool CB_UnderConstruction(Tag tag) => tag.Name.Contains("UnderConstruction");
        private static bool CB_SceneObjectsCount(List<SaveLoadRoot> item) => item.Count > 0;
    }
}
