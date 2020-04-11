using System;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000026 RID: 38
	public interface IHUConsumer : IHUSim200ms, IHUOverlayUpdate, IHaveHUCell
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000092 RID: 146
		int HUMax { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000093 RID: 147
		int HUMin { get; }

		// Token: 0x06000094 RID: 148
		int ConsumedHU(int HUavailable, float dt);
	}
}
