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
using System;
using System.Collections.Generic;
using ELANG = Localization.Language;

namespace SuperComicLib.ModONI
{
    public abstract class ModLocalization
    {
        private static readonly Dictionary<Guid, ModLocalization> pool = new Dictionary<Guid, ModLocalization>();
        public static ModLocalization GetInstance(Guid uuid)
        {
            pool.TryGetValue(uuid, out ModLocalization result);
            return result;
        }

        protected string[] texts_;

        protected ModLocalization(Guid uuid)
        {
            OnInitialize();

            var inst = GetInstance(uuid);
            if (inst != null)
            {
                int prevLen = texts_.Length;
                OnMergeTexts(inst);

                if (prevLen > texts_.Length)
                    throw new InvalidOperationException("some strings are deleted");
            }

            pool[uuid] = this;
        }

        public virtual string[] Texts => texts_;

        /// <summary>
        /// Main language used for translation.<para/>
        /// '<see cref="ELANG.Unspecified"/>' value is recognized as English.
        /// </summary>
        public abstract ELANG MainLanguage { get; }

        /// <summary>
        /// Load the translated strings
        /// </summary>
        protected abstract void OnInitialize();

        /// <summary>
        /// If the number of translated strings is less than the number of original strings, 
        /// import the missing strings from the original strings.
        /// </summary>
        /// <param name="other">Provider of the original strings</param>
        protected virtual void OnMergeTexts(ModLocalization other) =>
            texts_ = MergeTexts(other.texts_, texts_);

        protected static string[] MergeTexts(string[] originalStrings, string[] translatedStrings)
        {
            var old = originalStrings;
            var oldLen = old.Length;

            var me = translatedStrings;
            var meLen = me.Length;

            if (oldLen != meLen)
            {
                var resTexts = new string[Math.Max(oldLen, meLen)];

                // translated strings get higher priority
                Array.Copy(me, 0, resTexts, 0, meLen);

                // some strings left
                if (oldLen > meLen)
                    Array.Copy(old, meLen, resTexts, meLen, oldLen - meLen);

                return resTexts;
            }

            return translatedStrings;
        }
    }
}
