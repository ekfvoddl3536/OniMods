// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace FastWirelessAutomation.Networks;

public unsafe class OrChannelData : ChannelData
{
    protected int count_;

    public override void UpdateActivate(bool ison)
    {
        // count + 1 OR 0
        count_ = (count_ + 1) * (*(byte*)&ison & 1);

        base.UpdateActivate(ison);
    }

    public override bool IsDoNotNeedUpdate(bool ison)
    {
        int cnt = count_;
        if (cnt == 0)
            return !(active_ ^ ison);

        // ison ? 1 : -1
        // ((1 or 0) << 1) - 1
        cnt += ((*(byte*)&ison & 1) << 1) - 1;

        // 0SuperComicLib.CMath.Max:
        // https://github.com/ekfvoddl3536/0SuperComicLibs/blob/master/0SuperComicLib.Core/Public/__global__/CMath.cs
        ison = cnt > 0;
        cnt *= *(byte*)&ison;

        return (count_ = cnt) != 0;
    }

    public override void Subscribe(IChannelSignalEmitter emitter)
    {
        bool temp = emitter.OperationalState;

        if (count_ == 0)
            ChannelManager.SignalEmit(emitter.Channel, temp);
        else
            count_ += *(byte*)&temp;
    }

    public override void Unsubscribe(IChannelSignalEmitter emitter)
    {
        bool temp = emitter.OperationalState;

        int cnt = count_;
        cnt -= *(byte*)&temp;

        // 0SuperComicLib.CMath.Max:
        // https://github.com/ekfvoddl3536/0SuperComicLibs/blob/master/0SuperComicLib.Core/Public/__global__/CMath.cs
        cnt = Math.Max(cnt, 0);

        if ((count_ = cnt) == 0)
            ChannelManager.SignalEmit(emitter.Channel, false);
    }
}
