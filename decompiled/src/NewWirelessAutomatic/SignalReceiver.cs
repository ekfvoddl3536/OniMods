using System;
using KSerialization;
using NetworkManager;
using UnityEngine;

namespace NewWirelessAutomatic
{
	// Token: 0x0200000B RID: 11
	[SerializationConfig(1)]
	public class SignalReceiver : KMonoBehaviour, IIntSliderControl, ISliderControl, IChannelNetworkComponent, INetworkComponent
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000033 RID: 51 RVA: 0x0000267F File Offset: 0x0000087F
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002687 File Offset: 0x00000887
		public int ReceiveChannel
		{
			get
			{
				return this.receiveCh;
			}
			set
			{
				this.receiveCh = value;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002690 File Offset: 0x00000890
		protected override void OnSpawn()
		{
			this.ChangeChannel(this.receiveCh);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000269E File Offset: 0x0000089E
		protected override void OnCleanUp()
		{
			WirelessMgr.NetSystem.OnDisconnect(this);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000026AB File Offset: 0x000008AB
		protected virtual void UpdateAnim(bool abc)
		{
			base.GetComponent<KBatchedAnimController>().Play(abc ? "on_pst" : "off", 0, 1f, 0f);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000026D7 File Offset: 0x000008D7
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.UI.UISIDESCREENS.NEW_WIRELESS_AUTO_SCREEN.TITLE";
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000026DE File Offset: 0x000008DE
		public string SliderUnits
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000026E5 File Offset: 0x000008E5
		public float GetSliderMax(int index)
		{
			return 100000f;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000026EC File Offset: 0x000008EC
		public float GetSliderMin(int index)
		{
			return 0f;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000026F3 File Offset: 0x000008F3
		public string GetSliderTooltipKey(int index)
		{
			return "STRINGS.UI.UISIDESCREENS.NEW_WIRELESS_AUTO_SCREEN.TOOLTIP";
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000026FA File Offset: 0x000008FA
		public float GetSliderValue(int index)
		{
			return (float)this.receiveCh;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002703 File Offset: 0x00000903
		public void SetSliderValue(float percent, int index)
		{
			this.ChangeChannel(Mathf.RoundToInt(percent));
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002711 File Offset: 0x00000911
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002714 File Offset: 0x00000914
		public int GetChannel
		{
			get
			{
				return this.receiveCh;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000041 RID: 65 RVA: 0x0000271C File Offset: 0x0000091C
		public bool GetSignal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000271F File Offset: 0x0000091F
		public ConnectChangeEventHandler GetEventHandler
		{
			get
			{
				return new ConnectChangeEventHandler(this.ChangeState);
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000272D File Offset: 0x0000092D
		protected void ChangeState(INetworkMgr<IChannelNetworkComponent> sender, bool on)
		{
			this.UpdateState(on);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002738 File Offset: 0x00000938
		protected virtual void UpdateState(bool on)
		{
			this.UpdateAnim(on);
			this.logic.SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, on ? Db.Get().BuildingStatusItems.SwitchStatusActive : Db.Get().BuildingStatusItems.SwitchStatusInactive, null);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000027A2 File Offset: 0x000009A2
		protected virtual void ChangeChannel(int ch)
		{
			WirelessMgr.NetSystem.OnDisconnect(this);
			this.receiveCh = ch;
			this.UpdateState(WirelessMgr.NetSystem.IsChannelOn(this.receiveCh));
			WirelessMgr.NetSystem.OnConnect(this);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000027D7 File Offset: 0x000009D7
		public string GetSliderTooltip()
		{
			return "채널을 지정해주세요.";
		}

		// Token: 0x04000017 RID: 23
		[MyCmpAdd]
		protected Operational operational;

		// Token: 0x04000018 RID: 24
		[MyCmpReq]
		protected LogicPorts logic;

		// Token: 0x04000019 RID: 25
		[Serialize]
		protected int receiveCh;
	}
}
