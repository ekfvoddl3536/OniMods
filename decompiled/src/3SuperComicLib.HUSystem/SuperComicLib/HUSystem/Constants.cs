using System;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000004 RID: 4
	public static class Constants
	{
		// Token: 0x04000001 RID: 1
		public const int layer = 2;

		// Token: 0x04000002 RID: 2
		public static UtilityNetworkManager<HUNetwork, HUPipe> hupipeSystem;

		// Token: 0x04000003 RID: 3
		public static HUPipeManager manager;

		// Token: 0x04000004 RID: 4
		public static readonly Operational.Flag connectedFlag = new Operational.Flag("HUPipeConnected", 0);
	}
}
