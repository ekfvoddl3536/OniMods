using System;

namespace NetworkManager
{
    public class ChannelData
    {
        public bool Activate { get; protected set; }
        public event ConnectChangeEventHandler EventHandlers;

        public virtual void Clear()
        {
            EventHandlers = null;
            GC.Collect(0, GCCollectionMode.Forced);
        }

        public virtual void UpdateActivate(bool ison) => Activate = ison;

        public virtual bool IsActivate() => Activate;

        public virtual bool IsDoNotNeedUpdate(bool on) => Activate == on;

        public virtual void Subscribe(ChannelNetworkMgrBase mgr, IChannelNetworkComponentEX emitter) { }

        public virtual void Unsubscribe(ChannelNetworkMgrBase mgr, IChannelNetworkComponentEX emitter) { }

        public virtual void InvokeEvent(INetworkMgr<IChannelNetworkComponent> sender, bool sig) => EventHandlers?.Invoke(sender, sig);
    }
}
