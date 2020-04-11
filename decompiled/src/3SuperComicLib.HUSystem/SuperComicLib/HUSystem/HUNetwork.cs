using System;
using System.Collections.Generic;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000019 RID: 25
	public class HUNetwork : UtilityNetwork
	{
		// Token: 0x06000078 RID: 120 RVA: 0x0000492E File Offset: 0x00002B2E
		public override void AddItem(int cell, object item)
		{
			this.pipes.Add(item as IHaveHUCell);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004944 File Offset: 0x00002B44
		public override void Reset(UtilityNetworkGridNode[] grid)
		{
			for (int x = 0; x < this.pipes.Count; x++)
			{
				grid[this.pipes[x].HUCell].networkIdx = -1;
			}
			this.pipes.Clear();
		}

		// Token: 0x04000053 RID: 83
		public List<IHaveHUCell> pipes = new List<IHaveHUCell>();
	}
}
