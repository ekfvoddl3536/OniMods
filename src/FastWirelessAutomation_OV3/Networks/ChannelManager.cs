// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace FastWirelessAutomation.Networks;

public static class ChannelManager
{
    private static readonly Dictionary<int, ChannelData> channels = new Dictionary<int, ChannelData>();

    public static void OnConnect(IChannelNetwork st)
    {
        int ch = st.Channel;
        var hnd = st.EventHandler;

        if (!channels.ContainsKey(ch))
            CreateChannel(ch);

        channels[ch].OnConnectChanged += hnd;
    }

    public static void OnDisconnect(IChannelNetwork st)
    {
        int ch = st.Channel;
        var hnd = st.EventHandler;

        if (channels.ContainsKey(ch) && hnd != null)
            channels[ch].OnConnectChanged -= hnd;
    }

    public static void OnEmitterConnect(IChannelSignalEmitter st)
    {
        int ch = st.Channel;

        if (!channels.ContainsKey(ch))
            CreateChannel(ch);

        channels[ch].Subscribe(st);
    }

    public static void OnEmitterDisconenct(IChannelSignalEmitter st)
    {
        int ch = st.Channel;

        if (channels.ContainsKey(ch))
            channels[ch].Unsubscribe(st);
    }

    public static void SignalEmit(int ch, bool ison)
    {
        if (!channels.TryGetValue(ch, out ChannelData res) || res.IsDoNotNeedUpdate(ison))
            return;

        res.UpdateActivate(ison);
        res.InvokeEvent(ison);
    }

    public static void Reset()
    {
        var e1 = channels.Values.GetEnumerator();

        while (e1.MoveNext())
            e1.Current.Clear();

        channels.Clear();
    }

    public static bool IsChannelOn(int ch) =>
        channels.TryGetValue(ch, out ChannelData res) && res.Active;

    public static void CreateChannel(int ch) =>
        channels.Add(
            ch,
            (ch & 0x80) == 0
            ? new OrChannelData()
            : new ChannelData()
        );
}
