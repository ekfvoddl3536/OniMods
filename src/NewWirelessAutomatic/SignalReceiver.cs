using UnityEngine;
using KSerialization;
using NetworkManager;

namespace NewWirelessAutomatic
{
    using static EventSystem;
    using static WirelessMgr;
    using static NWA_CONST;
    [SerializationConfig(MemberSerialization.OptIn)]
    public class SignalReceiver : KMonoBehaviour, IIntSliderControl, IChannelNetworkComponent
    {
        protected static IntraObjectHandler<SignalReceiver> OnOperationalChangedDelegate =
            new IntraObjectHandler<SignalReceiver>(OnOperationalChangedStatic);

        [MyCmpAdd]
        protected Operational operational;
        [Serialize]
        protected int receiveCh;

        public int ReceiveChannel { get => receiveCh; set => receiveCh = value; }

        protected static void OnOperationalChangedStatic(SignalReceiver se, object data) => se.OnOperationalChange(data);

        protected virtual void OnOperationalChange(object data) { }

        protected override void OnPrefabInit() => Subscribe(OperationalChanged, OnOperationalChangedDelegate);

        protected override void OnSpawn() => ChangeChannel(receiveCh);

        protected override void OnCleanUp()
        {
            NetSystem.OnDisconnect(this);
            Unsubscribe(OperationalChanged, OnOperationalChangedDelegate, false);
        }

        protected virtual void UpdateAnim(bool abc)  => 
            GetComponent<KBatchedAnimController>().Play(abc ? "on_pst" : "off", KAnim.PlayMode.Loop);

        #region 슬라이더
        public string SliderTitleKey => TitleKey;

        public string SliderUnits => string.Empty;

        public float GetSliderMax(int index) => SliderMax;

        public float GetSliderMin(int index) => 0;

        public string GetSliderTooltipKey(int index) => ToolTipKey;

        public float GetSliderValue(int index) => receiveCh;

        public void SetSliderValue(float percent, int index) => ChangeChannel(Mathf.RoundToInt(percent));

        public int SliderDecimalPlaces(int index) => 0;
        #endregion

        #region 채널
        public int GetChannel => receiveCh;

        public bool GetSignal => false;

        public ConnectChangeEventHandler GetEventHandler => ChangeState;

        protected void ChangeState(INetworkMgr<IChannelNetworkComponent> sender, bool on) => UpdateState(on);
        #endregion

        protected virtual void UpdateState(bool on)
        {
            UpdateAnim(on);
            GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);

            GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power,
                on ? Db.Get().BuildingStatusItems.SwitchStatusActive : Db.Get().BuildingStatusItems.SwitchStatusInactive);
        }

        protected virtual void ChangeChannel(int ch)
        {
            NetSystem.OnDisconnect(this);

            receiveCh = ch;
            UpdateState(NetSystem.IsChannelOn(receiveCh));

            NetSystem.OnConnect(this);
        }
    }
}
