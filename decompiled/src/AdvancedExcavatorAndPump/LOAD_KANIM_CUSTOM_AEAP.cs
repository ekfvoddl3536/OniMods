using System;
using System.IO;
using System.Reflection;
using Harmony;
using SuperComicLib.Util;

namespace AdvancedExcavatorAndPump
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(NativeAnimBatchLoader), "Awake")]
	public static class LOAD_KANIM_CUSTOM_AEAP
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002590 File Offset: 0x00000790
		public static void Prefix()
		{
			AssetBundleLoader.LoadFromFile("sc_aeap_kanim", Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets\\n1909_aeap"));
		}
	}
}
