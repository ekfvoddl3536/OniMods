using System;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000006 RID: 6
	[Flags]
	public enum HUPipeTypes : byte
	{
		// Token: 0x04000006 RID: 6
		None = 0,
		// Token: 0x04000007 RID: 7
		InputOnly = 1,
		// Token: 0x04000008 RID: 8
		OutputOnly = 2,
		// Token: 0x04000009 RID: 9
		InOutput = 3
	}
}
