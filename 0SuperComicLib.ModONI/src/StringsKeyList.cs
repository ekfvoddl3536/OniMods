#region LICENSE
/*
MIT License

Copyright (c) 2022. Super Comic (ekfvoddl3535@naver.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion
namespace SuperComicLib.ModONI
{
    public sealed class StringsKeyList
    {
        public readonly string[] Buffer;
        public int count_;

        public StringsKeyList(int capacity)
        {
            Buffer = new string[capacity];
            count_ = 0;
        }

        public StringsKeyList AddItem(string value)
        {
            Buffer[count_++] = value.ToUpper();
            return this;
        }

        public StringsKeyList ADD_NAME_DESC_EFFECT(string buildingID)
        {
            const string KPATH = "STRINGS.BUILDINGS.PREFABS.";

            var buf = Buffer;
            int x = count_;

            string path = KPATH + buildingID.ToUpper();

            buf[x++] = $"{path}.NAME";
            buf[x++] = $"{path}.DESC";
            buf[x++] = $"{path}.EFFECT";

            count_ = x;

            return this;
        }
    }
}
