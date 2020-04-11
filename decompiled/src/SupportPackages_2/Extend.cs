using System;
using System.Collections.Generic;

namespace SupportPackages
{
	// Token: 0x02000004 RID: 4
	internal static class Extend
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002160 File Offset: 0x00000360
		public static void Alloc<T>(this List<T> vs, int size)
		{
			List<T> temp = new List<T>(size);
			vs.AddRange(temp);
		}
	}
}
