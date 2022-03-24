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

namespace FastWirelessAutomation.Networks
{
    public unsafe class OrChannelData : ChannelData
    {
        protected int count_;

        public override void UpdateActivate(bool ison)
        {
            // count + 1 OR 0
            count_ = (count_ + 1) * (*(byte*)&ison & 1);

            base.UpdateActivate(ison);
        }

        public override bool IsDoNotNeedUpdate(bool ison)
        {
            int cnt = count_;
            if (cnt == 0)
                return !(active_ ^ ison);

            // ison ? 1 : -1
            // ((1 or 0) << 1) - 1
            cnt += ((*(byte*)&ison & 1) << 1) - 1;

            // 0SuperComicLib.CMath.Max:
            // https://github.com/ekfvoddl3536/0SuperComicLibs/blob/master/0SuperComicLib.Core/Public/__global__/CMath.cs
            ison = cnt > 0;
            cnt *= *(byte*)&ison;

            return (count_ = cnt) != 0;
        }

        public override void Subscribe(IChannelSignalEmitter emitter)
        {
            bool temp = emitter.OperationalState;

            if (count_ == 0)
                ChannelManager.SignalEmit(emitter.Channel, temp);
            else
                count_ += *(byte*)&temp;
        }

        public override void Unsubscribe(IChannelSignalEmitter emitter)
        {
            bool temp = emitter.OperationalState;

            int cnt = count_;
            cnt -= *(byte*)&temp;

            // 0SuperComicLib.CMath.Max:
            // https://github.com/ekfvoddl3536/0SuperComicLibs/blob/master/0SuperComicLib.Core/Public/__global__/CMath.cs
            temp = cnt > 0;
            cnt *= *(byte*)&temp;

            if ((count_ = cnt) == 0)
                ChannelManager.SignalEmit(emitter.Channel, false);
        }
    }
}
