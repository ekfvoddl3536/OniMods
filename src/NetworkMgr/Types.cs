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

    public class OrChannelData : ChannelData
    {
        protected short Emitters;
        protected short Count;

        public override void UpdateActivate(bool ison)
        {
            if (ison) Count++;
            else Count = 0;
        }

        public override bool IsDoNotNeedUpdate(bool on)
        {
            if (Count > 0 && !on)
            {
                int temp = Count - 1;
                if (temp == 0)
                    return false;
                else
                    Count--;
            }
            else
                return false;
            return true;
        }

        public override void Subscribe(ChannelNetworkMgrBase mgr, IChannelNetworkComponentEX emitter)
        {
            if (emitter.PreSubscribe(mgr, Emitters)) return;
            if (Emitters == 0)
            {
                Activate = emitter.GetOperationalState;
                if (Activate)
                    Count++;
                mgr.SignalEmit(emitter.GetChannel, Activate);
            }
            else if (Count == 0 && emitter.GetOperationalState)
            {
                Count++;
                mgr.SignalEmit(emitter.GetChannel, true);
            }
            Emitters++;
            emitter.PostSubscribe(mgr, Emitters);
        }

        public override void Unsubscribe(ChannelNetworkMgrBase mgr, IChannelNetworkComponentEX emitter)
        {
            if (emitter.PreSubscribe(mgr, Emitters) || Emitters == 0) return;
            else if (emitter.GetOperationalState)
                Count--;
            Emitters--;
            if (Emitters == 0)
                Activate = false;
            emitter.PostUnsubscribe(mgr, Emitters);
        }
    }
}
