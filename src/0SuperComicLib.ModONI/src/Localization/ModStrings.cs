// MIT License
//
// Copyright (c) 2022-2023. Super Comic (ekfvoddl3535@naver.com)
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
using SuperComicLib.IO;

namespace SuperComicLib.ModONI
{
    /// <summary>
    /// Provides the string used in the mod
    /// </summary>
    public abstract class ModStrings
    {
        private readonly Guid _uuid;

        /// <summary>
        /// Initializes with the provided debug guid.
        /// </summary>
        protected ModStrings(Guid uuid) => _uuid = uuid;

        /// <summary>
        /// Initializes without the debug guid.
        /// </summary>
        protected ModStrings() : this(Guid.Empty)
        {
        }

        /// <summary>
        /// Retrieves the strings written in the native language or the default language of this mod.<br/>
        /// The exported order corresponds to the elements of the key list provided to <see cref="Apply(StringKeyList)"/>,<br/>
        /// so please be careful with the sequence.
        /// </summary>
        protected abstract string[] OnLoadOriginalStrings();

        /// <summary>
        /// Add strings to the game using element values from the provided <see cref="StringKeyList"/>.
        /// </summary>
        public void Apply(StringKeyList list)
        {
            var values = OnLoadOriginalStrings();

            var keys = list._buffer;

            if (values.Length < list._size)
                UnityEngine.Debug.LogWarning($"missing some strings: [Key: {GetType().FullName}, DEBUG-UUID: {_uuid}]");

            TryExportStrings(keys, values, GetType());

            for (int i = 0; i < keys.Length && i < values.Length; ++i)
                Strings.Add(keys[i], values[i]);
        }

        private static void TryExportStrings(string[] keys, string[] values, Type type)
        {
            var fi_info = new FileInfo(Path.Combine(ModDirectory.GetPath(type), "resources", "original_strings_UTF8.txt"));

            if (fi_info.Exists)
                return;

            var di_info = fi_info.Directory;
            if (di_info.Exists == false)
                di_info.Create();

            var w = new ETxtWriter(fi_info.Open(FileMode.CreateNew, FileAccess.Write));

            w.Comment(@"Auto-generated file.");
            w.Comment(@"To learn how to provide translations, check out the following links:");
            w.Comment(@"https://github.com/ekfvoddl3536/OniMods/blob/master/docs/ITPMM%20-%20String%20Translation%20Guide.md");
            w.WriteLine();
            w.Flush();

            for (int i = 0; i < keys.Length && i < values.Length; i++)
            {
                w.Comment($"Strings index [ {i} ]");
                w.Write(keys[i], values[i]);
                w.WriteLine();
                w.Flush();
            }

            w.Dispose();
        }
    }
}
