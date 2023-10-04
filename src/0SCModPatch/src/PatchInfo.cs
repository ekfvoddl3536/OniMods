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

using SuperComicLib.eTxt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DP = KMod.Label.DistributionPlatform;

namespace SCMod
{
    internal sealed class PatchInfo
    {
        public static List<SimpleCultureInfo> cultures;
        public static List<PatchInfo> patches;

        // !==== INSTANCE FIELDS ====!
        public readonly FileInfo file;      
        public readonly ulong pckLang;  // packed language (e.g.: kokr)

        public PatchInfo(FileInfo file)
        {
            this.file = file;

            var name = Path.GetFileNameWithoutExtension(file.Name);
            pckLang = PackedCultureName.ToUInt64(name);

            TryAdd(cultures, pckLang, name);
        }

        private static void TryAdd(List<SimpleCultureInfo> list, ulong pckLang, string cultureName)
        {
            for (int i = 0; i < list.Count; ++i)
                if (list[i].packedName == pckLang)
                    return;

            list.Add(new SimpleCultureInfo(pckLang, cultureName));
        }

        public void Apply()
        {
            UnityEngine.Debug.Log($"[SCPATCH] Usage: '{file.FullName}'");

            var fi = file;
            var rd = new ETxtReader(fi.OpenRead());

            var count = 0L;
            KeyValuePair<HeaderID, object> node;
            try
            {
                for (; ; ++count)
                {
                    node = rd.Read();
                    if (node.Value == null)
                        break;

                    Strings.Add(node.Key.FullName, (string)node.Value);
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Fail read file -> PATH: '{fi.FullName}', EXCEPTION -> {e}");
            }

            UnityEngine.Debug.Log($"[SCPATH] ({count}) strings were overloaded.");

            rd.Dispose();
        }

        #region static methods
        internal static void Init()
        {
            cultures = new List<SimpleCultureInfo>(4);
            patches = new List<PatchInfo>(4);
        }

        internal static void CleanUp()
        {
            cultures = null;
            
            patches.Clear();
            patches = null;

            GC.Collect(0);
        }
        #endregion

        internal static bool LoadStrings(ulong pckLang)
        {
            if (pckLang == 0)
            {
                UnityEngine.Debug.LogWarning($"[SCPATCH] invalid packed culture name.");
                return false;
            }

            var filtered = patches.Where(x => x.pckLang == pckLang);
            var result = filtered.Any();

            foreach (var info in filtered)
                info.Apply();

            return result;
        }

        internal static void LoadPatches()
        {
            var loaded = patches;

            var root = KMod.Manager.GetDirectory();

            for (DP platform = 0; platform != DP.Dev + 1; ++platform)
            {
                var di_info = new DirectoryInfo(Path.Combine(root, platform.ToString()));
                if (!di_info.Exists)
                    continue;

                try
                {
                    foreach (var scpatch in di_info.EnumerateFiles("scpatch.conf", SearchOption.AllDirectories))
                    {
                        UnityEngine.Debug.Log($"[SCPATCH] Find 'scpatch.conf' -> PATH: '{scpatch.FullName}'");

                        // Only 1-depth find
                        foreach (var dir2 in scpatch.Directory.EnumerateDirectories())
                            foreach (var file in dir2.EnumerateFiles("*.txt"))
                            {
                                UnityEngine.Debug.Log($"[SCPATCH::eTxt]    Loading [ '{file.Name}' ]");

                                loaded.Add(new PatchInfo(file));
                            }
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogWarning($"[SCPATCH] ERROR -> [ {platform}/ ] " + e);
                }
            }
        }
    }
}
