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
using Klei;
using KSerialization;
using ProcGenGame;
using SuperComicLib.MonoRuntime;
using System;
using System.IO;
using System.Linq;

namespace LazySaver
{
    using static GlobalHeader;
    [HarmonyPatch(typeof(Cluster), "Save")]
    public static unsafe class Save00_PATCH_3
    {
        public static bool Prefix(Cluster __instance)
        {
            var wr = m_writer;
            var ss = (NativeStream)wr.BaseStream;

            ss.FastClear();

            try
            {
                Manager.Clear();
                var layoutSave = new ClusterLayoutSave
                {
                    version = new Vector2I(1, 1),
                    size = __instance.size,
                    ID = __instance.Id,
                    numRings = __instance.numRings,
                    poiLocations = __instance.poiLocations,
                    poiPlacements = __instance.poiPlacements
                };

                var worlds = __instance.worlds;
                for (int i = 0; i < worlds.Count; i++)
                {
                    var world = worlds[i];
                    if (__instance.ShouldSkipWorldCallback == null || !__instance.ShouldSkipWorldCallback.Invoke(i, world))
                    {
                        var settings = world.Settings;
                        layoutSave.worlds.Add(new ClusterLayoutSave.World
                        {
                            data = world.data,
                            stats = world.stats,
                            name = settings.world.filePath,
                            isDiscovered = world.isStartingWorld,
                            traits = settings.GetWorldTraitIDs().ToList(),
                            storyTraits = settings.GetStoryTraitIDs().ToList()
                        });

                        if (world == __instance.currentWorld)
                            layoutSave.currentWorldIdx = i;
                    }
                }

                Serializer.Serialize(layoutSave, wr);
            }
            catch (Exception ex)
            {
                DebugUtil.LogErrorArgs("Couldn't serialize", ex.Message, ex.StackTrace);
            }

            var w2 = new BinaryWriter(File.Open(WorldGen.WORLDGEN_SAVE_FILENAME, FileMode.Create));

            Manager.SerializeDirectory(w2);

            ss.Position = 0;
            ss.UnsafeCopyTo(ss.Length, w2.BaseStream, m_tempBuffer);

            w2.Close();

            return false;
        }
    }
}
