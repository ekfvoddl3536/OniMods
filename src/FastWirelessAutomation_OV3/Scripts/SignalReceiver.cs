// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using FastWirelessAutomation.Networks;
using KSerialization;

namespace FastWirelessAutomation;

using static ChannelManager;

[SerializationConfig(MemberSerialization.OptIn)]
public class SignalReceiver : SignalNetworkBase
{
    [NonSerialized]
    protected Action<bool> on_updateState_cb;

    protected override void OnSpawn()
    {
        on_updateState_cb = OnUpdateState;
        base.OnSpawn();
    }

    protected override void OnCleanUp()
    {
        OnDisconnect(this);

        base.OnCleanUp();
    }

    public override Action<bool> EventHandler => on_updateState_cb;

    protected virtual unsafe void OnUpdateState(bool on)
    {
        // update anim
        OnOperationalChanged(on);

        logicports_.SendSignal(LogicSwitch.PORT_ID, *(byte*)&on & 1);

        var db = Db.Get();
        GetComponent<KSelectable>().SetStatusItem(
            db.StatusItemCategories.Power,
            on
            ? db.BuildingStatusItems.SwitchStatusActive
            : db.BuildingStatusItems.SwitchStatusInactive);
    }

    protected override void SetChannel(int new_channel)
    {
        OnDisconnect(this);

        channel_ = new_channel;
        OnUpdateState(IsChannelOn(new_channel));

        OnConnect(this);
    }
}
