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

namespace SuperComicLib.ModONI
{
    /// <summary>
    /// A list of keys of fixed array size.
    /// </summary>
    public sealed class StringKeyList
    {
        internal readonly string[] _buffer;
        internal int _size;

        public int Count => _size;

        public StringKeyList(FixedCapacity sz)
        {
            if ((uint)sz > (uint)FixedCapacity.SZ_128)
                throw new ArgumentOutOfRangeException(nameof(sz));

            _buffer = new string[1 << (int)sz];
        }

        public StringKeyList(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            _buffer = new string[capacity];
        }

        /// <summary>
        /// Adds <c>NAME</c>, <c>DESC</c>, <c>EFFECT</c> in order for the id specified in the <c>"STRINGS.BUILDINGS.PREFABS"</c> path.<br/>
        /// This method is equivalent to executing the following:<para/>
        /// <code>
        ///     <see cref="Add(string)">Add</see>("STRINGS.BUILDINGS.PREFABS." + <paramref name="id"/>.<see cref="string.ToUpper()">ToUpper</see>() + ".NAME");
        ///     <see cref="Add(string)">Add</see>("STRINGS.BUILDINGS.PREFABS." + <paramref name="id"/>.<see cref="string.ToUpper()">ToUpper</see>() + ".DESC");
        ///     <see cref="Add(string)">Add</see>("STRINGS.BUILDINGS.PREFABS." + <paramref name="id"/>.<see cref="string.ToUpper()">ToUpper</see>() + ".EFFECT");
        /// </code>
        /// </summary>
        /// <returns>This method returns itself.</returns>
        public StringKeyList ADD_BUILDINGS(string id)
        {
            const string KPATH = "STRINGS.BUILDINGS.PREFABS.";

            var buf = _buffer;
            var i = _size;

            var path = KPATH + id.ToUpper();

            buf[i] = path + ".NAME";
            buf[i + 1] = path + ".DESC";
            buf[i + 2] = path + ".EFFECT";

            _size = i + 3;

            return this;
        }

        /// <summary>
        /// Adds <c>NAME</c>, <c>DESC</c>, <c>EFFECT</c> in order for the id specified in the <c>"STRINGS.EQUIPMENT.PREFABS"</c> path.<br/>
        /// This method is equivalent to executing the following:<para/>
        /// <code>
        ///     <see cref="Add(string)">Add</see>("STRINGS.EQUIPMENT.PREFABS." + <paramref name="id"/>.<see cref="string.ToUpper()">ToUpper</see>() + ".NAME");
        ///     <see cref="Add(string)">Add</see>("STRINGS.EQUIPMENT.PREFABS." + <paramref name="id"/>.<see cref="string.ToUpper()">ToUpper</see>() + ".DESC");
        ///     <see cref="Add(string)">Add</see>("STRINGS.EQUIPMENT.PREFABS." + <paramref name="id"/>.<see cref="string.ToUpper()">ToUpper</see>() + ".EFFECT");
        /// </code>
        /// </summary>
        /// <returns>This method returns itself.</returns>
        public StringKeyList ADD_EQUIPMENT(string id)
        {
            const string KPATH = "STRINGS.EQUIPMENT.PREFABS.";

            var buf = _buffer;
            var i = _size;

            var path = KPATH + id.ToUpper();

            buf[i] = path + ".NAME";
            buf[i + 1] = path + ".DESC";
            buf[i + 2] = path + ".EFFECT";

            _size = i + 3;

            return this;
        }

        /// <summary>
        /// Adds a key using the given <paramref name="strkey_fullpath"/> as the full path.
        /// </summary>
        /// <param name="strkey_fullpath">Case does not matter, but string keys must contain the full path.</param>
        /// <returns>This method returns itself.</returns>
        public StringKeyList Add(string strkey_fullpath)
        {
            _buffer[_size++] = strkey_fullpath.ToUpper();

            return this;
        }
    }
}
