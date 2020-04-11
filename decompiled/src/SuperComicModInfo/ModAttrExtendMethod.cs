using System;

namespace SuperComicModInfo
{
	// Token: 0x02000003 RID: 3
	public static class ModAttrExtendMethod
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static bool IsEquals(this ModAttributes mod, ModAttributes m2)
		{
			return (mod & m2) == m2;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public static bool IsEquals(this ModAttributes mod, params ModAttributes[] m2s)
		{
			foreach (ModAttributes d in m2s)
			{
				bool flag = !mod.IsEquals(d);
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002098 File Offset: 0x00000298
		public static bool IsOrEquals(this ModAttributes mod, ModAttributes m2)
		{
			int fm = (int)mod;
			int m3 = (int)m2;
			while (fm > 0)
			{
				bool flag = (m3 & 1) == 0 || (fm & 1) == 0;
				if (!flag)
				{
					return true;
				}
				fm >>= 1;
				m3 >>= 1;
			}
			return false;
		}
	}
}
