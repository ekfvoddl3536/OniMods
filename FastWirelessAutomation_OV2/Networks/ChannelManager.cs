// MIT License
//
// Copyright (c) 2022-2023. SuperComic (ekfvoddl3535@naver.com)
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

namespace FastWirelessAutomation.Networks
{
    public static class ChannelManager
    {
        private static readonly Dictionary<int, ChannelData> channels = new Dictionary<int, ChannelData>();

        public static void OnConnect(IChannelNetwork st)
        {
            int ch = st.Channel;
            var hnd = st.EventHandler;

            if (!channels.ContainsKey(ch))
                CreateChannel(ch);

            channels[ch].OnConnectChanged += hnd;
        }

        public static void OnDisconnect(IChannelNetwork st)
        {
            int ch = st.Channel;
            var hnd = st.EventHandler;

            if (channels.ContainsKey(ch) && hnd != null)
                channels[ch].OnConnectChanged -= hnd;
        }

        public static void OnEmitterConnect(IChannelSignalEmitter st)
        {
            int ch = st.Channel;

            if (!channels.ContainsKey(ch))
                CreateChannel(ch);

            channels[ch].Subscribe(st);
        }

        public static void OnEmitterDisconenct(IChannelSignalEmitter st)
        {
            int ch = st.Channel;

            if (channels.ContainsKey(ch))
                channels[ch].Unsubscribe(st);
        }

        public static void SignalEmit(int ch, bool ison)
        {
            if (!channels.TryGetValue(ch, out ChannelData res) || res.IsDoNotNeedUpdate(ison)) 
                return;

            res.UpdateActivate(ison);
            res.InvokeEvent(ison);
        }

        public static void Reset()
        {
            var e1 = channels.Values.GetEnumerator();

            while (e1.MoveNext())
                e1.Current.Clear();

            channels.Clear();
        }

        public static bool IsChannelOn(int ch) => 
            channels.TryGetValue(ch, out ChannelData res) && res.Active;

        public static void CreateChannel(int ch) =>
            channels.Add(
                ch, 
                (ch & 0x80) == 0
                ? new OrChannelData()
                : new ChannelData()
            );
    }
}
