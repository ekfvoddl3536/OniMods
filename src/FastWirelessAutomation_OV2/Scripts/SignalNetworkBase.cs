// MIT License
//
// Copyright (c) 2022-2023. SuperComic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using KSerialization;
using FastWirelessAutomation.Networks;
using System;
using UnityEngine;

namespace FastWirelessAutomation
{
    using static EventSystem;
    using static GlobalConsts;
    [SerializationConfig(MemberSerialization.OptIn)]
    public abstract class SignalNetworkBase : KMonoBehaviour, IIntSliderControl, IChannelNetwork, ISaveLoadable
    {
        #region 정적
        protected static readonly string[] animList = new[] { "off", "on_pst", "on", "on_pre" };
        protected static readonly IntraObjectHandler<SignalNetworkBase> OnOperChangedDele = new IntraObjectHandler<SignalNetworkBase>(OnOperChngSTATIC);
        protected static readonly IntraObjectHandler<SignalNetworkBase> OnCopySettingsDele = new IntraObjectHandler<SignalNetworkBase>(OnCopySettingsSTATIC);

        private static void OnOperChngSTATIC(SignalNetworkBase inst, object data) => inst.OnOperationalChanged((bool)data);
        private static void OnCopySettingsSTATIC(SignalNetworkBase inst, object data) => inst.OnCopySettings((GameObject)data);
        #endregion

        #region 필드
        protected Operational operational_;
        protected LogicPorts logicports_;
        [Serialize]
        public int channel_ = 128;
        #endregion

        #region 초기화 & 스폰 & 클린
        protected override void OnPrefabInit()
        {
            Subscribe((int)GameHashes.OperationalChanged, OnOperChangedDele);
            Subscribe((int)GameHashes.CopySettings, OnCopySettingsDele);
        }

        protected override void OnSpawn()
        {
            operational_ = gameObject.AddOrGet<Operational>();
            logicports_ = gameObject.AddOrGet<LogicPorts>();

            SetChannel(channel_);
        }

        protected override void OnCleanUp()
        {
            Unsubscribe((int)GameHashes.OperationalChanged, OnOperChangedDele);
            Unsubscribe((int)GameHashes.CopySettings, OnCopySettingsDele);
        }
        #endregion

        #region 슬라이더
        public int SliderDecimalPlaces(int _) => 0;

        public float GetSliderMin(int _) => 0;

        public float GetSliderMax(int _) => MAX_CHANNEL;

        public float GetSliderValue(int _) => channel_;

        public void SetSliderValue(float percent, int _) => SetChannel((int)percent);

        public string GetSliderTooltipKey(int _) => TOOLTIP_KEY;
        public string GetSliderTooltip(int _) => Strings.Get(TOOLTIP_KEY).String;

        public string SliderTitleKey => TITLE_KEY;
        public string SliderUnits => string.Empty;
        #endregion

        #region 네트워크
        public int Channel => channel_ & ushort.MaxValue;

        public abstract Action<bool> EventHandler { get; }

        protected abstract void SetChannel(int new_channel);
        #endregion

        protected virtual unsafe void OnOperationalChanged(bool on)
        {
            var controller = GetComponent<KBatchedAnimController>();

            int idx = (*(byte*)&on & 1) << 1; // 0 or 2

            controller.Play(animList[idx + 1]);
            controller.Queue(animList[idx]);
        }

        protected virtual void OnCopySettings(GameObject go)
        {
            var network = go.GetComponent<SignalNetworkBase>();
            if (network != null && channel_ != network.channel_)
                SetChannel(network.channel_);
        }
    }
}
