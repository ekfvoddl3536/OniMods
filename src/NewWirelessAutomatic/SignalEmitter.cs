using KSerialization;
using UnityEngine;
using NetworkManager;

namespace NewWirelessAutomatic
{
    using static EventSystem;
    using static WirelessMgr;
    using static NWA_CONST;
    [SerializationConfig(MemberSerialization.OptIn)]
    public class SignalEmitter : KMonoBehaviour, IIntSliderControl, IChannelNetworkComponent
    {
        protected static IntraObjectHandler<SignalEmitter> OnOperationalChangedDelegate =
            new IntraObjectHandler<SignalEmitter>(OnOperationalChangedStatic);

        [MyCmpAdd]
        protected Operational operational;
        [Serialize]
        protected int emitId;
        [Serialize]
        protected int emitCh;

        public int EmitChannel { get => emitCh; set => emitCh = value; }

        protected static void OnOperationalChangedStatic(SignalEmitter se, object data) => se.OnOperationalChange(data);

        #region 슬라이더
        public string SliderTitleKey => TitleKey;

        public string SliderUnits => string.Empty;

        public float GetSliderMax(int index) => SliderMax;

        public float GetSliderMin(int index) => 0;

        public string GetSliderTooltipKey(int index) => ToolTipKey;

        public float GetSliderValue(int index) => emitCh;

        public void SetSliderValue(float percent, int index) => ChangeChannel(Mathf.RoundToInt(percent));

        public int SliderDecimalPlaces(int index) => 0;
        #endregion

        #region 네트워크
        public int GetChannel => emitCh;

        public bool GetSignal { get; protected set; }

        public ConnectChangeEventHandler GetEventHandler => null;
        #endregion

        #region 본 함수
        protected virtual void OnOperationalChange(object data) => OnOperationalChangeSafe((bool)data);

        protected virtual void OnOperationalChangeSafe(bool on)
        {
            GetComponent<KBatchedAnimController>().Play(on ? "on_pst" : "off", KAnim.PlayMode.Loop);
            NetSystem.SignalEmit(emitCh, on);
        }

        protected virtual void ChangeChannel(int ch) => emitCh = ch;

        protected override void OnPrefabInit() => Subscribe(OperationalChanged, OnOperationalChangedDelegate);

        protected override void OnSpawn() => OnOperationalChangeSafe(operational.IsOperational);

        protected override void OnCleanUp() => Unsubscribe(OperationalChanged, OnOperationalChangedDelegate);
        #endregion
    }
}
