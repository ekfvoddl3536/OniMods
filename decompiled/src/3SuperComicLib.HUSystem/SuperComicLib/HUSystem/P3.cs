using System;
using Harmony;

namespace SuperComicLib.HUSystem
{
	// Token: 0x0200001B RID: 27
	[HarmonyPatch(typeof(Game), "OnPrefabInit")]
	public class P3
	{
		// Token: 0x0600007D RID: 125 RVA: 0x000049B1 File Offset: 0x00002BB1
		public static void Postfix()
		{
			Constants.hupipeSystem = new UtilityNetworkManager<HUNetwork, HUPipe>(Grid.WidthInCells, Grid.HeightInCells, 2);
			Constants.manager = new HUPipeManager();
		}
	}
}
