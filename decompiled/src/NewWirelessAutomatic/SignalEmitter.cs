using System;
using KSerialization;
using NetworkManager;
using UnityEngine;

namespace NewWirelessAutomatic
{
	// Token: 0x0200000A RID: 10
	[SerializationConfig(1)]
	public class SignalEmitter : KMonoBehaviour, IIntSliderControl, ISliderControl, IChannelNetworkComponentEX, IChannelNetworkComponent, INetworkComponent
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000252F File Offset: 0x0000072F
		// (set) Token: 0x06000018 RID: 24 RVA: 0x00002537 File Offset: 0x00000737
		public int EmitChannel
		{
			get
			{
				return this.emitCh;
			}
			set
			{
				this.emitCh = value;
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002540 File Offset: 0x00000740
		protected static void OnOperationalChangedStatic(SignalEmitter se, object data)
		{
			se.OnOperationalChangeSafe((bool)data);
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000254E File Offset: 0x0000074E
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.UI.UISIDESCREENS.NEW_WIRELESS_AUTO_SCREEN.TITLE";
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002555 File Offset: 0x00000755
		public string SliderUnits
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000255C File Offset: 0x0000075C
		public float GetSliderMax(int index)
		{
			return 100000f;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002563 File Offset: 0x00000763
		public float GetSliderMin(int index)
		{
			return 0f;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000256A File Offset: 0x0000076A
		public string GetSliderTooltipKey(int index)
		{
			return "STRINGS.UI.UISIDESCREENS.NEW_WIRELESS_AUTO_SCREEN.TOOLTIP";
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002571 File Offset: 0x00000771
		public float GetSliderValue(int index)
		{
			return (float)this.emitCh;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000257A File Offset: 0x0000077A
		public void SetSliderValue(float percent, int index)
		{
			this.ChangeChannel(Mathf.RoundToInt(percent));
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002588 File Offset: 0x00000788
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000022 RID: 34 RVA: 0x0000258B File Offset: 0x0000078B
		public int GetChannel
		{
			get
			{
				return this.emitCh;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002593 File Offset: 0x00000793
		// (set) Token: 0x06000024 RID: 36 RVA: 0x0000259B File Offset: 0x0000079B
		public bool GetSignal { get; protected set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000025A4 File Offset: 0x000007A4
		public ConnectChangeEventHandler GetEventHandler
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000025A7 File Offset: 0x000007A7
		public bool GetOperationalState
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000025B4 File Offset: 0x000007B4
		protected virtual void OnOperationalChangeSafe(bool on)
		{
			base.GetComponent<KBatchedAnimController>().Play(on ? "on_pst" : "off", 0, 1f, 0f);
			WirelessMgr.NetSystem.SignalEmit(this.emitCh, on);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000025F1 File Offset: 0x000007F1
		protected virtual void ChangeChannel(int ch)
		{
			WirelessMgr.NetSystem.OnEmitterDisconenct(this);
			this.emitCh = ch;
			WirelessMgr.NetSystem.OnEmitterConnect(this);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002610 File Offset: 0x00000810
		protected override void OnPrefabInit()
		{
			base.Subscribe<SignalEmitter>(-592767678, SignalEmitter.OnOperationalChangedDelegate);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002623 File Offset: 0x00000823
		protected override void OnSpawn()
		{
			WirelessMgr.NetSystem.OnEmitterConnect(this);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002630 File Offset: 0x00000830
		protected override void OnCleanUp()
		{
			WirelessMgr.NetSystem.OnEmitterDisconenct(this);
			base.Unsubscribe<SignalEmitter>(-592767678, SignalEmitter.OnOperationalChangedDelegate, false);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000264E File Offset: 0x0000084E
		public bool PreSubscribe(INetworkMgr<IChannelNetworkComponent> sender, object args)
		{
			return false;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002651 File Offset: 0x00000851
		public void PostSubscribe(INetworkMgr<IChannelNetworkComponent> sender, object args)
		{
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002653 File Offset: 0x00000853
		public bool PreUnsubscribe(INetworkMgr<IChannelNetworkComponent> sender, object args)
		{
			return false;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002656 File Offset: 0x00000856
		public void PostUnsubscribe(INetworkMgr<IChannelNetworkComponent> sender, object args)
		{
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002658 File Offset: 0x00000858
		public string GetSliderTooltip()
		{
			return "채널을 지정해주세요.";
		}

		// Token: 0x04000012 RID: 18
		protected static EventSystem.IntraObjectHandler<SignalEmitter> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SignalEmitter>(new Action<SignalEmitter, object>(SignalEmitter.OnOperationalChangedStatic));

		// Token: 0x04000013 RID: 19
		[MyCmpAdd]
		protected Operational operational;

		// Token: 0x04000014 RID: 20
		[Serialize]
		protected int emitId;

		// Token: 0x04000015 RID: 21
		[Serialize]
		protected int emitCh;
	}
}
