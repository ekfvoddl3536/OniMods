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

#pragma warning disable IDE1006
using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System.Collections.Generic;

namespace LazySaver.Options
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class ModConfig : IOptions
    {
        [JsonProperty, Option(
            "Enable forced delay",
            "Enabling this feature will result in longer save times and will maintain a higher average frame rate",
            "Advanced")]
        public bool isForcedDelay { get; set; } = true;

        [JsonProperty, Option(
            "Forced delay period (ms)",
            "If you set this value large, the average frame rate increases. If set low, the save will be completed faster",
            "Advanced")]
        [Limit(20, 2000)]
        public int delayDuration { get; set; } = 100;

        IEnumerable<IOptionsEntry> IOptions.CreateOptions() => null;
        void IOptions.OnOptionsChanged() => GlobalHeader.option = this;
    }
}
