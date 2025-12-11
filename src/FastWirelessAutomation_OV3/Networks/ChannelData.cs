// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace FastWirelessAutomation.Networks;

public class ChannelData
{
    protected bool active_;
    public event Action<bool> OnConnectChanged;

    public bool Active => active_;

    public void Clear() => OnConnectChanged = null;

    public virtual void UpdateActivate(bool ison) => active_ = ison;

    public virtual bool IsDoNotNeedUpdate(bool ison) => active_ == ison;

    public virtual void Subscribe(IChannelSignalEmitter emitter)
    {
    }

    public virtual void Unsubscribe(IChannelSignalEmitter emitter)
    {
    }

    public void InvokeEvent(bool sig) => OnConnectChanged?.Invoke(sig);
}
