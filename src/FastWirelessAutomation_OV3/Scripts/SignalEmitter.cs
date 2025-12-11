// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using FastWirelessAutomation.Networks;
using KSerialization;

namespace FastWirelessAutomation;

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
