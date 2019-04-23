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
        protected Dictionary<int, HashSet<ConnectChangeEventHandler>> channels;
        protected Dictionary<int, bool> onChannels;

        public ChannelNetworkMgrBase()
        {
            channels = new Dictionary<int, HashSet<ConnectChangeEventHandler>>();
            onChannels = new Dictionary<int, bool>();
        }

        public override bool IsConnected(IChannelNetworkComponent st, IChannelNetworkComponent ed) => st.GetChannel == ed.GetChannel;

        public override void OnConnect(IChannelNetworkComponent st)
        {
            int ch = st.GetChannel;
            ConnectChangeEventHandler hnd = st.GetEventHandler;

            if (!channels.ContainsKey(ch))
                channels.Add(ch, new HashSet<ConnectChangeEventHandler>());

            channels[ch].Add(hnd);
        }

        public override void OnDisconnect(IChannelNetworkComponent st)
        {
            int ch = st.GetChannel;
            ConnectChangeEventHandler hnd = st.GetEventHandler;

            if (channels.ContainsKey(ch) && hnd != null)
                channels[ch].Remove(hnd);
        }

        public virtual void SetSignalEmit(IChannelNetworkComponent st)
        {
            if (st.GetEventHandler == null)
                SignalEmit(st.GetChannel, st.GetSignal);
        }

        public virtual void SignalEmit(int ch, bool ison)
        {
            if (!channels.ContainsKey(ch)) return;

            if (!onChannels.ContainsKey(ch)) onChannels.Add(ch, ison);
            else onChannels[ch] = ison;

            // Parallel.ForEach(channels[ch], option, x => x.Invoke(this, ison));
            foreach (ConnectChangeEventHandler x in channels[ch])
                x.Invoke(this, ison);
        }

        public virtual void Reset()
        {
            onChannels.Clear();
            foreach (KeyValuePair<int, HashSet<ConnectChangeEventHandler>> x in channels)
                x.Value.Clear();
            channels.Clear();
            GC.Collect(0, GCCollectionMode.Forced);
        }

        public virtual bool IsChannelOn(int ch) => onChannels.TryGetValue(ch, out bool res) ? res : false;
    }
}
