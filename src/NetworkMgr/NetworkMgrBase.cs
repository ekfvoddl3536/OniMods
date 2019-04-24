using System;
using System.Collections.Generic;

namespace NetworkManager
{
    public abstract class NetworkMgrBase<T> : INetworkMgr<T> where T : INetworkComponent
    {
        public abstract bool IsConnected(T st, T ed);

        public abstract void OnConnect(T st);

        public abstract void OnDisconnect(T st);
    }

    public class ChannelNetworkMgrBase : NetworkMgrBase<IChannelNetworkComponent>
    {
        protected Dictionary<int, ChannelData> channels;

        public ChannelNetworkMgrBase() => channels = new Dictionary<int, ChannelData>();

        public override bool IsConnected(IChannelNetworkComponent st, IChannelNetworkComponent ed) => st.GetChannel == ed.GetChannel;

        public override void OnConnect(IChannelNetworkComponent st)
        {
            int ch = st.GetChannel;
            ConnectChangeEventHandler hnd = st.GetEventHandler;

            if (!channels.ContainsKey(ch))
                channels.Add(ch, new ChannelData());

            channels[ch].EventHandlers.Add(hnd);
        }

        public override void OnDisconnect(IChannelNetworkComponent st)
        {
            int ch = st.GetChannel;
            ConnectChangeEventHandler hnd = st.GetEventHandler;

            if (channels.ContainsKey(ch) && hnd != null)
                channels[ch].EventHandlers.Remove(hnd);
        }

        public virtual void SetSignalEmit(IChannelNetworkComponent st)
        {
            if (st.GetEventHandler == null)
                SignalEmit(st.GetChannel, st.GetSignal);
        }

        public virtual void SignalEmit(int ch, bool ison)
        {
            if (!channels.ContainsKey(ch)) return;
            
            channels[ch].Activate = ison;
            
            foreach (ConnectChangeEventHandler x in channels[ch].EventHandlers)
                x.Invoke(this, ison);
        }

        public virtual void Reset()
        {
            foreach (KeyValuePair<int, ChannelData> x in channels)
                x.Value.Clear();
            channels.Clear();
            GC.Collect(0, GCCollectionMode.Forced);
        }

        public virtual bool IsChannelOn(int ch) => channels.TryGetValue(ch, out ChannelData res) ? res.Activate : false;
    }
}
