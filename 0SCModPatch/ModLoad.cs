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

using System.Collections.Generic;
using PeterHan.PLib.Options;
using HarmonyLib;
using KMod;
using System.Globalization;
using System;

namespace SCMod
{
    public sealed class ModLoad : UserMod2
    {
        public override void OnLoad(Harmony _)
        {
            new POptions().RegisterOptions(this, typeof(ModConfigV2_2));

            UnityEngine.Debug.Log($"[SCPATCH] Ready.");
        }

        public override void OnAllModsLoaded(Harmony _, IReadOnlyList<Mod> __)
        {
            UnityEngine.Debug.Log($"[SCPATCH] Start finding for translation files...");

            PatchInfo.Init();

            PatchInfo.LoadPatches();

            var config = POptions.ReadSettings<ModConfigV2_2>();
            if (config != null)
            {
                ModConfigV2_2.instance = config;
                PatchInfo.LoadStrings(config.Language);
            }
            else
                try
                {
                    ModConfigV2_2.instance = new ModConfigV2_2();
                    ReadCultureInfoAndLoad();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogWarning($"[SCPATCH] Fail auto-detect CultureInfo -> " + e);
                }
        }

        private static void ReadCultureInfoAndLoad()
        {
            var info =
                CultureInfo.InstalledUICulture ??
                CultureInfo.DefaultThreadCurrentUICulture ??
                CultureInfo.DefaultThreadCurrentCulture ?? 
                new CultureInfo("en-us");

            var name = info.Name;
            if (name.Length > 5) // ISO-BCP?
            {
                name = info.TextInfo.CultureName;

                // default 'en-us'
                if (name.Length != 5)
                    name = "en-us";
            }

            UnityEngine.Debug.Log($"[SCPATCH] CultureInfo auto-detect result -> '{name ?? "(NULL)"}'");

            var pckLang = PackedCultureName.ToUInt64(name);
            if (PatchInfo.LoadStrings(pckLang))
            {
                UnityEngine.Debug.Log($"[SCPATCH] Done!");

                ModConfigV2_2.instance.Language = pckLang;
                POptions.WriteSettings(ModConfigV2_2.instance);
            }
            else if (PatchInfo.cultures.Count > 0)
            {
                ModConfigV2_2.instance.Language = PatchInfo.cultures[0].packedName;
                POptions.WriteSettings(ModConfigV2_2.instance);
            }
        }
    }
}
