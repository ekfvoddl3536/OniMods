// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace FastWirelessAutomation.Networks;

public interface INetworkComponent
{
    int Channel { get; }
}

public interface IChannelNetwork : INetworkComponent
{
    Action<bool> EventHandler { get; }
}

public interface IChannelSignalEmitter : IChannelNetwork
{
    bool OperationalState { get; }
}
