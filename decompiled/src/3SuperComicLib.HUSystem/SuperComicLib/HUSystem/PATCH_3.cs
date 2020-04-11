using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony;

namespace SuperComicLib.HUSystem
{
	// Token: 0x02000009 RID: 9
	[HarmonyPatch(typeof(FilteredDragTool), "OnOverlayChanged")]
	public class PATCH_3
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002198 File Offset: 0x00000398
		public static bool Prepare()
		{
			Type typeFromHandle = typeof(FilteredDragTool);
			BindingFlags f = BindingFlags.Instance | BindingFlags.NonPublic;
			PATCH_3.current = typeFromHandle.GetField("currentFilterTargets", f);
			PATCH_3.filter = typeFromHandle.GetField("filterTargets", f);
			PATCH_3.isact = typeFromHandle.GetField("active", f);
			return true;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021E8 File Offset: 0x000003E8
		public static bool Prefix(FilteredDragTool __instance, HashedString overlay)
		{
			if (!(bool)PATCH_3.isact.GetValue(__instance))
			{
				return false;
			}
			if (overlay == "HUPipe")
			{
				Dictionary<string, ToolParameterMenu.ToggleState> fi = (Dictionary<string, ToolParameterMenu.ToggleState>)PATCH_3.filter.GetValue(__instance);
				foreach (string i in new List<string>(fi.Keys))
				{
					fi[i] = 2;
					if (i == ToolParameterMenu.FILTERLAYERS.BACKWALL)
					{
						fi[i] = 0;
					}
				}
				PATCH_3.current.SetValue(__instance, fi);
				ToolMenu.Instance.toolParameterMenu.PopulateMenu(fi);
				return false;
			}
			return true;
		}

		// Token: 0x0400000D RID: 13
		private static FieldInfo current;

		// Token: 0x0400000E RID: 14
		private static FieldInfo filter;

		// Token: 0x0400000F RID: 15
		private static FieldInfo isact;
	}
}
