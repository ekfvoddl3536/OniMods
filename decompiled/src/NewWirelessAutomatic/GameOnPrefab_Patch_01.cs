using System;
using Harmony;
using NetworkManager;

namespace NewWirelessAutomatic
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(Game), "OnPrefabInit")]
	public class GameOnPrefab_Patch_01
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000021CC File Offset: 0x000003CC
		public static void Prefix()
		{
			if (WirelessMgr.NetSystem == null)
			{
				WirelessMgr.NetSystem = new ChannelNetworkMgrBase();
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021DF File Offset: 0x000003DF
		public static void Postfix()
		{
			WirelessMgr.NetSystem.Reset();
		}
	}
}
