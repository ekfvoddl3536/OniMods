using System;
using System.Collections.Generic;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000016 RID: 22
	public static class HUSimUpdater
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00004448 File Offset: 0x00002648
		public static void Clear()
		{
			HUSimUpdater.Items.Clear();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004454 File Offset: 0x00002654
		public static void AddListener(IHUSim200ms listener)
		{
			HUSimUpdater.Items.Add(listener);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004462 File Offset: 0x00002662
		public static void RemoveListener(IHUSim200ms listener)
		{
			HUSimUpdater.Items.Remove(listener);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004470 File Offset: 0x00002670
		public static void Update(float dt)
		{
			foreach (IHUSim200ms ihusim200ms in HUSimUpdater.Items)
			{
				ihusim200ms.HUSim200ms(dt);
			}
		}

		// Token: 0x0400004B RID: 75
		private static readonly HashSet<IHUSim200ms> Items = new HashSet<IHUSim200ms>();
	}
}
