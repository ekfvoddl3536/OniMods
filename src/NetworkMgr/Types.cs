namespace NetworkManager
{
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

    public delegate void ConnectChangeEventHandler(INetworkMgr<IChannelNetworkComponent> sender, bool signal);

    public interface IChannelNetworkComponent : INetworkComponent
    {
        bool GetSignal { get; }
        ConnectChangeEventHandler GetEventHandler { get; }
    }
}
