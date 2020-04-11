using System;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000025 RID: 37
	public interface IHUGenerator : IHUSim200ms, IHUOverlayUpdate, IHaveHUCell
	{
		// Token: 0x06000091 RID: 145
		int GenerateHeat(float dt);
	}
}
