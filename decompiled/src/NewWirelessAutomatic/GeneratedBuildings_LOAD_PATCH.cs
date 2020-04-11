using System;
using Harmony;
using NetworkManager;

namespace NewWirelessAutomatic
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	public class GeneratedBuildings_LOAD_PATCH
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000021F4 File Offset: 0x000003F4
		public static void Prefix()
		{
			GeneratedBuildings_LOAD_PATCH.Stpre(NWA_CONST.Emitter.ID_UPPER, "자동화 신호 송신기 (E)", "입력받은 자동화 신호를 지정된 채널로 송신합니다.", "미래식 자동화 제어, 신호는 (E)에서 (R)로 향합니다.");
			GeneratedBuildings_LOAD_PATCH.Stpre(NWA_CONST.Receiver.ID_UPPER, "자동화 신호 수신기 (R)", "지정된 채널로 부터 신호를 수신받습니다.", "미래식 자동화 제어, 신호는 (E)에서 (R)로 향합니다.");
			Strings.Add(new string[]
			{
				"STRINGS.UI.UISIDESCREENS.NEW_WIRELESS_AUTO_SCREEN.TOOLTIP",
				"채널을 지정해주세요."
			});
			Strings.Add(new string[]
			{
				"STRINGS.UI.UISIDESCREENS.NEW_WIRELESS_AUTO_SCREEN.TITLE",
				"채널"
			});
			ModUtil.AddBuildingToPlanScreen("Automation", "WirelessSignalEmitter");
			ModUtil.AddBuildingToPlanScreen("Automation", "WirelessSignalReceiver");
			if (WirelessMgr.NetSystem == null)
			{
				WirelessMgr.NetSystem = new ChannelNetworkMgrBase();
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022A4 File Offset: 0x000004A4
		private static void Stpre(string idupper, string name, string desc, string effc)
		{
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS." + idupper + ".NAME",
				name
			});
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS." + idupper + ".DESC",
				desc
			});
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS." + idupper + ".EFFECT",
				effc
			});
		}
	}
}
