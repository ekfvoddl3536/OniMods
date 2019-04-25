namespace NetworkManager
{
    public class OrChannelData : ChannelData
    {
        protected short Count;

        public override void UpdateActivate(bool ison)
        {
            if (ison) Count++;
            else Count = 0;
            base.UpdateActivate(ison);
        }

        public override bool IsDoNotNeedUpdate(bool on)
        {
            if (Count == 0) return Activate ^ !on;
            else if (!on)
                if (Count - 1 == 0) return false;
                else Count--;
            else Count++;
            return true;
        }

        public override void Subscribe(ChannelNetworkMgrBase mgr, IChannelNetworkComponentEX emitter)
        {
            if (emitter.PreSubscribe(mgr, Count)) return;
            if (Count == 0)
                mgr.SignalEmit(emitter.GetChannel, emitter.GetOperationalState);
            else if (emitter.GetOperationalState)
                Count++;
            emitter.PostSubscribe(mgr, Count);
        }

        public override void Unsubscribe(ChannelNetworkMgrBase mgr, IChannelNetworkComponentEX emitter)
        {
            if (emitter.PreUnsubscribe(mgr, Count) || Count == 0) return;
            if (emitter.GetOperationalState)
                Count--;
            if (Count == 0)
                mgr.SignalEmit(emitter.GetChannel, false);
            emitter.PostUnsubscribe(mgr, Count);
        }
    }
}
