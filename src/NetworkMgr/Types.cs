using System;

namespace NetworkManager
{
    public delegate void ConnectChangeEventHandler(INetworkMgr<IChannelNetworkComponent> sender, bool signal);

    public interface INetworkComponent
    {
        int GetChannel { get; }
    }

    public interface INetworkMgr<T> where T : INetworkComponent
    {
        bool IsConnected(T st, T ed);
        void OnConnect(T st);
        void OnDisconnect(T st);
    }

    public interface IChannelNetworkComponent : INetworkComponent
    {
        bool GetSignal { get; }
        ConnectChangeEventHandler GetEventHandler { get; }
    }

    public interface IChannelNetworkComponentEX : IChannelNetworkComponent
    {
        bool GetOperationalState { get; }
        bool PreSubscribe(INetworkMgr<IChannelNetworkComponent> sender, object args);
        void PostSubscribe(INetworkMgr<IChannelNetworkComponent> sender, object args);
        bool PreUnsubscribe(INetworkMgr<IChannelNetworkComponent> sender, object args);
        void PostUnsubscribe(INetworkMgr<IChannelNetworkComponent> sender, object args);
    }
}
