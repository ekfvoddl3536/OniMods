﻿// MIT License
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

using KSerialization;
using FastWirelessAutomation.Networks;
using System;

namespace FastWirelessAutomation
{
    using static ChannelManager;
    [SerializationConfig(MemberSerialization.OptIn)]
    public class SignalEmitter : SignalNetworkBase, IChannelSignalEmitter
    {
        public bool OperationalState => operational_.IsOperational;

        public override Action<bool> EventHandler => null;

        protected override void OnCleanUp()
        {
            OnEmitterDisconenct(this);

            base.OnCleanUp();
        }

        protected override void SetChannel(int new_channel)
        {
            OnEmitterDisconenct(this);
            
            channel_ = new_channel;
            OnEmitterConnect(this);
        }

        protected override void OnOperationalChanged(bool on)
        {
            base.OnOperationalChanged(on);

            SignalEmit(channel_, on);
        }
    }
}
