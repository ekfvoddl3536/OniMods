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
    
    public class ChannelData : IDisposable
    {
        public bool Activate { get; protected set; }
        public HashSet<ConnectChangeEventHandler> EventHandlers { get; protected set; }

        public ChannelData() => EventHandlers = new HashSet<ConnectChangeEventHandler>();

        public virtual void Clear()
        {
            EventHandlers.Clear();
            GC.Collect(0, GCCollectionMode.Forced);
        }

        public static implicit operator HashSet<ConnectChangeEventHandler>(ChannelData data) => data.EventHandlers;

        public void Dispose() => Clear();
    }
}
